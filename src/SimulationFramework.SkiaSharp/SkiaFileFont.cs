using SimulationFramework.Drawing;
using SkiaSharp;
using System;

namespace SimulationFramework.SkiaSharp;

class SkiaFileFont : SkiaFont
{
    private readonly SKTypeface typeface;

    public override string Name => typeface.FamilyName;
    public override bool SupportsBold => typeface.IsBold;
    public override bool SupportsItalic => typeface.IsItalic;

    public unsafe SkiaFileFont(ReadOnlySpan<byte> encodedBytes)
    {
        fixed (byte* encodedBytesPtr = encodedBytes)
        {
            using var data = SKData.Create((nint)encodedBytesPtr, encodedBytes.Length);
            typeface = SKTypeface.FromData(data);
        }
    }

    public override SKTypeface GetTypeface(FontStyle style)
    {
        return typeface;
    }

    public override void Dispose()
    {
        typeface.Dispose();
        base.Dispose();
    }
}
