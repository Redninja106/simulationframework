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
    private Func<IntPtr, IGraphicsProvider> graphics;

    public IAppController CreateController()
    {
        return new DesktopAppController(this.Window);
    }

    private IGraphicsProvider CreateGraphics()
    {
        if (graphics is null)
        {
            frameProvider = new DesktopSkiaFrameProvider(Window.Size.X, Window.Size.Y);
            return new SkiaGraphicsProvider(frameProvider, name =>
            {
                Window.GLContext.TryGetProcAddress(name, out nint addr);
                return addr;
            });
        }
        else
        {
            return graphics(Window.Native.Win32.Value.Hwnd);
        }
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
        application.AddComponent(new ImGuiNETProvider(new DesktopImGuiBackend(Window)));
        application.Dispatcher.Subscribe<ResizeMessage>(m =>
        {
            frameProvider?.Resize(m.Width, m.Height);
        });
    }
}