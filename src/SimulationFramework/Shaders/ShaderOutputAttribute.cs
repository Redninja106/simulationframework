using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class ShaderOutputAttribute : Attribute
{
    public OutputSemantic Semantic { get; }
    public string? LinkageName { get; init; }

    public ShaderOutputAttribute() : this(OutputSemantic.None)
    {
    }

    public ShaderOutputAttribute(OutputSemantic semantic)
    {
        Semantic = semantic;
    }
}
