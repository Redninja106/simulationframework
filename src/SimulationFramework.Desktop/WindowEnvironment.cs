using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.Windowing;
using SimulationFramework.SkiaSharp;
using ImGuiNET;
using SimulationFramework.IMGUI;
using SimulationFramework.Drawing.Direct3D11;

namespace SimulationFramework.Desktop;

/// <summary>
/// Implements a simulation environment which runs the simulation in a window.
/// </summary>
public sealed partial class WindowEnvironment : ISimulationEnvironment
{
    private readonly IWindow window;

    public WindowEnvironment(string title, int width, int height, bool resizable)
    {
        window = Silk.NET.Windowing.Window.Create(WindowOptions.Default with
        {
            Size = new(width, height),
            Title = title,
            WindowBorder = resizable ? WindowBorder.Resizable : WindowBorder.Fixed,
            API = GraphicsAPI.None,
        });
        window.Initialize();
        MakeContextCurrent();
    }

    public void MakeContextCurrent()
    {
        //window.MakeCurrent();
    }

    public void Dispose()
    {
    }

    public IEnumerable<ISimulationComponent> CreateSupportedComponents()
    {
        yield return new RealtimeProvider();

        var graphicsEnabled = Window.graphicsEnabled;
        if (graphicsEnabled)
        {
            //var frameProvider = new WindowFrameProvider(window.Size.X, window.Size.Y);

            window.FramebufferResize += size =>
            {
                //frameProvider.Resize(size.X, size.Y);
            };

            yield return new D3D11Graphics(window.Native.Win32.Value.Hwnd);
            //yield return new ImGuiNETProvider(new WindowImGuiBackend(window));
        }

        yield return new WindowInputProvider(window);
    }

    public void ProcessEvents()
    {
        window.DoEvents();
    }

    public bool ShouldExit()
    {
        return window.IsClosing;
    }

    public void EndFrame()
    {
        //window.GLContext.SwapBuffers();
    }

    public (int, int) GetOutputSize()
    {
        return (window.Size.X, window.Size.Y);
    }
}