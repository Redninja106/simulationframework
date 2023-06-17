using ImGuiNET;
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
    private Rectangle windowedBounds;
    private readonly Glfw glfw = Glfw.GetApi();

    private string title;

    public unsafe string Title 
    { 
        get
        {
            return title;
        }
        set
        {
            glfw.SetWindowTitle(WindowHandle, title);
            title = value;
        }
    }
    public IDisplay Display => GetDisplay();
    
    public int Width
    {
        get => window.Size.X;
        set => window.Size = new(value, Height);
    }

    public int Height 
    { 
        get => window.Size.Y;
        set => window.Size = new(Width, value);
    }

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

    public DesktopWindowProvider(IWindow window)
    {
        this.window = window;
    }

    public void Dispose()
    {
    }

    unsafe IDisplay GetDisplay()
    {
        var displays = DesktopDisplay.GetDisplayList();
        DesktopDisplay fullscreenDisplay = displays.SingleOrDefault(display => display.monitor == glfw.GetWindowMonitor(WindowHandle));

        GetPosition(out int x, out int y);
        Rectangle clientArea = new(x, y, Width, Height);

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
        if (display is not DesktopDisplay desktopDisplay)
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

    public unsafe void GetPosition(out int x, out int y)
    {
        glfw.GetWindowPos(WindowHandle, out x, out y);
    }

    public void Initialize(MessageDispatcher dispatcher)
    {
    }

    public unsafe bool TryResize(int width, int height)
    {
        glfw.SetWindowSize(WindowHandle, width, height);
        return glfw.GetError(out _) == ErrorCode.NoError;
    }

    public unsafe bool TryMove(int x, int y)
    {
        glfw.SetWindowPos(WindowHandle, x, y);
        return glfw.GetError(out _) == ErrorCode.NoError;
    }
}
