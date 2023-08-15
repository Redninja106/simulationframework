using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler;
internal static class TypeUtilities
{
    private static readonly Dictionary<Type, int> primitives = new()
    {
        { typeof(sbyte), sizeof(sbyte) },
        { typeof(byte), sizeof(byte) },
        { typeof(short), sizeof(short) },
        { typeof(ushort), sizeof(ushort) },
        { typeof(int), sizeof(int) },
        { typeof(uint), sizeof(uint) },
        { typeof(long), sizeof(long) },
        { typeof(ulong), sizeof(ulong) },
        { typeof(float), sizeof(float) },
        { typeof(double), sizeof(double) },
    };

    public static bool IsPrimitive(Type type)
    {
        return primitives.ContainsKey(type);
    }

    public static int SizeOf(Type type)
    {
        Debug.Assert(type.IsValueType);

        if (IsPrimitive(type))
        {
            return primitives[type];
        }

        int size = 0;
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (var field in fields)
        {
            size += SizeOf(field.FieldType);
        }

        return size;
    }
}
