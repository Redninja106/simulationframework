using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL;
internal class OpenGLTexture : ITexture
{
    public int Width { get; }
    public int Height { get; }
    public Span<Color> Pixels => throw new NotImplementedException();

    public OpenGLTexture(int width, int height, ReadOnlySpan<Color> pixels, TextureOptions options)
    {
        this.Width = width;
        this.Height = height;
    }

    public void ApplyChanges()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
    }

    public void Encode(Stream destination, TextureEncoding encoding)
    {
        throw new NotImplementedException();
    }

    public ICanvas GetCanvas()
    {
        throw new NotImplementedException();
    }
}
