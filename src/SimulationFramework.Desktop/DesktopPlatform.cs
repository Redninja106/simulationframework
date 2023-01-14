using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.Windowing;
using SimulationFramework.Drawing;
using SimulationFramework.Messaging;
using SimulationFramework.Desktop;
using SimulationFramework;
using SimulationFramework.Drawing.Direct3D11;

[assembly: ApplicationPlatform(typeof(DesktopPlatform))]

namespace SimulationFramework.Desktop;

/// <summary>
/// Implements a simulation environment which runs the simulation in a window.
/// </summary>
public sealed class DesktopPlatform : IApplicationPlatform
{
    public IWindow Window { get; }
    private IGraphicsProvider? graphics;

    public IApplicationController CreateController()
    {
        return new DesktopAppController(this.Window);
    }

    public IGraphicsProvider CreateGraphicsProvider()
    {
        return graphics ?? new D3D11Graphics(Window.Native.Win32.Value.Hwnd);
    }

    public DesktopPlatform()
    {
        Window = Silk.NET.Windowing.Window.Create(WindowOptions.Default);
        Window.Initialize();
    }

    public IApplicationPlatform WithGraphics(IGraphicsProvider graphics)
    {
        this.graphics = graphics;
        return this;
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
    }

    public static bool IsSupported()
    {
        return OperatingSystem.IsWindows() || OperatingSystem.IsMacOS() || OperatingSystem.IsLinux();
    }

    public ITimeProvider CreateTimeProvider()
    {
        return new RealtimeProvider();
    }
}