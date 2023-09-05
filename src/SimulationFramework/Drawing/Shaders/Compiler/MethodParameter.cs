using ImGuiNET;
using SimulationFramework.Drawing.Shaders.Compiler.ILDisassembler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler;
public class MethodParameter
{
    private readonly ParameterInfo? parameterInfo;
    private readonly string name;
    private readonly MethodParameterModifier modifier;

    public ParameterInfo? ParameterInfo => parameterInfo;
    public string Name => name;
    public MethodParameterModifier Modifier => modifier;
    public Type ParameterType { get; set; }
    private MethodDisassembly method;

    internal MethodParameter(MethodDisassembly method, ParameterInfo? parameterInfo)
    {
        this.method = method;
        this.parameterInfo = parameterInfo;
        ParameterType = parameterInfo?.ParameterType ?? method.Method.DeclaringType;

        if (ParameterType is not null && ParameterType.IsByRef)
            ParameterType = ParameterType.GetElementType();

        modifier = GetModifier(parameterInfo);
        name = parameterInfo?.Name ?? "self";
    }

    private static MethodParameterModifier GetModifier(ParameterInfo? paramInfo)
    {
        if (paramInfo is null)
        {
            return MethodParameterModifier.Ref;
        }

        if (paramInfo.IsOut)
        {
            if (paramInfo.IsIn)
            {
                return MethodParameterModifier.Ref;
            }

            return MethodParameterModifier.Out;
        }

        return MethodParameterModifier.In;
    }
}
