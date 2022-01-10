namespace SimulationFramework;

/// <summary>
/// Options to control the behavior of gradients when drawn outside their bounds.
/// </summary>
public enum GradientTileMode
{
    /// <summary>
    /// Clamp the region outside the gradient to the color at the nearest border.
    /// </summary>
    Clamp = 0,
    /// <summary>
    /// Repeat the colors of the gradient.
    /// </summary>
    Repeat = 1,
    /// <summary>
    /// Repeat the gradient with every other tile inverted.
    /// </summary>
    Mirror = 2,
    /// <summary>
    /// Stop using the gradient and instead fill using the shape's color.
    /// </summary>
    Stop = 3,
}