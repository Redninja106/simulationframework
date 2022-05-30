using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.Windowing;
using SimulationFramework.SkiaSharp;
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

    private DesktopSkiaFrameProvider frameProvider;
    
    public IAppController CreateController()
    {
        return new DesktopAppController(this.Window);
    }

    private IGraphicsProvider CreateGraphics()
    {
        frameProvider = new DesktopSkiaFrameProvider(Window.Size.X, Window.Size.Y);
        return new SkiaGraphicsProvider(frameProvider, name =>
        {
            Window.GLContext.TryGetProcAddress(name, out nint addr);
            return addr;
        });
    }

    public DesktopPlatform()
    {
        Window = Silk.NET.Windowing.Window.Create(WindowOptions.Default);
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
        application.Dispatcher.Subscribe<ResizeMessage>(m =>
        {
            frameProvider?.Resize(m.Width, m.Height);
        });
    }
}