using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
internal class ShaderInterceptAttribute : Attribute
{
    public const string ConstructorName = ".ctor";
    public const string MultiplyOperatorName = "op_Multiply";
    public const string DivideOperatorName = "op_Division";
    public const string AddOperatorName = "op_Addition";
    public const string SubtractOperatorName = "op_Subtraction";
    public const string GetItemName = "get_Item";
    public const string SetItemName = "set_Item";

    public string MemberName { get; }
    public Type MethodType { get; }
    public InterceptKind Kind { get; }

    public ShaderInterceptAttribute(string memberName, Type methodType, InterceptKind kind = InterceptKind.Method)
    {
        MemberName = memberName;
        MethodType = methodType;
        Kind = kind;
    }

    public MethodBase? GetMethod(Type[] types)
    {
        return MemberName switch
        {
            ConstructorName => MethodType.GetConstructor(types),
            GetItemName => MethodType.GetMethod("get_Item", types[1..]),
            SetItemName => MethodType.GetMethod("set_Item", types[1..]),
            _ when Kind is InterceptKind.Property => MethodType.GetProperty(MemberName)!.GetMethod,
            _ => MethodType.GetMethod(MemberName, types),
        };
    }

    public enum InterceptKind
    {
        Method,
        Property,
    }
}
