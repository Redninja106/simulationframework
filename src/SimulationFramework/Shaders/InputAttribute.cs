using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders;

/// <summary>
/// Specifies that a field is a shader input.
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class InputAttribute : Attribute
{
    public InputSemantic Semantic { get; }
    public string? LinkageName { get; init; }
    public bool Interpolated { get; init; }
    public Type SourceType { get; init; }

    public InputAttribute() : this(InputSemantic.None)
    {
    }

    public InputAttribute(InputSemantic semantic)
    {
        Semantic = semantic;
    }
}