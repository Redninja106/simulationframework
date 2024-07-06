using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Disassembler;

[Flags]
internal enum OpCodeFlags
{
    None = 0,
    Prefix = 1,
}
