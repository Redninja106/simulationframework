using System;
using System.Diagnostics;
using Silk.NET.OpenGL;
using SimulationFramework.Drawing;
using SkiaSharp;

namespace SimulationFramework.SkiaSharp;

internal sealed class SkiaTexture : SkiaGraphicsObject, ITexture
{
    private readonly SkiaGraphicsProvider provider;
    private readonly SKSurface surface;
    private SKPixmap pixmap;
    private readonly bool owner;
    private readonly TextureOptions options;

    private readonly Color[] colors;
    private SkiaCanvas canvas;
    private bool pixelsDirty;
    private SKImage snapshot;
    private uint glTexture;
    private uint fbo;
    private GRBackendTexture backendTexture;
    private GRBackendRenderTarget renderTarget;

    public unsafe SkiaTexture(SkiaGraphicsProvider provider, uint glTexture, int width, int height, bool owner, TextureOptions options)
    {
        this.Width = width;
        this.Height = height;

        this.provider = provider;
        this.owner = owner;
        this.options = options;

        if (!options.HasFlag(TextureOptions.Constant))
        {
            this.colors = new Color[width * height];
            UpdateLocalPixels();
        }

        provider.gl.BindTexture(GLEnum.Texture2D, this.glTexture);
        
        if (this.colors is not null)
        {
            fixed (void* c = &this.colors[0])
            {
                provider.gl.TexImage2D(GLEnum.Texture2D, 0, InternalFormat.Rgba8, (uint)width, (uint)height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, c);
            }
        }
        else
        {
            provider.gl.TexImage2D(GLEnum.Texture2D, 0, InternalFormat.Rgba8, (uint)width, (uint)height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, null);
        }

        if (!options.HasFlag(TextureOptions.NonRenderTarget))
        {
            fbo = provider.gl.GenFramebuffer();
            provider.gl.BindFramebuffer(FramebufferTarget.Framebuffer, fbo);
            provider.gl.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, this.glTexture, 0);
            renderTarget = new GRBackendRenderTarget(width, height, 1, 0, new(fbo, (uint)SizedInternalFormat.Rgba8));
            this.surface = SKSurface.Create(provider.backendContext, renderTarget, SKColorType.Rgba8888, new(SKSurfacePropsFlags.None, SKPixelGeometry.Unknown));
        }
        else
        {
            this.backendTexture = new GRBackendTexture(width, height, false, new((uint)GLEnum.Texture2D, glTexture, (uint)SizedInternalFormat.Rgba8));
            Debug.Assert(backendTexture is not null);
            this.surface = SKSurface.Create(provider.backendContext, backendTexture, SKColorType.Rgba8888, new(SKSurfacePropsFlags.None, SKPixelGeometry.Unknown));
            Debug.Assert(surface is not null);
        }
    }

    public int Width { get; private set; }
    public int Height { get; private set; }
    
    public unsafe Span<Color> Pixels
    {
        get
        {
            if (colors is null)
                throw new InvalidOperationException("CPU access is not allowed on this texture!");

            if (pixelsDirty)
            {
                fixed (Color* colorPtr = &colors[0])
                {
                    surface.ReadPixels(new(Width, Height), (nint)colorPtr, Width * sizeof(Color), 0, 0);
                }
                pixelsDirty = false;
            }

            return colors;
        }
    }

    public void InvalidatePixels()
    {
        pixelsDirty = true;
    }

    public override void Dispose()
    {
        if (owner)
            surface.Dispose();

        this.canvas?.Dispose();
        base.Dispose();
    }

    private void UpdateLocalPixels()
    {
        unsafe
        {
            fixed (void* colorsPtr = &colors[0]) 
            {
                provider.gl.GetTexImage(GLEnum.Texture2D, 0, PixelFormat.Rgba, PixelType.UnsignedInt, colorsPtr);
            }
            pixelsDirty = false;
        }
    }

    public ICanvas GetCanvas()
    {
        if (options.HasFlag(TextureOptions.NonRenderTarget))
        {
            throw new InvalidOperationException("Can only render to a texture with the TextureOptions.RenderTarget flag.");
        }

        if (this.canvas is null || this.canvas.IsDisposed)
        {
            this.canvas = new SkiaCanvas(this.provider, this, surface.Canvas, false);
        }

        return this.canvas;
    }

    public void ApplyChanges()
    {
        if (pixelsDirty)
        {
            Log.Warn("Texture was changed on the gpu and then ApplyChanges() was called. This could lead to changes being lost.");
        }

        unsafe
        {

            var pixmap = surface.PeekPixels();
            var pixels = pixmap.GetPixelSpan<Color>();
            for (int i = 0; i < colors.Length; i++)
            {
                pixels[i] = colors[i];
            }

        }
    }
    
    private static int ToArgb(Color color)
    {
        return color.A << 24 | color.R << 16 | color.G << 8 | color.B;
    }
    
    private static Color ToColor(int argb)
    {
        return new Color((byte)(argb >> 16 & 0xFF), (byte)(argb >> 8 & 0xFF), (byte)(argb & 0xFF), (byte)(argb >> 24 & 0xFF));
    }

    public SKSurface GetSurface()
    {
        return surface;
    }

    public SKImage GetImage()
    {
        if (pixelsDirty || snapshot is null)
        {
            snapshot?.Dispose();
            snapshot = surface.Snapshot();
        }

        return snapshot;
    }

    internal uint GetGLTextureID()
    {
        return glTexture;
    }
}