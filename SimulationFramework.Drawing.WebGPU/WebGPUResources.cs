using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using WebGPU;

namespace SimulationFramework.Drawing.WebGPU;
internal class WebGPUResources : IDisposable
{
    public Instance Instance { get; }
    public Surface Surface { get; }
    public Adapter Adapter { get; }
    public Device Device { get; }
    public Queue Queue { get; }

    public WebGPUResources(int width, int height, IChainable surfaceDescriptor)
    {
        Instance = Instance.Create();
        Adapter = Instance.RequestAdapter(new() { PowerPreference = PowerPreference.HighPerformance });
        Device = Adapter.RequestDevice(default);

        Device.SetUncapturedErrorCallback((type, message) =>
        {
            throw new Exception($"WebGPU error! [{type}] {message}");
        });

        Queue = Device.Queue;

        Surface = Instance.CreateSurface(new()
        {
            NextInChain = surfaceDescriptor
        });

        Resize(width, height);
    }

    public void Resize(int width, int height)
    {
        if (width == 0 || height == 0)
            return;

        Surface.Configure(new()
        {
            AlphaMode = CompositeAlphaMode.Opaque,
            Device = Device,
            Format = Surface.GetPreferredFormat(Adapter),
            Height = (uint)height,
            Width = (uint)width,
            PresentMode = PresentMode.Fifo,
            Usage = TextureUsage.RenderAttachment,
        });
    }

    public void Dispose()
    {
        Queue.Dispose();
        Surface.Dispose();
        Device.Dispose();
        Adapter.Dispose();
        Instance.Dispose();
    }
}
