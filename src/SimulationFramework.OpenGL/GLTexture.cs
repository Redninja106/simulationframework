using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL;
internal sealed class GLTexture : ITexture
{
    public int Width { get; }
    public int Height { get; }
    public TextureOptions Options { get; }

    private readonly Color[] colors;
    private bool pixelsDirty;

    private readonly uint id;
    private readonly GLCanvas? canvas;

    public unsafe GLTexture(GLGraphicsProvider provider, int width, int height, ReadOnlySpan<Color> colors, TextureOptions options)
    {
        this.Width = width;
        this.Height = height;
        this.Options = options;

        fixed (uint* idPtr = &id)
        {
            glGenTextures(1, idPtr);
        }

        glBindTexture(GL_TEXTURE_2D, id);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, (int)GL_NEAREST);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, (int)GL_NEAREST);

        fixed (Color* data = colors)
        {
            glTexImage2D(GL_TEXTURE_2D, 0, unchecked((int)GL_RGBA8), width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, data);
        }

        canvas = new(provider, this);

        if (!options.HasFlag(TextureOptions.Constant))
        {
            this.colors = new Color[width * height];
            UpdateLocalPixels();
        }
    }

    public unsafe Span<Color> Pixels
    {
        get
        {
            if (colors is null)
                throw new InvalidOperationException("CPU access is not allowed on this texture!");

            if (pixelsDirty)
                UpdateLocalPixels();

            return colors;
        }
    }

    public void InvalidatePixels()
    {
        pixelsDirty = true;
    }

    public void Dispose()
    {
        // if (IsDisposed)
        //     return;
        // 
        // this.canvas.Dispose();
        // this.image.Dispose();
        // this.surface.Dispose();
        // this.backendTexture.Dispose();
        // 
        // provider.gl.DeleteTexture(this.id);
        // 
        // base.Dispose();
    }

    private unsafe void UpdateLocalPixels()
    {
        glFinish();
        glBindTexture(GL_TEXTURE_2D, id);
        fixed (Color* buffer = colors) 
        {
            glGetTexImage(GL_TEXTURE_2D, 0, GL_RGBA, GL_UNSIGNED_BYTE, buffer);
        }
        glBindTexture(GL_TEXTURE_2D, 0);
        pixelsDirty = false;
    }

    public unsafe void Update(ReadOnlySpan<Color> pixels)
    {
        glBindTexture(GL_TEXTURE_2D, id);
        fixed (Color* buffer = pixels)
        {
            glTexSubImage2D(GL_TEXTURE_2D, 0, 0, 0, this.Width, this.Height, GL_RGBA, GL_UNSIGNED_BYTE, buffer);
        }
        glBindTexture(GL_TEXTURE_2D, 0);
    }

    public ICanvas GetCanvas()
    {
        // if (IsDisposed)
        //     throw new InvalidOperationException("Cannot use disposed texture!");

        if (Options.HasFlag(TextureOptions.NonRenderTarget))
        {
            throw new InvalidOperationException("Cannot render to a texture with the TextureOptions.NonRenderTarget flag.");
        }

        return this.canvas;
    }

    public void ApplyChanges()
    {
        if (pixelsDirty)
        {
            Log.Warn("Texture was changed on the gpu and then ApplyChanges() was called. This could lead to changes being lost.");
        }

        Update(this.Pixels);
    }

    public uint GetID()
    {
        // if (IsDisposed)
        //     throw new InvalidOperationException("Cannot use disposed texture!");
        return id;
    }

    public void Encode(Stream destination, TextureEncoding encoding)
    {
        // var data = destination.Encode(encoding switch
        // {
        //     TextureEncoding.PNG => SKEncodedImageFormat.Png,
        //     _ => throw new Exception("Unsupported or unrecognized encoding!")
        // }, 100);
        // 
        // data.SaveTo(destination);
        throw new NotSupportedException("image encoding not supported on opengl yet!");
    }
}