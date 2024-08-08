using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using System.Reflection;

namespace SimulationFramework.Drawing.Shaders.Compiler;

public class ShaderMethod
{
    public ShaderVariable[] Parameters { get; set; }
    public ShaderVariable[] Locals { get; set; }
    public ShaderExpression Body { get; set; }
    public ShaderType ReturnType { get; set; }
    public ShaderName Name { get; set; }
    public MethodBase? BackingMethod { get; set; }
}
