using SimulationFramework.Shaders.Compiler.Expressions;
using SimulationFramework.Shaders.Compiler.ILDisassembler;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler.Expressions;

internal class ExpressionBuilder
{
    private InstructionStream InstructionStream { get; set; }
    private Stack<Expression> Expressions { get; set; }
    private ParameterExpression[] Arguments { get; set; }
    private ParameterExpression[] Locals { get; set; }
    private LabelTarget ReturnTarget { get; set; }
    private Type ReturnType { get; set; }
    private bool IsConstructor { get; set; }

    private ExpressionBuilder()
    {

    }

    public static BlockExpression BuildExpression(MethodDisassembly disassembly, out ParameterExpression[] Parameters)
    {
        ExpressionBuilder builder = new();

        builder.IsConstructor = disassembly.Method is ConstructorInfo;

        builder.Expressions = new();
        builder.InstructionStream = new InstructionStream(disassembly);

        builder.Arguments = GetArguments(disassembly);
        builder.Locals = GetLocals(disassembly);

        builder.ReturnType = disassembly.Method is MethodInfo method ? method.ReturnType : typeof(void);
        builder.ReturnTarget = Expression.Label(builder.ReturnType, "return");

        var builders = new Func<Instruction, bool>[]
        {
            builder.BuildNopExpr,
            builder.BuildBinaryExpr,
            builder.BuildFieldExpr,
            builder.BuildUnaryExpr,
            builder.BuildLocalExpr,
            builder.BuildReturnExpr,
            builder.BuildConstantExpr,
            builder.BuildArgumentExpr,
            builder.BuildNewObjExpr,
            builder.BuildCallExpr,

            i => i.OpCode is OpCode.Dup or OpCode.Pop or OpCode.Br_S or OpCode.Ldobj,

            i => throw new NotSupportedException("Unsupported instruction '" + i.OpCode + "'."),
        };

        while (!builder.InstructionStream.IsAtEnd)
        {
            var instruction = builder.InstructionStream.Read();

            foreach (var b in builders)
            {
                if (b(instruction))
                    break;
            }
        }

        Parameters = builder.Arguments;
        return Expression.Block(builder.ReturnType, builder.Locals, builder.Expressions.Reverse());
    }

    static ParameterExpression[] GetArguments(MethodDisassembly disassembly)
    {
        var infos = disassembly.Method.GetParameters();

        var parameters = infos.Select(p => Expression.Parameter(p.ParameterType, p.Name));

        if (!disassembly.Method.IsStatic)
        {
            parameters = parameters.Prepend(Expression.Parameter(disassembly.Method.DeclaringType!, "this"));
        }

        return parameters.ToArray();
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

        Expressions.Push(Expression.MakeBinary(exprType.Value, left, right));

        return true;

        static bool IsBinaryExpression(Instruction instruction, [NotNullWhen(true)] out ExpressionType? type)
        {
            type = instruction.OpCode switch
            {
                OpCode.Add => ExpressionType.Add,
                OpCode.Sub => ExpressionType.Subtract,
                OpCode.Mul => ExpressionType.Multiply,
                OpCode.Div => ExpressionType.Divide,
                _ => null
            };

            return type is not null;
        }
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
            Expressions.Push(Expression.Assign(Locals[(int)storeIndex], Expressions.Pop()));
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

        if (InstructionStream.IsAtEnd)
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
                _ => null
            };

            return value is not null;
        }
    }

    bool BuildArgumentExpr(Instruction instruction)
    {
        if (IsLoadArgumentExpression(instruction, out uint? loadIndex))
        {
            Expressions.Push(Arguments[(int)loadIndex]);
            return true;
        }

        if (IsStoreArgumentExpression(instruction, out uint? storeIndex))
        {
            Expressions.Push(Expression.Assign(Arguments[(int)storeIndex], Expressions.Pop()));
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
            Expressions.Push(new ConstructorCallExpression(constructor, args));
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


}