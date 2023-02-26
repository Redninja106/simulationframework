namespace SimulationFramework.Drawing;

/// <summary>
/// Specifies the behavior of gradients, textures, and other resources when sampled outside their bounds.
/// </summary>
public enum TileMode
{
    /// <summary>
    /// Outside the region, fill with <see cref="Color.Transparent"/>.
    /// </summary>
    None,
    /// <summary>
    /// Clamp outside the region to the color at the nearest border.
    /// </summary>
    Clamp,
    /// <summary>
    /// Repeat the contents of the region.
    /// </summary>
    Repeat,
    /// <summary>
    /// Repeat the contents of the region, with every other tile inverted.
    /// </summary>
    Mirror,
}