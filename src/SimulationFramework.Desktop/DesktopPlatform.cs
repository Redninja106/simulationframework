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
using SimulationFramework;

[assembly: ApplicationPlatform(typeof(DesktopPlatform))]

namespace SimulationFramework.Desktop;

/// <summary>
/// Implements a simulation environment which runs the simulation in a window.
/// </summary>
public sealed class DesktopPlatform : IApplicationPlatform
{
    public IWindow Window { get; }

    private DesktopSkiaFrameProvider frameProvider;
    
    public IApplicationController CreateController()
    {
        return new DesktopAppController(this.Window);
    }

    public IGraphicsProvider CreateGraphicsProvider()
    {
        frameProvider = new DesktopSkiaFrameProvider(Window.Size.X, Window.Size.Y);
        return new SkiaGraphicsProvider(frameProvider, name =>
        {
            Window.GLContext.TryGetProcAddress(name, out nint addr);
            return addr;
        });
    }

    public ITimeProvider CreateTimeProvider()
    {
        return new RealtimeProvider();
    }

    public DesktopPlatform()
    {
        Window = Silk.NET.Windowing.Window.Create(WindowOptions.Default with { IsVisible = false });
    }

    public void Dispose()
    {
    }

    public IEnumerable<IApplicationComponent> CreateAdditionalComponents()
    {
        yield return new DesktopInputComponent(this.Window);
    }

    public void Initialize(Application application)
    {
        Window.Initialize();
        application.Dispatcher.Subscribe<ResizeMessage>(m =>
        {
            frameProvider?.Resize(m.Width, m.Height);
        });
    }

    public static bool IsSupported()
    {
        return OperatingSystem.IsWindows() || OperatingSystem.IsMacOS() || OperatingSystem.IsLinux();
    }
}