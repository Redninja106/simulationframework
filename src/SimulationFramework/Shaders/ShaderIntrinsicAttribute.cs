using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders;

[AttributeUsage(AttributeTargets.Method)]
internal class ShaderIntrinsicAttribute : Attribute
{
    public const string ConstructorMethodName = ".ctor";

    public string MethodName { get; }
    public Type MethodType { get; }

    public ShaderIntrinsicAttribute(string methodName, Type methodType)
    {
        MethodName = methodName;
        MethodType = methodType;
    }
}
