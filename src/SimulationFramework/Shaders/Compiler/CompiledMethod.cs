using SimulationFramework.Shaders.Compiler.Expressions;
using SimulationFramework.Shaders.Compiler.ILDisassembler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler;
public class CompiledMethod
{
    public MethodBase Method { get; private set; }
    public Type ReturnType { get; private set; }
    public BlockExpression Body { get; private set; }
    public string Name { get; private set; }
    public List<ParameterExpression> Parameters { get; private set; }
    internal MethodDisassembly Disassembly { get; private set; }

    public CompiledMethod(MethodBase method)
    {
        Method = method;
        ReturnType = GetReturnType(method);

        Disassembly = new MethodDisassembly(method);

        Body = ExpressionBuilder.BuildExpression(Disassembly, out var parameters);

        Parameters = new(parameters);

        Name = GetName(method);
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

    private string GetName(MethodBase method)
    {
        if (method is MethodInfo methodInfo)
        {
            return methodInfo.Name;
        }

        if (method is ConstructorInfo constructor)
        {
            return constructor.DeclaringType?.Name ?? throw new Exception();
        }

        throw new Exception();
    }

    public void TransformBody(ExpressionVisitor visitor)
    {
        var result = visitor.Visit(Body);

        if (result is not BlockExpression blockExpression)
        {
            blockExpression = Expression.Block(result);
        }

        Body = blockExpression;
    }
}
