using SimulationFramework.Drawing.Canvas;
using SimulationFramework.Drawing.Pipelines;

namespace SimulationFramework.Drawing.OpenGL;

public class OpenGLGraphicsProvider : IGraphicsProvider
{
    public void Apply(Simulation simulation)
    {
        throw new NotImplementedException();
    }

    public IBuffer<T> CreateBuffer<T>(int size, ResourceOptions flags) where T : unmanaged
    {
        throw new NotImplementedException();
    }

    public IShader CreateShader(ShaderKind kind, string source)
    {
        throw new NotImplementedException();
    }

    public ITexture CreateTexture(int width, int height, Span<Color> data, ResourceOptions flags)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public ICanvas GetFrameCanvas()
    {
        throw new NotImplementedException();
    }

    public ITexture GetFrameTexture()
    {
        throw new NotImplementedException();
    }

    public IRenderer GetRenderer()
    {
        throw new NotImplementedException();
    }

    public void Initialize(Application application)
    {
        throw new NotImplementedException();
    }

    public ITexture LoadTexture(Span<byte> encodedData, ResourceOptions flags)
    {
        throw new NotImplementedException();
    }

    public void SetResourceLifetime(int lifetimeInFrames)
    {
        throw new NotImplementedException();
    }
}
