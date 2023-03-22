using SimulationFramework.Shaders;
using OpenTK.Graphics.OpenGL;

namespace SimulationFramework.Drawing.OpenGL;

public class OpenGLGraphicsProvider : IGraphicsProvider
{
    public IRenderingContext ImmediateRenderer { get; }
    public IGraphicsQueue ImmediateQueue { get; }
    public GraphicsCapabilities Capabilities { get; }

    public OpenGLGraphicsProvider()
    {

    }

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

    public ITexture<Color> GetDefaultRenderTarget()
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

    public ITexture<float> GetDefaultDepthTarget()
    {
        throw new NotImplementedException();
    }

    public IRenderingContext CreateRenderer()
    {
        throw new NotImplementedException();
    }

    public IGraphicsQueue CreateDeferredQueue()
    {
        throw new NotImplementedException();
    }

    public IRenderingContext CreateRenderingContext(IGraphicsQueue? queue)
    {
        throw new NotImplementedException();
    }

    public void DispatchComputeShader(IShader? shader, int threadsX, int threadsY, int threadsZ, IGraphicsQueue? queue = null)
    {
        throw new NotImplementedException();
    }
}