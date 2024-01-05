using System.Numerics;

namespace SimulationFramework;

/// <summary>
/// Extends <see cref="Random"/> with methods to generate SimulationFramework types.
/// </summary>
public static class RandomExtensions
{
    /// <summary>
    /// Generates a random color with an alpha of one.
    /// </summary>
    /// <param name="random">The <see cref="Random"/> instance.</param>
    public static ColorF NextColorF(this Random random)
    {
        return random.NextColorF(1f);
    }

    /// <summary>
    /// Generates a random color with the specified alpha.
    /// </summary>
    /// <param name="random">The <see cref="Random"/> instance.</param>
    /// <param name="alpha">The alpha of the color to generate</param>
    public static ColorF NextColorF(this Random random, float alpha)
    {
        return new(
            random.NextSingle(), 
            random.NextSingle(), 
            random.NextSingle(), 
            alpha
            );
    }

    /// <summary>
    /// Generates a random color with an alpha of one.
    /// </summary>
    /// <param name="random">The <see cref="Random"/> instance.</param>
    public static Color NextColor(this Random random)
    {
        return random.NextColor((byte)random.Next(256));
    }

    /// <summary>
    /// Generates a random color with the specified alpha.
    /// </summary>
    /// <param name="random">The <see cref="Random"/> instance.</param>
    /// <param name="alpha">The alpha of the color to generate</param>
    public static Color NextColor(this Random random, byte alpha)
    {
        return new(
            (byte)random.Next(256),
            (byte)random.Next(256),
            (byte)random.Next(256),
            alpha
            );
    }

    /// <summary>
    /// Generates a random Vector2 with X and Y components both greater than or equal to 0.0, and less than 1.0.
    /// </summary>
    /// <param name="random">The <see cref="Random"/> instance.</param>
    public static Vector2 NextVector2(this Random random)
    {
        return new(random.NextSingle(), random.NextSingle());
    }

    /// <summary>
    /// Generates a random Vector2 with a random direction and length of 1.0.
    /// </summary>
    /// <param name="random">The <see cref="Random"/> instance.</param>
    public static Vector2 NextUnitVector2(this Random random)
    {
        return Angle.ToVector(random.NextSingle() * MathF.Tau);
    }

    /// <summary>
    /// Generates a random float greater than or equal to 0.0, and less than the specified value.
    /// </summary>
    /// <param name="random">The <see cref="Random"/> instance.</param>
    /// <param name="maxValue">The maximum value.</param>
    public static float NextSingle(this Random random, float maxValue)
    {
        return random.NextSingle() * maxValue;
    }

    /// <summary>
    /// Generates a random float greater than or equal to <paramref name="minValue"></paramref>, and less than <paramref name="maxValue"/>.
    /// </summary>
    /// <param name="random">The <see cref="Random"/> instance.</param>
    /// <param name="minValue">The minimum value.</param>
    /// <param name="maxValue">The maximum value.</param>
    public static float NextSingle(this Random random, float minValue, float maxValue)
    {
        return minValue + random.NextSingle() * (maxValue - minValue);
    }
}
