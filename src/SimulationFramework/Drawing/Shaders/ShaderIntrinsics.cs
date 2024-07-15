using SimulationFramework.Drawing.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders;

public static class ShaderIntrinsics
{
    [ShaderIntrinsic, ShaderIntercept(nameof(Transform), typeof(Vector4))]
    public static Vector4 Transform(Vector4 vector, Matrix4x4 matrix) => Vector4.Transform(vector, matrix);

    [ShaderIntrinsic]
    public static int AsInt(bool value) => value ? 1 : 0;

    [ShaderIntrinsic]
    public static bool AsBool(int value) => value > 0;

    #region Derivatives

    [ShaderIntrinsic]
    public static T DDX<T>(T value) => throw new NotImplementedException();

    [ShaderIntrinsic]
    public static T DDY<T>(T value) => throw new NotImplementedException();

    #endregion

    #region Constructors

    [ShaderIntrinsic, ShaderIntercept(ShaderInterceptAttribute.ConstructorName, typeof(Vector4))]
    public static Vector4 Vec4(Vector3 xyz, float w) => new(xyz, w);

    [ShaderIntrinsic, ShaderIntercept(ShaderInterceptAttribute.ConstructorName, typeof(Vector4))]
    public static Vector4 Vec4(Vector2 xy, float z, float w) => new(xy.X, xy.Y, z, w);

    [ShaderIntrinsic, ShaderIntercept(ShaderInterceptAttribute.ConstructorName, typeof(Vector4))]
    public static Vector4 Vec4(float x, float y, float z, float w) => new(x, y, z, w);

    [ShaderIntrinsic, ShaderIntercept(ShaderInterceptAttribute.ConstructorName, typeof(Vector3))]
    public static Vector3 Vec3(float x, float y, float z) => new(x, y, z);

    [ShaderIntrinsic, ShaderIntercept(ShaderInterceptAttribute.ConstructorName, typeof(Vector3))]
    public static Vector3 Vec3(Vector2 xy, float z) => new(xy, z);
    [ShaderIntrinsic, ShaderIntercept(ShaderInterceptAttribute.ConstructorName, typeof(Vector3))]
    public static Vector3 Vec3(float xyz) => new(xyz);

    [ShaderIntrinsic, ShaderIntercept(ShaderInterceptAttribute.ConstructorName, typeof(Vector2))]
    public static Vector2 Vec2(float xy) => new(xy, xy);

    [ShaderIntrinsic, ShaderIntercept(ShaderInterceptAttribute.ConstructorName, typeof(Vector2))]
    public static Vector2 Vec2(float x, float y) => new(x, y);

    [ShaderIntrinsic, ShaderIntercept(ShaderInterceptAttribute.ConstructorName, typeof(ColorF))]
    public static ColorF ColorF(float r, float g, float b, float a) => new(r, g, b, a);

    #endregion

    #region Float Constants

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(float.IsNaN), typeof(float))]
    public static bool IsNaN(float value) => float.IsNaN(value);
    
    [ShaderIntrinsic]
    [ShaderIntercept(nameof(float.IsInfinity), typeof(float))]
    public static bool IsInfinity(float value) => float.IsInfinity(value);
    
    [ShaderIntrinsic]
    [ShaderIntercept(nameof(float.IsPositiveInfinity), typeof(float))]
    public static bool IsPositiveInfinity(float value) => float.IsPositiveInfinity(value);
    
    [ShaderIntrinsic]
    [ShaderIntercept(nameof(float.IsNegativeInfinity), typeof(float))]
    public static bool IsNegativeInfinity(float value) => float.IsNegativeInfinity(value);

    #endregion

    #region Addition

    [ShaderIntrinsic]
    [ShaderIntercept(ShaderInterceptAttribute.AdditionOperatorName, typeof(Vector2))]
    public static Vector2 Add(Vector2 left, Vector2 right) => left + right;

    [ShaderIntrinsic]
    [ShaderIntercept(ShaderInterceptAttribute.AdditionOperatorName, typeof(Vector3))]
    public static Vector3 Add(Vector3 left, Vector3 right) => left + right;

    [ShaderIntrinsic]
    [ShaderIntercept(ShaderInterceptAttribute.AdditionOperatorName, typeof(Vector4))]
    public static Vector4 Add(Vector4 left, Vector4 right) => left + right;

    #endregion

    #region Subtraction

    [ShaderIntrinsic]
    [ShaderIntercept(ShaderInterceptAttribute.SubtractionOperatorName, typeof(Vector2))]
    public static Vector2 Subtract(Vector2 left, Vector2 right) => left - right;

    [ShaderIntrinsic]
    [ShaderIntercept(ShaderInterceptAttribute.SubtractionOperatorName, typeof(Vector3))]
    public static Vector3 Subtract(Vector3 left, Vector3 right) => left - right;

    [ShaderIntrinsic]
    [ShaderIntercept(ShaderInterceptAttribute.SubtractionOperatorName, typeof(Vector4))]
    public static Vector4 Subtract(Vector4 left, Vector4 right) => left - right;

    #endregion

    #region Multiply

    [ShaderIntrinsic, ShaderIntercept(ShaderInterceptAttribute.MultiplyOperatorName, typeof(ColorF))]
    public static ColorF Multiply(ColorF left, ColorF right) => left * right;

    [ShaderIntrinsic, ShaderIntercept(ShaderInterceptAttribute.MultiplyOperatorName, typeof(ColorF))]
    public static ColorF Multiply(float left, ColorF right) => left * right;

    [ShaderIntrinsic, ShaderIntercept(ShaderInterceptAttribute.MultiplyOperatorName, typeof(ColorF))]
    public static ColorF Multiply(ColorF left, float right) => left * right;

    [ShaderIntrinsic]
    [ShaderIntercept(ShaderInterceptAttribute.MultiplyOperatorName, typeof(Vector2))]
    public static Vector2 Multiply(float left, Vector2 right) => left * right;

    [ShaderIntrinsic]
    [ShaderIntercept(ShaderInterceptAttribute.MultiplyOperatorName, typeof(Vector2))]
    public static Vector2 Multiply(Vector2 left, float right) => left * right;

    [ShaderIntrinsic]
    [ShaderIntercept(ShaderInterceptAttribute.MultiplyOperatorName, typeof(Vector2))]
    public static Vector2 Multiply(Vector2 left, Vector2 right) => left * right;

    [ShaderIntrinsic]
    [ShaderIntercept(ShaderInterceptAttribute.MultiplyOperatorName, typeof(Vector3))]
    public static Vector3 Multiply(float left, Vector3 right) => left * right;

    [ShaderIntrinsic]
    [ShaderIntercept(ShaderInterceptAttribute.MultiplyOperatorName, typeof(Vector3))]
    public static Vector3 Multiply(Vector3 left, float right) => left * right;

    [ShaderIntrinsic]
    [ShaderIntercept(ShaderInterceptAttribute.MultiplyOperatorName, typeof(Vector3))]
    public static Vector3 Multiply(Vector3 left, Vector3 right) => left * right;

    [ShaderIntrinsic]
    [ShaderIntercept(ShaderInterceptAttribute.MultiplyOperatorName, typeof(Vector4))]
    public static Vector4 Multiply(float left, Vector4 right) => left * right;

    [ShaderIntrinsic]
    [ShaderIntercept(ShaderInterceptAttribute.MultiplyOperatorName, typeof(Vector4))]
    public static Vector4 Multiply(Vector4 left, float right) => left * right;

    [ShaderIntrinsic]
    [ShaderIntercept(ShaderInterceptAttribute.MultiplyOperatorName, typeof(Vector4))]
    public static Vector4 Multiply(Vector4 left, Vector4 right) => left * right;

    #endregion

    #region Divide

    [ShaderIntrinsic]
    [ShaderIntercept(ShaderInterceptAttribute.DivisionOperatorName, typeof(Vector2))]
    public static Vector2 Divide(Vector2 left, float right) => left / right;

    [ShaderIntrinsic]
    [ShaderIntercept(ShaderInterceptAttribute.DivisionOperatorName, typeof(Vector2))]
    public static Vector2 Divide(Vector2 left, Vector2 right) => left / right;

    [ShaderIntrinsic]
    [ShaderIntercept(ShaderInterceptAttribute.DivisionOperatorName, typeof(Vector3))]
    public static Vector3 Divide(Vector3 left, float right) => left / right;

    [ShaderIntrinsic]
    [ShaderIntercept(ShaderInterceptAttribute.DivisionOperatorName, typeof(Vector3))]
    public static Vector3 Divide(Vector3 left, Vector3 right) => left / right;

    [ShaderIntrinsic]
    [ShaderIntercept(ShaderInterceptAttribute.DivisionOperatorName, typeof(Vector4))]
    public static Vector4 Divide(Vector4 left, float right) => left / right;

    [ShaderIntrinsic]
    [ShaderIntercept(ShaderInterceptAttribute.DivisionOperatorName, typeof(Vector4))]
    public static Vector4 Divide(Vector4 left, Vector4 right) => left / right;

    #endregion

    #region Vector Elements

    [ShaderIntrinsic]
    [ShaderIntercept(ShaderInterceptAttribute.GetItemName, typeof(Vector2))]
    public static float GetElement(Vector2 vector, int index) => vector[index]; 

    [ShaderIntrinsic]
    [ShaderIntercept(ShaderInterceptAttribute.GetItemName, typeof(Vector3))]
    public static float GetElement(Vector3 vector, int index) => vector[index]; 

    [ShaderIntrinsic]
    [ShaderIntercept(ShaderInterceptAttribute.GetItemName, typeof(Vector4))]
    public static float GetElement(Vector4 vector, int index) => vector[index];

    [ShaderIntrinsic]
    [ShaderIntercept(ShaderInterceptAttribute.SetItemName, typeof(Vector2))]
    public static void SetElement(Vector2 vector, int index, float element) => vector[index] = element;

    [ShaderIntrinsic]
    [ShaderIntercept(ShaderInterceptAttribute.SetItemName, typeof(Vector3))]
    public static void SetElement(Vector3 vector, int index, float element) => vector[index] = element;

    [ShaderIntrinsic]
    [ShaderIntercept(ShaderInterceptAttribute.SetItemName, typeof(Vector4))]
    public static void SetElement(Vector4 vector, int index, float element) => vector[index] = element;

    #endregion

    #region Abs

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(MathF.Abs), typeof(MathF))]
    public static float Abs(float value) => MathF.Abs(value);

    [ShaderIntrinsic]
    public static Vector2 Abs(Vector2 value) => System.Numerics.Vector2.Abs(value);

    [ShaderIntrinsic]
    public static Vector3 Abs(Vector3 value) => Vector3.Abs(value);

    [ShaderIntrinsic]
    public static Vector4 Abs(Vector4 value) => System.Numerics.Vector4.Abs(value);
    #endregion

    #region Acos
    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Acos), typeof(MathF))]
    public static float Acos(float value) => MathF.Acos(value);
    [ShaderIntrinsic]
    public static Vector2 Acos(Vector2 value) => new(MathF.Acos(value.X), MathF.Acos(value.Y));
    [ShaderIntrinsic]
    public static Vector3 Acos(Vector3 value) => new(MathF.Acos(value.X), MathF.Acos(value.Y), MathF.Acos(value.Z));
    [ShaderIntrinsic]
    public static Vector4 Acos(Vector4 value) => new(MathF.Acos(value.X), MathF.Acos(value.Y), MathF.Acos(value.Z), MathF.Acos(value.W));
    #endregion

    #region Acosh

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Acosh), typeof(MathF))]
    public static float Acosh(float value) => MathF.Acosh(value);
    [ShaderIntrinsic]
    public static Vector2 Acosh(Vector2 value) => new(MathF.Acosh(value.X), MathF.Acosh(value.Y));
    [ShaderIntrinsic]
    public static Vector3 Acosh(Vector3 value) => new(MathF.Acosh(value.X), MathF.Acosh(value.Y), MathF.Acosh(value.Z));
    [ShaderIntrinsic]
    public static Vector4 Acosh(Vector4 value) => new(MathF.Acosh(value.X), MathF.Acosh(value.Y), MathF.Acosh(value.Z), MathF.Acosh(value.W));

    #endregion

    #region Acos

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Asin), typeof(MathF))]
    public static float Asin(float value) => MathF.Asin(value);
    [ShaderIntrinsic]
    public static Vector2 Asin(Vector2 value) => new(MathF.Asin(value.X), MathF.Asin(value.Y));
    [ShaderIntrinsic]
    public static Vector3 Asin(Vector3 value) => new(MathF.Asin(value.X), MathF.Asin(value.Y), MathF.Asin(value.Z));
    [ShaderIntrinsic]
    public static Vector4 Asin(Vector4 value) => new(MathF.Asin(value.X), MathF.Asin(value.Y), MathF.Asin(value.Z), MathF.Asin(value.W));

    #endregion

    #region Asinh

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Asinh), typeof(MathF))]
    public static float Asinh(float value) => MathF.Asinh(value);
    [ShaderIntrinsic]
    public static Vector2 Asinh(Vector2 value) => new(MathF.Asinh(value.X), MathF.Asinh(value.Y));
    [ShaderIntrinsic]
    public static Vector3 Asinh(Vector3 value) => new(MathF.Asinh(value.X), MathF.Asinh(value.Y), MathF.Asinh(value.Z));
    [ShaderIntrinsic]
    public static Vector4 Asinh(Vector4 value) => new(MathF.Asinh(value.X), MathF.Asinh(value.Y), MathF.Asinh(value.Z), MathF.Asinh(value.W));

    #endregion

    #region Atan

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Atan), typeof(MathF))]
    public static float Atan(float value) => MathF.Atan(value);
    [ShaderIntrinsic]
    public static Vector2 Atan(Vector2 value) => new(MathF.Atan(value.X), MathF.Atan(value.Y));
    [ShaderIntrinsic]
    public static Vector3 Atan(Vector3 value) => new(MathF.Atan(value.X), MathF.Atan(value.Y), MathF.Atan(value.Z));
    [ShaderIntrinsic]
    public static Vector4 Atan(Vector4 value) => new(MathF.Atan(value.X), MathF.Atan(value.Y), MathF.Atan(value.Z), MathF.Atan(value.W));

    #endregion

    #region Atan2

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Atan2), typeof(MathF))]
    public static float Atan2(float y, float x) => MathF.Atan2(y, x);
    [ShaderIntrinsic]
    public static Vector2 Atan2(Vector2 y, Vector2 x) => new(MathF.Atan2(y.X, x.X), MathF.Atan2(y.Y, x.Y));
    [ShaderIntrinsic]
    public static Vector3 Atan2(Vector3 y, Vector3 x) => new(MathF.Atan2(y.X, x.X), MathF.Atan2(y.Y, x.Y), MathF.Atan2(y.Z, x.Z));
    [ShaderIntrinsic]
    public static Vector4 Atan2(Vector4 y, Vector4 x) => new(MathF.Atan2(y.X, x.X), MathF.Atan2(y.Y, x.Y), MathF.Atan2(y.Z, x.Z), MathF.Atan2(y.W, x.W));

    #endregion

    #region Atanh

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Atanh), typeof(MathF))]
    public static float Atanh(float value) => MathF.Atanh(value);
    [ShaderIntrinsic]
    public static Vector2 Atanh(Vector2 value) => new(MathF.Atanh(value.X), MathF.Atanh(value.Y));
    [ShaderIntrinsic]
    public static Vector3 Atanh(Vector3 value) => new(MathF.Atanh(value.X), MathF.Atanh(value.Y), MathF.Atanh(value.Z));
    [ShaderIntrinsic]
    public static Vector4 Atanh(Vector4 value) => new(MathF.Atanh(value.X), MathF.Atanh(value.Y), MathF.Atanh(value.Z), MathF.Atanh(value.W));

    #endregion

    #region Ceil

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Clamp), typeof(MathF))]
    public static float Ceiling(float value) => MathF.Ceiling(value);
    [ShaderIntrinsic]
    public static Vector2 Ceiling(Vector2 value) => new(MathF.Ceiling(value.X), MathF.Ceiling(value.Y));
    [ShaderIntrinsic]
    public static Vector3 Ceiling(Vector3 value) => new(MathF.Ceiling(value.X), MathF.Ceiling(value.Y), MathF.Ceiling(value.Z));
    [ShaderIntrinsic]
    public static Vector4 Ceiling(Vector4 value) => new(MathF.Ceiling(value.X), MathF.Ceiling(value.Y), MathF.Ceiling(value.Z), MathF.Ceiling(value.W));

    #endregion

    #region Clamp

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Clamp), typeof(Math))]
    public static float Clamp(float value, float min, float max) => Math.Clamp(value, min, max);

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Clamp), typeof(Vector2))]
    public static Vector2 Clamp(Vector2 value, Vector2 min, Vector2 max) => System.Numerics.Vector2.Clamp(value, min, max);

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Clamp), typeof(Vector3))]
    public static Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max) => Vector3.Clamp(value, min, max);

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Clamp), typeof(Vector4))]
    public static Vector4 Clamp(Vector4 value, Vector4 min, Vector4 max) => System.Numerics.Vector4.Clamp(value, min, max);

    #endregion

    #region Cos

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(MathF.Cos), typeof(MathF))]
    public static float Cos(float x) => MathF.Cos(x);
    [ShaderIntrinsic]
    public static Vector2 Cos(Vector2 value) => new(MathF.Cos(value.X), MathF.Cos(value.Y));
    [ShaderIntrinsic]
    public static Vector3 Cos(Vector3 value) => new(MathF.Cos(value.X), MathF.Cos(value.Y), MathF.Cos(value.Z));
    [ShaderIntrinsic]
    public static Vector4 Cos(Vector4 value) => new(MathF.Cos(value.X), MathF.Cos(value.Y), MathF.Cos(value.Z), MathF.Cos(value.W));

    #endregion

    #region Cosh

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(MathF.Cosh), typeof(MathF))]
    public static float Cosh(float x) => MathF.Cos(x);
    [ShaderIntrinsic]
    public static Vector2 Cosh(Vector2 value) => new(MathF.Cosh(value.X), MathF.Cosh(value.Y));
    [ShaderIntrinsic]
    public static Vector3 Cosh(Vector3 value) => new(MathF.Cosh(value.X), MathF.Cosh(value.Y), MathF.Cosh(value.Z));
    [ShaderIntrinsic]
    public static Vector4 Cosh(Vector4 value) => new(MathF.Cosh(value.X), MathF.Cosh(value.Y), MathF.Cosh(value.Z), MathF.Cosh(value.W));

    #endregion

    #region Cross

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Cross), typeof(MathHelper))]
    public static float Cross(Vector2 x, Vector2 y) => MathHelper.Cross(x, y);

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Cross), typeof(Vector3))]
    public static Vector3 Cross(Vector3 x, Vector3 y) => Vector3.Cross(x, y);

    #endregion

    #region Distance

    [ShaderIntrinsic]
    public static float Distance(float x, float y) => MathF.Abs(y - x);

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Distance), typeof(Vector2))]
    public static float Distance(Vector2 x, Vector2 y) => System.Numerics.Vector2.Distance(x, y);

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Distance), typeof(Vector3))]
    public static float Distance(Vector3 x, Vector3 y) => Vector3.Distance(x, y);

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Distance), typeof(Vector4))]
    public static float Distance(Vector4 x, Vector4 y) => System.Numerics.Vector4.Distance(x, y);

    #endregion

    #region Dot

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Vector2.Dot), typeof(Vector2))]
    public static float Dot(Vector2 vector1, Vector2 vector2) => System.Numerics.Vector2.Dot(vector1, vector2);

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Vector3.Dot), typeof(Vector3))]
    public static float Dot(Vector3 vector1, Vector3 vector2) => Vector3.Dot(vector1, vector2);

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Vector4.Dot), typeof(Vector4))]
    public static float Dot(Vector4 vector1, Vector4 vector2) => System.Numerics.Vector4.Dot(vector1, vector2);

    #endregion

    #region Exp

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(MathF.Exp), typeof(MathF))]
    public static float Exp(float x) => MathF.Exp(x);
    [ShaderIntrinsic]
    public static Vector2 Exp(Vector2 value) => new(MathF.Exp(value.X), MathF.Exp(value.Y));
    [ShaderIntrinsic]
    public static Vector3 Exp(Vector3 value) => new(MathF.Exp(value.X), MathF.Exp(value.Y), MathF.Exp(value.Z));
    [ShaderIntrinsic]
    public static Vector4 Exp(Vector4 value) => new(MathF.Exp(value.X), MathF.Exp(value.Y), MathF.Exp(value.Z), MathF.Exp(value.W));

    #endregion

    #region Floor

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(MathF.Floor), typeof(MathF))]
    public static float Floor(float x) => MathF.Floor(x);
    [ShaderIntrinsic]
    public static Vector2 Floor(Vector2 value) => new(MathF.Floor(value.X), MathF.Floor(value.Y));
    [ShaderIntrinsic]
    public static Vector3 Floor(Vector3 value) => new(MathF.Floor(value.X), MathF.Floor(value.Y), MathF.Floor(value.Z));
    [ShaderIntrinsic]
    public static Vector4 Floor(Vector4 value) => new(MathF.Floor(value.X), MathF.Floor(value.Y), MathF.Floor(value.Z), MathF.Floor(value.W));

    #endregion

    #region Fract

    [ShaderIntrinsic]
    public static float Fract(float x) => x - Floor(x);
    [ShaderIntrinsic]
    public static Vector2 Fract(Vector2 x) => x - Floor(x);
    [ShaderIntrinsic]
    public static Vector3 Fract(Vector3 x) => x - Floor(x);
    [ShaderIntrinsic]
    public static Vector4 Fract(Vector4 x) => x - Floor(x);

    #endregion

    #region Length

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Length), typeof(Vector2))]
    public static float Length(Vector2 vector) => vector.Length();

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Vector3.Length), typeof(Vector3))]
    public static float Length(Vector3 vector) => vector.Length();

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Vector4.Length), typeof(Vector4))]
    public static float Length(Vector4 vector) => vector.Length();

    #endregion

    #region Log

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Log), typeof(MathF))]
    public static float Log(float x) => MathF.Log(x);

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Log), typeof(Vector2))]
    public static Vector2 Log(Vector2 vector) => new(MathF.Log(vector.X), MathF.Log(vector.Y));

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Log), typeof(Vector3))]
    public static Vector3 Log(Vector3 vector) => new(MathF.Log(vector.X), MathF.Log(vector.Y), MathF.Log(vector.Z));

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Log), typeof(Vector2))]
    public static Vector4 Log(Vector4 vector) => new(MathF.Log(vector.X), MathF.Log(vector.Y), MathF.Log(vector.Z), MathF.Log(vector.W));

    #endregion

    #region Max

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Max), typeof(Math))]
    [ShaderIntercept(nameof(Max), typeof(MathF))]
    public static float Max(float a, float b) => MathF.Max(a, b);

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Max), typeof(Vector2))]
    public static Vector2 Max(Vector2 a, Vector2 b) => System.Numerics.Vector2.Max(a, b);

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Max), typeof(Vector3))]
    public static Vector3 Max(Vector3 a, Vector3 b) => Vector3.Max(a, b);

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Max), typeof(Vector4))]
    public static Vector4 Max(Vector4 a, Vector4 b) => System.Numerics.Vector4.Max(a, b);

    #endregion

    #region Min

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Min), typeof(Math))]
    public static float Min(float a, float b) => MathF.Min(a, b);

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Min), typeof(Vector2))]
    public static Vector2 Min(Vector2 a, Vector2 b) => System.Numerics.Vector2.Min(a, b);

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Min), typeof(Vector3))]
    public static Vector3 Min(Vector3 a, Vector3 b) => Vector3.Min(a, b);

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Min), typeof(Vector4))]
    public static Vector4 Min(Vector4 a, Vector4 b) => System.Numerics.Vector4.Min(a, b);

    #endregion

    #region Lerp

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Lerp), typeof(MathHelper))]
    [ShaderIntercept(nameof(Lerp), typeof(float))]
    public static float Lerp(float a, float b, float t) => float.Lerp(a, b, t);

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Lerp), typeof(Vector2))]
    public static Vector2 Lerp(Vector2 a, Vector2 b, float t) => System.Numerics.Vector2.Lerp(a, b, t);

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Lerp), typeof(Vector3))]
    public static Vector3 Lerp(Vector3 a, Vector3 b, float t) => Vector3.Lerp(a, b, t);

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Lerp), typeof(Vector4))]
    public static Vector4 Lerp(Vector4 a, Vector4 b, float t) => System.Numerics.Vector4.Lerp(a, b, t);

    #endregion

    #region Normalize

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Normalize), typeof(Vector2))]
    [ShaderIntercept(nameof(VectorExtensions.Normalized), typeof(VectorExtensions))]
    public static Vector2 Normalize(Vector2 vector) => System.Numerics.Vector2.Normalize(vector);

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Normalize), typeof(Vector3))]
    [ShaderIntercept(nameof(VectorExtensions.Normalized), typeof(VectorExtensions))]
    public static Vector3 Normalize(Vector3 vector) => Vector3.Normalize(vector);

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Normalize), typeof(Vector4))]
    [ShaderIntercept(nameof(VectorExtensions.Normalized), typeof(VectorExtensions))]
    public static Vector4 Normalize(Vector4 vector) => System.Numerics.Vector4.Normalize(vector);

    #endregion

    #region Pow

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Pow), typeof(MathF))]
    public static float Pow(float a, float b) => MathF.Pow(a, b);

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Pow), typeof(MathF))]
    public static Vector2 Pow(Vector2 a, Vector2 b) => new(MathF.Pow(a.X, b.X), MathF.Pow(a.Y, b.Y));

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Pow), typeof(MathF))]
    public static Vector3 Pow(Vector3 a, Vector3 b) => new(MathF.Pow(a.X, b.X), MathF.Pow(a.Y, b.Y), MathF.Pow(a.Z, b.Z));

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Pow), typeof(MathF))]
    public static Vector4 Pow(Vector4 a, Vector4 b) => new(MathF.Pow(a.X, b.X), MathF.Pow(a.Y, b.Y), MathF.Pow(a.Z, b.Z), MathF.Pow(a.W, b.W));

    #endregion

    #region Reflect

    public static Vector2 Reflect(Vector2 vector, Vector2 normal) => System.Numerics.Vector2.Reflect(vector, normal);
    public static Vector3 Reflect(Vector3 vector, Vector3 normal) => Vector3.Reflect(vector, normal);

    #endregion

    #region Refract

    public static Vector2 Refract(Vector2 vector, Vector2 normal, float index)
    {
        throw new NotImplementedException();
    }

    public static Vector3 Refract(Vector3 vector, Vector3 normal, float index)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Round

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Round), typeof(MathF))]
    public static float Round(float x) => MathF.Round(x);

    [ShaderIntrinsic]
    public static Vector2 Round(Vector2 vector) => new(MathF.Round(vector.X), MathF.Round(vector.Y));

    [ShaderIntrinsic]
    public static Vector3 Round(Vector3 vector) => new(MathF.Round(vector.X), MathF.Round(vector.Y), MathF.Round(vector.Z));

    [ShaderIntrinsic]
    public static Vector4 Round(Vector4 vector) => new(MathF.Round(vector.X), MathF.Round(vector.Y), MathF.Round(vector.Z), MathF.Round(vector.W));

    #endregion

    #region Sign

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(MathF.Sign), typeof(MathF))]
    public static float Sign(float x) => MathF.Sign(x);
    
    [ShaderIntrinsic]
    public static Vector2 Sign(Vector2 vector) => new(MathF.Round(vector.X), MathF.Round(vector.Y));
    
    [ShaderIntrinsic]
    public static Vector3 Sign(Vector3 vector) => new(MathF.Round(vector.X), MathF.Round(vector.Y), MathF.Round(vector.Z));
    
    [ShaderIntrinsic]
    public static Vector4 Sign(Vector4 vector) => new(MathF.Round(vector.X), MathF.Round(vector.Y), MathF.Round(vector.Z), MathF.Round(vector.W));

    #endregion

    #region Sin

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(MathF.Sin), typeof(MathF))]
    public static float Sin(float x) => MathF.Sin(x);
    [ShaderIntrinsic]
    public static Vector2 Sin(Vector2 value) => new(MathF.Sin(value.X), MathF.Sin(value.Y));
    [ShaderIntrinsic]
    public static Vector3 Sin(Vector3 value) => new(MathF.Sin(value.X), MathF.Sin(value.Y), MathF.Sin(value.Z));
    [ShaderIntrinsic]
    public static Vector4 Sin(Vector4 value) => new(MathF.Sin(value.X), MathF.Sin(value.Y), MathF.Sin(value.Z), MathF.Sin(value.W));

    #endregion

    #region Sinh

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(MathF.Sinh), typeof(MathF))]
    public static float Sinh(float x) => MathF.Sinh(x);
    [ShaderIntrinsic]
    public static Vector2 Sinh(Vector2 value) => new(MathF.Sinh(value.X), MathF.Sinh(value.Y));
    [ShaderIntrinsic]
    public static Vector3 Sinh(Vector3 value) => new(MathF.Sinh(value.X), MathF.Sinh(value.Y), MathF.Sinh(value.Z));
    [ShaderIntrinsic]
    public static Vector4 Sinh(Vector4 value) => new(MathF.Sinh(value.X), MathF.Sinh(value.Y), MathF.Sinh(value.Z), MathF.Sinh(value.W));

    #endregion

    #region Mod 

    [ShaderIntrinsic]
    public static float Mod(float x, float y) => x - y * Floor(x / y);
    [ShaderIntrinsic]
    public static Vector2 Mod(Vector2 value1, Vector2 value2) => value1 - value2 * Floor(value1 / value2);
    [ShaderIntrinsic]
    public static Vector3 Mod(Vector3 value1, Vector3 value2) => value1 - value2 * Floor(value1 / value2);
    [ShaderIntrinsic]
    public static Vector4 Mod(Vector4 value1, Vector4 value2) => value1 - value2 * Floor(value1 / value2);
    [ShaderIntrinsic]
    public static Vector2 Mod(Vector2 value1, float value2) => value1 - value2 * Floor(value1 / value2);
    [ShaderIntrinsic]
    public static Vector3 Mod(Vector3 value1, float value2) => value1 - value2 * Floor(value1 / value2);
    [ShaderIntrinsic]
    public static Vector4 Mod(Vector4 value1, float value2) => value1 - value2 * Floor(value1 / value2);

    #endregion

    #region SmoothStep

    [ShaderIntrinsic]
    public static float SmoothStep(float a, float b, float t)
    {
        float tc = Clamp((t - a) / (b - a), 0.0f, 1.0f);
        return tc * tc * (3.0f - 2.0f * tc);
    }

    [ShaderIntrinsic]
    public static Vector2 SmoothStep(Vector2 a, Vector2 b, float t) => new(SmoothStep(a.X, b.X, t), SmoothStep(a.Y, b.Y, t));

    [ShaderIntrinsic]
    public static Vector3 SmoothStep(Vector3 a, Vector3 b, float t) => new(SmoothStep(a.X, b.X, t), SmoothStep(a.Y, b.Y, t), SmoothStep(a.Z, b.Z, t));

    [ShaderIntrinsic]
    public static Vector4 SmoothStep(Vector4 a, Vector4 b, float t) => new(SmoothStep(a.X, b.X, t), SmoothStep(a.Y, b.Y, t), SmoothStep(a.Z, b.Z, t), SmoothStep(a.W, b.W, t));

    #endregion

    #region Sqrt

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Sqrt), typeof(MathF))]
    public static float Sqrt(float x) => MathF.Sqrt(x);

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Vector2.SquareRoot), typeof(Vector2))]
    public static Vector2 Sqrt(Vector2 value) => Vector2.SquareRoot(value);

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Vector3.SquareRoot), typeof(Vector3))]
    public static Vector3 Sqrt(Vector3 value) => Vector3.SquareRoot(value);

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(Vector4.SquareRoot), typeof(Vector4))]
    public static Vector4 Sqrt(Vector4 value) => Vector4.SquareRoot(value);

    #endregion

    #region Tan

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(MathF.Tan), typeof(MathF))]
    public static float Tan(float x) => MathF.Tan(x);
    [ShaderIntrinsic]
    public static Vector2 Tan(Vector2 value) => new(MathF.Tan(value.X), MathF.Tan(value.Y));
    [ShaderIntrinsic]
    public static Vector3 Tan(Vector3 value) => new(MathF.Tan(value.X), MathF.Tan(value.Y), MathF.Tan(value.Z));
    [ShaderIntrinsic]
    public static Vector4 Tan(Vector4 value) => new(MathF.Tan(value.X), MathF.Tan(value.Y), MathF.Tan(value.Z), MathF.Tan(value.W));

    #endregion

    #region Tanh

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(MathF.Tanh), typeof(MathF))]
    public static float Tanh(float x) => MathF.Tanh(x);
    [ShaderIntrinsic]
    public static Vector2 Tanh(Vector2 value) => new(MathF.Tanh(value.X), MathF.Tanh(value.Y));
    [ShaderIntrinsic]
    public static Vector3 Tanh(Vector3 value) => new(MathF.Tanh(value.X), MathF.Tanh(value.Y), MathF.Tanh(value.Z));
    [ShaderIntrinsic]
    public static Vector4 Tanh(Vector4 value) => new(MathF.Tanh(value.X), MathF.Tanh(value.Y), MathF.Tanh(value.Z), MathF.Tanh(value.W));

    #endregion

    #region Truncate

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(MathF.Truncate), typeof(MathF))]
    public static float Truncate(float x) => MathF.Truncate(x);

    [ShaderIntrinsic]
    public static Vector2 Truncate(Vector2 value) => new(MathF.Truncate(value.X), MathF.Truncate(value.Y));

    [ShaderIntrinsic]
    public static Vector3 Truncate(Vector3 value) => new(MathF.Truncate(value.X), MathF.Truncate(value.Y), MathF.Truncate(value.Z));

    [ShaderIntrinsic]
    public static Vector4 Truncate(Vector4 value) => new(MathF.Truncate(value.X), MathF.Truncate(value.Y), MathF.Truncate(value.Z), MathF.Truncate(value.W));

    #endregion

    #region Textures

    [ShaderIntrinsic]
    public static ColorF TextureSampleUV(ITexture texture, Vector2 uv)
    {
        throw new NotImplementedException();
    }

    [ShaderIntrinsic]
    public static ColorF TextureSample(ITexture texture, Vector2 position)
    {
        throw new NotImplementedException();
    }

    [ShaderIntrinsic]
    public static ColorF TextureLoad(ITexture texture, int x, int y)
    {
        return texture.GetPixel(x, y).ToColorF();
    }

    [ShaderIntrinsic]
    public static void TextureStore(ITexture texture, int x, int y, ColorF color)
    {
        texture.GetPixel(x, y) = color.ToColor();
    }

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(ITexture.Width), typeof(ITexture), ShaderInterceptAttribute.InterceptKind.Property)]
    public static int TextureWidth(this ITexture texture) => texture.Width;

    [ShaderIntrinsic]
    [ShaderIntercept(nameof(ITexture.Height), typeof(ITexture), ShaderInterceptAttribute.InterceptKind.Property)]
    public static int TextureHeight(ITexture texture) => texture.Height;

    #endregion

    #region Buffers

    [ShaderIntrinsic]
    public static int BufferLength(object buffer)
    {
        if (buffer is Array arr)
            return arr.Length;

        throw new ArgumentException(null, nameof(buffer));
    }

    [ShaderIntrinsic]
    public static T BufferLoad<T>(object buffer, int element)
    {
        if (buffer is T[] arr)
        {
            return arr[element];
        }

        throw new ArgumentException(null, nameof(buffer));
    }

    [ShaderIntrinsic]
    public static void BufferStore<T>(object buffer, int element, T value)
    {
        if (buffer is T[] arr)
        {
            arr[element] = value;
            return;
        }

        throw new ArgumentException(null, nameof(buffer));
    }

    #endregion
}