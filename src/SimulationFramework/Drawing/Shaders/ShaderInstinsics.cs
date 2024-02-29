﻿using SimulationFramework.Drawing.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders;

public static class ShaderIntrinsics
{
    [ShaderIntrinsic, Replace(nameof(Vector4.Transform), typeof(Vector4))]
    public static Vector4 Transform(Vector4 vector, Matrix4x4 matrix) => Vector4.Transform(vector, matrix);

    [ShaderIntrinsic, Replace(ReplaceAttribute.ConstructorName, typeof(Vector4))]
    public static Vector4 Vec4(Vector3 xyz, float w) => new(xyz, w);

    [ShaderIntrinsic, Replace(ReplaceAttribute.ConstructorName, typeof(Vector4))]
    public static Vector4 Vec4(Vector2 xy, float z, float w) => new(xy.X, xy.Y, z, w);

    [ShaderIntrinsic, Replace(ReplaceAttribute.ConstructorName, typeof(Vector4))]
    public static Vector4 Vec4(float x, float y, float z, float w) => new(x, y, z, w);

    [ShaderIntrinsic, Replace(ReplaceAttribute.ConstructorName, typeof(Vector3))]
    public static Vector3 Vec3(float x, float y, float z) => new(x, y, z);

    [ShaderIntrinsic, Replace(ReplaceAttribute.ConstructorName, typeof(Vector3))]
    public static Vector3 Vec3(Vector2 xy, float z) => new(xy, z);
    [ShaderIntrinsic, Replace(ReplaceAttribute.ConstructorName, typeof(Vector3))]
    public static Vector3 Vec3(float xyz) => new(xyz);

    [ShaderIntrinsic, Replace(ReplaceAttribute.ConstructorName, typeof(Vector2))]
    public static Vector2 Vec2(float x, float y) => new(x, y);

    [ShaderIntrinsic, Replace(nameof(Vector2.Length), typeof(Vector2))]
    public static float Length(Vector2 vector) => vector.Length();
    [ShaderIntrinsic, Replace(nameof(Vector3.Length), typeof(Vector3))]
    public static float Length(Vector3 vector) => vector.Length();
    [ShaderIntrinsic, Replace(nameof(Vector4.Length), typeof(Vector4))]
    public static float Length(Vector4 vector) => vector.Length();

    [ShaderIntrinsic, Replace(nameof(MathF.Sqrt), typeof(MathF))]
    public static float Sqrt(float x) => MathF.Sqrt(x);

    [ShaderIntrinsic, Replace(nameof(MathF.Pow), typeof(MathF))]
    public static float Pow(float x, float y) => MathF.Pow(x, y);

    [ShaderIntrinsic, Replace(nameof(Vector3.Dot), typeof(Vector3))]
    public static float Dot(Vector3 vector1, Vector3 vector2) => Vector3.Dot(vector1, vector2);

    [ShaderIntrinsic]
    [Replace(nameof(Vector3.Normalize), typeof(Vector3))]
    [Replace(nameof(VectorExtensions.Normalized), typeof(VectorExtensions))]
    public static Vector3 Normalize(Vector3 vector) => Vector3.Normalize(vector);

    [ShaderIntrinsic, Replace(ReplaceAttribute.ConstructorName, typeof(ColorF))]
    public static ColorF ColorF(float r, float g, float b, float a) => new(r, g, b, a);

    [ShaderIntrinsic, Replace(nameof(MathF.Max), typeof(MathF))]
    public static float Max(float a, float b) => MathF.Max(a, b);

    [ShaderIntrinsic, Replace(nameof(Vector3.Reflect), typeof(Vector3))]
    public static Vector3 Reflect(Vector3 vector, Vector3 normal) => Vector3.Reflect(vector, normal);

    [ShaderIntrinsic, Replace(nameof(Math.Clamp), typeof(Math))]
    public static float Clamp(float value, float min, float max) => Math.Clamp(value, min, max);

    [ShaderIntrinsic, Replace(ReplaceAttribute.MultiplyOperatorName, typeof(ColorF))]
    public static ColorF Multiply(ColorF left, ColorF right) => left * right;

    [ShaderIntrinsic, Replace(ReplaceAttribute.MultiplyOperatorName, typeof(ColorF))]
    public static ColorF Multiply(float left, ColorF right) => left * right;

    [ShaderIntrinsic, Replace(ReplaceAttribute.MultiplyOperatorName, typeof(ColorF))]
    public static ColorF Multiply(ColorF left, float right) => left * right;

    [ShaderIntrinsic, Replace(ReplaceAttribute.MultiplyOperatorName, typeof(Vector3))]
    public static Vector3 Multiply(Vector3 left, Vector3 right) => left * right;

    [ShaderIntrinsic, Replace(nameof(MathF.Sin), typeof(MathF))]
    public static float Sin(float x) => MathF.Sin(x);

    [ShaderIntrinsic, Replace(nameof(MathF.Cos), typeof(MathF))]
    public static float Cos(float x) => MathF.Cos(x);

    [ShaderIntrinsic, Replace(nameof(MathF.Tan), typeof(MathF))]
    public static float Tan(float x) => MathF.Tan(x);


    // inline hlsl, only supported on directx, will be removed
    // [ShaderIntrinsic, Replace(nameof(Hlsl), typeof(ShaderIntrinsics))]
    // public static void Hlsl(string hlsl) { throw new NotSupportedException(); }

    [ShaderIntrinsic]
    public static int AsInt(bool value) => value ? 1 : 0;

    [ShaderIntrinsic]
    public static bool AsBool(int value) => value > 0;

    // inline hlsl, only supported on directx, will be removed
    // [ShaderIntrinsic(nameof(ShaderIntrinsics.Hlsl<>), typeof(ShaderIntrinsics))]
    // public static T? Hlsl<T>(string hlsl) => default;
}