using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders;

public static class ShaderIntrinsics
{
    [ShaderIntrinsic(nameof(Vector4.Transform), typeof(Vector4))]
    public static Vector4 Mul(Vector4 vector, Matrix4x4 matrix) => Vector4.Transform(vector, matrix);

    [ShaderIntrinsic(ShaderIntrinsicAttribute.ConstructorMethodName, typeof(Vector4))]
    public static Vector4 Vec4(Vector3 xyz, float w) => new(xyz, w);

    [ShaderIntrinsic(ShaderIntrinsicAttribute.ConstructorMethodName, typeof(Vector4))]
    public static Vector4 Vec4(float x, float y, float z, float w) => new(x, y, z, w);

    [ShaderIntrinsic(ShaderIntrinsicAttribute.ConstructorMethodName, typeof(Vector3))]
    public static Vector3 Vec3(float x, float y, float z) => new(x, y, z);

    [ShaderIntrinsic(ShaderIntrinsicAttribute.ConstructorMethodName, typeof(Vector3))]
    public static Vector3 Vec3(float value) => new(value);

    [ShaderIntrinsic(nameof(MathF.Sqrt), typeof(MathF))]
    public static float Sqrt(float x) => MathF.Sqrt(x);

    [ShaderIntrinsic(nameof(Vector3.Dot), typeof(Vector3))]
    public static float Dot(Vector3 vector1, Vector3 vector2) => Vector3.Dot(vector1, vector2);

    [ShaderIntrinsic(nameof(Vector3.Normalize), typeof(Vector3))]
    public static Vector3 Normalize(Vector3 vector) => Vector3.Normalize(vector);
}
