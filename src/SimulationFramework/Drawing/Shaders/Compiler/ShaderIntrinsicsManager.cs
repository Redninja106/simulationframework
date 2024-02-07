using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler;
internal class ShaderIntrinsicsManager
{
    internal static readonly List<Type> intrinsicTypes = new()
    {
        typeof(bool),
        typeof(float),
        typeof(int),
        typeof(uint),
        typeof(byte),
        typeof(void),
        typeof(string), // for inline shader source & printfs
        typeof(Vector2),
        typeof(Vector3),
        typeof(Vector4),
        typeof(ColorF),
        typeof(Color),
        typeof(Matrix4x4),
        typeof(Matrix3x2),
    };

    private CompilationContextOLD context;


    public bool IsIntrinsic(MethodBase method) => method.GetCustomAttribute<ShaderIntrinsicAttribute>() is not null || PropertyFromAccessor(method)?.GetCustomAttribute<ShaderIntrinsicAttribute>() is not null;
    public bool IsIntrinsic(Type type) => intrinsicTypes.Contains(type) || type.GetCustomAttribute<ShaderIntrinsicAttribute>() is not null;

    private static PropertyInfo PropertyFromAccessor(MethodBase info)
    {
        var type = info.DeclaringType ?? throw new Exception();
        foreach (var property in type.GetProperties())
        {
            if (property.GetAccessors().Contains(info))
                return property;
        }
        return null;
    }
}
