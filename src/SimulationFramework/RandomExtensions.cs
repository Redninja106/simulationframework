using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Extends <see cref="Random"/> with methods to generate SimulationFramework types.
/// </summary>
public static class RandomExtensions
{
    public static ColorF NextColorF(this Random random)
    {
        return random.NextColorF(random.NextSingle());
    }

    public static ColorF NextColorF(this Random random, float alpha)
    {
        return new(
            random.NextSingle(), 
            random.NextSingle(), 
            random.NextSingle(), 
            alpha
            );
    }


    public static Color NextColor(this Random random)
    {
        return random.NextColor((byte)random.Next(256));
    }

    public static Color NextColor(this Random random, byte alpha)
    {
        return new(
            (byte)random.Next(256),
            (byte)random.Next(256),
            (byte)random.Next(256),
            alpha
            );
    }

    public static Vector2 NextVector2(this Random random)
    {
        return new(random.NextSingle(), random.NextSingle());
    }

    public static Vector2 NextUnitVector2(this Random random)
    {
        return Angle.ToVector(random.NextSingle() * MathF.Tau);
    }

    public static float NextSingle(this Random random, float maxValue)
    {
        return random.NextSingle() * maxValue;
    }

    public static float NextSingle(this Random random, float minValue, float maxValue)
    {
        return minValue + random.NextSingle() * (maxValue - minValue);
    }
}
