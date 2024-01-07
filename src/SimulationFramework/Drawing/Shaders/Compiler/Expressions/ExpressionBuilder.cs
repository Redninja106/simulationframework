﻿using SimulationFramework.Drawing.Shaders.Compiler.ControlFlow;
using SimulationFramework.Drawing.Shaders.Compiler.ILDisassembler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;

internal class ExpressionBuilder
{
    private MethodDisassembly Disassembly { get; set; }
    private InstructionStream InstructionStream { get; set; }
    private LocalVariableExpression[] Locals { get; set; }
    private MethodParameter[] Arguments { get; set; }
    // private LabelTarget ReturnTarget { get; set; }
    private Type ReturnType { get; set; }
    private bool IsConstructor { get; set; }
    private ExpressionStack Expressions { get; set; }

    private ExpressionBuilder(MethodDisassembly disassembly, MethodParameter[] arguments)
    {
        Disassembly = disassembly;
        this.Arguments = arguments;
    }

    public static BlockExpression BuildExpressions(ShaderMethod shaderMethod)
    {
        var graph = shaderMethod.ControlFlowGraph;
        var disassembly = shaderMethod.Disassembly;

        ExpressionBuilder builder = new(disassembly, shaderMethod.Parameters.ToArray());

        builder.Expressions = new();
        builder.IsConstructor = disassembly.Method is ConstructorInfo;

        builder.InstructionStream = new InstructionStream(disassembly);

        builder.Locals = GetLocals(disassembly);

        builder.ReturnType = disassembly.Method is MethodInfo method ? method.ReturnType : typeof(void);
        // builder.ReturnTarget = Expression.Label(builder.ReturnType, "return");

        var exprs = builder.BuildNodeChain(graph.EntryNode, graph.ExitNode);

        Expression result = builder.SimplifyExpression(exprs);
        if (result is BlockExpression resultBlock)
        {
            return resultBlock;
        }
        else
        {
            return new BlockExpression(new[] { result });
        }
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
            BuildInitObj,
            BuildDup,
            BuildBranch,

            i => i.OpCode is OpCode.Pop or OpCode.Ldobj,

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

        return CreateBlockExpressionIfNeeded(Expressions.Reverse());
    }

    private bool BuildInitObj(Instruction instruction)
    {
        if (instruction.OpCode is not OpCode.Initobj)
            return false;

        var left = Expressions.Pop();
        var right = new DefaultExpression(((MetadataToken)instruction.Argument).Resolve() as Type ?? throw new());
        Expressions.Push(new BinaryExpression(BinaryOperation.Assignment, left, right));

        return true;
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
                Expressions.Push(new BinaryExpression(BinaryOperation.NotEqual, left, right));
                break;
            case OpCode.Beq or OpCode.Beq_S:
                right = Expressions.Pop();
                left = Expressions.Pop();
                Expressions.Push(new BinaryExpression(BinaryOperation.Equal, left, right));
                break;
            case OpCode.Blt or OpCode.Blt_S or OpCode.Blt_Un or OpCode.Blt_Un_S:
                // emit less than comparison
                right = Expressions.Pop();
                left = Expressions.Pop();
                Expressions.Push(new BinaryExpression(BinaryOperation.LessThan, left, right));
                break;
            case OpCode.Ble or OpCode.Ble_S or OpCode.Ble_Un or OpCode.Ble_Un_S:
                // emit less than or equal comparison
                right = Expressions.Pop();
                left = Expressions.Pop();
                Expressions.Push(new BinaryExpression(BinaryOperation.LessThanEqual, left, right));
                break;
            case OpCode.Bgt or OpCode.Bgt_S or OpCode.Bgt_Un or OpCode.Bgt_Un_S:
                // emit greater than comparison
                right = Expressions.Pop();
                left = Expressions.Pop();
                Expressions.Push(new BinaryExpression(BinaryOperation.GreaterThan, left, right));
                break;
            case OpCode.Bge or OpCode.Bge_S or OpCode.Bge_Un or OpCode.Bge_Un_S:
                // emit greater than or equal comparison
                right = Expressions.Pop();
                left = Expressions.Pop();
                Expressions.Push(new BinaryExpression(BinaryOperation.GreaterThanEqual, left, right));
                break;
            case OpCode.Brfalse or OpCode.Brfalse_S:
                // invert condition
                var expr = Expressions.Pop();
                Expressions.Push(new UnaryExpression(UnaryOperation.Not, expr));
                break;
            case OpCode.Br or OpCode.Br_S or OpCode.Brtrue or OpCode.Brtrue_S:
                break; // do nothing for these
            default:
                return false; // not a branch instruction
        }
        
        return true;
    }

    Expression BuildNode(ControlFlowNode node, bool clearStack = true)
    {
        if (node is ConditionalNode conditional)
        {
            var beginExpr = BuildNode(conditional.Subgraph.EntryNode, clearStack);

            Debug.Assert(beginExpr is not null);

            beginExpr = ExtractConditionExpression(SimplifyExpression(beginExpr), out var condition);

            Debug.Assert(condition is not null);

            var trueExpr = BuildNodeChain(conditional.TrueBranch, conditional.Subgraph.ExitNode);

            Expression? falseExpr = conditional.FalseBranch is null ? null : BuildNodeChain(conditional.FalseBranch, conditional.Subgraph.ExitNode);

            bool isTernary = false;
            Expression condExpr;
            if (trueExpr is not BlockExpression && /* trueExpr.Type != typeof(void) && */ falseExpr is not BlockExpression)
            {
                // might be ternary
                isTernary = true;
                condExpr = new ConditionalExpression(condition, trueExpr, falseExpr);
                Expressions.Clear();
                Expressions.Push(condExpr);
            }
            else
            {
                condExpr = new ConditionalExpression(condition, trueExpr, falseExpr);
            }

            var endExpr = BuildNode(conditional.Subgraph.ExitNode, !isTernary);

            return CreateBlockExpressionIfNeeded(new Expression[] { beginExpr, isTernary ? null : condExpr, endExpr }.Where(ex => ex is not null).Cast<Expression>());
        }
        else if (node is LoopNode loop)
        {
            var header = BuildNode(loop.Header);

            header = ExtractConditionExpression(header, out var cond);

            var condition = new ConditionalExpression(cond, null, new BreakExpression());

            var body = BuildNodeChain(loop.Tail, loop.Header);

            return new LoopExpression(CreateBlockExpressionIfNeeded(new Expression[] { header, condition, body }));
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

    private static Expression CreateBlockExpressionIfNeeded(IEnumerable<Expression> expressions)
    {
        // if (expressions.Count() is 1)
        //     return expressions.Single()!;

        return new BlockExpression(expressions
            .Where(x => x is not null)
            .ToArray()
            );
    }

    // trims the last expr from a block used for grabbing head statements for ifs and loops
    private Expression ExtractConditionExpression(Expression expr, out Expression lastExpr)
    {
        if (expr is not BlockExpression blockExpr)
        {
            lastExpr = expr;
            return null;
        }

        lastExpr = blockExpr.Expressions.Last();
        return CreateBlockExpressionIfNeeded(blockExpr.Expressions.SkipLast(1));
    }

    Expression BuildNodeChain(ControlFlowNode start, ControlFlowNode end)
    {
        Debug.Assert(start.Graph == end.Graph);

        var graph = start.Graph;
        List<Expression> exprs = new();

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
            return new ConditionalExpression(
                SimplifyExpression(conditionalExpr.Condition),
                ForceVoidType(SimplifyExpression(conditionalExpr.Success)),
                ForceVoidType(SimplifyExpression(conditionalExpr.Failure))
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

        return CreateBlockExpressionIfNeeded(expressions);
    }

    Expression ForceVoidType(Expression expression)
    {
        return expression;

        // if (expression.Type == typeof(void))
        //     return expression;
        // 
        // return Expression.Block(typeof(void), expression);
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

    static LocalVariableExpression[] GetLocals(MethodDisassembly disassembly)
    {
        var locals = disassembly.MethodBody.LocalVariables.Select(l => new LocalVariableExpression(new(l)));
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
        Expressions.Push(new BinaryExpression(exprType.Value, left, right));
        return true;

        static bool IsBinaryExpression(Instruction instruction, [NotNullWhen(true)] out BinaryOperation? operation)
        {
            operation = instruction.OpCode switch
            {
                OpCode.Add => BinaryOperation.Add,
                OpCode.Sub => BinaryOperation.Subtract,
                OpCode.Mul => BinaryOperation.Multiply,
                OpCode.Div or OpCode.Div_Un => BinaryOperation.Divide,
                OpCode.And => BinaryOperation.And,
                OpCode.Or => BinaryOperation.Or,
                OpCode.Shr => BinaryOperation.RightShift,
                OpCode.Shr_Un => BinaryOperation.RightShift,
                OpCode.Shl => BinaryOperation.LeftShift,
                OpCode.Ceq => BinaryOperation.Equal,
                OpCode.Clt => BinaryOperation.LessThan,
                OpCode.Cgt => BinaryOperation.GreaterThan,
                OpCode.Rem => BinaryOperation.Modulus,
                OpCode.Rem_Un => BinaryOperation.Modulus,

                _ => null
            };

            return operation is not null;
        }
    }

    // Expression ConvertExpressionType(Expression expression, Type type)
    // {
    //     if (expression.Type == type)
    //         return expression;
    // 
    //     // handle true/false
    //     if (type == typeof(bool) && expression is ConstantExpression constExpr)
    //     {
    //         if (constExpr.Value is 0)
    //             return Expression.Constant(false);
    // 
    //         if (constExpr.Value is 1)
    //             return Expression.Constant(true);
    //     }
    // 
    //     if (type == typeof(void))
    //     {
    //         return Expression.Block(type, expression);
    //     }
    // 
    //     return Expression.Convert(expression, type);
    // }

    bool BuildFieldExpr(Instruction instruction)
    {
        if (instruction.OpCode is not (OpCode.Ldfld or OpCode.Ldflda or OpCode.Stfld or OpCode.Ldsfld))
            return false;

        var metadataToken = (MetadataToken)instruction.Argument!;
        var fieldInfo = (FieldInfo)metadataToken.Resolve()!;

        if (instruction.OpCode is OpCode.Stfld)
        {
            var value = Expressions.Pop();
            var obj = Expressions.Pop();

            Expressions.Push(new BinaryExpression(BinaryOperation.Assignment, new MemberAccessExpression(obj, fieldInfo), value));
        }
        else if (instruction.OpCode is OpCode.Ldsfld)
        {
            if (!fieldInfo.Attributes.HasFlag(FieldAttributes.InitOnly))
                throw new Exception("Cannot access a non-readonly static field!");

            Expressions.Push(new ConstantExpression(fieldInfo.GetValue(null)!));
        }
        else // OpCode is Ldfld or Ldflda
        {
            var obj = Expressions.Pop();

            Expressions.Push(new MemberAccessExpression(obj, fieldInfo));
        }

        return true;
    }

    bool BuildUnaryExpr(Instruction instruction)
    {
        if (!IsUnaryExpression(instruction, out var operation, out var castType))
            return false;

        var operand = Expressions.Pop();

        Expressions.Push(new UnaryExpression(operation.Value, operand));

        return true;

        static bool IsUnaryExpression(Instruction instruction, [NotNullWhen(true)] out UnaryOperation? operation, out Type? castType)
        {
            operation = instruction.OpCode switch
            {
                OpCode.Neg => UnaryOperation.Negate,
                OpCode.Not => UnaryOperation.Not,
                _ => null
            };

            if (operation is not null)
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
                throw new NotImplementedException("TODO: implement castin");
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
            Expressions.Push(new BinaryExpression(BinaryOperation.Assignment, local, expr));

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
        
        Expressions.Push(new ReturnExpression(ReturnType == typeof(void) ? null : Expressions.Pop()));

        return true;
    }

    bool BuildConstantExpr(Instruction instruction)
    {
        if (!IsConstantExpression(instruction, out var value))
            return false;

        Expressions.Push(new ConstantExpression(value));
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
            Expressions.Push(new MethodParameterExpression(this.Arguments[(int)loadIndex]));
            return true;
        }

        if (IsStoreArgumentExpression(instruction, out uint? storeIndex))
        {
            Expressions.Push(new BinaryExpression(BinaryOperation.Assignment, new MethodParameterExpression(Arguments[(int)storeIndex]), Expressions.Pop()), false);
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

        Expressions.Push(new NewExpression(ctor, args));

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
            Expressions.Push(new CallExpression(instance, methodInfo, args), methodInfo.ReturnType != typeof(void));
        }
        else if (method is ConstructorInfo constructor)
        {
            var expr = new NewExpression(constructor, args);
            if (instance is not null)
            {
                Expressions.Push(new BinaryExpression(BinaryOperation.Assignment, instance, expr));
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

        // var refExpr = Expressions.Pop();
        // Expressions.Push(new DereferenceExpression(refExpr));

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

                Expressions.Push(new BinaryExpression(BinaryOperation.Assignment, reference, value), false);
                return true;
            default:
                return false;
        }
    }
}