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
    // public ExpressionStack Expressions { get; set; }

    public readonly CompilerContext context;
    public ControlFlowGraph controlFlowGraph;
    public readonly ShaderVariable[] parameters;
    public readonly ShaderVariable[] locals;

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
        // Expressions = new();
        ReturnType = Disassembly.Method is MethodInfo method ? method.ReturnType : ((ConstructorInfo)Disassembly.Method).DeclaringType!;

        var exprs = new ExpressionStack();
        BuildNodeChain(controlFlowGraph.EntryNode, controlFlowGraph.ExitNode, exprs);

        ShaderExpression result = SimplifyExpression(CreateBlockExpressionIfNeeded(exprs.GetExpressions()));
        if (result is BlockExpression resultBlock)
        {
            return resultBlock;
        }
        else
        {
            return new BlockExpression(new[] { result });
        }
    }

    void BuildBasicBlockExpression(BasicBlockNode basicBlock, ExpressionStack expressions)
    {
        if (basicBlock.Instructions.Count is 0)
            return;

        foreach (var instruction in basicBlock.Instructions)
        {
            BuildInstruction(instruction, expressions);
        }
    }

    void BuildNode(ControlFlowNode node, ExpressionStack expressions)
    {
        if (node is ControlFlowGraph graph)
        {
            if (graph.SubgraphKind is SubgraphKind.Conditional)
            {
                var conditionNode = graph.EntryNode;
                var trueNode = conditionNode.Successors.First();
                var falseNode = conditionNode.Successors.Last();

                BuildNode(graph.EntryNode, expressions);
                var condition = expressions.Pop();

                Debug.Assert(condition is not null);

                var trueExprs = new ExpressionStack();
                BuildNodeChain(trueNode, graph.ExitNode, trueExprs);

                ExpressionStack? falseExprs = null;
                if (falseNode != null)
                {
                    falseExprs = new ExpressionStack();
                    BuildNodeChain(falseNode, graph.ExitNode, falseExprs);
                }

                bool ternary = trueExprs.HasElementsOnStack;
                ShaderExpression trueExpr = CreateBlockExpressionIfNeeded(trueExprs.GetExpressions());
                ShaderExpression? falseExpr = null;
                if (falseExprs != null)
                {
                    falseExpr = CreateBlockExpressionIfNeeded(falseExprs.GetExpressions());
                }

                if (ternary)
                {
                    FixBoolConstants(ref trueExpr, ref falseExpr);
                }

                ConditionalExpression conditionalExpr = new ConditionalExpression(
                    ternary,
                    condition,
                    trueExpr,
                    falseExpr
                    );

                if (ternary)
                {
                    expressions.Push(conditionalExpr);
                }
                else 
                {
                    expressions.PushStatement(conditionalExpr);
                }
            }
            else if (graph.SubgraphKind is SubgraphKind.Loop)
            {
                var headNode = graph.EntryNode;
                // var tailNode = headNode.Predecessors.Single();

                BuildNode(headNode, expressions);

                var cond = expressions.Pop();
                ShaderExpression condition = new ConditionalExpression(false, new UnaryExpression(UnaryOperation.Not, cond, null), new BreakExpression(), null);
                
                ExpressionStack exprs = new();
                BuildNodeChain(headNode.Successors.Single(n => n != graph.ExitNode), graph.ExitNode, exprs);

                expressions.PushStatement(
                    new LoopExpression(
                        CreateBlockExpressionIfNeeded([condition, ..exprs.GetExpressions()])
                        )
                    );
            }
            else if (graph.SubgraphKind is SubgraphKind.Switch)
            {
                throw new NotImplementedException("switches not implemented");
            }
            else
            {
                throw new UnreachableException();
            }
        }
        else if (node is BasicBlockNode basicBlock)
        {
            BuildBasicBlockExpression(basicBlock, expressions);
        }
        else if (node is BreakNode)
        {
            expressions.PushStatement(new BreakExpression());
        }
        else if (node is ContinueNode)
        {
            expressions.PushStatement(new ContinueExpression());
        }
        else if (node is ReturnNode retNode)
        {
            ShaderExpression? returnValue = null;
            if (ReturnType != typeof(void))
            {
                if (retNode.isReturnVarBlock)
                {
                    returnValue = (expressions.GetExpressions().Last() as BinaryExpression)!.LeftOperand as ShaderVariableExpression;
                }
                else
                {
                    returnValue = expressions.Pop();
                }
            }

            expressions.PushStatement(new ReturnExpression(returnValue));
        }
        else if (node is DummyNode)
        {
            return;
        }
        else
        {
            throw new Exception("unknown node type!");
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

    void BuildNodeChain(ControlFlowNode start, ControlFlowNode end, ExpressionStack expressions)
    {
        if (start == end)
        {
            BuildNode(start, expressions);
            return;
        }

        var node = start;
        while (node != null && node != end)
        {
            BuildNode(node, expressions);
            node = node.Successors.SingleOrDefault();
        }

        if (node != null)
        {
            BuildNode(node, expressions);
        }
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
                    conditional.IsTernary,
                    SimplifyExpression(unary.Operand), 
                    SimplifyExpression(conditional.Failure), 
                    SimplifyExpression(conditional.Success)
                    );
            }


            return new ConditionalExpression(
                conditional.IsTernary,
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

    public void BuildInstruction(Instruction instruction, ExpressionStack expressions)
    {
        switch (instruction.OpCode)
        {
            case OpCode.Nop or OpCode.Ldobj:
                return;
            case OpCode.Pop:
                BuildPop(instruction, expressions);
                break;
            case OpCode.Dup:
                BuildDup(instruction, expressions);
                break;
            case OpCode.Initobj:
                BuildInitObj(instruction, expressions);
                break;
            case OpCode.Ret:
                BuildReturnExpr(instruction, expressions);
                break;
            case OpCode.Add:
            case OpCode.Sub:
            case OpCode.Xor:
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
                BuildBinaryExpr(instruction, expressions);
                break;
            case OpCode.Ldfld or OpCode.Ldflda or OpCode.Stfld or OpCode.Ldsfld:
                BuildFieldExpr(instruction, expressions);
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
                BuildUnaryExpr(instruction, expressions);
                break;
            case OpCode.Ldloc:
            case OpCode.Ldloc_S:
            case OpCode.Ldloc_0:
            case OpCode.Ldloc_1:
            case OpCode.Ldloc_2:
            case OpCode.Ldloc_3:
            case OpCode.Ldloca:
            case OpCode.Ldloca_S:
                BuildLoadLocalExpr(instruction, expressions);
                break;
            case OpCode.Stloc:
            case OpCode.Stloc_S:
            case OpCode.Stloc_0:
            case OpCode.Stloc_1:
            case OpCode.Stloc_2:
            case OpCode.Stloc_3:
                BuildStoreLocalExpr(instruction, expressions);
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
                BuildConstantExpr(instruction, expressions);
                break;
            case OpCode.Ldarg:
            case OpCode.Ldarg_S:
            case OpCode.Ldarg_0:
            case OpCode.Ldarg_1:
            case OpCode.Ldarg_2:
            case OpCode.Ldarg_3:
            case OpCode.Ldarga:
            case OpCode.Ldarga_S:
                BuildLoadArgExpr(instruction, expressions);
                break;
            case OpCode.Starg:
            case OpCode.Starg_S:
                BuildStoreArgExpr(instruction, expressions);
                break;
            case OpCode.Newobj:
                BuildNewObjExpr(instruction, expressions);
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
                BuildStoreIndirect(instruction, expressions);
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
                BuildLoadIndirect(instruction, expressions);
                break;
            case OpCode.Bne_Un or OpCode.Bne_Un_S:
            case OpCode.Beq or OpCode.Beq_S:
            case OpCode.Blt or OpCode.Blt_S or OpCode.Blt_Un or OpCode.Blt_Un_S:
            case OpCode.Ble or OpCode.Ble_S or OpCode.Ble_Un or OpCode.Ble_Un_S:
            case OpCode.Bgt or OpCode.Bgt_S or OpCode.Bgt_Un or OpCode.Bgt_Un_S:
            case OpCode.Bge or OpCode.Bge_S or OpCode.Bge_Un or OpCode.Bge_Un_S:
            case OpCode.Brfalse or OpCode.Brfalse_S:
            case OpCode.Br or OpCode.Br_S or OpCode.Brtrue or OpCode.Brtrue_S:
                BuildBranch(instruction, expressions);
                break;
            case OpCode.Call or OpCode.Callvirt:
                BuildCallExpr(instruction, expressions);
                break;
            case OpCode.Ldlen:
                BuildLoadLength(instruction, expressions);
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
                BuildLoadElement(instruction, expressions);
                break;
            default:
                throw new NotSupportedException("Unsupported instruction '" + instruction.OpCode + "'.");
        }
    }
    private void BuildPop(Instruction instruction, ExpressionStack expressions)
    {
        expressions.Pop();
    }

    private void BuildLoadElement(Instruction instruction, ExpressionStack expressions)
    {
        var index = expressions.Pop();
        var array = expressions.Pop();

        var method = typeof(ShaderIntrinsics).GetMethod(nameof(ShaderIntrinsics.BufferLoad), [typeof(object), typeof(int)])!;
        expressions.Push(new ShaderIntrinsicCall(method, ((ShaderArrayType?)array.ExpressionType)!.ElementType, [array, index]));
    }

    private void BuildLoadLength(Instruction instruction, ExpressionStack expressions)
    {
        var arg = expressions.Pop();

        var method = typeof(ShaderIntrinsics).GetMethod(nameof(ShaderIntrinsics.BufferLength), [typeof(object)])!;
        expressions.Push(new ShaderIntrinsicCall(method, context.CompileType(method.ReturnType), [arg]));
    }

    private void BuildReturnExpr(Instruction instruction, ExpressionStack expressions)
    {
        if (Disassembly.IsConstructor)
        {
            expressions.Push(new ShaderVariableExpression(parameters[0]));
            return;
        }

        ShaderExpression? returnValue = ReturnType == typeof(void) ? null : expressions.Pop();

        if (ReturnType == typeof(bool))
        {
            if (returnValue is ConstantExpression constExpr2 && constExpr2.Value is 0 or 1)
            {
                returnValue = new ConstantExpression(ShaderType.Bool, (int)constExpr2.Value == 1);
            }
        }

        // actual return expression will be built by the return block
        expressions.Push(returnValue);
    }

    private void BuildInitObj(Instruction instruction, ExpressionStack expressions)
    {
        var type = ((MetadataToken)instruction.Argument).Resolve() as Type ?? throw new();
        var left = expressions.Pop();
        var right = new DefaultExpression(context.CompileType(type));
        expressions.PushStatement(new BinaryExpression(BinaryOperation.Assignment, left, right));
    }

    private void BuildBranch(Instruction instruction, ExpressionStack expressions)
    {
        ShaderExpression left, right;
        switch (instruction.OpCode)
        {
            case OpCode.Bne_Un or OpCode.Bne_Un_S:
                // emit not equal comparison
                right = expressions.Pop();
                left = expressions.Pop();
                expressions.Push(new BinaryExpression(BinaryOperation.NotEqual, left, right));
                break;
            case OpCode.Beq or OpCode.Beq_S:
                right = expressions.Pop();
                left = expressions.Pop();
                expressions.Push(new BinaryExpression(BinaryOperation.Equal, left, right));
                break;
            case OpCode.Blt or OpCode.Blt_S or OpCode.Blt_Un or OpCode.Blt_Un_S:
                // emit less than comparison
                right = expressions.Pop();
                left = expressions.Pop();
                expressions.Push(new BinaryExpression(BinaryOperation.LessThan, left, right));
                break;
            case OpCode.Ble or OpCode.Ble_S or OpCode.Ble_Un or OpCode.Ble_Un_S:
                // emit less than or equal comparison
                right = expressions.Pop();
                left = expressions.Pop();
                expressions.Push(new BinaryExpression(BinaryOperation.LessThanEqual, left, right));
                break;
            case OpCode.Bgt or OpCode.Bgt_S or OpCode.Bgt_Un or OpCode.Bgt_Un_S:
                // emit greater than comparison
                right = expressions.Pop();
                left = expressions.Pop();
                expressions.Push(new BinaryExpression(BinaryOperation.GreaterThan, left, right));
                break;
            case OpCode.Bge or OpCode.Bge_S or OpCode.Bge_Un or OpCode.Bge_Un_S:
                // emit greater than or equal comparison
                right = expressions.Pop();
                left = expressions.Pop();
                expressions.Push(new BinaryExpression(BinaryOperation.GreaterThanEqual, left, right));
                break;
            case OpCode.Brfalse or OpCode.Brfalse_S:
                // invert condition
                var expr = expressions.Pop();
                if (expr.ExpressionType != ShaderType.Bool)
                {
                    expr = new BinaryExpression(BinaryOperation.NotEqual, expr, new ConstantExpression(expr.ExpressionType, 0));
                }
                expressions.Push(new UnaryExpression(UnaryOperation.Not, expr, null));
                break;
            case OpCode.Brtrue or OpCode.Brtrue_S:
                expr = expressions.Pop();
                if (expr.ExpressionType != ShaderType.Bool)
                {
                    expr = new BinaryExpression(BinaryOperation.NotEqual, expr, new ConstantExpression(expr.ExpressionType, 0));
                }
                expressions.Push(expr);
                break;
            case OpCode.Br or OpCode.Br_S:
                break; // do nothing for these
            default:
                throw new UnreachableException();
        }
    }

    ShaderExpression ForceVoidType(ShaderExpression expression, ExpressionStack expressions)
    {
        return expression;
    }

    void BuildDup(Instruction instruction, ExpressionStack Expressions)
    {
        var expr = Expressions.Pop();
        Expressions.Push(expr);
        Expressions.Push(expr);
    }

    void BuildBinaryExpr(Instruction instruction, ExpressionStack expressions)
    {
        // TODO: bitwise operators
        // TODO: ternary exprs
        // TODO: support short-circuiting operators (&& and ||)

        var exprType = instruction.OpCode switch
        {
            OpCode.Add => BinaryOperation.Add,
            OpCode.Sub => BinaryOperation.Subtract,
            OpCode.Mul => BinaryOperation.Multiply,
            OpCode.Div or OpCode.Div_Un => BinaryOperation.Divide,
            OpCode.And => BinaryOperation.And,
            OpCode.Xor => BinaryOperation.XOr,
            OpCode.Or => BinaryOperation.Or,
            OpCode.Not => BinaryOperation.Not,
            OpCode.Shr => BinaryOperation.RightShift,
            OpCode.Shr_Un => BinaryOperation.RightShift,
            OpCode.Shl => BinaryOperation.LeftShift,
            OpCode.Ceq => BinaryOperation.Equal,
            OpCode.Clt or OpCode.Clt_Un => BinaryOperation.LessThan,
            OpCode.Cgt or OpCode.Cgt_Un => BinaryOperation.GreaterThan,
            OpCode.Rem => BinaryOperation.Modulus,
            OpCode.Rem_Un => BinaryOperation.Modulus,
            _ => throw new UnreachableException(),
        };

        var right = expressions.Pop();
        var left = expressions.Pop();

        // special case: the compiler will often emit == 0 on a comparison to invert it (ie (x == 10) == 0 is the same as x != 10)
        // in this case instead of emitting the second x == 0 we just emit !x.
        if (left is BinaryExpression bin && BinaryExpression.IsBooleanOperator(bin.Operation) && right is ConstantExpression constExpr && constExpr.Value is 0)
        {
            expressions.Push(new UnaryExpression(UnaryOperation.Not, left, null));
            return;
        }

        expressions.Push(CreateBinaryExpression(exprType, left, right));
    }

    private BinaryExpression CreateBinaryExpression(BinaryOperation operation, ShaderExpression left, ShaderExpression right)
    {
        FixBoolConstants(ref left, ref right);
        return new(operation, left, right);
    }

    private void FixBoolConstants([NotNullIfNotNull(nameof(left))] ref ShaderExpression? left, ref ShaderExpression? right)
    {
        if (left?.ExpressionType == ShaderType.Bool)
        {
            if (right is ConstantExpression constExpr2 && constExpr2.Value is 0 or 1)
            {
                right = new ConstantExpression(ShaderType.Bool, (int)constExpr2.Value == 1);
            }
        }

        if (right?.ExpressionType == ShaderType.Bool)
        {
            if (left is ConstantExpression constExpr2 && constExpr2.Value is 0 or 1)
            {
                left = new ConstantExpression(ShaderType.Bool, (int)constExpr2.Value == 1);
            }
        }
    }

    private void BuildFieldExpr(Instruction instruction, ExpressionStack expressions)
    {
        var metadataToken = (MetadataToken)instruction.Argument!;
        var fieldInfo = (FieldInfo)metadataToken.Resolve()!;

        if (context.IsSelfType(fieldInfo.DeclaringType!))
        {
            ShaderVariableExpression variable = new ShaderVariableExpression(context.Uniforms[fieldInfo]);
            if (instruction.OpCode is OpCode.Ldfld or OpCode.Ldflda)
            {
                _ = expressions.Pop(); // pop the 'self'
                expressions.Push(variable);
            }
            else
            {
                var value = expressions.Pop();
                _ = expressions.Pop(); // pop the 'self'
                expressions.PushStatement(CreateBinaryExpression(BinaryOperation.Assignment, variable, value));
            }
            return;
        }

        if (instruction.OpCode is OpCode.Stfld)
        {
            var value = expressions.Pop();
            var obj = expressions.Pop();

            expressions.PushStatement(CreateBinaryExpression(BinaryOperation.Assignment, new MemberAccess(obj, fieldInfo, context.CompileType(fieldInfo.FieldType)), value));
        }
        else if (instruction.OpCode is OpCode.Ldsfld)
        {
            if (!fieldInfo.Attributes.HasFlag(FieldAttributes.InitOnly))
                throw new Exception("Cannot access a non-readonly static field!");

            expressions.Push(new ConstantExpression(context.CompileType(fieldInfo.FieldType), fieldInfo.GetValue(null)!));
        }
        else // OpCode is Ldfld or Ldflda
        {
            var obj = expressions.Pop();

            expressions.Push(new MemberAccess(obj, fieldInfo, context.CompileType(fieldInfo.FieldType)));
        }
    }

    void BuildUnaryExpr(Instruction instruction, ExpressionStack expressions)
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

        var operand = expressions.Pop();

        ShaderType? shaderCastType = null;
        if (castType != null)
        {
            shaderCastType = context.CompileType(castType);
        }

        expressions.Push(new UnaryExpression(operation, operand, shaderCastType));
    }

    private void BuildLoadLocalExpr(Instruction instruction, ExpressionStack expressions)
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

        expressions.Push(new ShaderVariableExpression(this.locals[(int)loadIndex]));
    }

    private void BuildStoreLocalExpr(Instruction instruction, ExpressionStack expressions)
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

        var expr = expressions.Pop();
        var local = locals[(int)storeIndex];
        expressions.PushStatement(CreateBinaryExpression(BinaryOperation.Assignment, new ShaderVariableExpression(local), expr));
    }

    void BuildConstantExpr(Instruction instruction, ExpressionStack expressions)
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

        expressions.Push(new ConstantExpression(context.CompileType(value.GetType()), value));
    }

    private void BuildLoadArgExpr(Instruction instruction, ExpressionStack expressions)
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

        expressions.Push(new ShaderVariableExpression(this.parameters[(int)loadIndex]));
    }

    private void BuildStoreArgExpr(Instruction instruction, ExpressionStack expressions)
    {
        uint storeIndex = instruction.OpCode switch
        {
            OpCode.Starg => (uint)instruction.Argument!,
            OpCode.Starg_S => (byte)instruction.Argument!,
            _ => throw new UnreachableException(),
        };

        expressions.PushStatement(CreateBinaryExpression(BinaryOperation.Assignment, new ShaderVariableExpression(parameters[(int)storeIndex]), expressions.Pop()));
    }

    void BuildNewObjExpr(Instruction instruction, ExpressionStack expressions)
    {
        var metadataToken = (MetadataToken)instruction.Argument!;
        var ctor = (ConstructorInfo)(metadataToken.Resolve() ?? throw new Exception());

        var args = PopArgs(ctor, expressions);

        var intrinsic = context.ResolveMethodToIntrinsic(ctor);
        if (intrinsic != null)
        {
            expressions.Push(new ShaderIntrinsicCall(intrinsic, context.CompileType(intrinsic.ReturnType), args));
        }
        else
        {
            var shaderMethod = context.EnqueueMethod(ctor);
            expressions.Push(new ShaderMethodCall(shaderMethod, args));
        }
    }

    void BuildCallExpr(Instruction instruction, ExpressionStack expressions)
    {
        var metadataToken = (MetadataToken)instruction.Argument!;
        var method = (MethodBase)(metadataToken.Resolve() ?? throw new Exception());

        var args = PopArgs(method, expressions);

        var instance = method.IsStatic ? null : expressions.Pop();
        if (method is not ConstructorInfo && !context.IsSelfType(method.DeclaringType))
        {
            if (instance != null)
            {
                args = [instance, .. args];
            }
        }

        var intrinsic = context.ResolveMethodToIntrinsic(method);
        if (intrinsic != null)
        {
            // special case: BufferLoad and similar methods return a generic parameter for flexibility.
            // we compile the return type of the original method in that case.
            ShaderType returnType;
            if (intrinsic.ReturnType.IsGenericParameter && method is MethodInfo mi)
            {
                returnType = context.CompileType(mi.ReturnType);
            }
            else
            {
                returnType = context.CompileType(intrinsic.ReturnType);
            }

            expressions.Push(new ShaderIntrinsicCall(intrinsic, returnType, args));
        }
        else
        {
            var shaderMethod = context.EnqueueMethod(method);
            expressions.Push(new ShaderMethodCall(shaderMethod, args));
        }

        if (method is ConstructorInfo)
        {
            var value = expressions.Pop();
            expressions.PushStatement(CreateBinaryExpression(BinaryOperation.Assignment, instance, value));
        }
    }

    ShaderExpression[] PopArgs(MethodBase method, ExpressionStack expressions)
    {
        var args = new ShaderExpression[method.GetParameters().Length];

        for (int i = args.Length - 1; i >= 0; i--)
        {
            args[i] = expressions.Pop();
        }

        return args;
    }

    private void BuildLoadIndirect(Instruction instruction, ExpressionStack expressions)
    {
        // nop
    }

    private void BuildStoreIndirect(Instruction instruction, ExpressionStack expressions)
    {
        var value = expressions.Pop();
        var reference = expressions.Pop();

        expressions.PushStatement(CreateBinaryExpression(BinaryOperation.Assignment, reference, value));
    }
}