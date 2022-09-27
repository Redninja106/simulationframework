using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class ShaderInAttribute : Attribute
{
    public InSemantic Semantic { get; }
    public string? LinkageName { get; init; }
    public ShaderVariableInterpolation Interpolation { get; init; }

    public ShaderInAttribute() : this(InSemantic.None)
    {
    }

    public ShaderInAttribute(InSemantic semantic)
    {
        Semantic = semantic;
    }
}