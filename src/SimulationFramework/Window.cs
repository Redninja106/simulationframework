using System;
using System.Linq;
using System.Numerics;
using SimulationFramework.Components;
using SimulationFramework.Drawing;
using SimulationFramework.Messaging;

namespace SimulationFramework;

/// <summary>
/// Provides access to the simulation's window.
/// </summary>
public static class Window
{
    private static IWindowProvider Provider => Application.GetComponent<IWindowProvider>();
    private static IFullscreenProvider FullscreenProvider => Application.GetComponent<IFullscreenProvider>();

    /// <summary>
    /// The window's back buffer. Drawing to this texture during a frame will make it's contents visible in the window.
    /// </summary>
    public static ITexture BackBuffer => Provider.GetBackBuffer();

    /// <summary>
    /// The display the window is on.
    /// </summary>
    public static IDisplay Display => Provider.Display;

    /// <summary>
    /// The title of the window.
    /// </summary>
    public static string Title { get => Provider.Title; set => Provider.Title = value; }

    /// <summary>
    /// The width of the window, in pixels. To change the width of the window, see <see cref="TryResize(int, int)"/>.
    /// </summary>
    public static int Width => Provider.Width;

    /// <summary>
    /// The height of the window in pixels. To change the height of the window, see <see cref="TryResize(int, int)"/>.
    /// </summary>
    public static int Height => Provider.Height;

    /// <summary>
    /// Whether the window can be resized by the user. To handle user resize events, see <see cref="Simulation.OnResize(int, int)"/>.
    /// </summary>
    public static bool IsUserResizable { get => Provider.IsUserResizable; set => Provider.IsUserResizable = value; }

    /// <summary>
    /// Whether the platform-specific menus should be displayed with the window.
    /// <para>
    /// On platforms without a system menu, this value has no effect.
    /// </para>
    /// </summary>
    public static bool ShowSystemMenu { get => Provider.ShowSystemMenu; set => Provider.ShowSystemMenu = value; }

    /// <summary>
    /// <see langword="true"/> when the window is minimized; otherwise <see langword="false"/>.
    /// <para>
    /// If the window cannot be minimized, this is always <see langword="false"/>. 
    /// </para>
    /// </summary>
    public static bool IsMinimized => Provider.IsMinimized;

    /// <summary>
    /// <see langword="true"/> when the window is maximized; otherwise <see langword="false"/>.
    /// <para>
    /// If the window cannot be maximized, this is always <see langword="false"/>. 
    /// </para>
    /// </summary>
    public static bool IsMaximized => Provider.IsMaximized;

    /// <summary>
    /// <see langword="true"/> when the window is fullscreen; otherwise <see langword="false"/>.
    /// <para>
    /// On platforms that don't have window systems (like mobile), this value will always be true. 
    /// </para>
    /// </summary>
    public static bool IsFullscreen => FullscreenProvider.IsFullscreen;

    /// <summary>
    /// Attempts to enter fullscreen mode. This method does not have any immediate effects, as the fullscreen transition is made
    /// directly after the current frame. To check if the transition succeeded, use <see cref="IsFullscreen"/> during the next frame.
    /// <para>
    /// If the window is already in requested fullscreen state, this method has no effect.
    /// </para>
    /// </summary>
    /// <param name="display">The monitor to enter fullscreen on. <see cref="Display"/> is used if this value is <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the window successfully transitioned to fullscreen; otherwise <see langword="false"/>.</returns>
    public static void EnterFullscreen(IDisplay? display = null)
    {
        display ??= Display;

        if (!Application.GetDisplays().Contains(display))
            throw new ArgumentException("Invalid Display!", nameof(display));

        // make sure we throw from this method when there is no fullscreen provider
        _ = FullscreenProvider;

        SimulationHost.GetCurrent().Dispatcher.NotifyAfter<AfterRenderMessage>(m =>
        {
            FullscreenProvider.EnterFullscreen(display);
        });
    }

    /// <summary>
    /// Attempts to exit fullscreen mode. This method does not have any immediate effects, as the fullscreen transition is made
    /// directly after the current frame. To check if the transition succeeded, use <see cref="IsFullscreen"/> during the next frame.
    /// <para>
    /// If the window is not in fullscreen, or the platform requires the window to always be fullscreen (like mobile), this method has no effect.
    /// </para>
    /// </summary>
    /// <returns><see langword="true"/> if the window successfully transitioned out of fullscreen; otherwise <see langword="false"/>.</returns>
    public static void ExitFullscreen()
    {
        _ = FullscreenProvider;

        SimulationHost.GetCurrent().Dispatcher.NotifyAfter<AfterRenderMessage>(m =>
        {
            FullscreenProvider.ExitFullscreen();
        });
    }

    /// <summary>
    /// Gets the back buffer for the window.
    /// Any rendering done to the back buffer during <see cref="Simulation.OnRender(ICanvas)"/> will be visible on the window.
    /// </summary>
    /// <returns>The back buffer for the window.</returns>
    public static ITexture GetBackBuffer()
    {
        return Provider.GetBackBuffer();
    }

    /// <summary>
    /// Attempts to resize the window. Changes may not take effect until the next frame.
    /// <para>On some platforms, resizing the window. In those cases this method always returns <see langword="false"/>.</para>
    /// </summary>
    /// <param name="width">
    /// The width to resize the window to. Must not be negative. 
    /// <para>If this value is 0, the width of the window is left unchanged.</para>
    /// <para>This value may be clamped to be within the platform's limits.</para>
    /// </param>
    /// <param name="height">
    /// The height to resize the window to. Must not be negative. 
    /// <para>If this value is 0, the height of the window is left unchanged.</para>
    /// <para>This value may be clamped to be within the platform's limits.</para>
    /// </param>
    /// <returns><see langword="true"/> if the window was successfully resized; otherwise <see langword="false"/>.</returns>
    public static bool TryResize(int width, int height)
    { 
        if (width < 0)
            throw new ArgumentOutOfRangeException(nameof(width), "width must not be negative!");

        if (height < 0)
            throw new ArgumentOutOfRangeException(nameof(height), "height must not be negative!");

        // 0 means leave as is
        if (width is 0) width = Width;
        if (height is 0) height = Height;

        return Provider.TryResize(width, height);
    }

    /// <summary>
    /// The position of the window.
    /// </summary>
    public static Vector2 Position => Provider.Position;

    /// <summary>
    /// Sets the position of the window. 
    /// <para>
    /// On some platforms or when the window is fullscreen, the window cannot be moved, and this method always returns <see langword="false"/>.</para>
    /// </summary>
    /// <param name="x">The x position to move the window to desktop space.</param>
    /// <param name="y">The y position of move the window to in desktop space.</param>
    /// <returns><see langword="true"/> if moving the window succeeded; otherwise <see langword="false"/>.</returns>
    public static bool TryMove(int x, int y)
    {
        return Provider.TryMove(x, y);
    }
}
