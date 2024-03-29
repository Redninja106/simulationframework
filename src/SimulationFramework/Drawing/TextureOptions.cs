namespace SimulationFramework.Drawing;

/// <summary>
/// Provides control over the functionality of a texture. Used by <see cref="Graphics.CreateTexture(int, int, TextureOptions)"/>.
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
    Constant = 1 << 0,
    /// <summary>
    /// Specifies the texture will never rendered to.
    /// </summary>
    NonRenderTarget = 1 << 1,
}