using SimulationFramework.Drawing.Shaders.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor)]
public class ShaderMethodAttribute : Attribute
{
    public ShaderMethodAttribute()
    {

    }
}
