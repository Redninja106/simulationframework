using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders;

/// <summary>
/// Specifies that a field is a shader output.
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class OutputAttribute : Attribute
{
    public OutputSemantic Semantic { get; }
    public string? LinkageName { get; init; }

    public OutputAttribute() : this(OutputSemantic.None)
    {
    }

    public OutputAttribute(OutputSemantic semantic)
    {
        Semantic = semantic;
    }
}
