using SimulationFramework.Messaging;
using System.Diagnostics.CodeAnalysis;
using WebGPU;

namespace SimulationFramework.Drawing.WebGPU;

public sealed class WebGPUGraphicsProvider : IGraphicsProvider
{
    public IFont DefaultFont => throw new NotImplementedException();

    internal Instance Instance { get; }
    internal Surface Surface { get; }
    internal Adapter Adapter { get; }
    internal Device Device { get; }

    public void Dispose()
    {
    }

    public WebGPUGraphicsProvider(nint hinstnace, nint hwnd)
    {
        Instance = Instance.Create();
        Adapter = Instance.RequestAdapter(new() { PowerPreference = PowerPreference.HighPerformance });
        Device = Adapter.RequestDevice(default);

        Surface = Instance.CreateSurface(new()
        {
            NextInChain = new SurfaceDescriptorFromWindowsHWND()
            {
                Hinstance = hinstnace,
                Hwnd = hwnd
            }
        });


    }

    public ICanvas GetFrameCanvas()
    {
        throw new NotImplementedException();
    }

    public void Initialize(MessageDispatcher dispatcher)
    {
    }

    public bool TryCreateTexture(int width, int height, ReadOnlySpan<Color> pixels, TextureOptions options, [NotNullWhen(true)] out ITexture texture)
    {
        throw new NotImplementedException();
    }

    public bool TryLoadFont(ReadOnlySpan<byte> encodedData, [NotNullWhen(true)] out IFont? font)
    {
        throw new NotImplementedException();
    }

    public bool TryLoadSystemFont(string name, [NotNullWhen(true)] out IFont? font)
    {
        throw new NotImplementedException();
    }

    public bool TryLoadTexture(ReadOnlySpan<byte> encodedData, TextureOptions options, [NotNullWhen(true)] out ITexture? texture)
    {
        throw new NotImplementedException();
    }
}
