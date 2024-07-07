namespace SimulationFramework.Drawing.Shaders.Compiler;

internal class ShaderPrimitiveType : ShaderType 
{
    public PrimitiveKind primitive;

    internal ShaderPrimitiveType(PrimitiveKind primitive)
    {
        this.primitive = primitive;
    }

    public override string ToString()
    {
        return primitive.ToString();
    }
}

public enum PrimitiveKind
{
    Void,
    Bool,
    Int,
    Int2,
    Int3,
    Int4,
    Float,
    Float2,
    Float3,
    Float4,
    Matrix4x4,
    Matrix3x2,
    Texture,
}