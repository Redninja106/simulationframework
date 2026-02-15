using SimulationFramework.Drawing;
using StbImageWriteSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL;
internal sealed class GLTexture : IGLImage, ITexture
{
    public int Width { get; }
    public int Height { get; }
    public TextureOptions Options { get; }
    public IMask? Resident { get; set; }

    private readonly uint id;

    internal bool hasMipmaps;
    private bool pixelsDirty;

    private readonly GLGraphics graphics;

    private GLCanvas? canvas;
    private Color[]? colors;

    private WrapMode wrapModeX;
    private WrapMode wrapModeY;
    private TextureFilter filter;

    public WrapMode WrapModeX
    {
        get
        {
            return wrapModeX;
        }
        set
        {
            wrapModeX = value;
            glBindTexture(GL_TEXTURE_2D, id);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, (int)MapTileMode(value));
            glBindTexture(GL_TEXTURE_2D, 0);
        }
    }

    public WrapMode WrapModeY
    {
        get
        {
            return wrapModeY;
        }
        set
        {
            wrapModeY = value;
            glBindTexture(GL_TEXTURE_2D, id);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, (int)MapTileMode(value));
            glBindTexture(GL_TEXTURE_2D, 0);
        }
    }

    public TextureFilter Filter
    {
        get
        {
            return filter;
        }
        set
        {
            filter = value;

            int magFilter = value switch
            {
                TextureFilter.Point or TextureFilter.MipmapPoint => (int)GL_NEAREST,
                TextureFilter.Linear or TextureFilter.MipmapLinear => (int)GL_LINEAR,
                _ => throw new ArgumentException(null, nameof(value))
            };

            int minFilter = value switch
            {
                TextureFilter.Point => (int)GL_NEAREST,
                TextureFilter.Linear => (int)GL_LINEAR,
                TextureFilter.MipmapPoint => (int)GL_NEAREST_MIPMAP_NEAREST,
                TextureFilter.MipmapLinear => (int)GL_LINEAR_MIPMAP_LINEAR,
                _ => throw new ArgumentException(null, nameof(value))
            };

            glBindTexture(GL_TEXTURE_2D, id);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, hasMipmaps ? minFilter : magFilter);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, magFilter);
            glBindTexture(GL_TEXTURE_2D, 0);
        }
    }

    public unsafe Span<Color> Pixels
    {
        get
        {
            if ((Options & TextureOptions.Constant) != 0)
                throw new InvalidOperationException("CPU access is not allowed on this texture!");

            if (colors is null || pixelsDirty)
                UpdateLocalPixels();

            return colors;
        }
    }


    public unsafe GLTexture(GLGraphics graphics, int width, int height, ReadOnlySpan<Color> colors, TextureOptions options)
    {
        this.Width = width;
        this.Height = height;
        this.Options = options;
        this.graphics = graphics;

        fixed (uint* idPtr = &id)
        {
            glGenTextures(1, idPtr);
        }

        Filter = TextureFilter.Point;
        WrapModeX = WrapMode.Repeat;
        WrapModeY = WrapMode.Repeat;

        fixed (Color* data = colors)
        {
            glBindTexture(GL_TEXTURE_2D, id);
            glTexImage2D(GL_TEXTURE_2D, 0, unchecked((int)GL_RGBA8), width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, data);
        }

    }

    private static uint MapTileMode(WrapMode mode) => mode switch
    {
        WrapMode.Mirror => GL_MIRRORED_REPEAT,
        WrapMode.Repeat => GL_REPEAT,
        WrapMode.Clamp => GL_CLAMP_TO_EDGE,
        WrapMode.None => GL_CLAMP_TO_BORDER,
    };

    public void InvalidatePixels()
    {
        pixelsDirty = true;
    }

    public unsafe void Dispose()
    {
        this.canvas?.Dispose();

        fixed (uint* texPtr = &id) 
        {
            glDeleteTextures(1, texPtr);
        }
    }

    private unsafe void UpdateLocalPixels()
    {
        this.colors = new Color[Width * Height];

        glBindTexture(GL_TEXTURE_2D, id);
        fixed (Color* buffer = colors) 
        {
            var canvas = (GLCanvas)GetCanvas();
            glBindFramebuffer(GL_FRAMEBUFFER, canvas.fbo);
            glReadPixels(0, 0, Width, Height, GL_RGBA, GL_UNSIGNED_BYTE, buffer);
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

        if (canvas is null)
        {
            canvas = new(graphics, this);
            canvas.ResetState();
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
        return id;
    }

    public void Encode(Stream destination, TextureEncoding encoding)
    {
        ImageWriter writer = new();

        unsafe
        {
            fixed (Color* pixelsPtr = this.Pixels)
            {
                writer.WritePng(pixelsPtr, Width, Height, ColorComponents.RedGreenBlueAlpha, destination);
            }
        }
    }

    internal void PrepareForRender()
    {
        canvas?.SubmitCommands();
    }
}