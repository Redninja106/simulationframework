using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Translation;
internal static class IntrinsicTypes
{
    private static Type[] intrinsicTypeHandlers = [
        typeof(int),
        typeof(float),
        typeof(Vector2),
        typeof(Vector3),
        typeof(Vector4),
        typeof(ColorF),
        typeof(Matrix4x4),
    ];

    public static bool IsIntrinsic(Type type)
    {
        return intrinsicTypeHandlers.Contains(type);
    }
}
