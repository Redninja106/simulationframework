using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders;

/// <summary>
/// Specifies that a value is a shader uniform, meaning its value does not change during a single invocation of the shader.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class UniformAttribute : Attribute
{
}