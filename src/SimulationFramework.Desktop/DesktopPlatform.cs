using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.Windowing;
using ImGuiNET;
using SimulationFramework.IMGUI;
using SimulationFramework.Drawing;
using SimulationFramework.Messaging;

namespace SimulationFramework.Desktop;

/// <summary>
/// Implements a simulation environment which runs the simulation in a window.
/// </summary>
public sealed class DesktopPlatform : IAppPlatform
{
    public IWindow Window { get; }

    private Func<IntPtr, IGraphicsProvider> graphics;


    public IAppController CreateController()
    {
        return new DesktopAppController(this.Window);
    }

    private IGraphicsProvider CreateGraphics()
    {
        return graphics != null ? graphics(Window.Native.Win32.Value.Hwnd) : null;
    }

    public DesktopPlatform(Func<IntPtr, IGraphicsProvider> graphics = null)
    {
        Window = Silk.NET.Windowing.Window.Create(WindowOptions.Default);
        this.graphics = graphics;
    }

    public void Dispose()
    {
    }

    public void Initialize(Application application)
    {
        Window.Initialize();
        application.AddComponent(CreateGraphics());
        application.AddComponent(new RealtimeProvider());
        application.AddComponent(new DesktopInputComponent(this.Window));
    }
}