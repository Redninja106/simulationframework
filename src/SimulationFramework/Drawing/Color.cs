using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;

/// <summary>
/// Represents a 32-bit RGBA color.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct Color : IEquatable<Color>
{
    /// <summary>
    /// The 8-bit value of the alpha component of this color.
    /// </summary>
    public byte A { readonly get; init; }

    /// <summary>
    /// The 8-bit value of the blue component of this color.
    /// </summary>
    public byte B { readonly get; init; }

    /// <summary>
    /// The 8-bit value of the green component of this color.
    /// </summary>
    public byte G { readonly get; init; }

    /// <summary>
    /// The 8-bit value of the red component of this color.
    /// </summary>
    public byte R { readonly get; init; }

    /// <summary>
    /// The 32-bit, RGBA value of this color.
    /// </summary>
    public uint Value
    {
        readonly get
        {
            return Unsafe.As<Color, uint>(ref Unsafe.AsRef(in this));
        }
        init
        {
            this = Unsafe.As<uint, Color>(ref value);
        }
    }
    /// <summary>
    /// Creates a new color instance 
    /// </summary>
    public Color()
    {
        Unsafe.SkipInit(out this);
        Value = 0;
    }

    /// <summary>
    /// Creates a color from the provided uint RGBA value.
    /// </summary>
    /// <param name="value"></param>
    public Color(uint value) : this() => this.Value = value;

    /// <summary>
    /// Creates a new color with the provided RGB values, and an alpha of 255.
    /// </summary>
    /// <param name="r">The red component of the color.</param>
    /// <param name="g">The green component of the color.</param>
    /// <param name="b">The blue component of the color.</param>
    public Color(byte r, byte g, byte b) : this(r, g, b, 255) { }

    /// <summary>
    /// Creates a new color with the provided values.
    /// </summary>
    /// <param name="r">The red component of the color.</param>
    /// <param name="g">The green component of the color.</param>
    /// <param name="b">The blue component of the color.</param>
    /// <param name="a">The alpha component of the color.</param>
    public Color(byte r, byte g, byte b, byte a) : this()
    {
        this.R = r;
        this.G = g;
        this.B = b;
        this.A = a;
    }

    /// <summary>
    /// Creates a new color from the provided RGB values.
    /// </summary>
    public Color(Vector3 values) : this(values.X, values.Y, values.Z)
    {
    }

    /// <summary>
    /// Creates a new color from the provided RGBA values.
    /// </summary>
    public Color(Vector4 values) : this(values.X, values.Y, values.Z, values.W)
    {
    }

    /// <summary>
    /// Creates a new color from the provided RGB values.
    /// </summary>
    public Color(float r, float g, float b) : this(r, g, b, 1.0f)
    {
    }

    /// <summary>
    /// Creates a new color from the provided RGBA values.
    /// </summary>
    public Color(float r, float g, float b, float a) : this((byte)(r * 255), (byte)(g * 255), (byte)(b * 255), (byte)(a * 255))
    {
    }

    public static implicit operator Color((byte r, byte g, byte b) values) => new(values.r, values.g, values.b);
    public static implicit operator Color((byte r, byte g, byte b, byte a) values) => new(values.r, values.g, values.b, values.a);

    public static implicit operator System.Drawing.Color(Color value) => System.Drawing.Color.FromArgb(value.A, value.R, value.G, value.B);
    public static implicit operator Color(System.Drawing.Color value) => new(value.R, value.G, value.B, value.A);

    public static explicit operator uint(Color color) => color.Value;
    public static explicit operator Color(uint value) => new(value);

    public bool Equals(Color other)
    {
        return other.Value == this.Value;
    }

    public override bool Equals([NotNullWhen(true)] object obj)
    {
        if (obj is Color color)
            return this.Equals(color);

        return false;
    }

    /// <summary>
    /// Returns the hexadecimal-formatted value of this color.
    /// </summary>
    public override string ToString()
    {
        return $"#{Value:x8}";
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return unchecked((int)Value);
    }

    public static bool operator ==(Color left, Color right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Color left, Color right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Returns this color as a Vector3, with its R, G, and B values as X, Y, and Z, respectively.
    /// </summary>
    public Vector3 ToVector3()
    {
        return new Vector3(this.R / 255f, this.G / 255f, this.B / 255f);
    }

    /// <summary>
    /// Returns this color as a Vector3, with its R, G, B, and A values as X, Y, Z, and W, respectively.
    /// </summary>
    public Vector4 ToVector4()
    {
        return new Vector4(this.R / 255f, this.G / 255f, this.B / 255f, this.A / 255f);
    }
}