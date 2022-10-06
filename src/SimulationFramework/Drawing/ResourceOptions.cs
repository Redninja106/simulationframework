using System;

namespace SimulationFramework.Drawing;

/// <summary>
/// Provides of control over the functionality of a texture. Used by <see cref="Graphics.CreateTexture(int, int, ResourceOptions)"/>.
/// </summary>
[Flags]
public enum ResourceOptions
{
    /// <summary>
    /// Default resource behavior.
    /// </summary>
    None = 0,
    NoGPUWrite =  1 << 1,
    NoCPURead = 1 << 2,
    NoCPUWrite = 1 << 3,

    /// <summary>
    /// The resource's data may not be written to or read from by the cpu after creation.
    /// </summary>
    NoAccess = NoCPURead | NoCPUWrite,

    Readonly = NoGPUWrite | NoCPUWrite,

}