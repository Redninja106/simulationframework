using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders;

/// <summary>
/// Specifies the thread group size of a compute shader. If this attribute is not provided, the thread group size is assumed to be (1, 1, 1).
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class ThreadGroupSizeAttribute : Attribute
{
    /// <summary>
    /// The width of the thread group. Must be greater than 0 and less than 
    /// or equal to the value of <see cref="GraphicsCapabilities.MaxThreadGroupWidth"/> on 
    /// <see cref="Graphics.Capabilities"/>.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// The height of the thread group. Must be greater than 0 and less than 
    /// or equal to the value of <see cref="GraphicsCapabilities.MaxThreadGroupHeight"/> on 
    /// <see cref="Graphics.Capabilities"/>.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// The depth of the thread group. Must be greater than 0 and less than 
    /// or equal to the value of <see cref="GraphicsCapabilities.MaxThreadGroupDepth"/> on 
    /// <see cref="Graphics.Capabilities"/>.
    /// <para>
    /// Note: the maximum value for this is usually much lower than that of <see cref="Width"/> and <see cref="Height"/>.
    /// </para>
    /// </summary>
    public int Depth { get; }

    /// <summary>
    /// Creates a new instance of the <see cref="ThreadGroupSizeAttribute"/> class.
    /// </summary>
    /// <param name="width">
    /// The width of the thread group. Must be greater than 0 and less than or equal to
    /// the value of <see cref="GraphicsCapabilities.MaxThreadGroupWidth"/> on 
    /// <see cref="Graphics.Capabilities"/>.
    /// </param>
    /// <param name="height">
    /// The height of the thread group. Must be greater than 0 and less than 
    /// or equal to the value of <see cref="GraphicsCapabilities.MaxThreadGroupHeight"/> on 
    /// <see cref="Graphics.Capabilities"/>.
    /// </param>
    /// <param name="depth">
    /// The depth of the thread group. Must be greater than 0 and less than 
    /// or equal to the value of <see cref="GraphicsCapabilities.MaxThreadGroupDepth"/> on 
    /// <see cref="Graphics.Capabilities"/>.
    /// <para>
    /// Note: the maximum value for this is usually much lower than that of <see cref="Width"/> and <see cref="Height"/>.
    /// </para>
    /// </param>
    public ThreadGroupSizeAttribute(int width, int height = 1, int depth = 1)
    {
        Width = width;
        Height = height;
        Depth = depth;
    }
}