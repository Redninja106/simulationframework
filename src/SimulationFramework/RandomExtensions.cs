using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

public static class RandomExtensions
{
    public static Vector4 NextUnitVector4(this Random random)
    {
        Vector4 result;

        do
        {
            result = random.NextVector4();
        }
        while (result.LengthSquared() <= 1);

        return result.Normalized();
    }

    /// <summary>
    /// Returns a <see cref="Vector4"/> with random components between -1 and 1.
    /// </summary>
    public static Vector4 NextVector4(this Random random)
    {
        return new(
            random.NextSingle() * 2 - 1,
            random.NextSingle() * 2 - 1,
            random.NextSingle() * 2 - 1,
            random.NextSingle() * 2 - 1
            );
    }

    public static Vector3 NextUnitVector3(this Random random)
    {
        Vector3 result;

        do
        {
            result = random.NextVector3();
        }
        while (result.LengthSquared() <= 1);

        return result.Normalized();
    }

    /// <summary>
    /// Returns a <see cref="Vector3"/> with random components between -1 and 1.
    /// </summary>
    public static Vector3 NextVector3(this Random random)
    {
        return new(
            random.NextSingle() * 2 - 1,
            random.NextSingle() * 2 - 1,
            random.NextSingle() * 2 - 1
            );
    }

    public static Vector2 NextUnitVector2(this Random random)
    {
        Vector2 result;

        do
        {
            result = random.NextVector2();
        }
        while (result.LengthSquared() <= 1);

        return result.Normalized();
    }

    /// <summary>
    /// Returns a <see cref="Vector2"/> with random components between -1 and 1.
    /// </summary>
    public static Vector2 NextVector2(this Random random)
    {
        return new(
            random.NextSingle() * 2 - 1,
            random.NextSingle() * 2 - 1
            );
    }
}