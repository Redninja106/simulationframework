using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
internal class ReplaceAttribute : Attribute
{
    public const string ConstructorName = ".ctor";
    public const string MultiplyOperatorName = "op_Multiply";

    public string MethodName { get; }
    public Type MethodType { get; }

    public ReplaceAttribute(string methodName, Type methodType)
    {
        MethodName = methodName;
        MethodType = methodType;
    }
}
