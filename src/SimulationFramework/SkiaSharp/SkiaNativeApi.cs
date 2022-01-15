using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace SimulationFramework.SkiaSharp;

/// <summary>
/// SkiaSharp is missing some APIs, so we do a little trickery to have them here.
/// </summary>
internal static class SkiaNativeApi
{
    // from https://github.com/mono/SkiaSharp/blob/4e9a505aefd68882659af5c4f71aaf50af728151/binding/Binding/SkiaApi.cs
#if __IOS__ || __TVOS__ || __WATCHOS__
		private const string SKIA = "@rpath/libSkiaSharp.framework/libSkiaSharp";
#else
    private const string SKIA = "libSkiaSharp";
#endif

    // this is exposed because SKCanvas only accepts a managed array for the points, and we need to pass spans
    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static unsafe extern void sk_canvas_draw_points(IntPtr param0, SKPointMode param1, /* size_t */ IntPtr param2, SKPoint* param3, IntPtr param4);

    // these are exposed because SKShader.Create___Gradient() only accepts arrays
    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static unsafe extern IntPtr sk_shader_new_linear_gradient(Vector2* points, uint* colors, float* colorPos, int colorCount, SKShaderTileMode tileMode, SKMatrix* localMatrix);
    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static unsafe extern IntPtr sk_shader_new_radial_gradient(Vector2* center, Single radius, UInt32* colors, Single* colorPos, Int32 colorCount, SKShaderTileMode tileMode, SKMatrix* localMatrix);
}