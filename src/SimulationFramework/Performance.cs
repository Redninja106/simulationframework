using SimulationFramework.Components;

namespace SimulationFramework;

/// <summary>
/// Provides performance-related information.
/// </summary>
public static class Performance
{
    private static PerformanceProvider Provider => Application.GetComponent<PerformanceProvider>();

    /// <summary>
    /// The default value of <see cref="FramerateAverageDuration"/>.
    /// </summary>

    public const int DefaultFramerateAverageDuration = 5;

    /// <summary>
    /// The number of seconds that the value of <see cref="Framerate"/> should be averaged over.
    /// </summary>
    public static float FramerateAverageDuration { get => Provider.FramerateAverageDuration; set => Provider.FramerateAverageDuration = value; }

    /// <summary>
    /// The simulation's current framerate, averaged over a number a frames (determined by <see cref="FramerateAverageDuration"/>).
    /// </summary>
    public static float Framerate => Provider.Framerate;

    /// <summary>
    /// The simulation's current framerate.
    /// <para>
    /// This value can change drastically over multiple frames, making it hard to read. Consider using <see cref="Framerate"/> instead.
    /// </para>
    /// </summary>
    public static float RawFramerate => Provider.RawFramerate;
}     