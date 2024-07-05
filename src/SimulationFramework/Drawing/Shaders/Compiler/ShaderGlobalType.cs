
namespace SimulationFramework.Drawing.Shaders.Compiler;

internal class ShaderGlobalType : ShaderType
{
    private Type shaderType;

    public ShaderGlobalType(Type type)
    {
        this.shaderType = type;
    }
}