using System.Collections.Immutable;
using System.Numerics;

namespace SimulationFramework.Drawing.Shaders.Compiler;

public abstract class ShaderType
{
    /// <summary>
    /// Shader type equivalent of <see cref="void"/>.
    /// </summary>
    public static ShaderType Void { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.Void);

    /// <summary>
    /// Shader type equivalent of <see cref="bool"/>.
    /// </summary>
    public static ShaderType Bool { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.Bool);

    /// <summary>
    /// Shader type equivalent of <see cref="int"/>.
    /// </summary>
    public static ShaderType Int { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.Int);

    public static ShaderType Int2 { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.Int2);
    public static ShaderType Int3 { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.Int3);
    public static ShaderType Int4 { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.Int4);

    /// <summary>
    /// Shader type equivalent of <see cref="uint"/>.
    /// </summary>
    public static ShaderType UInt { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.UInt);
    public static ShaderType UInt2 { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.UInt2);
    public static ShaderType UInt3 { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.UInt3);
    public static ShaderType UInt4 { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.UInt4);

    /// <summary>
    /// Shader type equivalent of <see cref="float"/>.
    /// </summary>
    public static ShaderType Float { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.Float);

    /// <summary>
    /// Shader type equivalent of <see cref="Vector2"/>.
    /// </summary>
    public static ShaderType Float2 { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.Float2);

    /// <summary>
    /// Shader type equivalent of <see cref="Vector3"/>.
    /// </summary>
    public static ShaderType Float3 { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.Float3);

    /// <summary>
    /// Shader type equivalent of <see cref="Vector4"/>.
    /// </summary>
    public static ShaderType Float4 { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.Float4);

    /// <summary>
    /// Shader type equivalent of <see cref="System.Numerics.Matrix4x4"/>.
    /// </summary>
    public static ShaderType Matrix4x4 { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.Matrix4x4);

    /// <summary>
    /// Shader type equivalent of <see cref="System.Numerics.Matrix3x2"/>.
    /// </summary>
    public static ShaderType Matrix3x2 { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.Matrix3x2);

    /// <summary>
    /// Shader type equivalent of <see cref="ITexture"/>.
    /// </summary>
    public static ShaderType Texture { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.Texture);

    /// <summary>
    /// Shader type equivalent of <see cref="IDepthMask"/>.
    /// </summary>
    public static ShaderType DepthMask { get; } = new ShaderPrimitiveType(ShaderPrimitiveKind.DepthMask);

    /// <summary>
    /// Maps .NET types to their shader equivalents.
    /// </summary>
    public static ImmutableDictionary<Type, ShaderType> PrimitiveTypeMap { get; } = new Dictionary<Type, ShaderType>()
    {
        [typeof(void)] = Void,
        [typeof(bool)] = Bool,
        [typeof(sbyte)] = Int,
        [typeof(short)] = Int,
        [typeof(int)] = Int,
        [typeof(byte)] = UInt,
        [typeof(ushort)] = UInt,
        [typeof(uint)] = UInt,
        [typeof(float)] = Float,
        [typeof(Vector2)] = Float2,
        [typeof(Vector3)] = Float3,
        [typeof(Vector4)] = Float4,
        [typeof(ColorF)] = Float4,
        [typeof(Color)] = UInt,
        [typeof(Matrix4x4)] = Matrix4x4,
        [typeof(Matrix3x2)] = Matrix3x2,
        [typeof(ITexture)] = Texture,
        [typeof(IDepthMask)] = DepthMask,
    }.ToImmutableDictionary();
    
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