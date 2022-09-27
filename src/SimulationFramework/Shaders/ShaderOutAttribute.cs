using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class ShaderOutAttribute : Attribute
{
    public OutSemantic Semantic { get; }
    public string? LinkageName { get; init; }

    public ShaderOutAttribute() : this(OutSemantic.None)
    {
    }

    public ShaderOutAttribute(OutSemantic semantic)
    {
        Semantic = semantic;
    }
}
