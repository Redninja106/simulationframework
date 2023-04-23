using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SimulationFramework.Components;
using SimulationFramework.Drawing;

namespace SimulationFramework;

public static class Window
{
    private static IWindowProvider Provider => Application.GetComponent<IWindowProvider>();

    /// <summary>
    /// The display the window is on.
    /// </summary>
    public static IDisplay? Display => Provider.Display;

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
    public static bool IsFullscreen => Provider.IsFullscreen;

    /// <summary>
    /// Attempts to enter fullscreen mode.
    /// </summary>
    /// <param name="exclusive">
    /// Whether the window should try to enter exclusive fullscreen mode. 
    /// Exclusive fullscreen may provide better performance but prevents other windows from being focused.
    /// </param>
    /// <param name="display">The monitor to enter fullscreen on. <see cref="Display"/> is used if this value is <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the window successfully transitioned to fullscreen; otherwise <see langword="false"/>.</returns>
    public static bool EnterFullscreen(bool exclusive, IDisplay? display = null)
    {
        display ??= Display;

        if (!Application.GetDisplays().Contains(display))
            throw new ArgumentException("Invalid Display!", nameof(display));
        
        bool success = Provider.EnterFullscreen(exclusive, display);

        if (!success && exclusive)
        {
            IGraphicsProvider? graphics = Application.GetComponentOrDefault<IGraphicsProvider>();

            if (graphics is null)
                return false;

            return graphics.TryEnterFullscreenExclusive(display);
        }

        return success;
    }

    /// <summary>
    /// Attempts to exit fullscreen mode.
    /// <para>On some platforms, the window is always in fullscreen. In those cases this method always returns <see langword="false"/>.</para>
    /// </summary>
    /// <returns><see langword="true"/> if the window successfully transitioned out of fullscreen; otherwise <see langword="false"/>.</returns>
    public static bool ExitFullscreen()
    {
        return Provider.ExitFullscreen();
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
            throw new ArgumentOutOfRangeException(nameof(width), "height must not be negative!");

        // 0 means leave as is
        if (width is 0) width = Window.Width;
        if (height is 0) height = Window.Height;

        return Provider.TryResize(width, height);
    }

    public static void GetPosition(out int x, out int y)
    {
        Provider.GetPosition(out x, out y);
    }

    public static bool TrySetPosition(int x, int y)
    {
        return Provider.TrySetPosition(x, y);
    }
}
