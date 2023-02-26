using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders;

[AttributeUsage(AttributeTargets.Method)]
internal class ShaderMethodAttribute : Attribute
{
    public ShaderMethodAttribute(bool isShaderMethod = true)
    {

    }
}
