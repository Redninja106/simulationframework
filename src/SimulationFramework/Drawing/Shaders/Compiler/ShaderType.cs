namespace SimulationFramework.Drawing.Shaders.Compiler;

public abstract class ShaderType
{
   
    public static ShaderType Void { get; } = new ShaderPrimitiveType(PrimitiveKind.Void);
    public static ShaderType Bool { get; } = new ShaderPrimitiveType(PrimitiveKind.Bool);
    public static ShaderType Int { get; } = new ShaderPrimitiveType(PrimitiveKind.Int);
    public static ShaderType Int2 { get; } = new ShaderPrimitiveType(PrimitiveKind.Int2);
    public static ShaderType Int3 { get; } = new ShaderPrimitiveType(PrimitiveKind.Int3);
    public static ShaderType Int4 { get; } = new ShaderPrimitiveType(PrimitiveKind.Int4);
    public static ShaderType UInt { get; } = new ShaderPrimitiveType(PrimitiveKind.UInt);
    public static ShaderType UInt2 { get; } = new ShaderPrimitiveType(PrimitiveKind.UInt2);
    public static ShaderType UInt3 { get; } = new ShaderPrimitiveType(PrimitiveKind.UInt3);
    public static ShaderType UInt4 { get; } = new ShaderPrimitiveType(PrimitiveKind.UInt4);
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

    public static int CalculateTypeSize(ShaderType type)
    {
        if (type is ShaderStructureType structType)
        {
            return structType.structure.fields.Select(f => CalculateTypeSize(f.Type)).Sum();
        }
        else if (type.GetPrimitiveKind() is PrimitiveKind primitiveKind)
        {
            int channels = primitiveKind switch
            {
                PrimitiveKind.Float2 or PrimitiveKind.UInt2 or PrimitiveKind.Int2 => 2,
                PrimitiveKind.Float3 or PrimitiveKind.UInt3 or PrimitiveKind.Int3 => 3,
                PrimitiveKind.Float4 or PrimitiveKind.UInt4 or PrimitiveKind.Int4 => 4,
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