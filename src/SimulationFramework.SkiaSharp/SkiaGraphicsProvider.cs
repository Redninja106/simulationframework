using System.Diagnostics.CodeAnalysis;
using Silk.NET.OpenGL;
using SimulationFramework.Components;
using SimulationFramework.Drawing;
using SimulationFramework.Messaging;
using SkiaSharp;

namespace SimulationFramework.SkiaSharp;

public sealed class SkiaGraphicsProvider : SkiaGraphicsObject, IGraphicsProvider
{
    internal readonly ISkiaFrameProvider frameProvider;
    internal readonly GRGlGetProcedureAddressDelegate getProcAddress;

    internal GRGlInterface glInterface;
    public GRContext backendContext;
    internal SkiaCanvas frameCanvas;
    internal SkiaFrame frame;
    internal Dictionary<(string fontName, FontStyle styles, int size), SKFont> fonts = new();
    internal SkiaFont defaultFont;
    internal GL gl;

    public IFont DefaultFont => defaultFont;

    public SkiaGraphicsProvider(ISkiaFrameProvider frameProvider, GL gl, GRGlGetProcedureAddressDelegate getProcAddress)
    {
        this.frameProvider = frameProvider;
        this.getProcAddress = getProcAddress;
        this.gl = gl;

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
            texture = new SkiaTexture(this, width, height, options);

            if (!pixels.IsEmpty)
            {
                if (pixels.Length != width * height)
                    throw new ArgumentException("data.Length != width * height");

                pixels.CopyTo(texture.Pixels);
                texture.ApplyChanges();
            }

            return true;
        }
        catch (Exception ex)
        {
            Log.Error($"Error creating texture: " + ex);
            texture = null;
            return false;
        }
    }

    public bool TryLoadTexture(ReadOnlySpan<byte> encodedData, TextureOptions options, out ITexture texture)
    {
        try
        {
            var bitmap = SKBitmap.Decode(encodedData);
            var skiaTexture = new SkiaTexture(this, bitmap.Width, bitmap.Height, options);
            var pixels = bitmap.Pixels;
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    int index = y * bitmap.Width + x;
                    var color = pixels[index];
                    skiaTexture.Pixels[index] = new(color.Red, color.Green, color.Blue, color.Alpha);
                }
            }

            skiaTexture.ApplyChanges();

            texture = skiaTexture;
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