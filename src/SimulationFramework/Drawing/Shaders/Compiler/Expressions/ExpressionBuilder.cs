using Microsoft.VisualBasic;
using SimulationFramework.Drawing.Shaders.Compiler.ControlFlow;
using SimulationFramework.Drawing.Shaders.Compiler.Disassembler;
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
    public MethodDisassembly Disassembly { get; set; }
    public Type ReturnType { get; set; }
    public ExpressionStack Expressions { get; set; }

    private readonly CompilerContext context;
    public ControlFlowGraph controlFlowGraph;
    private readonly ShaderVariable[] parameters;
    private readonly ShaderVariable[] locals;

    public ExpressionBuilder(CompilerContext context, ControlFlowGraph controlFlowGraph, MethodDisassembly disassembly, ShaderVariable[] parameters, ShaderVariable[] locals)
    {
        this.context = context;
        this.controlFlowGraph = controlFlowGraph;
        this.Disassembly = disassembly;
        this.parameters = parameters;
        this.locals = locals;
    }

    public BlockExpression BuildExpressions()
    {
        Expressions = new();
        ReturnType = Disassembly.Method is MethodInfo method ? method.ReturnType : ((ConstructorInfo)Disassembly.Method).DeclaringType!;

        var exprs = BuildNodeChain(controlFlowGraph.EntryNode, controlFlowGraph.ExitNode);

        ShaderExpression result = SimplifyExpression(exprs);
        if (result is BlockExpression resultBlock)
        {
            return resultBlock;
        }
        else
        {
            return new BlockExpression(new[] { result });
        }
    }

    ShaderExpression? BuildBasicBlockExpression(BasicBlockNode basicBlock, bool clearStack = true)
    {
        if (basicBlock.Instructions.Count is 0)
            return null;

        if (clearStack)
            Expressions.Clear();

        foreach (var instruction in basicBlock.Instructions)
        {
            BuildInstruction(instruction);
        }

        return CreateBlockExpressionIfNeeded(Expressions.Reverse());
    }

    private void BuildInstruction(Instruction instruction)
    {
        switch (instruction.OpCode)
        {
            case OpCode.Nop or OpCode.Pop or OpCode.Ldobj:
                return;
            case OpCode.Dup:
                BuildDup(instruction);
                break;
            case OpCode.Initobj:
                BuildInitObj(instruction);
                break;
            case OpCode.Ret:
                BuildReturnExpr(instruction);
                break;
            case OpCode.Add:
            case OpCode.Sub:
            case OpCode.Mul:
            case OpCode.Div:
            case OpCode.And:
            case OpCode.Or:
            case OpCode.Shr:
            case OpCode.Shr_Un:
            case OpCode.Shl:
            case OpCode.Ceq:
            case OpCode.Clt:
            case OpCode.Cgt:
            case OpCode.Clt_Un:
            case OpCode.Cgt_Un:
            case OpCode.Rem:
            case OpCode.Rem_Un:
                BuildBinaryExpr(instruction);
                break;
            case OpCode.Ldfld or OpCode.Ldflda or OpCode.Stfld or OpCode.Ldsfld:
                BuildFieldExpr(instruction);
                break;
            case OpCode.Not:
            case OpCode.Neg:
            case OpCode.Conv_I:
            case OpCode.Conv_I1:
            case OpCode.Conv_I2:
            case OpCode.Conv_I4:
            case OpCode.Conv_I8:
            case OpCode.Conv_U:
            case OpCode.Conv_U1:
            case OpCode.Conv_U2:
            case OpCode.Conv_U4:
            case OpCode.Conv_U8:
            case OpCode.Conv_R4:
            case OpCode.Conv_R8:
            case OpCode.Conv_R_Un:
                BuildUnaryExpr(instruction);
                break;
            case OpCode.Ldloc:
            case OpCode.Ldloc_S:
            case OpCode.Ldloc_0:
            case OpCode.Ldloc_1:
            case OpCode.Ldloc_2:
            case OpCode.Ldloc_3:
            case OpCode.Ldloca:
            case OpCode.Ldloca_S:
                BuildLoadLocalExpr(instruction);
                break;
            case OpCode.Stloc:
            case OpCode.Stloc_S:
            case OpCode.Stloc_0:
            case OpCode.Stloc_1:
            case OpCode.Stloc_2:
            case OpCode.Stloc_3:
                BuildStoreLocalExpr(instruction);
                break;
            case OpCode.Ldc_I4:
            case OpCode.Ldc_I8:
            case OpCode.Ldc_R4:
            case OpCode.Ldc_R8:
            case OpCode.Ldc_I4_S:
            case OpCode.Ldc_I4_M1:
            case OpCode.Ldc_I4_0:
            case OpCode.Ldc_I4_1:
            case OpCode.Ldc_I4_2:
            case OpCode.Ldc_I4_3:
            case OpCode.Ldc_I4_4:
            case OpCode.Ldc_I4_5:
            case OpCode.Ldc_I4_6:
            case OpCode.Ldc_I4_7:
            case OpCode.Ldc_I4_8:
            case OpCode.Ldstr:
                BuildConstantExpr(instruction);
                break;
            case OpCode.Ldarg:
            case OpCode.Ldarg_S:
            case OpCode.Ldarg_0:
            case OpCode.Ldarg_1:
            case OpCode.Ldarg_2:
            case OpCode.Ldarg_3:
            case OpCode.Ldarga:
            case OpCode.Ldarga_S:
                BuildLoadArgExpr(instruction);
                break;
            case OpCode.Starg:
            case OpCode.Starg_S:
                BuildStoreArgExpr(instruction);
                break;
            case OpCode.Newobj:
                BuildNewObjExpr(instruction);
                break;
            case OpCode.Stind_I:
            case OpCode.Stind_I1:
            case OpCode.Stind_I2:
            case OpCode.Stind_I4:
            case OpCode.Stind_I8:
            case OpCode.Stind_R4:
            case OpCode.Stind_R8:
            case OpCode.Stind_Ref:
            case OpCode.Stobj:
                BuildStoreIndirect(instruction);
                break;
            case OpCode.Ldind_I:
            case OpCode.Ldind_I1:
            case OpCode.Ldind_I2:
            case OpCode.Ldind_I4:
            case OpCode.Ldind_I8:
            case OpCode.Ldind_R4:
            case OpCode.Ldind_R8:
            case OpCode.Ldind_Ref:
            // case OpCode.Ldobj:
                BuildLoadIndirect(instruction);
                break;
            case OpCode.Bne_Un or OpCode.Bne_Un_S:
            case OpCode.Beq or OpCode.Beq_S:
            case OpCode.Blt or OpCode.Blt_S or OpCode.Blt_Un or OpCode.Blt_Un_S:
            case OpCode.Ble or OpCode.Ble_S or OpCode.Ble_Un or OpCode.Ble_Un_S:
            case OpCode.Bgt or OpCode.Bgt_S or OpCode.Bgt_Un or OpCode.Bgt_Un_S:
            case OpCode.Bge or OpCode.Bge_S or OpCode.Bge_Un or OpCode.Bge_Un_S:
            case OpCode.Brfalse or OpCode.Brfalse_S:
            case OpCode.Br or OpCode.Br_S or OpCode.Brtrue or OpCode.Brtrue_S:
                BuildBranch(instruction);
                break;
            case OpCode.Call or OpCode.Callvirt:
                BuildCallExpr(instruction);
                break;
            case OpCode.Ldlen:
                BuildLoadLength(instruction);
                break;
            case OpCode.Ldelem:
            case OpCode.Ldelema:
            case OpCode.Ldelem_I:
            case OpCode.Ldelem_I1:
            case OpCode.Ldelem_I2:
            case OpCode.Ldelem_I4:
            case OpCode.Ldelem_I8:
            case OpCode.Ldelem_R4:
            case OpCode.Ldelem_R8:
            case OpCode.Ldelem_U1:
            case OpCode.Ldelem_U2:
            case OpCode.Ldelem_U4:
                BuildLoadElement(instruction);
                break;
            default:
                throw new NotSupportedException("Unsupported instruction '" + instruction.OpCode + "'.");
        }
    }

    private void BuildLoadElement(Instruction instruction)
    {
        var index = Expressions.Pop();
        var array = Expressions.Pop();

        var method = typeof(ShaderIntrinsics).GetMethod(nameof(ShaderIntrinsics.BufferLoad))!;
        Expressions.Push(new ShaderIntrinsicCall(method, ((ShaderArrayType?)array.ExpressionType)!.ElementType, [array, index]));
    }

    private void BuildLoadLength(Instruction instruction)
    {
        var arg = Expressions.Pop();

        var method = typeof(ShaderIntrinsics).GetMethod(nameof(ShaderIntrinsics.BufferLength))!;
        Expressions.Push(new ShaderIntrinsicCall(method, context.CompileType(method.ReturnType), [arg]));
    }

    private void BuildReturnExpr(Instruction instruction)
    {
        if (Disassembly.IsConstructor)
        {
            Expressions.Push(new ReturnExpression(new ShaderVariableExpression(parameters[0])));
            return;
        }

        Expressions.Push(new ReturnExpression(ReturnType == typeof(void) ? null : Expressions.Pop()));
    }

    private void BuildInitObj(Instruction instruction)
    {
        var type = ((MetadataToken)instruction.Argument).Resolve() as Type ?? throw new();
        var left = Expressions.Pop();
        var right = new DefaultExpression(context.CompileType(type));
        Expressions.Push(new BinaryExpression(BinaryOperation.Assignment, left, right));
    }

    private void BuildBranch(Instruction instruction)
    {
        ShaderExpression left, right;
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
                Expressions.Push(new UnaryExpression(UnaryOperation.Not, expr, null));
                break;
            case OpCode.Br or OpCode.Br_S or OpCode.Brtrue or OpCode.Brtrue_S:
                break; // do nothing for these
            default:
                throw new UnreachableException();
        }
    }

    ShaderExpression? BuildNode(ControlFlowNode node, bool clearStack = true)
    {
        if (node is ControlFlowGraph graph)
        {
            if (graph.SubgraphKind is SubgraphKind.Conditional)
            {
                var conditionNode = graph.EntryNode;
                var trueNode = conditionNode.Successors.First();
                var falseNode = conditionNode.Successors.Last();

                var beginExpr = BuildNode(graph.EntryNode, clearStack);

                Debug.Assert(beginExpr is not null);

                beginExpr = ExtractConditionExpression(SimplifyExpression(beginExpr), out var condition);

                Debug.Assert(condition is not null);

                var trueExpr = BuildNodeChain(trueNode, graph.ExitNode);

                ShaderExpression? falseExpr = falseNode is null ? null : BuildNodeChain(falseNode, graph.ExitNode);

                bool isTernary = false;
                ShaderExpression condExpr;
                if (trueExpr is not BlockExpression && falseExpr is not BlockExpression)
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

                return CreateBlockExpressionIfNeeded([beginExpr, condExpr /*isTernary?null:blockExpr*/]);
            }
            else if (graph.SubgraphKind is SubgraphKind.Loop)
            {
                var headNode = graph.EntryNode;
                var tailNode = headNode.Predecessors.Single();

                var header = BuildNode(headNode);

                header = ExtractConditionExpression(header, out var cond);
                header = SimplifyExpression(header, false);

                ShaderExpression condition = new ConditionalExpression(new UnaryExpression(UnaryOperation.Not, cond, null), new BreakExpression(), null);
                condition = SimplifyExpression(condition, false);

                var body = BuildNodeChain(headNode.Successors.Single(n => n != graph.ExitNode), tailNode);
                body = SimplifyExpression(body, false);

                return new LoopExpression(CreateBlockExpressionIfNeeded(new ShaderExpression[] { header, condition, body }));
            }
            else if (graph.SubgraphKind is SubgraphKind.Switch)
            {
                throw new("switches not added");
            }
            else
            {
                throw new UnreachableException();
            }
        }
        else if (node is BasicBlockNode basicBlock)
        {
            return BuildBasicBlockExpression(basicBlock, clearStack);
        }
        else if (node is DummyNode)
        {
            return null;
        }
        else
        {
            throw new Exception();
        }
    }

    private static ShaderExpression CreateBlockExpressionIfNeeded(IEnumerable<ShaderExpression> expressions)
    {
        if (expressions.Count() is 1)
            return expressions.Single()!;

        return new BlockExpression(expressions
            .Where(x => x is not null)
            .ToArray()
            );
    }

    // trims the last expr from a block used for grabbing head statements for ifs and loops
    private ShaderExpression? ExtractConditionExpression(ShaderExpression expr, out ShaderExpression lastExpr)
    {
        if (expr is not BlockExpression blockExpr)
        {
            lastExpr = expr;
            return null;
        }

        lastExpr = blockExpr.Expressions.Last();
        return CreateBlockExpressionIfNeeded(blockExpr.Expressions.SkipLast(1));
    }

    ShaderExpression BuildNodeChain(ControlFlowNode start, ControlFlowNode end)
    {
        if (start == end)
        {
            return BuildNode(start);
        }

        List<ShaderExpression> exprs = new();

        var node = start;
        while (node != end)
        {
            exprs.Add(BuildNode(node));
            node = node.Successors.Single();
        }

        exprs.Add(BuildNode(node));

        return CreateBlockExpressionIfNeeded(exprs.Where(ex => ex is not null).Cast<ShaderExpression>());
    }

    [return: NotNullIfNotNull(nameof(expression))]
    ShaderExpression? SimplifyExpression(ShaderExpression? expression, bool requireBlock = true)
    {
        if (expression is null)
        {
            return null;
        }
        if (expression is ConditionalExpression conditional)
        {
            if (conditional.Failure is not null && conditional.Success is null && conditional.Condition is UnaryExpression unary && unary.Operation is UnaryOperation.Not)
            {
                return new ConditionalExpression(
                    SimplifyExpression(unary.Operand), 
                    SimplifyExpression(conditional.Failure), 
                    SimplifyExpression(conditional.Success)
                    );
            }


            return new ConditionalExpression(
                SimplifyExpression(conditional.Condition, true),
                SimplifyExpression(conditional.Success, true),
                SimplifyExpression(conditional.Failure, true)
                );
        }
        if (expression is LoopExpression loop)
        {
            return new LoopExpression(SimplifyExpression(loop.Body));
        }
        if (expression is BlockExpression block)
        {
        }

        if (expression is not BlockExpression blockExpr)
            return expression;

        IEnumerable<ShaderExpression> expressions = Enumerable.Empty<ShaderExpression>();

        foreach (var expr in blockExpr.Expressions.Select(e => SimplifyExpression(e)))
        {
            expressions = expr is BlockExpression b ? 
                expressions.Concat(b.Expressions) :
                expressions = expressions.Append(expr);
        }

        if (!requireBlock && expressions.Count() == 1)
            return expressions.Single();

        return CreateBlockExpressionIfNeeded(expressions);
    }

    ShaderExpression ForceVoidType(ShaderExpression expression)
    {
        return expression;
    }

    void BuildDup(Instruction instruction)
    {
        var expr = Expressions.Pop();
        Expressions.Push(expr);
        Expressions.Push(expr);
    }

    void BuildBinaryExpr(Instruction instruction)
    {
        var exprType = instruction.OpCode switch
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
            OpCode.Clt or OpCode.Clt_Un => BinaryOperation.LessThan,
            OpCode.Cgt or OpCode.Cgt_Un => BinaryOperation.GreaterThan,
            OpCode.Rem => BinaryOperation.Modulus,
            OpCode.Rem_Un => BinaryOperation.Modulus,
            _ => throw new UnreachableException(),
        }; ;

        var right = Expressions.Pop();
        var left = Expressions.Pop();

        // special case: the compiler will often emit == 0 on a comparison to invert it (ie (x == 10) == 0 is the same as x != 10)
        // in this case instead of emitting the second x == 0 we just emit !x.
        if (left is BinaryExpression bin && BinaryExpression.IsBooleanOperator(bin.Operation) && right is ConstantExpression constExpr && constExpr.Value is 0)
        {
            Expressions.Push(new UnaryExpression(UnaryOperation.Not, left, null));
            return;
        }

        
        
        Expressions.Push(CreateBinaryExpression(exprType, left, right));
    }

    private BinaryExpression CreateBinaryExpression(BinaryOperation operation, ShaderExpression left, ShaderExpression right)
    {
        if (left.ExpressionType == ShaderType.Bool)
        {
            if (right is ConstantExpression constExpr2 && constExpr2.Value is 0 or 1)
            {
                right = new ConstantExpression(ShaderType.Bool, (int)constExpr2.Value == 1);
            }
        }

        return new(operation, left, right);
    }

    private void BuildFieldExpr(Instruction instruction)
    {
        var metadataToken = (MetadataToken)instruction.Argument!;
        var fieldInfo = (FieldInfo)metadataToken.Resolve()!;

        if (fieldInfo.DeclaringType == context.ShaderType)
        {
            // uniform!
            Expressions.Pop(); // pop the 'self'
            Expressions.Push(new ShaderVariableExpression(context.Uniforms[fieldInfo]));
            return;
        }

        if (instruction.OpCode is OpCode.Stfld)
        {
            var value = Expressions.Pop();
            var obj = Expressions.Pop();

            Expressions.Push(CreateBinaryExpression(BinaryOperation.Assignment, new MemberAccess(obj, fieldInfo, context.CompileType(fieldInfo.FieldType)), value));
        }
        else if (instruction.OpCode is OpCode.Ldsfld)
        {
            if (!fieldInfo.Attributes.HasFlag(FieldAttributes.InitOnly))
                throw new Exception("Cannot access a non-readonly static field!");

            Expressions.Push(new ConstantExpression(context.CompileType(fieldInfo.FieldType), fieldInfo.GetValue(null)!));
        }
        else // OpCode is Ldfld or Ldflda
        {
            var obj = Expressions.Pop();

            Expressions.Push(new MemberAccess(obj, fieldInfo, context.CompileType(fieldInfo.FieldType)));
        }
    }

    void BuildUnaryExpr(Instruction instruction)
    {
        var castType = instruction.OpCode switch
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

        var operation = instruction.OpCode switch
        {
            OpCode.Neg => UnaryOperation.Negate,
            OpCode.Not => UnaryOperation.Not,
            _ => castType != null ? UnaryOperation.Cast : throw new UnreachableException(),
        };

        var operand = Expressions.Pop();

        ShaderType? shaderCastType = null;
        if (castType != null)
        {
            shaderCastType = context.CompileType(castType);
        }

        Expressions.Push(new UnaryExpression(operation, operand, shaderCastType));
    }

    private void BuildLoadLocalExpr(Instruction instruction)
    {
        uint loadIndex = instruction.OpCode switch
        {
            OpCode.Ldloc => (uint)instruction.Argument!,
            OpCode.Ldloc_S => (byte)instruction.Argument!,
            OpCode.Ldloc_0 => 0,
            OpCode.Ldloc_1 => 1,
            OpCode.Ldloc_2 => 2,
            OpCode.Ldloc_3 => 3,
            OpCode.Ldloca => (uint)instruction.Argument!,
            OpCode.Ldloca_S => (byte)instruction.Argument!,
            _ => throw new UnreachableException()
        };

        Expressions.Push(new ShaderVariableExpression(this.locals[(int)loadIndex]));
    }

    private void BuildStoreLocalExpr(Instruction instruction)
    {
        uint storeIndex = instruction.OpCode switch
        {
            OpCode.Stloc => (uint)instruction.Argument!,
            OpCode.Stloc_S => (byte)instruction.Argument!,
            OpCode.Stloc_0 => 0,
            OpCode.Stloc_1 => 1,
            OpCode.Stloc_2 => 2,
            OpCode.Stloc_3 => 3,
            _ => throw new UnreachableException()
        };

        var expr = Expressions.Pop();
        var local = locals[(int)storeIndex];
        Expressions.Push(CreateBinaryExpression(BinaryOperation.Assignment, new ShaderVariableExpression(local), expr), false);
    }

    void BuildConstantExpr(Instruction instruction)
    {
        object value = instruction.OpCode switch
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
            OpCode.Ldstr => (((MetadataToken?)instruction?.Argument)?.Resolve() as string)!,
            _ => throw new UnreachableException(),
        };

        Expressions.Push(new ConstantExpression(context.CompileType(value.GetType()), value));
    }

    private void BuildLoadArgExpr(Instruction instruction)
    {
        uint loadIndex = instruction.OpCode switch
        {
            OpCode.Ldarg => (uint)instruction.Argument!,
            OpCode.Ldarg_S => (byte)instruction.Argument!,
            OpCode.Ldarg_0 => 0,
            OpCode.Ldarg_1 => 1,
            OpCode.Ldarg_2 => 2,
            OpCode.Ldarg_3 => 3,
            OpCode.Ldarga => (uint)instruction.Argument!,
            OpCode.Ldarga_S => (byte)instruction.Argument!,
            _ => throw new UnreachableException()
        };

        Expressions.Push(new ShaderVariableExpression(this.parameters[(int)loadIndex]));
    }

    private void BuildStoreArgExpr(Instruction instruction)
    {
        uint storeIndex = instruction.OpCode switch
        {
            OpCode.Starg => (uint)instruction.Argument!,
            OpCode.Starg_S => (byte)instruction.Argument!,
            _ => throw new UnreachableException(),
        };

        Expressions.Push(CreateBinaryExpression(BinaryOperation.Assignment, new ShaderVariableExpression(parameters[(int)storeIndex]), Expressions.Pop()), false);
    }

    void BuildNewObjExpr(Instruction instruction)
    {
        var metadataToken = (MetadataToken)instruction.Argument!;
        var ctor = (ConstructorInfo)(metadataToken.Resolve() ?? throw new Exception());

        var args = PopArgs(ctor);

        var intrinsic = context.ResolveMethodToIntrinsic(ctor);
        if (intrinsic != null)
        {
            Expressions.Push(new ShaderIntrinsicCall(intrinsic, context.CompileType(intrinsic.ReturnType), args));
        }
        else
        {
            var shaderMethod = context.EnqueueMethod(ctor);
            Expressions.Push(new ShaderMethodCall(shaderMethod, args));
        }
    }

    void BuildCallExpr(Instruction instruction)
    {
        var metadataToken = (MetadataToken)instruction.Argument!;
        var method = (MethodBase)(metadataToken.Resolve() ?? throw new Exception());

        var args = PopArgs(method);

        var instance = method.IsStatic ? null : Expressions.Pop();
        if (method is ConstructorInfo || method.DeclaringType == context.ShaderType)
        {
        }
        else
        {
            if (instance != null)
            {
                args = [instance, .. args];
            }
        }

        var intrinsic = context.ResolveMethodToIntrinsic(method);
        if (intrinsic != null)
        {
            Expressions.Push(new ShaderIntrinsicCall(intrinsic, context.CompileType(intrinsic.ReturnType), args));
        }
        else
        {
            var shaderMethod = context.EnqueueMethod(method);
            Expressions.Push(new ShaderMethodCall(shaderMethod, args));
        }

        if (method is ConstructorInfo)
        {
            var value = Expressions.Pop();
            Expressions.Push(CreateBinaryExpression(BinaryOperation.Assignment, instance, value));
        }
    }

    ShaderExpression[] PopArgs(MethodBase method)
    {
        var args = new ShaderExpression[method.GetParameters().Length];

        for (int i = args.Length - 1; i >= 0; i--)
        {
            args[i] = Expressions.Pop();
        }

        return args;
    }

    private void BuildLoadIndirect(Instruction instruction)
    {
        // nop
    }

    private void BuildStoreIndirect(Instruction instruction)
    {
        var value = Expressions.Pop();
        var reference = Expressions.Pop();

        Expressions.Push(CreateBinaryExpression(BinaryOperation.Assignment, reference, value), false);
    }
}