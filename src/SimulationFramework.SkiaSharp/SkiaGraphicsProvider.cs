using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace SimulationFramework.SkiaSharp;

internal sealed class SkiaGraphicsProvider : IGraphicsProvider
{
    internal readonly GRGlInterface glInterface;
    internal readonly GRContext backendContext;
    internal readonly ISkiaFrameProvider frameProvider;

    internal readonly SkiaSurface frameSurfaceAdapter;
    internal SkiaCanvas frameCanvas;
    internal Dictionary<(string fontName, TextStyles styles, int size), SKFont> fonts = new(); 

    public SkiaGraphicsProvider(ISkiaFrameProvider frameProvider, GRGlGetProcedureAddressDelegate getProcAddress)
    {
        this.frameProvider = frameProvider;

        glInterface = GRGlInterface.CreateOpenGl(getProcAddress);
        backendContext = GRContext.CreateGl(glInterface);

        frameProvider.SetContext(this.backendContext);

        frameSurfaceAdapter = new SkiaSurface(this, null, false);
    }

    public ICanvas GetFrameCanvas()
    {
        var canvas = frameProvider.GetCurrentFrame();

        if (frameCanvas is null || canvas != frameCanvas.GetSKCanvas())
        {
            frameCanvas?.Dispose();
            frameCanvas = new SkiaCanvas(this, null, canvas, false);
        }

        return frameCanvas;
    }


    public ISurface CreateSurface(int width, int height, Span<Color> data)
    {
        var bitmap = new SkiaSurface(this, new SKBitmap(width, height), true);

        if (!data.IsEmpty)
        {
        }

        return bitmap;
    }

    public ISurface LoadSurface(Span<byte> encodedData)
    {
        throw new NotImplementedException();
    }

    public void Apply(Simulation simulation)
    {
        
    }

    public void Dispose()
    {
        frameCanvas.Dispose();
        frameSurfaceAdapter.Dispose();
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

    public SKFont GetFont(string fontName, TextStyles styles, int size)
    {
        if (!fonts.ContainsKey((fontName, styles, size)))
        {
            var fontStyle = new SKFontStyle(
                styles.HasFlag(TextStyles.Bold) ? SKFontStyleWeight.Bold : SKFontStyleWeight.Normal,
                SKFontStyleWidth.Normal,
                styles.HasFlag(TextStyles.Italic) ? SKFontStyleSlant.Italic : SKFontStyleSlant.Upright
                );

            var typeface = SKTypeface.FromFamilyName(fontName, fontStyle);
            
            fonts.Add((fontName, styles, size), new SKFont(typeface, size));
        }

        return fonts[(fontName, styles, size)];
    }
}