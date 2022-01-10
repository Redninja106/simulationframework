namespace SimulationFramework;

/// <summary>
/// Represents an individual color in a gradient and its position relative to the bounds of the gradient.
/// </summary>
public struct GradientStop
{
    /// <summary>
    /// The color of the gradient stop.
    /// </summary>
    public Color Color;

    /// <summary>
    /// The position of the gradient stop in the range [0, 1]. If this value is not within this range, then the stop is located in the center of its neighbors.
    /// </summary>
    public float Position;

    /// <summary>
    /// Creates a new <see cref="GradientStop"/> with the provided color and a negative position.
    /// </summary>
    /// <param name="color">The color of the stop.</param>
    public GradientStop(Color color) : this(color, -1) { }

    /// <summary>
    /// Creates a new <see cref="GradientStop"/> with the provided color and position.
    /// </summary>
    /// <param name="color">The color of the stop.</param>
    /// <param name="position">The position of the stop, in the range [0, 1].</param>
    public GradientStop(Color color, float position)
    {
        Color = color;
        Position = position;
    }
    
    /// <summary>
    /// Casts a Color to a gradient using the <see cref="GradientStop.GradientStop(Color)"/> constructor.
    /// </summary>
    public static implicit operator GradientStop(Color color) => new(color);
    /// <summary>
    /// Casts a Color to a gradient using the <see cref="GradientStop.GradientStop(Color, float)"/> constructor.
    /// </summary>
    public static implicit operator GradientStop((Color color, float position) stop) => new(stop.color, stop.position);
}