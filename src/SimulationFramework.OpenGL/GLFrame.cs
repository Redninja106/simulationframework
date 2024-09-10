using SimulationFramework.Drawing;
using StbImageWriteSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL;
public class GLFrame : IGLImage, ITexture
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public TextureOptions Options { get; } = TextureOptions.None;
    public Span<Color> Pixels => throw new NotSupportedException();

    public WrapMode WrapModeX { get; set; }
    public WrapMode WrapModeY { get; set; }
    public TextureFilter Filter { get; set; }
    public IMask? Resident { get; set; }

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

    public unsafe void Encode(Stream destination, TextureEncoding encoding)
    {
        ImageWriter writer = new();

        Color[] pixels = new Color[Width * Height];
        fixed (Color* pixelsPtr = pixels)
        {
            glReadPixels(0, 0, this.Width, this.Height, GL_RGBA, GL_UNSIGNED_BYTE, pixelsPtr);
            writer.WritePng(pixelsPtr, Width, Height, ColorComponents.RedGreenBlueAlpha, destination);
        }
    }

    public ICanvas GetCanvas()
    {
        throw new NotImplementedException();
    }
}
