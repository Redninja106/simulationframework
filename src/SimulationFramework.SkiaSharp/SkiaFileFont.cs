using SimulationFramework.Drawing;
using SkiaSharp;

namespace SimulationFramework.SkiaSharp;

class SkiaFileFont : SkiaFont
{
    private readonly SKTypeface typeface;
    private byte[] data;

    public override string Name => typeface.FamilyName;
    public override bool SupportsBold => typeface.IsBold;
    public override bool SupportsItalic => typeface.IsItalic;

    public unsafe SkiaFileFont(ReadOnlySpan<byte> encodedBytes)
    {
        data = encodedBytes.ToArray();
        fixed (byte* encodedBytesPtr = data)
        {
            using var skdata = SKData.Create((nint)encodedBytesPtr, encodedBytes.Length);
            typeface = SKTypeface.FromData(skdata);
        }
    }

    public override SKTypeface GetTypeface(FontStyle style)
    {
        return typeface;
    }

    public override void Dispose()
    {
        if (IsDisposed)
            return;
        typeface.Dispose();
        base.Dispose();
    }
}
