using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.Windowing;
using SimulationFramework.SkiaSharp;
using SkiaSharp;

namespace SimulationFramework;

/// <summary>
/// Implements a simulation environment which runs the simulation in a window.
/// </summary>
public sealed class WindowEnvironment : ISimulationEnvironment
{
    IWindow window;

    public WindowEnvironment()
    {
        window = Window.Create(WindowOptions.Default);
        window.Initialize();
        MakeContextCurrent();
    }

    public void MakeContextCurrent()
    {
        window.GLContext.MakeCurrent();
    }

    public void Dispose()
    {
    }

    public IEnumerable<ISimulationComponent> CreateSupportedComponents()
    {
        yield return new RealtimeProvider();

        var frameProvider = new WindowFrameProvider(window.Size.X, window.Size.Y);

        window.FramebufferResize += size =>
        {
            frameProvider.Resize(size.X, size.Y);
        };

        yield return new SkiaGraphicsProvider(frameProvider, name =>
        {
            window.GLContext.TryGetProcAddress(name, out nint addr);
            return addr;
        });
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
        window.GLContext.SwapBuffers();
    }

    private class WindowFrameProvider : ISkiaFrameProvider
    {
        private GRContext context;
        private int width;
        private int height;

        public WindowFrameProvider(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        private GRBackendRenderTarget backendRenderTarget;
        private SKSurface frameSurface;

        public void Resize(int width, int height)
        {
            backendRenderTarget?.Dispose();
            frameSurface?.Dispose();

            const SKColorType format = SKColorType.Rgba8888;
            backendRenderTarget = new GRBackendRenderTarget(width, height, 1, 32, new GRGlFramebufferInfo { Format = format.ToGlSizedFormat(), FramebufferObjectId = 0 });
            frameSurface = SKSurface.Create(context, backendRenderTarget, format);

            this.width = width;
            this.height = height;
        }

        SKCanvas ISkiaFrameProvider.GetCurrentFrame()
        {
            return frameSurface.Canvas;
        }

        public void SetContext(GRContext context)
        {
            this.context = context;
            Resize(width, height);
        }
    }
}