using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Provides time values to the simulation.
/// </summary>
public static class Time
{
    internal static ITimeProvider Provider => Application.GetComponent<ITimeProvider>();

    /// <summary>
    /// The number of seconds since the last frame.
    /// </summary>
    public static float DeltaTime => Provider.GetDeltaTime();

    /// <summary>
    /// The number of seconds since the start of the simulation.
    /// </summary>
    public static float TotalTime => Provider.GetTotalTime();

    /// <summary>
    /// <see langword="true"/> if duration of the previous frame exceeded <see cref="MaxDeltaTime"/>.
    /// </summary>
    public static bool IsRunningSlowly => Provider.IsRunningSlowly();

    /// <summary>
    /// The highest allowed value of <see cref="DeltaTime"/>.
    /// </summary>
    public static float MaxDeltaTime { get => Provider.MaxDeltaTime; set => Provider.MaxDeltaTime = value; }

    /// <summary>
    /// The default value of <see cref="MaxDeltaTime"/>.
    /// </summary>
    public const float DefaultMaxDeltaTime = float.PositiveInfinity;
}