﻿using SimulationFramework.Shaders.Compiler.Expressions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler;
public class CodeGenerator : ExpressionVisitor
{
    public ShaderCompilation Compilation { get; private set; }

    public IndentedTextWriter Writer { get; private set; }

    public CodeGenerator(ShaderCompilation compilation)
    {
        Compilation = compilation;
    }

    public void Emit(TextWriter writer)
    {
        Writer = new(writer);

        VisitStructs(Compilation.Structs);
        VisitVariables(Compilation.Variables);
        VisitMethods(Compilation.Methods);
    }

    protected virtual void VisitStructs(IEnumerable<CompiledStruct> structs)
    {
        foreach (var @struct in structs)
        {
            VisitStruct(@struct);
        }
    }

    protected virtual void VisitVariables(IEnumerable<CompiledVariable> variables)
    {
        foreach (var variable in variables)
        {
            VisitVariable(variable);
        }
    }

    protected virtual void VisitMethods(IEnumerable<CompiledMethod> methods)
    {
        foreach (var method in methods)
        {
            VisitMethod(method);
        }
    }

    protected virtual void VisitMethod(CompiledMethod method)
    {
        Writer.WriteLine();
        VisitType(method.ReturnType);
        Writer.Write(' ');
        VisitIdentifier(method.Name);
        Writer.Write('(');

        if (method.Parameters.Any())
        {
            VisitParameterDeclaration(method.Parameters.First());
            foreach (var parameter in method.Parameters.Skip(1))
            {
                Writer.Write(", ");
                VisitParameterDeclaration(parameter);
            }
        }
        Writer.WriteLine(')');
        Writer.WriteLine('{');
        Writer.Indent++;

        foreach (var typeGroup in method.Body.Variables.GroupBy(p => p.Type))
        {
            VisitType(typeGroup.Key);
            Writer.Write(" ");

            VisitIdentifier(typeGroup.First().Name);
            foreach (var element in typeGroup.Skip(1))
            {
                Writer.Write(", ");
                VisitIdentifier(element.Name);
            }

            Writer.WriteLine(";");
        }

        Visit(method.Body);

        Writer.Indent--;
        Writer.Write('}');
    }

    protected virtual void VisitParameterDeclaration(ParameterExpression parameter)
    {
        VisitType(parameter.Type);
        Writer.Write(' ');
        VisitIdentifier(parameter.Name);
    }

    protected virtual void VisitStruct(CompiledStruct @struct)
    {
        Writer.WriteLine();
        Writer.Write("struct ");
        VisitIdentifier(@struct.StructType.Name);
        Writer.WriteLine();
        Writer.WriteLine('{');
        Writer.Indent++;

        foreach (var field in @struct.Fields)
        {
            VisitField(field);
            Writer.WriteLine(";");
        }

        Writer.Indent--;
        Writer.Write('}');
    }

    protected virtual void VisitField(FieldInfo field)
    {
        VisitType(field.FieldType);
        Writer.Write(' ');
        VisitIdentifier(field.Name);
    }

    protected virtual void VisitType(Type type)
    {
        Writer.Write(type.Name);
    }

    protected virtual void VisitIdentifier(string identifier)
    {
        Writer.Write(identifier);
    }

    protected virtual void VisitVariable(CompiledVariable variable)
    {
        Writer.WriteLine();

        if (variable.IsInput)
        {
            Writer.Write("in ");
        }
        else if (variable.IsOutput)
        {
            Writer.Write("out ");
        }

        VisitType(variable.VariableType);
        Writer.Write(' ');
        VisitIdentifier(variable.Name);
        Writer.Write(';');
    }

    protected override Expression VisitBlock(BlockExpression node)
    {
        foreach (var expr in node.Expressions)
        {
            Visit(expr);
            Writer.WriteLine(';');
        }

        return node;
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        if (NeedsParentheses(node.NodeType, node.Left.NodeType))
        {
            Writer.Write('(');
            Visit(node.Left);
            Writer.Write(')');
        }
        else
        {
            Visit(node.Left);
        }

        Writer.Write(' ');
        Writer.Write(GetExpressionOperator(node.NodeType));
        Writer.Write(' ');

        if (NeedsParentheses(node.NodeType, node.Right.NodeType))
        {
            Writer.Write('(');
            Visit(node.Right);
            Writer.Write(')');
        }
        else
        {
            Visit(node.Right);
        }

        return node;
    }

    protected override Expression VisitUnary(UnaryExpression node)
    {
        Writer.Write(GetExpressionOperator(node.NodeType));

        if (NeedsParentheses(ExpressionType.Negate, node.NodeType))
        {
            Writer.Write('(');
            Visit(node.Operand);
            Writer.Write(')');
        }
        else
        {
            Visit(node.Operand);
        }

        return node;
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        Writer.Write(node.Value?.ToString());

        return node;
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        if (NeedsParentheses(ExpressionType.MemberAccess, node.Expression.NodeType))
        {
            Writer.Write('(');
            Visit(node.Expression);
            Writer.Write(")");
        }
        else
        {
            Visit(node.Expression);
        }
        Writer.Write(".");
        VisitIdentifier(node.Member.Name);

        return node;
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        VisitIdentifier(node.Name);

        return node;
    }

    protected override Expression VisitLabel(LabelExpression node)
    {
        Writer.Write(node.Target.Name);

        if (node.DefaultValue is not null && node.DefaultValue.Type != typeof(void))
        {
            Writer.Write(' ');
            Visit(node.DefaultValue);
        }

        return node;
    }

    protected override Expression VisitExtension(Expression node)
    {
        if (node is CompiledMethodCallExpression compiledMethodCall)
        {
            return VisitCompiledMethodCall(compiledMethodCall);
        }

        if (node is IntrinsicCallExpression intrinsicCall)
        {
            return VisitIntrinsicCall(intrinsicCall);
        }

        if (node is CompiledVariableExpression compiledVariableExpression)
        {
            return VisitCompiledVariableExpression(compiledVariableExpression);
        }

        if (node is CompiledVariableAssignmentExpression compiledVariableAssignmentExpression)
        {
            return VisitCompiledVariableAssignmentExpression(compiledVariableAssignmentExpression);
        }

        return base.VisitExtension(node);
    }

    protected virtual Expression VisitCompiledMethodCall(CompiledMethodCallExpression node)
    {
        VisitIdentifier(node.Method.Name);
        Writer.Write('(');

        if (node.Arguments.Any())
        {
            Visit(node.Arguments.First());
            foreach (var arg in node.Arguments.Skip(1))
            {
                Writer.Write(", ");
                Visit(arg);
            }
        }

        Writer.Write(')');

        return node;
    }

    protected virtual Expression VisitIntrinsicCall(IntrinsicCallExpression node)
    {
        VisitIdentifier(node.Method.Name);

        Writer.Write("(");

        Visit(node.Arguments.First());
        foreach (var arg in node.Arguments.Skip(1))
        {
            Writer.Write(", ");
            Visit(arg);
        }

        Writer.Write(")");

        return node;
    }

    protected virtual Expression VisitCompiledVariableExpression(CompiledVariableExpression node)
    {
        VisitIdentifier(node.Variable.Name);
        return node;
    }

    protected virtual Expression VisitCompiledVariableAssignmentExpression(CompiledVariableAssignmentExpression node)
    {
        VisitCompiledVariableExpression(node.Left);
        Writer.Write(" = ");
        Visit(node.Right);
        return node;
    }

    protected virtual string GetExpressionOperator(ExpressionType type)
    {
        return type switch
        {
            ExpressionType.Add => "+",
            ExpressionType.Subtract => "-",
            ExpressionType.Multiply => "*",
            ExpressionType.Divide => "/",
            ExpressionType.Assign => "=",
            _ => "???"
        };
    }

    private bool NeedsParentheses(ExpressionType parent, ExpressionType child)
    {
        return GetPrecedence(parent) < GetPrecedence(child);
    }

    private int GetPrecedence(ExpressionType expression)
    {
        return expression switch
        {
            ExpressionType.Assign => 14,
            ExpressionType.Add
            or ExpressionType.Subtract => 4,
            ExpressionType.Multiply
            or ExpressionType.Divide
            or ExpressionType.Modulo => 3,
            ExpressionType.Negate => 2,
            ExpressionType.Call
            or ExpressionType.Extension
            or ExpressionType.Parameter
            or ExpressionType.MemberAccess => 1,
            _ => 0
        };
    }
}