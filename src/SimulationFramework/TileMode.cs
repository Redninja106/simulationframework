namespace SimulationFramework;

/// <summary>
/// Options to control the behavior of gradients when drawn outside their bounds.
/// </summary>
public enum TileMode
{
    /// <summary>
    /// Clamp outside the region to the color at the nearest border.
    /// </summary>
    Clamp = 0,
    /// <summary>
    /// Repeat the contents of the region.
    /// </summary>
    Repeat = 1,
    /// <summary>
    /// Repeat the contents of the region, with every other tile inverted.
    /// </summary>
    Mirror = 2,
    /// <summary>
    /// Outside the region, fill with <see cref="Color.Transparent"/>.
    /// </summary>
    None = 3,
}