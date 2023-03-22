using SimulationFramework.Shaders.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor)]
public class ShaderMethodAttribute : Attribute
{
    public ShaderKind Kind { get; set; }

    public ShaderMethodAttribute(bool isShaderMethod = true)
    {

    }
}
