﻿using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing.Shaders.Compiler.ControlFlow;
using SimulationFramework.Drawing.Shaders.Compiler.Disassembler;
using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler;

public class CanvasShaderCompiler
{
    private Dictionary<Type, ShaderType> primitiveTypeMap = [];

    // TODO: add method cacheing here

    public CanvasShaderCompiler()
    {
        primitiveTypeMap[typeof(void)] = ShaderType.Void;
        primitiveTypeMap[typeof(bool)] = ShaderType.Bool;
        primitiveTypeMap[typeof(int)] = ShaderType.Int;
        primitiveTypeMap[typeof(float)] = ShaderType.Float;
        primitiveTypeMap[typeof(Vector2)] = ShaderType.Float2;
        primitiveTypeMap[typeof(Vector3)] = ShaderType.Float3;
        primitiveTypeMap[typeof(Vector4)] = ShaderType.Float4;
        primitiveTypeMap[typeof(ColorF)] = ShaderType.Float4;
        primitiveTypeMap[typeof(Matrix4x4)] = ShaderType.Matrix4x4;
        primitiveTypeMap[typeof(Matrix3x2)] = ShaderType.Matrix3x2;
        primitiveTypeMap[typeof(ITexture)] = ShaderType.Texture;
    }

    public ShaderCompilation Compile(CanvasShader shader)
    {
        var shaderType = shader.GetType();
        var entryPoint = shaderType.GetMethod(
            nameof(CanvasShader.GetPixelColor),
            [typeof(Vector2)]
            ) ?? throw new();

        CompilerContext context = new(shaderType, entryPoint, primitiveTypeMap);

        foreach (var field in shaderType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
            ShaderVariable variable = new(context.CompileType(field.FieldType), new(field.Name), field);
            context.Uniforms.Add(field, variable);
            context.Compilation.Uniforms.Add(variable);
        }

        context.EnqueueMethod(entryPoint);

        while (context.MethodQueue.TryDequeue(out MethodBase? method))
        {
            var shaderMethod = context.Methods[method];
            CompileMethod(shaderMethod, context, method);
            context.Compilation.Methods.Add(shaderMethod);
        }

        return context.Compilation;
    }


    private void CompileMethod(ShaderMethod shaderMethod, CompilerContext context, MethodBase method)
    {
        MethodDisassembly disassembly = new(method);
        ControlFlowGraph graph = new ControlFlowGraph(disassembly);

        var locals = disassembly.MethodBody.LocalVariables.Select(l => new ShaderVariable(context.CompileType(l.LocalType), new("var" + l.LocalIndex), null)).ToArray();
        var parameters = method.GetParameters().Select(p => new ShaderVariable(context.CompileType(p.ParameterType), new(p.Name!), null)).ToArray();

        var parametersWithThis = parameters;

        ShaderVariable? self = null;
        if (!method.IsStatic) 
        {
            self = new ShaderVariable(context.CompileType(method.DeclaringType!), new("self"), null);
            
            parametersWithThis = parameters.Prepend(self).ToArray();
        }

        var expressionBuilder = new ExpressionBuilder(context, graph, disassembly, parametersWithThis, locals);
        var expression = expressionBuilder.BuildExpressions();

        RedundantVariableFinder redundantVariableFinder = new();
        expression.Accept(redundantVariableFinder);
        var redundants = redundantVariableFinder.GetRedundantVariables();
        RedundantVariableReplacer replacer = new(redundants);
        expression = (BlockExpression)expression.Accept(replacer);
        locals = locals.Except(redundants).ToArray();
        Debug.Assert(replacer.values.Count is 0);

        if (!method.IsStatic && method is ConstructorInfo)
        {
            locals = locals.Append(self).ToArray();
        }

        ShaderName name;
        if (method.DeclaringType == context.ShaderType)
        {
            name = new(method.Name);
        }
        else
        {
            name = new(method.DeclaringType!.FullName + "_" + method.Name);

            if (method is not ConstructorInfo)
            {
                parameters = parametersWithThis;
            }
        }

        shaderMethod.Body = expression;
        shaderMethod.Name = name;
        shaderMethod.Locals =  locals;
        shaderMethod.Parameters = parameters;
        shaderMethod.ReturnType = context.CompileType(expressionBuilder.ReturnType);
    }

    class RedundantVariableFinder : ShaderExpressionVisitor
    {
        /* 
         * redundant variables are variables that are both assignened and used exactly once.
         * these variables are usually not in the original source, instead being inserted by the compiler.
         */

        Dictionary<ShaderVariable, int> assignments = [];
        Dictionary<ShaderVariable, int> usages = [];

        public override ShaderExpression VisitBinaryExpression(BinaryExpression expression)
        {
            if (expression.Operation is BinaryOperation.Assignment)
            {
                if (expression.LeftOperand is ShaderVariableExpression variableExpr)
                {
                    if (assignments.TryGetValue(variableExpr.Variable, out int assigmentCount))
                    {
                        assignments[variableExpr.Variable] = assigmentCount + 1;
                    }
                    else
                    {
                        assignments[variableExpr.Variable] = 1;
                    }
                    
                    expression.RightOperand.Accept(this);
                    return expression;
                }
            }

            return base.VisitBinaryExpression(expression);
        }

        public override ShaderExpression VisitShaderVariableExpression(ShaderVariableExpression expression)
        {
            if (usages.TryGetValue(expression.Variable, out int usageCount))
            {
                usages[expression.Variable] = usageCount + 1;
            }
            else
            {
                usages[expression.Variable] = 1;
            }
            return base.VisitShaderVariableExpression(expression);
        }

        public IEnumerable<ShaderVariable> GetRedundantVariables()
        {
            foreach (var (var, assignmentCount) in assignments)
            {
                if (assignmentCount == 1 && usages.GetValueOrDefault(var, 0) == 1)
                {
                    yield return var;
                }
            }
        }
    }

    class RedundantVariableReplacer : ShaderExpressionVisitor
    {
        internal Dictionary<ShaderVariable, ShaderExpression?> values = [];

        public RedundantVariableReplacer(IEnumerable<ShaderVariable> variables)
        {
            foreach (var value in variables)
            {
                values.Add(value, null);
            }
        }

        public override ShaderExpression VisitBinaryExpression(BinaryExpression expression)
        {
            if (expression.Operation is BinaryOperation.Assignment)
            {
                if (expression.LeftOperand is ShaderVariableExpression varExpr)
                {
                    if (values.ContainsKey(varExpr.Variable))
                    {
                        values[varExpr.Variable] = expression.RightOperand.Accept(this);
                        return ShaderExpression.Empty;
                    }
                }
            }

            return base.VisitBinaryExpression(expression);
        }

        public override ShaderExpression VisitShaderVariableExpression(ShaderVariableExpression expression)
        {
            if (values.Remove(expression.Variable, out ShaderExpression? value))
            {
                Debug.Assert(value is not null);

                return value;
            }

            return base.VisitShaderVariableExpression(expression);
        }
    }
}