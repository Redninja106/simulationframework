namespace SimulationFramework.Drawing.Shaders.Compiler;

public class ShaderReferenceType : ShaderType
{
    public ShaderType ElementType { get; }

    public ShaderReferenceType(ShaderType elementType)
    {
        ElementType = elementType;
    }

    public override string ToString()
    {
        return "ref " + ElementType.ToString();
    }
}
