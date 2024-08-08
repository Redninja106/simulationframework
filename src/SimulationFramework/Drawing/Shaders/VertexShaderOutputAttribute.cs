using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders;

/// <summary>
/// Indicates a shader field is passed from a vertex shader to a canvas shader.
/// <para>
/// In a vertex shader, fields decorated with this attribute are outputs (and therefore must be assigned).
/// </para>
/// <para>
/// In a canvas shader, fields decorated with this attribute are inputs. 
/// </para>
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class VertexShaderOutputAttribute : Attribute 
{
}
