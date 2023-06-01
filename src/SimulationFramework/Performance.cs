using SimulationFramework.Components;

namespace SimulationFramework;

/// <summary>
/// Provides performance-related information.
/// </summary>
public static class Performance
{
    /// <summary>
    /// The simulations current framerate.
    /// </summary>
    public static float Framerate => 1f / Application.GetComponent<ITimeProvider>().GetDeltaTime();
}     