using SimulationFramework.Shaders;
using SimulationFramework.Shaders.Compiler;
using SimulationFramework.Shaders.Compiler.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler.Passes;

internal class DependencyResolver : CompilerPass
{
    public List<MethodBase> methodDependencies = new();

    private static readonly List<Type> intrinsicTypes = new()
    {
        typeof(float),
        typeof(int),
        typeof(uint),
        typeof(byte),
        typeof(void),
        typeof(string), // for inline shader source & printfs
        typeof(Vector2),
        typeof(Vector3),
        typeof(Vector4),
        typeof(ColorF),
        typeof(Matrix4x4),
        typeof(Matrix3x2),
    };

    public override void CheckMethod(CompilationContext context, CompiledMethod compiledMethod)
    {
        RequireType(context, compiledMethod.ReturnType);

        foreach (var param in compiledMethod.Parameters)
        {
            RequireType(context, param.Type);
        }

        Visit(compiledMethod.Body);

        var deps = methodDependencies.ToArray();
        methodDependencies.Clear();

        foreach (var dependency in deps)
        {
            if (context.methods.Any(m => m.Method == dependency))
                continue;

            context.Compiler.CompileMethod(context, dependency);
        }

        base.CheckMethod(context, compiledMethod);
    }

    public override void CheckVariable(CompilationContext context, CompiledVariable compiledVariable)
    {
        RequireType(context, compiledVariable.VariableType);

        base.CheckVariable(context, compiledVariable);
    }

    public override void CheckStruct(CompilationContext context, CompiledStruct compiledStruct)
    {
        foreach (var field in compiledStruct.Fields)
        {
            if (field.FieldType == compiledStruct.StructType)
            {
                throw new Exception();
            }

            RequireType(context, field.FieldType);
        }

        base.CheckStruct(context, compiledStruct);
    }

    private void RequireType(CompilationContext context, Type type)
    {
        if (intrinsicTypes.Contains(type))
            return;

        if (context.structs.Any(s => s.StructType == type))
            return;

        context.Compiler.CompileStruct(context, type);
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        if (node.Method.GetCustomAttribute<ReplaceAttribute>() is null && !methodDependencies.Contains(node.Method))
        {
            methodDependencies.Add(node.Method);
        }

        return base.VisitMethodCall(node);
    }

    protected override Expression VisitNew(NewExpression node)
    {
        if (node.Constructor!.GetCustomAttribute<ReplaceAttribute>() is null)
        {
            methodDependencies.Add(node.Constructor!);
        }

        return base.VisitNew(node);
    }

    protected override Expression VisitExtension(Expression node)
    {
        if (node is ConstructorCallExpression ctorCall)
        {
            methodDependencies.Add(ctorCall.Constructor);
            return node;
        }

        return base.VisitExtension(node);
    }
}