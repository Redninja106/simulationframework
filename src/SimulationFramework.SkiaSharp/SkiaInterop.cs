using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
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

    public static SKBitmap GetBitmap(ITexture texture)
    {
        ArgumentNullException.ThrowIfNull(texture);

        if (texture is not SkiaTexture skiaTexture)
            throw new ArgumentException("'texture' must be a texture created using the SkiaSharp graphics backend!");

        return skiaTexture.GetBitmap();
    }

    public static GRContext GetBackendContext(IGraphicsProvider graphics)
    {
        ArgumentNullException.ThrowIfNull(graphics);
        
        if (graphics is not SkiaGraphicsProvider skiaGraphics)
            throw new ArgumentException("'graphics' must be a graphics context created using the SkiaSharp graphics backend!");

        return skiaGraphics.backendContext;
    }

    // 
    public unsafe static int GetGLTextureID(ITexture texture)
    {
        // there doesn't seem to be a way to do this with skiasharp...
        throw new NotSupportedException();
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