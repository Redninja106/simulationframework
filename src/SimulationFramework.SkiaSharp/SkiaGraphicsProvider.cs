using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Silk.NET.Core.Contexts;
using Silk.NET.OpenGL;
using SimulationFramework.Drawing;
using SimulationFramework.Messaging;
using SkiaSharp;

namespace SimulationFramework.SkiaSharp;

public sealed class SkiaGraphicsProvider : SkiaGraphicsObject, IGraphicsProvider
{
    internal readonly ISkiaFrameProvider frameProvider;
    internal readonly GRGlGetProcedureAddressDelegate getProcAddress;

    internal GRGlInterface glInterface;
    internal GRContext backendContext;
    internal SkiaCanvas frameCanvas;
    internal SkiaFrame frame;
    internal Dictionary<(string fontName, FontStyle styles, int size), SKFont> fonts = new();
    internal SkiaFont defaultFont;
    internal GL gl;

    public IFont DefaultFont => defaultFont;

    public unsafe SkiaGraphicsProvider(ISkiaFrameProvider frameProvider, GRGlGetProcedureAddressDelegate getProcAddress)
    {
        this.frameProvider = frameProvider;
        this.getProcAddress = getProcAddress;
        gl = GL.GetApi(s => getProcAddress(s));
        gl.DebugMessageCallback((source, type, id, severity, length, messagePtr, userParam) =>
        {
            var strBytes = new ReadOnlySpan<byte>((void*)messagePtr, length);
            var message = Encoding.UTF8.GetString(strBytes);

            Console.WriteLine("message: " + message);
        }, null);

        defaultFont = SkiaFont.FromName("Verdana");
    }

    public void Initialize(MessageDispatcher application)
    {
        glInterface = GRGlInterface.CreateOpenGl(getProcAddress);
        backendContext = GRContext.CreateGl(glInterface);
        

        frameProvider.SetContext(this.backendContext);
    }

    public ICanvas GetFrameCanvas()
    {
        var canvas = frameProvider.GetCurrentFrame();

        if (frameCanvas is null || canvas != frameCanvas.GetSKCanvas())
        {
            frame?.Dispose();
            frameCanvas?.Dispose();
            frameProvider.GetCurrentFrameSize(out int width, out int height);
            frame = new(width, height);
            frameCanvas = new SkiaCanvas(this, frame, canvas, true);
        }

        return frameCanvas;
    }

    public bool TryCreateTexture(int width, int height, ReadOnlySpan<Color> pixels, TextureOptions options, out ITexture texture)
    {
        try
        {
            uint tex = gl.GenTexture();
            texture = new SkiaTexture(this, tex, width, height, true, options);

            if (!pixels.IsEmpty)
            {
                if (pixels.Length != width * height)
                    throw new ArgumentException("data.Length != width * height");

                pixels.CopyTo(texture.Pixels);
                texture.ApplyChanges();
            }

            return true;
        }
        catch
        {
            texture = null;
            return false;
        }
    }

    public bool TryLoadTexture(ReadOnlySpan<byte> encodedData, TextureOptions options, out ITexture texture)
    {
        try
        {
            var bmp = SKBitmap.Decode(encodedData);
            var tex = (SkiaTexture)Graphics.CreateTexture(bmp.Width, bmp.Height, options);
            bmp.GetPixelSpan().CopyTo(MemoryMarshal.Cast<Color, byte>(tex.Pixels));
            texture = tex;
            return true;
        }
        catch
        {
            texture = null;
            return false;
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        frameCanvas.Dispose();
        backendContext.Dispose();
        glInterface.Dispose();
    }

    public SKFont GetFont(string fontName, FontStyle styles, int size)
    {
        if (!fonts.ContainsKey((fontName, styles, size)))
        {
            var fontStyle = new SKFontStyle(
                styles.HasFlag(FontStyle.Bold) ? SKFontStyleWeight.Bold : SKFontStyleWeight.Normal,
                SKFontStyleWidth.Normal,
                styles.HasFlag(FontStyle.Italic) ? SKFontStyleSlant.Italic : SKFontStyleSlant.Upright
                );

            var typeface = SKTypeface.FromFamilyName(fontName, fontStyle);
            
            fonts.Add((fontName, styles, size), new SKFont(typeface, size));
        }

        return fonts[(fontName, styles, size)];
    }

    public bool TryLoadFont(ReadOnlySpan<byte> encodedData, [NotNullWhen(true)] out IFont font)
    {
        try
        {
            font = SkiaFont.FromFileData(encodedData);
            return true;
        }
        catch
        {
            font = null;
            return false;
        }
    }

    public bool TryLoadSystemFont(string name, [NotNullWhen(true)] out IFont font)
    {
        try
        {
            font = SkiaFont.FromName(name);
            return true;
        }
        catch
        {
            font = null;
            return false;
        }
    }
}