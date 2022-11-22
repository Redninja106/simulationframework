using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class ShaderInputAttribute : Attribute
{
    public InputSemantic Semantic { get; }
    public string? LinkageName { get; init; }
    public ShaderVariableInterpolation Interpolation { get; init; }

    public ShaderInputAttribute() : this(InputSemantic.None)
    {
    }

    public ShaderInputAttribute(InputSemantic semantic)
    {
        Semantic = semantic;
    }
}