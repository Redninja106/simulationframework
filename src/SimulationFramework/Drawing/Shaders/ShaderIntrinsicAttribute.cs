using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders;

/// <summary>
/// Specifies that a type or method is a shader intrinsic, and should not be compiled by the shader compiler.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
internal sealed class ShaderIntrinsicAttribute : Attribute
{
    public Type CustomMethodHandler { get; set; }
}