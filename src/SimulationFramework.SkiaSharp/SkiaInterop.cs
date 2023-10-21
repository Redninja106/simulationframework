using System;
using System.Runtime.InteropServices;
using SimulationFramework.Components;
using SimulationFramework.Drawing;
using SkiaSharp;

namespace SimulationFramework.SkiaSharp;

public static class SkiaInterop
{
    public static SKCanvas GetCanvas(ICanvas canvas)
    {
        ArgumentNullException.ThrowIfNull(canvas);

        if (canvas is not SkiaCanvas skiaCanvas)
            throw new ArgumentException("'canvas' must be a canvas created using the SkiaSharp graphics backend!");
        
        return skiaCanvas.GetSKCanvas();
    }

    public static SKImage GetImage(ITexture texture)
    {
        ArgumentNullException.ThrowIfNull(texture);

        if (texture is not SkiaTexture skiaTexture)
            throw new ArgumentException("'texture' must be a texture created using the SkiaSharp graphics backend!");

        return skiaTexture.GetImage();
    }

    public static GRContext GetBackendContext(IGraphicsProvider graphics)
    {
        ArgumentNullException.ThrowIfNull(graphics);
        
        if (graphics is not SkiaGraphicsProvider skiaGraphics)
            throw new ArgumentException("'graphics' must be a graphics context created using the SkiaSharp graphics backend!");

        return skiaGraphics.backendContext;
    }

    // 
    public unsafe static uint GetGLTextureID(ITexture texture)
    {
        if (texture is not SkiaTexture skiaTexture)
            throw new ArgumentException("'texture' must be a texture created using the SkiaSharp graphics backend!");

        return skiaTexture.GetGLTexture();
    }

    public static SKPaint GetPaint(CanvasState state)
    {
        if (state is not SkiaCanvasState skstate)
            throw new ArgumentException("'state' must be a canvas state from a canvas created using the SkiaSharp graphics backend!");

        return skstate.Paint;
    }

    [DllImport("Kernel32")]
    internal static unsafe extern int GetModuleFileNameW(IntPtr hModule, char* lpFilename, int nSize);

    [DllImport("libSkiaSharp", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sk_texture_get_texture_handle(int param1);
}