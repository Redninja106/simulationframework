using ImGuiNET;
using Silk.NET.Core;
using Silk.NET.GLFW;
using Silk.NET.Windowing;
using SimulationFramework.Components;
using SimulationFramework.Drawing;
using SimulationFramework.Messaging;
using System.Numerics;

namespace SimulationFramework.Desktop;
internal class DesktopWindowProvider : IWindowProvider, IFullscreenProvider
{
    private readonly IWindow window;
    private readonly Glfw glfw = Glfw.GetApi();

    private string title = "Simulation";

    public unsafe string Title 
    { 
        get
        {
            return title;
        }
        set
        {
            glfw.SetWindowTitle(WindowHandle, value);
            title = value;
        }
    }
    public IDisplay Display => GetDisplay();
    
    public bool IsUserResizable
    {
        get => isUserResizable;
        set
        {
            isUserResizable = value;
            UpdateWindowBorder();
        }
    }

    public bool ShowSystemMenu 
    {
        get => showSystemMenu;
        set
        {
            showSystemMenu = value;
            UpdateWindowBorder();
        } 
    }

    private void UpdateWindowBorder()
    {
        if (showSystemMenu)
        {
            if (IsUserResizable)
            {
                window.WindowBorder = WindowBorder.Resizable;
            }
            else
            {
                window.WindowBorder = WindowBorder.Fixed;
            }
        }
        else
        {
            window.WindowBorder = WindowBorder.Hidden;
        }
    }

    public bool IsMinimized => window.WindowState == WindowState.Minimized;
    public bool IsMaximized => window.WindowState == WindowState.Maximized;
    public bool IsFullscreen => window.WindowState == WindowState.Fullscreen;

    private unsafe WindowHandle* WindowHandle => (WindowHandle*)window.Native.Glfw!.Value;

    public bool PreferExclusive { get; set; }

    private bool isUserResizable = true;
    private bool showSystemMenu = true;

    public unsafe Vector2 Position 
    { 
        get 
        { 
            glfw.GetWindowPos(WindowHandle, out int x, out int y);
            return new(x, y);
        }
    }

    public unsafe Vector2 Size
    {
        get
        {
            glfw.GetWindowSize(WindowHandle, out int x, out int y);
            return new(x, y);
        }
    }

    public DesktopWindowProvider(IWindow window)
    {
        this.window = window;
        window.Title = this.title;
    }

    public void Dispose()
    {
    }

    unsafe IDisplay GetDisplay()
    {
        var displays = DesktopDisplay.GetDisplayList();
        DesktopDisplay fullscreenDisplay = displays.SingleOrDefault(display => display.monitor == glfw.GetWindowMonitor(WindowHandle));

        Rectangle clientArea = new((int)Position.X, (int)Position.Y, Size.X, Size.Y);

        foreach (var d in displays)
        {
            if (d.Bounds.ContainsPoint(clientArea.Center))
            {
                return d;
            }
        }

        throw new Exception("Error Finding Display");
    }

    public unsafe void EnterFullscreen(IDisplay display)
    {
        if (display is not DesktopDisplay)
            throw new ArgumentException("display must be a desktop display.");
        
        window.WindowState = WindowState.Fullscreen;
    }

    public unsafe void ExitFullscreen()
    {
        window.WindowState = WindowState.Normal;
    }

    public ITexture GetBackBuffer()
    {
        throw new NotImplementedException();
    }

    public void Initialize(MessageDispatcher dispatcher)
    {
    }

    public unsafe void SetPosition(Vector2 position)
    {
        glfw.SetWindowPos(WindowHandle, (int)position.X, (int)position.Y);
    }

    public unsafe void Resize(Vector2 size)
    {
        glfw.SetWindowSize(WindowHandle, (int)size.X, (int)size.Y);
    }

    public void Maximize()
    {
        window.WindowState = WindowState.Maximized;
    }

    public void Minimize()
    {
        window.WindowState = WindowState.Minimized;
    }

    public void Restore()
    {
        window.WindowState = WindowState.Normal;
    }

    public unsafe void SetIcon(ReadOnlySpan<Color> icon, int width, int height)
    {
        fixed (Color* colorsPtr = icon)
        {
            var memoryManager = new UnsafePinnedMemoryManager<byte>((byte*)colorsPtr, sizeof(Color) * width * height);
            RawImage image = new(width, height, memoryManager.Memory);
            window.SetWindowIcon(ref image);
        }
    }
}
