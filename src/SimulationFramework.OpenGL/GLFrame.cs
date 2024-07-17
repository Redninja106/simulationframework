using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL;
public class GLFrame : ITexture
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public TextureOptions Options { get; } = TextureOptions.None;
    public Span<Color> Pixels => throw new NotSupportedException();

    public TileMode WrapModeX { get; set; }
    public TileMode WrapModeY { get; set; }
    public TextureFilter Filter { get; set; }

    public GLFrame(int width, int height)
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
        throw new NotImplementedException();
    }

    public void Resize(int width, int height)
    {
        this.Width = width;
        this.Height = height;
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
