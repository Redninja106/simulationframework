using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Disassembler;

/// <summary>
/// Represents a disassembled method.
/// </summary>
internal class MethodDisassembly
{
    public byte[] ilBytes;
    public Instruction[] instructions;
    public MethodBody MethodBody;
    public MethodBase Method;

    public bool IsConstructor => Method is ConstructorInfo;

    public MethodDisassembly(MethodBase method)
    {
        ArgumentNullException.ThrowIfNull(method);

        Method = method;
        MethodBody = method.GetMethodBody() ?? throw new Exception("Method has no body");
        ilBytes = MethodBody.GetILAsByteArray() ?? throw new Exception("Method has no body");
        instructions = ReadInstructions();
    }

    private Instruction[] ReadInstructions()
    {
        var result = new List<Instruction>();
        var stream = new MemoryStream(ilBytes);

        Instruction prefix = null;

        while (stream.Position < stream.Length)
        {
            var startLocation = (int)stream.Position;

            var opCode = ReadOpCode(stream);

            var argumentType = opCode.GetArgumentType();
            var argument = ReadArgument(stream, argumentType);

            var size = (int)stream.Position - startLocation;

            var instruction = new Instruction(this, prefix, opCode, argument, startLocation, size);
            result.Add(instruction);

            prefix = opCode.IsPrefix() ? instruction : null;
        }

        return result.ToArray();
    }

    private OpCode ReadOpCode(Stream stream)
    {
        var op = (OpCode)stream.ReadByte();

        if (op == OpCode.Prefix1)
        {
            op = (OpCode)(0xFE00 | stream.ReadByte());
        }

        return op;
    }

    private object? ReadArgument(Stream stream, Type argumentType)
    {
        return argumentType switch
        {
            _ when argumentType == typeof(sbyte) => stream.Read<sbyte>(),
            _ when argumentType == typeof(byte) => stream.Read<byte>(),
            _ when argumentType == typeof(short) => stream.Read<short>(),
            _ when argumentType == typeof(ushort) => stream.Read<ushort>(),
            _ when argumentType == typeof(int) => stream.Read<int>(),
            _ when argumentType == typeof(uint) => stream.Read<uint>(),
            _ when argumentType == typeof(long) => stream.Read<long>(),
            _ when argumentType == typeof(ulong) => stream.Read<ulong>(),
            _ when argumentType == typeof(float) => stream.Read<float>(),
            _ when argumentType == typeof(double) => stream.Read<double>(),
            _ when argumentType == typeof(MetadataToken) => new MetadataToken(this.Method.Module, stream.Read<int>()),
            _ when argumentType == typeof(int[]) => ReadSwitchTable(stream),
            _ => null,
        };

        int[] ReadSwitchTable(Stream stream)
        {
            uint n = stream.Read<uint>();
            int[] result = new int[n];
            for (int i = 0; i < n; i++)
            {
                result[i] = stream.Read<int>();
            }
            return result;
        }
    }

    public Instruction GetInstructionAt(int ilOffset)
    {
        return instructions.SingleOrDefault(i => i.Location == ilOffset) ?? throw new Exception("An instruction does not start at the provided offset.");
    }
}
