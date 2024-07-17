using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace SimulationFramework;

/// <summary>
/// A RGBA floating point color.
/// </summary>
public partial struct ColorF : IEquatable<ColorF>
{
    /// <summary>
    /// The R component of the color.
    /// </summary>
    public float R;

    /// <summary>
    /// The G component of the color.
    /// </summary>
    public float G;

    /// <summary>
    /// The B component of the color.
    /// </summary>
    public float B;

    /// <summary>
    /// The A component of the color.
    /// </summary>
    public float A;

    /// <summary>
    /// Creates a new <see cref="ColorF"/> with default values.
    /// </summary>
    public ColorF()
    {
        this = default;
    }

    /// <summary>
    /// Creates a new <see cref="ColorF"/>.
    /// </summary>
    /// <param name="rgb">The RGB values of the color.</param>
    public ColorF(Vector3 rgb)
    {
        this.R = rgb.X;
        this.G = rgb.Y;
        this.B = rgb.Z;
        this.A = 1f;
    }

    /// <summary>
    /// Creates a new <see cref="ColorF"/>.
    /// </summary>
    /// <param name="r">The R component of the color.</param>
    /// <param name="g">The G component of the color.</param>
    /// <param name="b">The B component of the color.</param>
    public ColorF(float r, float g, float b)
    {
        this.R = r;
        this.G = g;
        this.B = b;
        this.A = 1f;
    }

    /// <summary>
    /// Creates a new <see cref="ColorF"/>.
    /// </summary>
    /// <param name="rgba">The RGBA values of the color.</param>
    public ColorF(Vector4 rgba)
    {
        this.R = rgba.X;
        this.G = rgba.Y;
        this.B = rgba.Z;
        this.A = rgba.W;
    }

    /// <summary>
    /// Creates a new <see cref="ColorF"/>.
    /// </summary>
    /// <param name="r">The R component of the color.</param>
    /// <param name="g">The G component of the color.</param>
    /// <param name="b">The B component of the color.</param>
    /// <param name="a">The A component of the color.</param>
    public ColorF(float r, float g, float b, float a)
    {
        this.R = r;
        this.G = g;
        this.B = b;
        this.A = a;
    }

    /// <summary>
    /// Returns a normalized version of this color, with each channel clamped between 0.0 and 1.0.
    /// </summary>
    public ColorF Normalized()
    {
        float r = MathHelper.Normalize(this.R);
        float g = MathHelper.Normalize(this.G);
        float b = MathHelper.Normalize(this.B);
        float a = MathHelper.Normalize(this.A);
        return new(r, g, b, a);
    }

    /// <inheritdoc/>
    public bool Equals(ColorF other)
    {
        return this.R == other.R && this.G == other.G && this.B == other.B && this.A == other.A;
    }

    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is ColorF other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(R, G, B, A);
    }

    /// <inheritdoc/>
    public override string? ToString()
    {
        return $"{{{R}, {G}, {B}, {A}}}";
    }

    /// <summary>
    /// Linearly interpolates between two <see cref="ColorF"/> values in RGBA space.
    /// </summary>
    /// <param name="color1">The first color.</param>
    /// <param name="color2">The second color.</param>
    /// <param name="t">The interpolation amount, from 0.0 to 1.0.</param>
    /// <returns>The interpolated color.</returns>
    public static ColorF Lerp(ColorF color1, ColorF color2, float t)
    {
        Vector4 vec1 = color1.ToVector4();
        Vector4 vec2 = color2.ToVector4();

        Vector4 result = Vector4.Lerp(vec1, vec2, t);
        return new(result);
    }

    /// <summary>
    /// Linearly interpolates between two <see cref="ColorF"/> values in HSV space.
    /// </summary>
    /// <param name="color1">The first color.</param>
    /// <param name="color2">The second color.</param>
    /// <param name="t">The interpolation amount, from 0.0 to 1.0.</param>
    /// <returns>The interpolated color.</returns>
    public static ColorF LerpHSV(ColorF color1, ColorF color2, float t)
    {
        var hsva1 = color1.ToHSVA();
        var hsva2 = color2.ToHSVA();

        var result = Vector4.Lerp(hsva1, hsva2, t);
        return new(result);
    }

    /// <summary>
    /// Returns this <see cref="Color"/> as a <see cref="Vector3"/>, with its R, G, and B values as X, Y, and Z, respectively.
    /// </summary>
    /// <returns>The converted <see cref="Vector3"/>.</returns>
    public Vector3 ToVector3()
    {
        return new(this.R, this.G, this.B);
    }

    /// <summary>
    /// Converts this <see cref="Color"/> to <see cref="Vector4"/>, with its R, G, B, and A values as X, Y, Z, and W, respectively.
    /// </summary>
    /// <returns>The converted <see cref="Vector4"/>.</returns>
    public Vector4 ToVector4()
    {
        return new(ToVector3(), this.A);
    }

    /// <summary>
    /// Converts this <see cref="ColorF"/> as a <see cref="Color"/>.
    /// </summary>
    /// <returns>The converted <see cref="Color"/>.</returns>
    public Color ToColor()
    {
        return new Color(
            (byte)(this.R * 255f),
            (byte)(this.G * 255f),
            (byte)(this.B * 255f),
            (byte)(this.A * 255f)
            );
    }

    /// <summary>
    /// Creates a color given a hue, saturation, and value.
    /// </summary>
    /// <param name="hue">The hue of the color. This must be between 0 and 1.</param>
    /// <param name="saturation">The saturation of the color. This must be between 0 and 1.</param>
    /// <param name="value">The value (brightness) of the color. This must be between 0 and 1.</param>
    /// <returns>The created color.</returns>
    public static ColorF FromHSV(float hue, float saturation, float value)
    {
        return FromHSV(hue, saturation, value, 1.0f);
    }

    /// <summary>
    /// Creates a color given a hue, saturation, value, and alpha.
    /// </summary>
    /// <param name="hue">The hue of the color. This must be between 0 and 1.</param>
    /// <param name="saturation">The saturation of the color. This must be between 0 and 1.</param>
    /// <param name="value">The value (brightness) of the color. This must be between 0 and 1.</param>
    /// <param name="alpha">The alpha  of the color. This must be between 0 and 1.</param>
    /// <returns>The created color.</returns>
    public static ColorF FromHSV(float hue, float saturation, float value, float alpha)
    {
        hue = MathHelper.Normalize(hue);
        saturation = MathHelper.Normalize(saturation);
        value = MathHelper.Normalize(value);
        alpha = MathHelper.Normalize(alpha);

        // https://en.wikipedia.org/wiki/HSL_and_HSV#HSV_to_RGB_alternative
        static float Channel(float n, float hue, float saturation, float value)
        {
            float k = (n + hue * 6f) % 6f;
            return value - (value * saturation * MathF.Max(0, MathF.Min(k, MathF.Min(4 - k, 1))));
        }

        float r = Channel(5, hue, saturation, value);
        float g = Channel(3, hue, saturation, value);
        float b = Channel(1, hue, saturation, value);
        return new ColorF(r, g, b, alpha);
    }

    /// <summary>
    /// Computes the values of this <see cref="ColorF"/> in the HSV color space.
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

        hue *= (1f / 6);

        if (hue < 0)
            hue++;

        return new(hue, sat, value);
    }

    /// <summary>
    /// Creates a <see cref="ColorF"/> from a <see cref="Color"/> instance.
    /// </summary>
    public static ColorF FromColor(Color color)
    {
        return new ColorF(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
    }

    /// <summary>
    /// Computes the values of this <see cref="ColorF"/> in the HSV color space.
    /// </summary>
    /// <returns>The computed values, where X is hue, Y is saturation, Z is value, W is alpha.</returns>
    public Vector4 ToHSVA()
    {
        return new(ToHSV(), 1.0f);
    }

    /// <summary>
    /// Indicates if two <see cref="ColorF"/> objects are equal.
    /// </summary>
    /// <param name="left">The first color.</param>
    /// <param name="right">The second color.</param>
    /// <returns><see langword="true"/> if the <see cref="ColorF"/> objects are equal, otherwise <see langword="false"/>.</returns>
    public static bool operator ==(ColorF left, ColorF right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Indicates if two <see cref="ColorF"/> objects are not equal.
    /// </summary>
    /// <param name="left">The first color.</param>
    /// <param name="right">The second color.</param>
    /// <returns><see langword="true"/> if the <see cref="ColorF"/> objects are not equal, otherwise <see langword="false"/>.</returns>
    public static bool operator !=(ColorF left, ColorF right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Multiplies the values in the color by a constant.
    /// </summary>
    public static ColorF operator *(float left, ColorF right)
    {
        return right * left;
    }

    /// <summary>
    /// Multiplies the values in the color by a constant.
    /// </summary>
    public static ColorF operator *(ColorF left, float right)
    {
        return new(left.ToVector4() * right);
    }

    /// <summary>
    /// Divides the values in the color by a constant.
    /// </summary>
    public static ColorF operator /(float left, ColorF right)
    {
        return right / left;
    }

    /// <summary>
    /// Divides the values in the color by a constant.
    /// </summary>
    public static ColorF operator /(ColorF left, float right)
    {
        return new(left.ToVector4() / right);
    }

    /// <summary>
    /// Multiples the channels in the color by the channels of another.
    /// </summary>
    public static ColorF operator *(ColorF left, ColorF right)
    {
        return new(left.ToVector4() * right.ToVector4());
    }

    /// <summary>
    /// Adds the channels of two colors.
    /// </summary>
    public static ColorF operator +(ColorF left, ColorF right) => new(left.ToVector4() + right.ToVector4());

    /// <summary>
    /// Subtracts the channels of two colors.
    /// </summary>
    public static ColorF operator -(ColorF left, ColorF right) => new(left.ToVector4() - right.ToVector4());

    /// <summary>
    /// Divides the channels of two colors.
    /// </summary>
    public static ColorF operator /(ColorF left, ColorF right) => new(left.ToVector4() / right.ToVector4());
}
