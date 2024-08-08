using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler;
public enum ShaderVariableKind
{
    Local,
    Parameter,
    Field,
    Uniform,
    VertexData,
    // InstanceData,
    VertexShaderOutput,
}
