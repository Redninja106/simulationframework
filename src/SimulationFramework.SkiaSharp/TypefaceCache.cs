using SimulationFramework.Drawing.Canvas;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.SkiaSharp;

internal static class FontTypefaceCache
{
    private static readonly Dictionary<(string, FontStyle), SKTypeface> cache = new();

    // flags that arent controlled in SKTypeface
    private const FontStyle TYPEFACE_IGNORE_BITS = FontStyle.Underline | FontStyle.Strikethrough;

    public static SKTypeface GetTypeface(string name, FontStyle style)
    {
        // we only care about flags that effect the typeface
        style &= ~TYPEFACE_IGNORE_BITS;
        
        if (!cache.ContainsKey((name, style)))
        {
            SKFontStyleWeight weight = style.HasFlag(FontStyle.Bold) ? SKFontStyleWeight.Bold : SKFontStyleWeight.Normal;
            SKFontStyleWidth width = SKFontStyleWidth.Normal;
            SKFontStyleSlant slant = style.HasFlag(FontStyle.Italic) ? SKFontStyleSlant.Italic : SKFontStyleSlant.Upright;
            var typeface = SKTypeface.FromFamilyName(name, weight, width, slant);
            cache.Add((name, style), typeface);
        }
        
        return cache[(name, style)];
    }

    public static void Clear()
    {
        foreach (var (_, value) in cache)
        {
            value.Dispose(); 
        }
    }
}