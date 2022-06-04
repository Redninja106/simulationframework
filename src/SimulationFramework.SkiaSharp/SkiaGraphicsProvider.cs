using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Canvas;
using SkiaSharp;

namespace SimulationFramework.SkiaSharp;

public sealed class SkiaGraphicsProvider : IGraphicsProvider
{
    internal readonly GRGlInterface glInterface;
    internal readonly GRContext backendContext;
    internal readonly ISkiaFrameProvider frameProvider;

    internal SkiaCanvas frameCanvas;
    internal Dictionary<(string fontName, FontStyle styles, int size), SKFont> fonts = new(); 

    public SkiaGraphicsProvider(ISkiaFrameProvider frameProvider, GRGlGetProcedureAddressDelegate getProcAddress)
    {
        this.frameProvider = frameProvider;

        glInterface = GRGlInterface.CreateOpenGl(getProcAddress);
        backendContext = GRContext.CreateGl(glInterface);

        frameProvider.SetContext(this.backendContext);
    }

    public ICanvas GetFrameCanvas()
    {
        var canvas = frameProvider.GetCurrentFrame();

        if (frameCanvas is null || canvas != frameCanvas.GetSKCanvas())
        {
            frameCanvas?.Dispose();
            frameProvider.GetCurrentFrameSize(out int width, out int height);
            frameCanvas = new SkiaCanvas(this, new SkiaFrame(width, height), canvas, false);
        }

        return frameCanvas;
    }


    public ITexture CreateTexture(int width, int height, Span<Color> data, TextureFlags flags = TextureFlags.None)
    {
        var bitmap = new SkiaTexture(this, new SKBitmap(width, height), true);

        if (!data.IsEmpty)
        {
        }

        return bitmap;
    }

    public ITexture LoadTexture(Span<byte> encodedData, TextureFlags flags = TextureFlags.None)
    {
        return new SkiaTexture(this, SKBitmap.Decode(encodedData), true);
    }

    public void Dispose()
    {
        frameCanvas.Dispose();
        backendContext.Dispose();
        glInterface.Dispose();

        ClearFontCache();
    }

    public void ClearFontCache()
    {
        while (fonts.Count > 0)
        {
            if (fonts.Remove(fonts.Keys.Last(), out var font))
            {
                font.Dispose();
            }
        }
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

    public void Initialize(Application application)
    {
    }
}