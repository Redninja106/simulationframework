using SimulationFramework.Shaders;

namespace SimulationFramework.Drawing.OpenGL;

public class OpenGLGraphicsProvider : IGraphicsProvider
{
    public void Apply(Simulation simulation)
    {
        throw new NotImplementedException();
    }

    public void CompileShader<T>(ShaderKind kind) where T : struct, IShader
    {
        throw new NotImplementedException();
    }

    public IBuffer<T> CreateBuffer<T>(int size, ResourceOptions flags) where T : unmanaged
    {
        throw new NotImplementedException();
    }

    public ITexture<T> CreateTexture<T>(int width, int height, Span<T> data, ResourceOptions flags) where T : unmanaged
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

    public ITexture<Color> GetFrameTexture()
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

    public void InvalidateShader(Type shaderType)
    {
        throw new NotImplementedException();
    }

    public ITexture<Color> LoadTexture(Span<byte> encodedData, ResourceOptions flags)
    {
        throw new NotImplementedException();
    }

    public void SetResourceLifetime(int lifetimeInFrames)
    {
        throw new NotImplementedException();
    }
}
