using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders;

/// <summary>
/// Specifies that a type or method is a shader intrinsic, and should not be compiled by the shader compiler.
/// </summary>
[AttributeUsage(TARGETS, AllowMultiple = false, Inherited = true)]
public sealed class ShaderIntrinsicAttribute : Attribute
{
    private const AttributeTargets TARGETS = 
        AttributeTargets.Property |
        AttributeTargets.Class |
        AttributeTargets.Method |
        AttributeTargets.Interface |
        AttributeTargets.Struct;
}