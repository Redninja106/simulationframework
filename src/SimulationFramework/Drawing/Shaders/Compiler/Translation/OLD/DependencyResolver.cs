using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Translation.OLD;

internal class DependencyResolver
{
    private readonly CompilationContextOLD context;
    private readonly ShaderIntrinsicsManager intrinsicManager;

    public DependencyResolver(CompilationContextOLD context, ShaderIntrinsicsManager intrinsicManager)
    {
        this.context = context;
        this.intrinsicManager = intrinsicManager;
    }

    internal void AddDependencies(CompilationContextOLD context, ShaderMethod shaderMethod)
    {
        var visitor = new DependencyVisitor(context);
        shaderMethod.Body.Accept(visitor);
    }

    internal void AddDependencies(CompilationContextOLD context, ShaderStructure structure)
    {
        var visitor = new DependencyVisitor(context);

        foreach (var field in structure.Fields)
        {
        }
    }


    //public override void CheckMethod(CompilationContext context, ShaderMethod compiledMethod)
    //{
    //    RequireType(context, compiledMethod.ReturnType);

    //    foreach (var param in compiledMethod.Parameters)
    //    {
    //        RequireType(context, param.expr.Type);
    //    }

    //    Visit(compiledMethod.Body);

    //    var deps = methodDependencies.ToArray();
    //    methodDependencies.Clear();

    //    foreach (var dependency in deps)
    //    {
    //        if (context.methods.Any(m => m.Method == dependency))
    //            continue;

    //        context.Compiler.CompileMethod(context, dependency);
    //    }

    //    base.CheckMethod(context, compiledMethod);
    //}

    //public override void CheckVariable(CompilationContext context, ShaderVariable compiledVariable)
    //{
    //    RequireType(context, compiledVariable.VariableType);

    //    base.CheckVariable(context, compiledVariable);
    //}

    //public override void CheckStruct(CompilationContext context, ShaderStructure compiledStruct)
    //{
    //    foreach (var field in compiledStruct.Fields)
    //    {
    //        if (field.FieldType == compiledStruct.StructType)
    //        {
    //            throw new Exception();
    //        }

    //        RequireType(context, field.FieldType);
    //    }
    //    base.CheckStruct(context, compiledStruct);
    //}

    //private void RequireType(CompilationContext context, Type type)
    //{
    //    if (intrinsicTypes.Contains(type))
    //        return;

    //    if (context.structs.Any(s => s.StructType == type))
    //        return;

    //    context.Compiler.CompileStruct(context, type);
    //}

    //protected override Expression VisitMethodCall(CallExpression node)
    //{
    //    if (node.Method.GetCustomAttribute<ShaderIntrinsicAttribute>() is null && !methodDependencies.Contains(node.Method))
    //    {
    //        methodDependencies.Add(node.Method);
    //    }

    //    return base.VisitMethodCall(node);
    //}

    //protected override Expression VisitNew(NewExpression node)
    //{
    //    if (node.Constructor!.GetCustomAttribute<ReplaceAttribute>() is null)
    //    {
    //        methodDependencies.Add(node.Constructor!);
    //    }

    //    return base.VisitNew(node);
    //}

    //protected override Expression VisitExtension(Expression node)
    //{
    //    if (node is ConstructorCallExpression ctorCall)
    //    {
    //        methodDependencies.Add(ctorCall.Constructor);
    //    }

    //    return base.VisitExtension(node);
    //}

    //public IEnumerable<MethodInfo> GetMethodDependenciesOf(ShaderMethod method)
    //{
    //    List<MethodInfo> dependencies = new();
    //    method.Body.Visit<CallExpression>(e =>
    //    {
    //        if (ShouldCompile(e.ExpressionType))
    //            dependencies.Add(e.Callee);
    //    });
    //    return dependencies;
    //}

    //public IEnumerable<Type> GetTypeDependenciesOf(ShaderMethod method)
    //{
    //    List<Type> dependencies = new();
    //    method.Body.Accept(e =>
    //    {
    //        if (ShouldCompile(e.ExpressionType))
    //            dependencies.Add(e.ExpressionType);
    //    });
    //    return dependencies;
    //}

    //public IEnumerable<Type> GetTypeDependenciesOf(ShaderStructure structure)
    //{
    //    foreach (var f in structure.Fields)
    //    {
    //        if (ShouldCompile(f.FieldType))
    //            yield return f.FieldType;
    //    }

    //    foreach (var g in structure.StructType.GetGenericArguments())
    //    {
    //        if (ShouldCompile(g))
    //            yield return g;
    //    }
    //}


    //public bool ShouldCompile([NotNullWhen(true)] Type? type)
    //{
    //    if (type is null)
    //        return false;

    //    return !IsIntrinsic(type);
    //}

    //public bool ShouldCompile([NotNullWhen(true)] MethodInfo? method)
    //{
    //    if (method is null)
    //        return false;

    //    return !IsIntrinsic(method);
    //}
}

class DependencyVisitor : ExpressionVisitor
{
    readonly CompilationContextOLD context;

    public DependencyVisitor(CompilationContextOLD context)
    {
        this.context = context;
    }

    public override Expression VisitCallExpression(CallExpression expression)
    {
        context.AddMethod(expression.Callee);
        return base.VisitCallExpression(expression);
    }

    public override Expression VisitNewExpression(NewExpression expression)
    {
        context.AddMethod(expression.Constructor);
        return base.VisitNewExpression(expression);
    }
    public override Expression VisitLocalVariableExpression(LocalVariableExpression expression)
    {
        context.AddType(expression.LocalVariable.VariableType);
        return base.VisitLocalVariableExpression(expression);
    }
}