using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler.ILDisassembler;
internal static class OpCodeExtensions
{
    private static readonly Dictionary<Type, OpCode[]> argTypes = new()
    {
        [typeof(byte)] = new[]
        {
            OpCode.Ldarg_S,
            OpCode.Ldarga_S,
            OpCode.Starg_S,
            OpCode.Ldloc_S,
            OpCode.Ldloca_S,
            OpCode.Stloc_S
        },
        [typeof(sbyte)] = new[]
        {
            OpCode.Ldc_I4_S,
            OpCode.Br_S,
            OpCode.Brfalse_S,
            OpCode.Brtrue_S,
            OpCode.Beq_S,
            OpCode.Bge_S,
            OpCode.Bgt_S,
            OpCode.Ble_S,
            OpCode.Blt_S,
            OpCode.Bne_Un_S,
            OpCode.Bge_Un_S,
            OpCode.Bgt_Un_S,
            OpCode.Ble_Un_S,
            OpCode.Blt_Un_S
        },
        [typeof(int)] = new[]
        {
            OpCode.Ldc_I4,
            OpCode.Leave,
            OpCode.Br,
            OpCode.Brfalse,
            OpCode.Brtrue,
            OpCode.Beq,
            OpCode.Bge,
            OpCode.Bgt,
            OpCode.Ble,
            OpCode.Blt,
            OpCode.Bne_Un,
            OpCode.Bge_Un,
            OpCode.Bgt_Un,
            OpCode.Ble_Un,
            OpCode.Blt_Un
        },
        [typeof(long)] = new[]
        {
            OpCode.Ldc_I8
        },
        [typeof(float)] = new[]
        {
            OpCode.Ldc_R4
        },
        [typeof(double)] = new[]
        {
            OpCode.Ldc_R8
        },
        [typeof(ushort)] = new[]
        {
            OpCode.Ldarg,
            OpCode.Ldarga,
            OpCode.Starg,
            OpCode.Ldloc,
            OpCode.Ldloca,
            OpCode.Stloc,
        },
        [typeof(MetadataToken)] = new[]
        {
            OpCode.Jmp,
            OpCode.Call,
            OpCode.Callvirt,
            OpCode.Ldftn,
            OpCode.Ldstr,
            OpCode.Ldfld,
            OpCode.Ldflda,
            OpCode.Ldsfld,
            OpCode.Ldsflda,
            OpCode.Ldobj,
            OpCode.Ldtoken,
            OpCode.Stfld,
            OpCode.Newobj,
            OpCode.Initobj,
        },
    };

    private static readonly Dictionary<BranchBehavior, OpCode[]> branchBehaviors = new()
    {
        [BranchBehavior.Branch] = new[]
        {
            OpCode.Br,
            OpCode.Br_S,
        },
        [BranchBehavior.BranchOrContinue] = new[]
        {
            OpCode.Brfalse_S,
            OpCode.Brtrue_S,
            OpCode.Beq_S,
            OpCode.Bge_S,
            OpCode.Bgt_S,
            OpCode.Ble_S,
            OpCode.Blt_S,
            OpCode.Bne_Un_S,
            OpCode.Bge_Un_S,
            OpCode.Bgt_Un_S,
            OpCode.Ble_Un_S,
            OpCode.Blt_Un_S,
            OpCode.Br,
            OpCode.Brfalse,
            OpCode.Brtrue,
            OpCode.Beq,
            OpCode.Bge,
            OpCode.Bgt,
            OpCode.Ble,
            OpCode.Blt,
            OpCode.Bne_Un,
            OpCode.Bge_Un,
            OpCode.Bgt_Un,
            OpCode.Ble_Un,
            OpCode.Blt_Un,
        },
        [BranchBehavior.Return] = new[]
        {
            OpCode.Ret,
            OpCode.Throw
        }
    };

    private static readonly Dictionary<OpCodeFlags, OpCode[]> opCodeFlags = new()
    {
        [OpCodeFlags.Prefix] = new[]
        {
            OpCode.Unaligned_,
            OpCode.Volatile_,
            OpCode.Constrained_,
            OpCode.Readonly_
        }
    };

    public static BranchBehavior GetBranchBehavior(this OpCode opCode)
    {
        return branchBehaviors.SingleOrDefault(pair => pair.Value.Contains(opCode)).Key;
    }

    public static bool IsPrefix(this OpCode opCode)
    {
        return opCodeFlags.SingleOrDefault(pair => pair.Value.Contains(opCode)).Key.HasFlag(OpCodeFlags.Prefix);
    }

    public static Type? GetArgumentType(this OpCode opCode)
    {
        return argTypes.SingleOrDefault(pair => pair.Value.Contains(opCode)).Key;
    }

}
