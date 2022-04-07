namespace SimulationFramework.Drawing;

/// <summary>
/// Options to control the behavior of gradients when drawn outside their bounds.
/// </summary>
public enum TileMode
{
    /// <summary>
    /// Clamp the any color outside the region to that at the nearest border.
    /// </summary>
    Clamp = 0,
    /// <summary>
    /// Repeat the contents of the region.
    /// </summary>
    Repeat = 1,
    /// <summary>
    /// Repeat the region with every other tile inverted.
    /// </summary>
    Mirror = 2,
    /// <summary>
    /// Stop using the region and instead fill using the shape's color.
    /// </summary>
    Stop = 3,
}