namespace SimulationFramework.Drawing.Shaders.Compiler;

internal class ShaderPrimitiveType : ShaderType 
{
    public ShaderPrimitiveKind primitive;

    internal ShaderPrimitiveType(ShaderPrimitiveKind primitive)
    {
        this.primitive = primitive;
    }

    public override string ToString()
    {
        return primitive.ToString();
    }
}

public enum ShaderPrimitiveKind
{
    Void,
    Bool,
    UInt,
    UInt2,
    UInt3,
    UInt4,
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
    DepthMask,
}
