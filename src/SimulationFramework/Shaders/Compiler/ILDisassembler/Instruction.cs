using System;
using System.Collections.Generic;
using System.Text;

namespace SimulationFramework.Shaders.Compiler.ILDisassembler;

/// <summary>
/// Represents a disassembled IL instruction.
/// </summary>
internal class Instruction
{
    public MethodDisassembly Method { get; set; }
    public Instruction? ImmediatePrefix { get; set; }
    public OpCode OpCode { get; set; }
    public object? Argument { get; set; }

    public int Location { get; set; }
    public int Size { get; set; }

    public Instruction(MethodDisassembly method, Instruction? prefix, OpCode opCode, object? argument, int location, int size)
    {
        Method = method;
        ImmediatePrefix = prefix;
        OpCode = opCode;
        Argument = argument;

        Location = location;
        Size = size;
    }

    public IEnumerable<Instruction> GetPrefixes()
    {
        var instruction = this;

        while (instruction.ImmediatePrefix != null)
        {
            yield return instruction.ImmediatePrefix;

            instruction = instruction.ImmediatePrefix;
        }
    }

    public bool CanBranch()
    {
        return OpCode.GetBranchBehavior() is BranchBehavior.Branch or BranchBehavior.BranchOrContinue or BranchBehavior.Return;
    }

    public bool IsBranchTarget()
    {
        foreach (var instruction in Method.instructions)
        {
            if (this == instruction.GetBranchTarget())
            {
                return true;
            }
        }

        return false;
    }

    public Instruction? GetBranchTarget()
    {
        if (OpCode.GetBranchBehavior() is not (BranchBehavior.Branch or BranchBehavior.BranchOrContinue))
        {
            return null;
        }

        int offset = GetBranchOffset();

        return Method.GetInstructionAt(Location + Size + offset);
    }

    public int GetBranchOffset()
    {
        int offset;

        if (Argument is sbyte shortOffset)
        {
            offset = shortOffset;
        }
        else if (Argument is int longOffset)
        {
            offset = longOffset;
        }
        else
        {
            throw new Exception("Branch instruction had unexpected argument type");
        }

        return offset;
    }

    public override string ToString()
    {
        StringBuilder result = new();

        result.Append(GetLabel());
        result.Append(": ");

        result.Append(OpCode.ToString().Replace('_', '.'));

        result.Append(' ');

        if (OpCode.GetBranchBehavior() is BranchBehavior.Branch or BranchBehavior.BranchOrContinue)
        {
            result.Append(GetBranchTarget()?.GetLabel());
        }
        else
        {
            result.Append(Argument?.ToString());
        }

        return result.ToString();
    }

    private string GetLabel()
    {
        return $"IL_{Location:x4}";
    }
}
