using SimulationFramework.Shaders.Compiler.ControlFlow;
using SimulationFramework.Shaders.Compiler.Expressions;
using SimulationFramework.Shaders.Compiler.ILDisassembler;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler.Expressions;

internal class ExpressionBuilder
{
    private MethodDisassembly Disassembly { get; set; }
    private InstructionStream InstructionStream { get; set; }
    private (ParameterExpression expr, ParamModifier modifier)[] Arguments { get; set; }
    private ParameterExpression[] Locals { get; set; }
    private LabelTarget ReturnTarget { get; set; }
    private Type ReturnType { get; set; }
    private bool IsConstructor { get; set; }
    private Stack<Expression> Expressions { get; set; }

    private ExpressionBuilder(MethodDisassembly disassembly)
    {
        this.Disassembly = disassembly;
    }

    public static BlockExpression BuildExpressions(ControlFlowGraph graph, out (ParameterExpression, ParamModifier)[] parameters)
    {
        var disassembly = graph.Disassembly;

        ExpressionBuilder builder = new(disassembly);

        builder.Expressions = new();
        builder.IsConstructor = disassembly.Method is ConstructorInfo;

        builder.InstructionStream = new InstructionStream(disassembly);

        builder.Arguments = GetArguments(disassembly);
        builder.Locals = GetLocals(disassembly);

        builder.ReturnType = disassembly.Method is MethodInfo method ? method.ReturnType : typeof(void);
        builder.ReturnTarget = Expression.Label(builder.ReturnType, "return");

        var exprs = builder.BuildNodeChain(graph.EntryNode, graph.ExitNode);

        parameters = builder.Arguments;
        
        Expression result = Expression.Block(builder.ReturnType, builder.Locals, exprs);
        result = builder.SimplifyExpression(result);

        return result is BlockExpression blockExpr ? blockExpr : Expression.Block(builder.Locals, result);
    }

    Expression? BuildBasicBlockExpression(BasicBlockNode basicBlock, bool clearStack = true)
    {
        if (basicBlock.Instructions.Count is 0)
            return null;

        if (clearStack)
            Expressions.Clear();

        // var instructions = new InstructionStream(this.Disassembly);
        // instructions.Position = basicBlock.Instructions

        var builders = new Func<Instruction, bool>[]
        {
            BuildNopExpr,
            BuildBinaryExpr,
            BuildFieldExpr,
            BuildUnaryExpr,
            BuildLocalExpr,
            BuildReturnExpr,
            BuildConstantExpr,
            BuildArgumentExpr,
            BuildNewObjExpr,
            BuildCallExpr,
            BuildLoadIndirect,
            BuildStoreIndirect,
            BuildDup,
            BuildBranch,

            i => i.OpCode is OpCode.Pop or OpCode.Initobj or OpCode.Ldobj,

            i => throw new NotSupportedException("Unsupported instruction '" + i.OpCode + "'."),
        };

        foreach (var instruction in basicBlock.Instructions)
        {
            foreach (var b in builders)
            {
                if (b(instruction))
                    break;
            }
        }

        return CreateBlockExpressionIfNeeded(Expressions.Reverse(), Locals);
    }

    bool BuildBranch(Instruction instruction)
    {
        Expression left, right;
        switch (instruction.OpCode)
        {
            case OpCode.Bne_Un or OpCode.Bne_Un_S:
                // emit not equal comparison
                right = Expressions.Pop();
                left = Expressions.Pop();
                Expressions.Push(Expression.NotEqual(left, right));
                break;
            case OpCode.Beq or OpCode.Beq_S:
                right = Expressions.Pop();
                left = Expressions.Pop();
                Expressions.Push(Expression.Equal(left, right));
                break;
            case OpCode.Blt or OpCode.Blt_S or OpCode.Blt_Un or OpCode.Blt_Un_S:
                // emit less than comparison
                right = Expressions.Pop();
                left = Expressions.Pop();
                Expressions.Push(Expression.LessThan(left, right));
                break;
            case OpCode.Ble or OpCode.Ble_S or OpCode.Ble_Un or OpCode.Ble_Un_S:
                // emit less than or equal comparison
                right = Expressions.Pop();
                left = Expressions.Pop();
                Expressions.Push(Expression.LessThanOrEqual(left, right));
                break;
            case OpCode.Bgt or OpCode.Bgt_S or OpCode.Bgt_Un or OpCode.Bgt_Un_S:
                // emit greater than or equal comparison
                right = Expressions.Pop();
                left = Expressions.Pop();
                Expressions.Push(Expression.GreaterThan(left, right));
                break;
            case OpCode.Bge or OpCode.Bge_S or OpCode.Bge_Un or OpCode.Bge_Un_S:
                // emit greater than or equal comparison
                right = Expressions.Pop();
                left = Expressions.Pop();
                Expressions.Push(Expression.GreaterThanOrEqual(left, right));
                break;
            case OpCode.Brfalse or OpCode.Brfalse_S:
                // invert condition
                var expr = Expressions.Pop();
                Expressions.Push(Expression.Not(expr));
                break;
            case OpCode.Br or OpCode.Br_S or OpCode.Brtrue or OpCode.Brtrue_S:
                break; // do nothing for these
            default:
                return false; // not a branch instruction
        }

        return true;

        bool IsBranchInstruction(OpCode opCode) =>
            opCode is
            OpCode.Br or
            OpCode.Br_S or
            OpCode.Brfalse or
            OpCode.Brfalse_S or
            OpCode.Brtrue or
            OpCode.Brtrue_S or
            OpCode.Beq or
            OpCode.Beq_S or
            OpCode.Bne_Un or
            OpCode.Bne_Un_S or
            OpCode.Bgt or
            OpCode.Bgt_S or
            OpCode.Bgt_Un or
            OpCode.Bgt_Un_S or
            OpCode.Blt or
            OpCode.Blt_S or
            OpCode.Blt_Un or
            OpCode.Blt_Un_S;
    }

    Expression? BuildNode(ControlFlowNode node, bool clearStack = true)
    {
        if (node is ConditionalNode conditional)
        {
            var beginExpr = BuildNode(conditional.Subgraph.EntryNode, clearStack);

            Debug.Assert(beginExpr is not null);

            beginExpr = GetConditionExpression(SimplifyExpression(beginExpr), out var condition);

            Debug.Assert(condition is not null);

            var trueExpr = BuildNodeChain(conditional.TrueBranch, conditional.Subgraph.ExitNode);
            
            Expression falseExpr = conditional.FalseBranch is null ? Expression.Default(trueExpr.Type) : BuildNodeChain(conditional.FalseBranch, conditional.Subgraph.ExitNode);

            bool isTernary = false;
            Expression condExpr;
            if (trueExpr is not BlockExpression && trueExpr.Type != typeof(void) && falseExpr is not BlockExpression && falseExpr.Type != typeof(void))
            {
                // might be ternary
                isTernary = true;
                condExpr = Expression.Condition(condition, ConvertExpressionType(trueExpr, typeof(object)), ConvertExpressionType(falseExpr, typeof(object)));
                Expressions.Clear();
                Expressions.Push(condExpr);
            }
            else
            {
                condExpr = Expression.Condition(condition, ConvertExpressionType(trueExpr, typeof(void)), ConvertExpressionType(falseExpr, typeof(void)));
            }

            var endExpr = BuildNode(conditional.Subgraph.ExitNode, !isTernary);

            return CreateBlockExpressionIfNeeded(new Expression?[] { beginExpr, isTernary ? null : condExpr, endExpr }.Where(ex => ex is not null).Cast<Expression>());
        }
        else if (node is LoopNode loop)
        {
            throw new Exception();
        }
        else if (node is BasicBlockNode basicBlock)
        {
            return BuildBasicBlockExpression(basicBlock, clearStack);
        }
        else
        {
            throw new Exception();
        }
    }

    private Expression CreateBlockExpressionIfNeeded(IEnumerable<Expression?> expressions, IEnumerable<ParameterExpression>? parameters = null)
    {
        if (expressions.Count() is 1)
            return expressions.Single()!;
        
        return Expression.Block(
            parameters ?? Array.Empty<ParameterExpression>(),
            expressions.Where(x => x is not null).Cast<Expression>()
            );
    }

    // trims the last expr from a block used for grabbing head statements for ifs and loops
    private Expression? GetConditionExpression(Expression expr, out Expression lastExpr)
    {
        if (expr is not BlockExpression blockExpr)
        {
            lastExpr = expr;
            return null;
        }

        lastExpr = blockExpr.Expressions.Last();
        return CreateBlockExpressionIfNeeded(blockExpr.Expressions.SkipLast(1), blockExpr.Variables);
    }

    Expression BuildNodeChain(ControlFlowNode start, ControlFlowNode end)
    {
        Debug.Assert(start.Graph == end.Graph);

        var graph = start.Graph;
        List<Expression?> exprs = new();

        var node = start;
        while (node != end)
        {
            exprs.Add(BuildNode(node));
            node = node.Successors.Single();
        }

        return CreateBlockExpressionIfNeeded(exprs.Where(ex => ex is not null).Cast<Expression>());
    }

    Expression SimplifyExpression(Expression expression)
    {
        if (expression is ConditionalExpression conditionalExpr)
            return Expression.Condition(
                SimplifyExpression(conditionalExpr.Test),
                ForceVoidType(SimplifyExpression(conditionalExpr.IfTrue)),
                ForceVoidType(SimplifyExpression(conditionalExpr.IfFalse))
                );
            
        if (expression is not BlockExpression blockExpr)
            return expression;

        IEnumerable<Expression> expressions = Enumerable.Empty<Expression>();

        foreach (var expr in blockExpr.Expressions.Select(SimplifyExpression))
        {
            expressions = expr is BlockExpression b ?
                expressions.Concat(b.Expressions) :
                expressions = expressions.Append(expr);
        }

        return CreateBlockExpressionIfNeeded(expressions, blockExpr.Variables);
    }

    Expression ForceVoidType(Expression expression)
    {
        if (expression.Type == typeof(void))
            return expression;

        return Expression.Block(typeof(void), expression);
    }

    bool BuildDup(Instruction instruction)
    {
        if (instruction.OpCode is not OpCode.Dup)
            return false;

        var expr = Expressions.Pop();
        Expressions.Push(expr);
        Expressions.Push(expr);

        return true;
    }

    static (ParameterExpression, ParamModifier)[] GetArguments(MethodDisassembly disassembly)
    {
        var infos = disassembly.Method.GetParameters();

        var parameters = infos.Select(p => (Expression.Parameter(p.ParameterType, p.Name), GetModifier(p)));

        if (!disassembly.Method.IsStatic)
        {
            parameters = parameters.Prepend((Expression.Parameter(disassembly.Method.DeclaringType!, "self"), ParamModifier.Ref));
        }

        return parameters.ToArray();

        ParamModifier GetModifier(ParameterInfo paramInfo)
        {
            if (paramInfo.IsOut)
            {
                if (paramInfo.IsIn)
                {
                    return ParamModifier.Ref;
                }

                return ParamModifier.Out;
            }

            return ParamModifier.In;
        }
    }

    static ParameterExpression[] GetLocals(MethodDisassembly disassembly)
    {
        var locals = disassembly.MethodBody.LocalVariables.Select(l => Expression.Variable(l.LocalType, "var" + l.LocalIndex));

        return locals.ToArray();
    }

    bool BuildNopExpr(Instruction instruction)
    {
        return instruction.OpCode is OpCode.Nop;
    }

    bool BuildBinaryExpr(Instruction instruction)
    {
        if (!IsBinaryExpression(instruction, out var exprType))
            return false;

        var right = Expressions.Pop();
        var left = Expressions.Pop();
        try
        {
            Expressions.Push(Expression.MakeBinary(exprType.Value, left, right));
        }
        catch (InvalidOperationException)
        {
            Expressions.Push(Expression.MakeBinary(exprType.Value, left, ConvertExpressionType(right, left.Type)));
        }
        return true;

        static bool IsBinaryExpression(Instruction instruction, [NotNullWhen(true)] out ExpressionType? type)
        {
            type = instruction.OpCode switch
            {
                OpCode.Add => ExpressionType.Add,
                OpCode.Sub => ExpressionType.Subtract,
                OpCode.Mul => ExpressionType.Multiply,
                OpCode.Div or OpCode.Div_Un => ExpressionType.Divide,
                OpCode.And => ExpressionType.And,
                OpCode.Or => ExpressionType.Or,
                OpCode.Shr => ExpressionType.RightShift,
                OpCode.Shr_Un => ExpressionType.RightShift,
                OpCode.Shl => ExpressionType.LeftShift,
                OpCode.Ceq => ExpressionType.Equal,
                OpCode.Clt => ExpressionType.LessThan,
                OpCode.Cgt => ExpressionType.GreaterThan,
                OpCode.Rem => ExpressionType.Modulo,
                OpCode.Rem_Un => ExpressionType.Modulo,

                _ => null
            };

            return type is not null;
        }
    }

    Expression ConvertExpressionType(Expression expression, Type type)
    {
        if (expression.Type == type)
            return expression;

        // handle true/false
        if (type == typeof(bool) && expression is ConstantExpression constExpr)
        {
            if (constExpr.Value is 0)
                return Expression.Constant(false);

            if (constExpr.Value is 1)
                return Expression.Constant(true);
        }

        if (type == typeof(void))
        {
            return Expression.Block(type, expression);
        }

        return Expression.Convert(expression, type);
    }

    bool BuildFieldExpr(Instruction instruction)
    {
        if (instruction.OpCode is not (OpCode.Ldfld or OpCode.Ldflda or OpCode.Stfld))
            return false;

        var metadataToken = (MetadataToken)instruction.Argument!;
        var fieldInfo = (FieldInfo)metadataToken.Resolve()!;

        if (instruction.OpCode is OpCode.Stfld)
        {
            var value = Expressions.Pop();
            var obj = Expressions.Pop();

            Expressions.Push(Expression.Assign(Expression.Field(obj, fieldInfo), value));
        }
        else // OpCode is Ldfld or Ldflda
        {
            var obj = Expressions.Pop();

            Expressions.Push(Expression.Field(obj, fieldInfo));
        }

        return true;
    }

    bool BuildUnaryExpr(Instruction instruction)
    {
        if (!IsUnaryExpression(instruction, out var exprType, out var castType))
            return false;

        var operand = Expressions.Pop();

        Expressions.Push(Expression.MakeUnary(exprType.Value, operand, castType!));

        return true;

        static bool IsUnaryExpression(Instruction instruction, [NotNullWhen(true)] out ExpressionType? exprType, out Type? castType)
        {
            exprType = instruction.OpCode switch
            {
                OpCode.Neg => ExpressionType.Negate,
                OpCode.Not => ExpressionType.Not,
                _ => null
            };

            if (exprType is not null)
            {
                castType = null;
                return true;
            }

            castType = instruction.OpCode switch
            {
                OpCode.Conv_I => typeof(nint),
                OpCode.Conv_I1 => typeof(sbyte),
                OpCode.Conv_I2 => typeof(short),
                OpCode.Conv_I4 => typeof(int),
                OpCode.Conv_I8 => typeof(long),
                OpCode.Conv_U => typeof(nuint),
                OpCode.Conv_U1 => typeof(byte),
                OpCode.Conv_U2 => typeof(ushort),
                OpCode.Conv_U4 => typeof(uint),
                OpCode.Conv_U8 => typeof(ulong),
                OpCode.Conv_R4 => typeof(float),
                OpCode.Conv_R8 => typeof(double),
                OpCode.Conv_R_Un => typeof(float),
                _ => null
            };

            if (castType is not null)
            {
                exprType = ExpressionType.Convert;
                return true;
            }

            return false;
        }
    }

    bool BuildLocalExpr(Instruction instruction)
    {
        if (IsLoadLocalExpression(instruction, out uint? loadIndex))
        {
            Expressions.Push(Locals[(int)loadIndex]);
            return true;
        }

        if (IsStoreLocalExpression(instruction, out uint? storeIndex))
        {
            var expr = Expressions.Pop();
            var local = Locals[(int)storeIndex];
            Expressions.Push(Expression.Assign(local, ConvertExpressionType(expr, local.Type)));

            return true;
        }

        return false;

        static bool IsLoadLocalExpression(Instruction instruction, [NotNullWhen(true)] out uint? value)
        {
            value = instruction.OpCode switch
            {
                OpCode.Ldloc => (uint)instruction.Argument!,
                OpCode.Ldloc_S => (byte)instruction.Argument!,
                OpCode.Ldloc_0 => 0,
                OpCode.Ldloc_1 => 1,
                OpCode.Ldloc_2 => 2,
                OpCode.Ldloc_3 => 3,
                OpCode.Ldloca => (uint)instruction.Argument!,
                OpCode.Ldloca_S => (byte)instruction.Argument!,
                _ => null
            };

            return value is not null;
        }

        static bool IsStoreLocalExpression(Instruction instruction, [NotNullWhen(true)] out uint? value)
        {
            value = instruction.OpCode switch
            {
                OpCode.Stloc => (uint)instruction.Argument!,
                OpCode.Stloc_S => (byte)instruction.Argument!,
                OpCode.Stloc_0 => 0,
                OpCode.Stloc_1 => 1,
                OpCode.Stloc_2 => 2,
                OpCode.Stloc_3 => 3,
                _ => null
            };

            return value is not null;
        }
    }

    bool BuildReturnExpr(Instruction instruction)
    {
        if (instruction.OpCode is not OpCode.Ret)
            return false;

        var returnExpr = ReturnType == typeof(void) ? null : Expressions.Pop();

        if (instruction.IsLast)
        {
            Expressions.Push(Expression.Label(ReturnTarget, returnExpr));
        }
        else
        {
            Expressions.Push(Expression.Return(ReturnTarget, returnExpr));
        }

        return true;
    }

    bool BuildConstantExpr(Instruction instruction)
    {
        if (!IsConstantExpression(instruction, out var value))
            return false;

        Expressions.Push(Expression.Constant(value));
        return true;

        static bool IsConstantExpression(Instruction instruction, [NotNullWhen(true)] out object? value)
        {
            value = instruction.OpCode switch
            {
                OpCode.Ldc_I4
                or OpCode.Ldc_I8
                or OpCode.Ldc_R4
                or OpCode.Ldc_R8 => instruction.Argument,
                OpCode.Ldc_I4_S => (int)(sbyte)instruction.Argument!,
                OpCode.Ldc_I4_M1 => -1,
                OpCode.Ldc_I4_0 => 0,
                OpCode.Ldc_I4_1 => 1,
                OpCode.Ldc_I4_2 => 2,
                OpCode.Ldc_I4_3 => 3,
                OpCode.Ldc_I4_4 => 4,
                OpCode.Ldc_I4_5 => 5,
                OpCode.Ldc_I4_6 => 6,
                OpCode.Ldc_I4_7 => 7,
                OpCode.Ldc_I4_8 => 8,
                OpCode.Ldstr => ((MetadataToken?)instruction?.Argument)?.Resolve() as string,
                _ => null
            };

            return value is not null;
        }
    }

    bool BuildArgumentExpr(Instruction instruction)
    {
        if (IsLoadArgumentExpression(instruction, out uint? loadIndex))
        {
            Expressions.Push(Arguments[(int)loadIndex].expr);
            return true;
        }

        if (IsStoreArgumentExpression(instruction, out uint? storeIndex))
        {
            Expressions.Push(Expression.Assign(Arguments[(int)storeIndex].expr, Expressions.Pop()));
            return true;
        }

        return false;

        static bool IsLoadArgumentExpression(Instruction instruction, [NotNullWhen(true)] out uint? value)
        {
            value = instruction.OpCode switch
            {
                OpCode.Ldarg => (uint)instruction.Argument!,
                OpCode.Ldarg_S => (byte)instruction.Argument!,
                OpCode.Ldarg_0 => 0,
                OpCode.Ldarg_1 => 1,
                OpCode.Ldarg_2 => 2,
                OpCode.Ldarg_3 => 3,
                OpCode.Ldarga => (uint)instruction.Argument!,
                OpCode.Ldarga_S => (byte)instruction.Argument!,
                _ => null
            };

            return value is not null;
        }

        static bool IsStoreArgumentExpression(Instruction instruction, [NotNullWhen(true)] out uint? value)
        {
            value = instruction.OpCode switch
            {
                OpCode.Starg => (uint)instruction.Argument!,
                OpCode.Starg_S => (uint)(sbyte)instruction.Argument!,
                _ => null
            };

            return value is not null;
        }
    }

    bool BuildNewObjExpr(Instruction instruction)
    {
        if (instruction.OpCode is not OpCode.Newobj)
            return false;

        var metadataToken = (MetadataToken)instruction.Argument!;
        var ctor = (ConstructorInfo)(metadataToken.Resolve() ?? throw new Exception());

        var args = PopArgs(ctor);

        Expressions.Push(Expression.New(ctor, args));

        return true;
    }

    bool BuildCallExpr(Instruction instruction)
    {
        if (instruction.OpCode is not (OpCode.Call or OpCode.Callvirt))
            return false;

        var metadataToken = (MetadataToken)instruction.Argument!;
        var method = (MethodBase)(metadataToken.Resolve() ?? throw new Exception());

        var args = PopArgs(method);

        var instance = method.IsStatic ? null : Expressions.Pop();

        if (method is MethodInfo methodInfo)
        {
            Expressions.Push(Expression.Call(instance, methodInfo, args));
        }
        else if (method is ConstructorInfo constructor)
        {
            var expr = new ConstructorCallExpression(constructor, args);
            if (instance is not null)
            {
                Expressions.Push(Expression.Assign(instance, expr));
            }
            else
            {
                Expressions.Push(expr);
            }
        }

        return true;
    }

    Expression[] PopArgs(MethodBase method)
    {
        var args = new Expression[method.GetParameters().Length];

        for (int i = args.Length - 1; i >= 0; i--)
        {
            args[i] = Expressions.Pop();
        }

        return args;
    }

    bool BuildLoadIndirect(Instruction instruction)
    {
        if (instruction.OpCode is not OpCode.Ldind_R4)
            return false;

        var refExpr = Expressions.Pop();
        Expressions.Push(new DereferenceExpression(refExpr));

        return true;
    }

    bool BuildStoreIndirect(Instruction instruction)
    {
        switch (instruction.OpCode)
        {
            case OpCode.Stind_I:
            case OpCode.Stind_I1:
            case OpCode.Stind_I2:
            case OpCode.Stind_I4:
            case OpCode.Stind_I8:
            case OpCode.Stind_R4:
            case OpCode.Stind_R8:
            case OpCode.Stind_Ref:
            case OpCode.Stobj:
                var value = Expressions.Pop();
                var reference = Expressions.Pop();

                Expressions.Push(new ReferenceAssignmentExpression(reference, value));
                return true;
            default:
                return false;
        }
    }
}