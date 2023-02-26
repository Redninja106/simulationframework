﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Represents a 32-bit RGBA color.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct Color : IEquatable<Color>
{
    private const uint RED_MASK   = 0xFF000000;
    private const uint BLUE_MASK  = 0x00FF0000;
    private const uint GREEN_MASK = 0x0000FF00;
    private const uint ALPHA_MASK = 0x000000FF;

    private const int RED_SHIFT = 24;
    private const int BLUE_SHIFT = 16;
    private const int GREEN_SHIFT = 8;
    private const int ALPHA_SHIFT = 0;


    /// <summary>
    /// The 8-bit value of the alpha component of this color.
    /// </summary>
    public byte A 
    { 
        readonly get => (byte)((Value & ALPHA_MASK) >> ALPHA_SHIFT); 
        init => Value = (Value & ~ALPHA_MASK) | ((uint)value << ALPHA_SHIFT); 
    }

    /// <summary>
    /// The 8-bit value of the blue component of this color.
    /// </summary>
    public byte B
    {
        readonly get => (byte)((Value & BLUE_MASK) >> BLUE_SHIFT);
        init => Value = (Value & ~BLUE_MASK) | ((uint)value << BLUE_SHIFT);
    }

    /// <summary>
    /// The 8-bit value of the green component of this color.
    /// </summary>
    public byte G 
    {
        readonly get => (byte)((Value & GREEN_MASK) >> GREEN_SHIFT);
        init => Value = (Value & ~GREEN_MASK) | ((uint)value << GREEN_SHIFT);
    }

    /// <summary>
    /// The 8-bit value of the red component of this color.
    /// </summary>
    public byte R 
    { 
        readonly get => (byte)((Value & RED_MASK) >> RED_SHIFT);
        init => Value = (Value & ~RED_MASK) | ((uint)value << RED_SHIFT);
    }


    /// <summary>
    /// The 32-bit, RGBA value of this color.
    /// </summary>
    public uint Value { readonly get; init; }

    /// <summary>
    /// Creates a new color.
    /// </summary>
    public Color()
    {
        Unsafe.SkipInit(out this);
        Value = 0;
    }

    /// <summary>
    /// Creates a color from the provided uint RGBA value.
    /// </summary>
    /// <param name="value">The RGBA color value, where R is the least significant byte, and A is the most significant byte.</param>
    public Color(uint value) : this()
    {
        EndianHelper.MakeLttleEndian(ref value);
        this.Value = value;
    }

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
    /// <param name="values">The RGB values of the color.</param>
    public Color(Vector3 values) : this(values.X, values.Y, values.Z)
    {
    }

    /// <summary>
    /// Creates a new color from the provided RGBA values.
    /// </summary>
    /// <param name="values">The RGBA values of the color.</param>
    public Color(Vector4 values) : this(values.X, values.Y, values.Z, values.W)
    {
    }

    /// <summary>
    /// Creates a new color from the provided RGB values, and an alpha component of 255.
    /// </summary>
    /// <param name="r">The red component of the color.</param>
    /// <param name="g">The green component of the color.</param>
    /// <param name="b">The blue component of the color.</param>
    public Color(float r, float g, float b) : this(r, g, b, 1.0f)
    {
    }

    /// <summary>
    /// Creates a new color from the provided RGBA values.
    /// </summary>
    /// <param name="r">The red component of the color.</param>
    /// <param name="g">The green component of the color.</param>
    /// <param name="b">The blue component of the color.</param>
    /// <param name="a">The alpha component of the color.</param>
    public Color(float r, float g, float b, float a)
    {
        this.R = (byte)(MathHelper.Normalize(r) * 255);
        this.G = (byte)(MathHelper.Normalize(g) * 255);
        this.B = (byte)(MathHelper.Normalize(b) * 255);
        this.A = (byte)(MathHelper.Normalize(a) * 255);
    }

    /// <summary>
    /// Converts a <see cref="Color"/> to a <see cref="System.Drawing.Color"/>.
    /// </summary>
    /// <param name="value">The color to convert.</param>
    public static implicit operator System.Drawing.Color(Color value) => System.Drawing.Color.FromArgb(value.A, value.R, value.G, value.B);

    /// <summary>
    /// Converts a <see cref="System.Drawing.Color"/> to a <see cref="Color"/>.
    /// </summary>
    /// <param name="value">The color to convert.</param>
    public static implicit operator Color(System.Drawing.Color value) => new(value.R, value.G, value.B, value.A);

    /// <summary>
    /// Converts a <see cref="Color"/> to a <see langword="uint"/>.
    /// </summary>
    /// <param name="color">The color to convert.</param>
    public static explicit operator uint(Color color) => color.Value;

    /// <summary>
    /// Converts a <see langword="uint"/> to a <see cref="Color"/>.
    /// </summary>
    /// <param name="value"></param>
    public static explicit operator Color(uint value) => new(value);

    /// <summary>
    /// Indicates that this color is the same as another.
    /// </summary>
    /// <param name="other">The other color.</param>
    /// <returns><see langword="true"/> if <paramref name="other"/> and this instance represent the same value, otherwise <see langword="false"/>.</returns>
    public bool Equals(Color other)
    {
        return other.Value == this.Value;
    }

    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Color color && this.Equals(color);
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

    /// <summary>
    /// Indicates if two <see cref="Color"/> objects are equal.
    /// </summary>
    /// <param name="left">The first color.</param>
    /// <param name="right">The second color.</param>
    /// <returns><see langword="true"/> if the <see cref="Color"/> objects are equal, otherwise <see langword="false"/>.</returns>
    public static bool operator ==(Color left, Color right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Indicates if two <see cref="Color"/> objects are not equal.
    /// </summary>
    /// <param name="left">The first color.</param>
    /// <param name="right">The second color.</param>
    /// <returns><see langword="true"/> if the <see cref="Color"/> objects are not equal, otherwise <see langword="false"/>.</returns>
    public static bool operator !=(Color left, Color right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Returns this <see cref="Color"/> as a <see cref="Vector3"/>, with its R, G, and B values as X, Y, and Z, respectively.
    /// </summary>
    /// <returns>The converted <see cref="Vector3"/>.</returns>
    public Vector3 ToVector3()
    {
        return new Vector3(this.R / 255f, this.G / 255f, this.B / 255f);
    }

    /// <summary>
    /// Converts this <see cref="Color"/> to <see cref="Vector4"/>, with its R, G, B, and A values as X, Y, Z, and W, respectively.
    /// </summary>
    /// <returns>The converted <see cref="Vector4"/>.</returns>
    public Vector4 ToVector4()
    {
        return new Vector4(ToVector3(), this.A / 255f);
    }

    /// <summary>
    /// Computes the values of this <see cref="Color"/> in the HSV color space.
    /// </summary>
    /// <returns>The computed values, where X is hue, Y is saturation, and Z is value.</returns>
    public Vector3 ToHSV()
    {
        // https://www.cs.rit.edu/~ncs/color/t_convert.html

        float min, max, delta;
        float hue, sat, value;

        var (r, g, b) = this.ToVector3();


        min = MathF.Min(r, MathF.Min(g, b));
        max = MathF.Max(r, MathF.Max(g, b));

        value = max;

        delta = max - min;

        if (max != 0)
        {
            sat = delta / max;
        }
        else
        {
            sat = 0;
            hue = 0;
            return new(hue, sat, value);
        }

        if (r == max)
        {
            hue = (g - b) / delta;
        }
        else if (g == max)
        {
            hue = 2 + (b - r) / delta;
        }
        else
        {
            hue = 4 + (r - g) / delta;
        }

        hue *= (1f/6);

        if (hue < 0)
            hue++;

        return new(hue, sat, value);
    }

    /// <summary>
    /// Computes the values of this <see cref="Color"/> in the HSV color space.
    /// </summary>
    /// <returns>The computed values, where X is hue, Y is saturation, Z is value, and W is alpha.</returns>
    public Vector4 ToHSVA()
    {
        return new(ToHSV(), this.A / 255f);
    }

    /// <summary>
    /// Creates a color given a hue, saturation, and value.
    /// </summary>
    /// <param name="hue">The hue of the color. This must be between 0 and 1.</param>
    /// <param name="saturation">The saturation of the color. This must be between 0 and 1.</param>
    /// <param name="value">The value (brightness) of the color. This must be between 0 and 1.</param>
    /// <returns>The created color.</returns>
    public static Color FromHSV(float hue, float saturation, float value)
    {
        return FromHSV(hue, saturation, value, 1.0f);
    }

    /// <summary>
    /// Creates a color given a hue, saturation, value, and alpha.
    /// </summary>
    /// <param name="hue">The hue of the color, in degress. This must be between 0 and 360.</param>
    /// <param name="saturation">The saturation of the color. This must be between 0 and 1.</param>
    /// <param name="value">The value (brightness) of the color. This must be between 0 and 1.</param>
    /// <param name="alpha">The alpha  of the color. This must be between 0 and 1.</param>
    /// <returns>The created color.</returns>
    public static Color FromHSV(float hue, float saturation, float value, float alpha)
    {
        hue = MathHelper.Normalize(hue);
        saturation = MathHelper.Normalize(saturation);
        value = MathHelper.Normalize(value);
        alpha = MathHelper.Normalize(alpha);

        // https://en.wikipedia.org/wiki/HSL_and_HSV#HSV_to_RGB_alternative
        float Channel(float n)
        {
            float k = (n + hue * 6f) % 6f;
            return value - (value * saturation * MathF.Max(0, MathF.Min(k, MathF.Min(4 - k, 1))));
        }

        float r = Channel(5);
        float g = Channel(3);
        float b = Channel(1);
        return new Color(r, g, b, alpha);
    }

    /// <summary>
    /// Linearly interpolates between two <see cref="Color"/> values in RGBA space.
    /// </summary>
    /// <param name="color1">The first color.</param>
    /// <param name="color2">The second color.</param>
    /// <param name="t">The interpolation amount, from 0.0 to 1.0.</param>
    /// <returns>The interpolated color.</returns>
    public static Color Lerp(Color color1, Color color2, float t)
    {
        return ColorF.Lerp(color1.ToColorF(), color2.ToColorF(), t).ToColor();
    }

    /// <summary>
    /// Linearly interpolates between two <see cref="Color"/> values in HSV space.
    /// </summary>
    /// <param name="color1">The first color.</param>
    /// <param name="color2">The second color.</param>
    /// <param name="t">The interpolation amount, from 0.0 to 1.0.</param>
    /// <returns>The interpolated color.</returns>
    public static Color LerpHSV(Color color1, Color color2, float t)
    {
        return ColorF.LerpHSV(color1.ToColorF(), color2.ToColorF(), t).ToColor();
    }

    /// <summary>
    /// Attempts to converts to a color from its string representation.
    /// </summary>
    /// <param name="value">The string representation of the color.</param>
    /// <param name="result">The value of the color.</param>
    /// <returns><see langword="true"/> if the string represents a color, otherwise <see langword="false"/>.</returns>
    public static bool TryParse(ReadOnlySpan<char> value, out Color result)
    {
        byte r, g, b, a;
        result = default;

        // consume valid prefixes
        if (value.StartsWith("#"))
            value = value[1..];
        if (value.StartsWith("0x"))
            value = value[2..];

        if (value.Length < 2 || !byte.TryParse(value[..2], NumberStyles.AllowHexSpecifier, null, out r))
        {
            return false;
        }

        value = value[2..];

        if (value.Length < 2 || !byte.TryParse(value[..2], NumberStyles.AllowHexSpecifier, null, out g))
        {
            return false;
        }

        value = value[2..];

        if (value.Length < 2 || !byte.TryParse(value[..2], NumberStyles.AllowHexSpecifier, null, out b))
        {
            return false;
        }

        value = value[2..];

        if (value.IsEmpty)
        {
            a = 255;
        }
        else
        {
            if (value.Length < 2 || !byte.TryParse(value[..2], NumberStyles.AllowHexSpecifier, null, out a))
            {
                return false;
            }

            value = value[2..];
        }

        if (!value.IsEmpty)
            return false;

        result = new(r, g, b, a);
        return true;
    }

    /// <summary>
    /// Converts to a color from its string representation.
    /// </summary>
    /// <param name="value">The string representation of the color.</param>
    /// <returns>The color that the string represents.</returns>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public static Color Parse(ReadOnlySpan<char> value)
    {
        if (TryParse(value, out Color color))
        {
            return color;
        }
        else
        {
            throw Exceptions.ParseFailed(nameof(value));
        }
    }

    /// <summary>
    /// Converts this <see cref="Color"/> as a <see cref="ColorF"/>.
    /// </summary>
    /// <returns>The converted <see cref="ColorF"/>.</returns>
    public ColorF ToColorF()
    {
        return new(ToVector4());
    }
}