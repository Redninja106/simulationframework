namespace SimulationFramework.Drawing.Shaders.Compiler;

public abstract class ShaderType
{
   
    public static ShaderType Void { get; } = new ShaderPrimitiveType(PrimitiveKind.Void);
    public static ShaderType Bool { get; } = new ShaderPrimitiveType(PrimitiveKind.Bool);
    public static ShaderType Int { get; } = new ShaderPrimitiveType(PrimitiveKind.Int);
    public static ShaderType Int2 { get; } = new ShaderPrimitiveType(PrimitiveKind.Int2);
    public static ShaderType Int3 { get; } = new ShaderPrimitiveType(PrimitiveKind.Int3);
    public static ShaderType Int4 { get; } = new ShaderPrimitiveType(PrimitiveKind.Int4);
    public static ShaderType Float { get; } = new ShaderPrimitiveType(PrimitiveKind.Float);
    public static ShaderType Float2 { get; } = new ShaderPrimitiveType(PrimitiveKind.Float2);
    public static ShaderType Float3 { get; } = new ShaderPrimitiveType(PrimitiveKind.Float3);
    public static ShaderType Float4 { get; } = new ShaderPrimitiveType(PrimitiveKind.Float4);
    public static ShaderType Matrix4x4 { get; } = new ShaderPrimitiveType(PrimitiveKind.Matrix4x4);
    public static ShaderType Matrix3x2 { get; } = new ShaderPrimitiveType(PrimitiveKind.Matrix3x2);
    public static ShaderType Texture { get; } = new ShaderPrimitiveType(PrimitiveKind.Texture);

    public PrimitiveKind? GetPrimitiveKind()
    {
        if (this is ShaderPrimitiveType primitiveType)
        {
            return primitiveType.primitive;
        }
        return null;
    }
}