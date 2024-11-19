namespace SimulationFramework.Drawing.Shaders.Compiler;

public class ShaderArrayType : ShaderType
{
    public ShaderArrayType(ShaderType elementType, int dimensions)
    {
        ElementType = elementType;
        Dimensions = dimensions;
    }

    public ShaderType ElementType { get; set; }
    public int Dimensions { get; set; }

}
