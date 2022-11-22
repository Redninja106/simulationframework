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

[assembly: ApplicationPlatform(typeof(DesktopPlatform))]

namespace SimulationFramework.Desktop;

/// <summary>
/// Implements a simulation environment which runs the simulation in a window.
/// </summary>
public sealed class DesktopPlatform : IApplicationPlatform
{
    public IWindow Window { get; }

    private Func<IntPtr, IGraphicsProvider> graphics;


    public IAppController CreateController()
    {
        return new DesktopAppController(this.Window);
    }

    public IGraphicsProvider CreateGraphicsProvider()
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

    public IEnumerable<IApplicationComponent> CreateAdditionalComponents()
    {
        yield return new DesktopInputComponent(this.Window);
        yield return new DesktopImGuiComponent(this.Window);
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

    public static bool IsSupported()
    {
        return OperatingSystem.IsWindows() || OperatingSystem.IsMacOS() || OperatingSystem.IsLinux();
    }
}