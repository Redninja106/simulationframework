namespace SimulationFramework.Drawing.Shaders.Compiler;

public class ReferenceType : ShaderType
{
    public ShaderType ElementType { get; }

    public ReferenceType(ShaderType elementType)
    {
        ElementType = elementType;
    }

    public override string ToString()
    {
        return "ref " + ElementType.ToString();
    }
}
