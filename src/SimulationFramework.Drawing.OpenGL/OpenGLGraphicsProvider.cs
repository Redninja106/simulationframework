using SimulationFramework.Shaders;
using OpenTK.Graphics.OpenGL;

namespace SimulationFramework.Drawing.OpenGL;

public class OpenGLGraphicsProvider : IGraphicsProvider
{
    private OpenGLRenderer ImmediateRenderer;

    public IBuffer<T> CreateBuffer<T>(int size, ResourceOptions flags) where T : unmanaged
    {
        return new OpenGLBuffer<T>(size, flags);
    }

    public ITexture<T> CreateTexture<T>(int width, int height, Span<T> data, ResourceOptions flags) where T : unmanaged
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
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
    }

    public void InvalidateShader(Type shaderType)
    {
        throw new NotImplementedException();
    }

    public ITexture<Color> LoadTexture(Span<byte> encodedData, ResourceOptions flags)
    {
        throw new NotImplementedException();
    }
}