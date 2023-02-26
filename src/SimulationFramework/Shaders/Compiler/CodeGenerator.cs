using SimulationFramework.Shaders.Compiler.Expressions;
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
using System.Xml.Linq;
using static SimulationFramework.Shaders.Compiler.ControlFlow.DgmlBuilder;

namespace SimulationFramework.Shaders.Compiler;
public class CodeGenerator : ExtendedExpressionVisitor
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
        VisitTypeName(method.Method.DeclaringType);
        Writer.Write('_');
        VisitIdentifier(method.Name);
        Writer.Write('(');

        if (method.Parameters.Any())
        {
            if (method.Method is MethodInfo && !method.Method.IsStatic)
            {
                Writer.Write("inout ");
            }

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
            if (typeGroup.First().Name is "this")
            {
                Writer.Write(" = (");
                VisitType(typeGroup.Key);
                Writer.Write(")0");
            }

            foreach (var element in typeGroup.Skip(1))
            {
                Writer.Write(", ");
                VisitIdentifier(element.Name);
            }

            Writer.WriteLine(";");
        }

        foreach (var expr in method.Body.Expressions)
        {
            Visit(expr);
            Writer.WriteLine(';');
        }

        Writer.Indent--;
        Writer.Write('}');
    }

    protected override Expression VisitConditional(ConditionalExpression expression)
    {
        Writer.Write("if (");
        Visit(expression.Test);
        Writer.WriteLine(")");
        Visit(expression.IfTrue);
        if (!IsEmptyElseExpr(expression.IfFalse))
        {
            Writer.WriteLine("else");
            Visit(expression.IfFalse);
        }

        return expression;
    
        static bool IsEmptyElseExpr(Expression expr)
        {
            while (expr.NodeType is ExpressionType.Block)
                expr = (expr as BlockExpression)!.Expressions.First();

            return expr.NodeType == ExpressionType.Default;
        }
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
        VisitType(@struct.StructType);
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

    protected override Expression VisitInlineSourceExpression(InlineSourceExpression expression)
    {
        Writer.Write(expression.Source);
        return expression;
    }

    protected virtual void VisitField(FieldInfo field)
    {
        VisitType(field.FieldType);
        Writer.Write(' ');
        VisitIdentifier(field.Name);
    }

    protected virtual void VisitType(Type type)
    {
        VisitTypeName(type);
    }

    protected virtual void VisitTypeName(Type type)
    {
        var name = FormatTypeName(type);

        if (type.IsGenericType)
        {
            // trim TypeName`n
            name = name[..name.IndexOf('`')];
        }

        Writer.Write(name);

        foreach (var genericArg in type.GetGenericArguments())
        {
            Writer.Write('_');
            VisitType(genericArg);
        }
    }

    private string FormatTypeName(Type type)
    {
        var name = type.Name;

        return name;
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
        Writer.WriteLine('{');
        Writer.Indent++;

        foreach (var expr in node.Expressions)
        {
            Visit(expr);
            Writer.WriteLine(';');
        }

        Writer.Indent--;
        Writer.WriteLine('}');

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
        if (node.NodeType is ExpressionType.Convert)
        {
            Writer.Write('(');
            VisitType(node.Type);
            Writer.Write(')');
            Writer.Write('(');
            Visit(node.Operand);
            Writer.Write(')');
            return node;
        }

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

    //protected override Expression VisitExtension(Expression node)
    //{
    //    if (node is CompiledMethodCallExpression compiledMethodCall)
    //    {
    //        return VisitCompiledMethodCall(compiledMethodCall);
    //    }

    //    if (node is IntrinsicCallExpression intrinsicCall)
    //    {
    //        return VisitIntrinsicCall(intrinsicCall);
    //    }

    //    if (node is CompiledVariableExpression compiledVariableExpression)
    //    {
    //        return VisitCompiledVariableExpression(compiledVariableExpression);
    //    }

    //    if (node is CompiledVariableAssignmentExpression compiledVariableAssignmentExpression)
    //    {
    //        return VisitCompiledVariableAssignmentExpression(compiledVariableAssignmentExpression);
    //    }

    //    if (node is InlineSourceExpression inlineSourceExpression)
    //    {
    //        return VisitInlineSourceExpression(inlineSourceExpression);
    //    }

    //    if (node is ReferenceAssignmentExpression referenceAssignmentExpression)
    //    {
    //        return VisitReferenceAssignmentExpression(referenceAssignmentExpression);
    //    }

    //    return base.VisitExtension(node);
    //}

    protected override Expression VisitReferenceAssignmentExpression(ReferenceAssignmentExpression node)
    {
        Visit(node.Left);
        Writer.Write(" = ");
        Visit(node.Right);
        return node;
    }

    protected override Expression VisitCompiledMethodCallExpression(CompiledMethodCallExpression node)
    {
        VisitTypeName(node.Method.Method.DeclaringType);
        Writer.Write('_');
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

    protected override Expression VisitIntrinsicCallExpression(IntrinsicCallExpression node)
    {
        VisitTypeName(node.Method.DeclaringType);
        Writer.Write('_');
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

    protected override Expression VisitCompiledVariableExpression(CompiledVariableExpression node)
    {
        VisitIdentifier(node.Variable.Name);
        return node;
    }

    protected override Expression VisitCompiledVariableAssignmentExpression(CompiledVariableAssignmentExpression node)
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
            ExpressionType.And => "&",
            ExpressionType.Or => "|",
            ExpressionType.LeftShift => "<<",
            ExpressionType.RightShift => ">>",
            ExpressionType.Not => "-",
            ExpressionType.Negate => "-",
            ExpressionType.Equal => "==",
            ExpressionType.NotEqual => "!=",
            ExpressionType.GreaterThan => ">",
            ExpressionType.GreaterThanOrEqual => ">=",
            ExpressionType.LessThan => "<",
            ExpressionType.LessThanOrEqual => "<=",

            _ => " ??? "
        };
    }

    private bool NeedsParentheses(ExpressionType parent, ExpressionType child)
    {
        return GetPrecedence(parent) < GetPrecedence(child);
    }

    private int GetPrecedence(ExpressionType expression)
    {
        // this ain't c but still relevant
        // https://en.cppreference.com/w/c/language/operator_precedence
        return expression switch
        {
            ExpressionType.Assign => 14,
            
            ExpressionType.Or => 10,

            ExpressionType.And => 8,

            ExpressionType.Equal or
            ExpressionType.NotEqual => 7,

            ExpressionType.GreaterThan or
            ExpressionType.GreaterThanOrEqual or
            ExpressionType.LessThan or
            ExpressionType.LessThanOrEqual => 6,

            ExpressionType.LeftShift
            or ExpressionType.RightShift => 5,

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
