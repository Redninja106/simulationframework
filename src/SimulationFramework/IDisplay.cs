namespace SimulationFramework;

/// <summary>
/// Provides display information to the simulation.
/// </summary>
public interface IDisplay
{
    /// <summary>
    /// <see langword="true"/> if this display is the system's primary display; otherwise <see langword="false"/>.
    /// </summary>
    bool IsPrimary { get; }

    /// <summary>
    /// The bounds of the display relative to the desktop origin.
    /// </summary>
    Rectangle Bounds { get; }

    /// <summary>
    /// The name of the display.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The scaling factor of the display.
    /// </summary>
    float Scaling { get; }

    /// <summary>
    /// The refresh rate of the display, in hz.
    /// </summary>
    float RefreshRate { get; }
}