using SimulationFramework.Components;
using SimulationFramework.Messaging;
using System.Diagnostics.CodeAnalysis;
using WebGPU;

namespace SimulationFramework.Drawing.WebGPU;

public sealed class WebGPUGraphicsProvider : IGraphicsProvider
{
    public IFont DefaultFont => null!;

    private WebGPUCanvas frameCanvas;
    private GraphicsResources resources;
    private SurfaceTextureViewProvider frameViewProvider;

    public void Dispose()
    {
        resources.Dispose();
    }

    public WebGPUGraphicsProvider(int width, int height, IChainable surfaceDescriptor)
    {
        resources = new(width, height, surfaceDescriptor);
        frameViewProvider = new(resources);
        frameCanvas = new(resources, frameViewProvider);
    }

    public ICanvas GetFrameCanvas()
    {
        return frameCanvas;
    }

    public void Initialize(MessageDispatcher dispatcher)
    {
        dispatcher.Subscribe<BeforeRenderMessage>(m =>
        {
            frameViewProvider.NewFrame();
            resources.Queue.Submit([]);
        });

        dispatcher.Subscribe<AfterRenderMessage>(m =>
        {
        });

        dispatcher.Subscribe<PresentMessage>(m =>
        {
            resources.Surface.Present();
        });
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
