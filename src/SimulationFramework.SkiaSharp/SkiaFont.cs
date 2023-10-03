using SimulationFramework.Drawing;
using SkiaSharp;
using System;

namespace SimulationFramework.SkiaSharp;
internal abstract class SkiaFont : SkiaGraphicsObject, IFont
{
    public abstract string Name { get; }
    public abstract bool SupportsBold { get; }
    public abstract bool SupportsItalic { get; }

    public static SkiaFont FromName(string name)
    {
        return new SkiaSystemFont(name);
    }

    public static unsafe SkiaFont FromFileData(ReadOnlySpan<byte> encodedBytes)
    {
        return new SkiaFileFont(encodedBytes);
    }

    public abstract SKTypeface GetTypeface(FontStyle style);
    
}
