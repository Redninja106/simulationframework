namespace SimulationFramework.Components;

/// <summary>
/// Provides Fullscreen functionality to the application.
/// </summary>
public interface IFullscreenProvider : ISimulationComponent
{
    // Although this is conceptually a part of the window, it is not always internally handled by the platform's window system.
    // this interface exists in order to not force the window provider into handling fullscreen, and leave it to the backend how to implement fullscreen.

    /// <summary>
    /// Whether to prefer exclusive or borderless fullscreen.
    /// </summary>
    bool PreferExclusive { get; set; }

    /// <summary>
    /// Whether the simulation is currently fullscreen.
    /// </summary>
    bool IsFullscreen { get; }

    /// <summary>
    /// Attempts to enter fullscreen. The fullscreen mode is determined by <see cref="PreferExclusive"/>.
    /// <para>
    /// On some platforms, windowed mode may not be supported. On those platforms, this method does nothing.
    /// </para>
    /// <para>
    /// To know if this method succeeded, check <see cref="IsFullscreen"/> on the next frame.
    /// </para>
    /// </summary>
    /// <param name="display">The display to go fullscreen on. If this is <see langword="null"/>, the primary display is used.</param>
    void EnterFullscreen(IDisplay? display);

    /// <summary>
    /// Attempts to exit fullscreen.
    /// <para>
    /// On some platforms, windowed mode may not be supported. On those platforms, this method does nothing.
    /// </para>
    /// <para>
    /// To know if this method succeeded, check <see cref="IsFullscreen"/> on the next frame.
    /// </para>
    /// </summary>
    void ExitFullscreen();
}