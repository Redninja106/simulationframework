using Silk.NET.OpenGL;
using SimulationFramework.Components;
using SimulationFramework.Drawing;
using SimulationFramework.Messaging;
using System.Diagnostics.CodeAnalysis;

namespace SimulationFramework.OpenGL;

public class OpenGLGraphicsProvider : IGraphicsProvider
{
    private GL gl;

    public OpenGLGraphicsProvider(GL gl)
    {
        this.gl = gl;
    }

    public IFont DefaultFont => throw new NotImplementedException();

    public void Dispose()
    {
        gl.Dispose();
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
        texture = new OpenGLTexture(width, height, pixels, options);
        return true;
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
