using SimulationFramework.Components;
using System.Collections.Generic;

namespace SimulationFramework;

/// <summary>
/// Provides performance-related information.
/// </summary>
public static class Performance
{
    private static PerformanceProvider Provider => Application.GetComponent<PerformanceProvider>();

    /// <summary>
    /// The default value of <see cref="FramerateAverageCount"/>.
    /// </summary>

    public const int DefaultFramerateAverageCount = 60;

    /// <summary>
    /// The number of frames that the value of <see cref="Framerate"/> should be averaged over.
    /// </summary>
    public static int FramerateAverageCount { get => Provider.FramerateAverageCount; set => Provider.FramerateAverageCount = value; }

    /// <summary>
    /// The simulation's current framerate, averaged over a number a frames (determined by <see cref="FramerateAverageCount"/>).
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