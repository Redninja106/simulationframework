namespace SimulationFramework.Drawing.Shaders.Compiler;

public class ShaderArrayType : ShaderType
{
    public ShaderArrayType(ShaderType elementType)
    {
        ElementType = elementType;
    }

    public ShaderType ElementType { get; set; }


}
