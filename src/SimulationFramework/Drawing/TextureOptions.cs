using System;

namespace SimulationFramework.Drawing;

/// <summary>
/// Provides of control over the functionality of a texture. Used by <see cref="Graphics.CreateTexture(int, int, TextureOptions)"/>.
/// </summary>
[Flags]
public enum TextureOptions
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