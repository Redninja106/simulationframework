using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler;

/// <summary>
/// gradually built up by stages during compilation
/// </summary>
public class ShaderCompilation
{
    public ShaderKind Kind { get; set; }
    public ShaderMethod EntryPoint { get; set; }
    public List<ShaderMethod> Methods { get; } = [];
    public List<ShaderVariable> Variables { get; } = [];
    public List<ShaderStructure> Structures { get; } = [];
}
