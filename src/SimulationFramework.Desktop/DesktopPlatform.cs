using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.Windowing;
using SimulationFramework.SkiaSharp;
using ImGuiNET;
using SimulationFramework.Drawing;
using SimulationFramework.Messaging;
using SimulationFramework.Desktop;
using SimulationFramework.Components;

namespace SimulationFramework.Desktop;

/// <summary>
/// Implements a simulation environment which runs the simulation in a window.
/// </summary>
public sealed class DesktopPlatform : ISimulationPlatform
{
    public IWindow Window { get; }

    private DesktopSkiaFrameProvider frameProvider;
    
    public DesktopPlatform()
    {
        Window = Silk.NET.Windowing.Window.Create(WindowOptions.Default with { IsVisible = false });
        Window.Initialize();
    }

    public void Dispose()
    {
    }

    public void Initialize(MessageDispatcher dispatcher)
    {
        dispatcher.Subscribe<ResizeMessage>(m =>
        {
            frameProvider?.Resize(m.Width, m.Height);
        });

        frameProvider = new DesktopSkiaFrameProvider(Window.Size.X, Window.Size.Y);
        Application.RegisterComponent(new SkiaGraphicsProvider(frameProvider, name => Window.GLContext.TryGetProcAddress(name, out nint addr) ? addr : 0));
        Application.RegisterComponent(new RealtimeProvider());
        Application.RegisterComponent(new DesktopAppController(this.Window));
        Application.RegisterComponent(new InputContext());
        Application.RegisterComponent(new DesktopInputComponent(this.Window));
        Application.RegisterComponent(new DesktopImGuiComponent(this.Window));
    }
}