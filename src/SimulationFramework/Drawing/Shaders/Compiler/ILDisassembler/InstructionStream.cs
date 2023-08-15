using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.ILDisassembler;

class InstructionStream
{
    private MethodDisassembly disassembly;

    public int Position { get; set; }

    public bool IsAtEnd => Position >= disassembly.ilBytes.Length;

    public InstructionStream(MethodDisassembly disassembly)
    {
        this.disassembly = disassembly;
        Position = 0;
    }

    public Instruction Peek()
    {
        return disassembly.GetInstructionAt(Position);
    }

    public Instruction Read()
    {
        var result = Peek();

        if (result.Size is 0)
            throw new Exception();

        Position += result.Size;
        return result;
    }

    public InstructionStream Clone(int byteOffset = 0)
    {
        var result = new InstructionStream(this.disassembly);
        result.Position = Position + byteOffset;
        return result;
    }
}