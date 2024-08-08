using SimulationFramework.Components;
using SimulationFramework.Drawing.Shaders;

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

    public static float DeltaTime
    {
        [ImplicitUniform]
        get => Provider.GetDeltaTime();
    }

    /// <summary>
    /// The number of seconds since the start of the simulation.
    /// </summary>
    [ImplicitUniform]
    public static float TotalTime 
    {
        get => Provider.GetTotalTime();
    }

    /// <summary>
    /// <see langword="true"/> if duration of the previous frame exceeded <see cref="MaxDeltaTime"/>.
    /// </summary>
    public static bool IsRunningSlowly => Provider.IsRunningSlowly();

    /// <summary>
    /// The highest allowed value of <see cref="DeltaTime"/>. Defaults to <see cref="float.PositiveInfinity"/>.
    /// </summary>
    public static float MaxDeltaTime { get => Provider.MaxDeltaTime; set => Provider.MaxDeltaTime = value; }

    /// <summary>
    /// Controls the rate at which <see cref="DeltaTime"/> and <see cref="TotalTime"/> passes.
    /// </summary>
    public static float Scale { get => Provider.Scale; set => Provider.Scale = value; }

    /// <summary>
    /// The default value of <see cref="MaxDeltaTime"/>.
    /// </summary>
    public const float DefaultMaxDeltaTime = float.PositiveInfinity;
}