using System;

namespace SimulationFramework;

/// <summary>
/// Provides of control over the functionality of a texture. Passed to <see cref="Graphics.Create"/>
/// </summary>
[Flags]
public enum TextureFlags
{
    /// <summary>
    /// Default texture behavior.
    /// </summary>
    None = 0,
    /// <summary>
    /// The texture's data may not be written to or read from after creation.
    /// </summary>
    NoAccess = 1 << 0
}