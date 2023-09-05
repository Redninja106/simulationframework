using SimulationFramework.Drawing.Shaders.Compiler.ControlFlow;
using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using SimulationFramework.Drawing.Shaders.Compiler.ILDisassembler;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler;
public class ShaderMethod
{
    public MethodBase Method { get; private set; }
    public Type ReturnType { get; set; }
    public string BaseName => GetBaseName();
    public string FullName => GetFullName();
    public List<MethodParameter> Parameters { get; private set; }
    public List<LocalVariable> Locals { get; private set; }
    public bool IsStatic => Method.IsStatic;

    public BlockExpression Body { get; private set; }
    internal MethodDisassembly Disassembly { get; private set; }
    internal ControlFlowGraph ControlFlowGraph { get; private set; }

    public ShaderMethod(MethodBase method)
    {
        this.Method = method;
        ReturnType = GetReturnType(method);
        // 
        // Disassembly = new MethodDisassembly(method);
        // 
        // Console.WriteLine($"Disassembly for method '{Method.Name}({string.Join(',',method.GetParameters().Select(p => $"{p.ParameterType} {p.Name}"))})'");
        // 
        // foreach (var i in Disassembly.instructions)
        // {
        //     Console.WriteLine(i);
        // }
        // 
        // Console.WriteLine(new string('=', 100));
        // 
        // var graph = new ControlFlowGraph(Disassembly);
        // 
        // Body = ExpressionBuilder.BuildExpressions(graph);
        // 
        // 
    }

    static MethodParameter[] GetParameters(MethodDisassembly disassembly)
    {
        var infos = disassembly.Method.GetParameters();

        var parameters = infos.Select(p => new MethodParameter(disassembly, p));

        if (!disassembly.Method.IsStatic)
        {
            parameters = parameters.Prepend(new MethodParameter(disassembly, null));
        }

        return parameters.ToArray();
    }

    private Type GetReturnType(MethodBase method)
    {
        if (method is MethodInfo methodInfo)
        {
            return methodInfo.ReturnType;
        }

        if (method is ConstructorInfo constructor)
        {
            return constructor.DeclaringType ?? throw new Exception();
        }

        throw new Exception();
    }

    private string GetBaseName()
    {
        if (Method is MethodInfo methodInfo)
        {
            return methodInfo.Name;
        }

        if (Method is ConstructorInfo)
        {
            return "_ctor" ?? throw new Exception();
        }

        throw new Exception();
    }

    private string GetFullName()
    {
        var baseName = GetBaseName();

        for (int i = 0; i < Parameters.Count; i++)
        {
            baseName += "_" + Parameters[i].ParameterType.Name;
        }

        return baseName;
    }

    //public void MutateBody<T>(ExpressionMutator<T> mutator) where T : Expression
    //{
    //    var mutated = Body.Mutate<T>(mutator);

    //    if (mutated is not BlockExpression block)
    //        block = new BlockExpression(new[] { mutated });

    //    this.Body = block;
    //}

    public void VisitBody(ExpressionVisitor visitor)
    {
        var result = Body.Accept(visitor);

        if (result is not BlockExpression blockExpression)
        {
            blockExpression = new BlockExpression(new[] { result });
        }

        Body = blockExpression;
    }

    internal void SetBody(BlockExpression body)
    {
        this.Body = body;
        VariableVisitor visitor = new();
        VisitBody(visitor);
        Locals = visitor.LocalVariables;
    }
    internal void SetDisassembly(MethodDisassembly disassembly)
    {
        this.Disassembly = disassembly;
        Parameters = GetParameters(disassembly).ToList();
    }
    internal void SetControlFlowGraph(ControlFlowGraph controlFlowGraph)
    {
        this.ControlFlowGraph = controlFlowGraph;
    }

    public override string ToString()
    {
        return FullName;
    }

    // finds local variables
    private class VariableVisitor : ExpressionVisitor
    {
        public List<LocalVariable> LocalVariables { get; } = new();

        public override Expression VisitLocalVariableExpression(LocalVariableExpression expression)
        {
            if (!LocalVariables.Contains(expression.LocalVariable))
            {
                LocalVariables.Add(expression.LocalVariable);
            }
            return base.VisitLocalVariableExpression(expression);
        }
    }
}
