namespace SimulationFramework.Drawing.Shaders.Compiler;

public abstract class ShaderType
{
   
    public static ShaderType Void { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.Void);
    public static ShaderType Bool { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.Bool);
    public static ShaderType Int { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.Int);
    public static ShaderType Int2 { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.Int2);
    public static ShaderType Int3 { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.Int3);
    public static ShaderType Int4 { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.Int4);
    public static ShaderType UInt { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.UInt);
    public static ShaderType UInt2 { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.UInt2);
    public static ShaderType UInt3 { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.UInt3);
    public static ShaderType UInt4 { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.UInt4);
    public static ShaderType Float { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.Float);
    public static ShaderType Float2 { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.Float2);
    public static ShaderType Float3 { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.Float3);
    public static ShaderType Float4 { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.Float4);
    public static ShaderType Matrix4x4 { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.Matrix4x4);
    public static ShaderType Matrix3x2 { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.Matrix3x2);
    public static ShaderType Texture { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.Texture);

    public ShaderPrimitiveKind? GetPrimitiveKind()
    {
        if (this is ShaderPrimitiveType primitiveType)
        {
            return primitiveType.primitive;
        }
        return null;
    }

    public static int CalculateTypeSize(ShaderType type)
    {
        if (type is ShaderStructureType structType)
        {
            return structType.structure.fields.Select(f => CalculateTypeSize(f.Type)).Sum();
        }
        else if (type.GetPrimitiveKind() is ShaderPrimitiveKind primitiveKind)
        {
            int channels = primitiveKind switch
            {
                ShaderPrimitiveKind.Float2 or ShaderPrimitiveKind.UInt2 or ShaderPrimitiveKind.Int2 => 2,
                ShaderPrimitiveKind.Float3 or ShaderPrimitiveKind.UInt3 or ShaderPrimitiveKind.Int3 => 3,
                ShaderPrimitiveKind.Float4 or ShaderPrimitiveKind.UInt4 or ShaderPrimitiveKind.Int4 => 4,
                _ => 1,
            };
            return 4 * channels;
        }
        else
        {
            throw new NotSupportedException();
        }
    }
}