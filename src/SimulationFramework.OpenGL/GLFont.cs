using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SimulationFramework.Drawing;
using StbTrueTypeSharp;
using static StbTrueTypeSharp.StbTrueType;

namespace SimulationFramework.OpenGL;
public unsafe class GLFont : IFont
{
    private stbtt_fontinfo fontinfo;
    private byte[] pinnedData;
    private GCHandle pinnedDataHandle;

    public string Name { get; }
    public bool SupportsBold { get; }
    public bool SupportsItalic { get; }

    private GLTexture atlas;
    stbtt_bakedchar[] chars;

    public GLFont(GLGraphicsProvider provider, ReadOnlySpan<byte> ttfData)
    {
        pinnedData = GC.AllocateArray<byte>(ttfData.Length, true);
        ttfData.CopyTo(pinnedData);

        fontinfo = new();

        byte* dataPtr = (byte*)Unsafe.AsPointer(ref MemoryMarshal.GetArrayDataReference(pinnedData));
        if (stbtt_InitFont(fontinfo, dataPtr, 0) == 0)
        {
            throw new Exception("Error initializing font!");
        }
        byte[] tempBitmap = new byte[512 * 512];
        chars = new stbtt_bakedchar[96];
        fixed (byte* bmpPtr = tempBitmap) 
        {
            fixed (stbtt_bakedchar* charsPtr = chars)
            {
                stbtt_BakeFontBitmap(dataPtr, 0, 24, bmpPtr, 512, 512, 32, 96, charsPtr);
                atlas = new(provider, 512, 512, null, TextureOptions.None);
                TextureFlip(512, 512, tempBitmap, atlas.Pixels);
                atlas.ApplyChanges();
            }
        }
    }

    private void TextureFlip(int width, int height, ReadOnlySpan<byte> src, Span<Color> dest)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var srcIndex = y * width + x;
                var dstIndex = (height - y - 1) * width + x;

                dest[dstIndex] = new(255, 255, 255, src[srcIndex]);
            }
        }
    }

    internal Vector2 GetCodepointPosition(int codepoint, Vector2 baseline, float size, FontStyle style, out Rectangle source, out Rectangle destination)
    {
        stbtt_aligned_quad quad;
        fixed (stbtt_bakedchar* charsPtr = chars)
        {
            float x = baseline.X, y = baseline.Y;
            stbtt_GetBakedQuad(charsPtr, 1, 1, codepoint - 32, &x, &y, &quad, 1);
        }
        float scale = size / 24;
        source = Rectangle.FromLTRB(quad.s0, quad.t0, quad.s1, quad.t1);

        stbtt_bakedchar c = chars[codepoint - 32];

        float cx = baseline.X + (scale * c.xoff) + .5f;
        float cy = baseline.Y + (scale * c.yoff) + .5f;

        destination = Rectangle.FromLTRB(
            cx, 
            cy,
            cx + scale * (c.x1 - c.x0),
            cy + scale * (c.y1 - c.y0)
            );

        return new(
            baseline.X + MathF.Floor(c.xadvance * scale),
            baseline.Y
            );
    }

    internal GLTexture GetAtlasTexture(float fontSize, FontStyle style)
    {
        return atlas;
    }

    public Rectangle MeasureText(ReadOnlySpan<char> text, float size, FontStyle style, float maxLength, out int charsMeasured)
    {
        Rectangle result = default;

        Vector2 baseline = Vector2.Zero;
        for (int i = 0; i < text.Length; i++)
        {
            baseline = GetCodepointPosition(text[i], baseline, size, style, out Rectangle source, out Rectangle destination);

            if (i == 0)
            {
                result = destination;
            }
            else
            {
                result = result.Union(destination);
            }
        }
        charsMeasured = text.Length;
        return result;
    }

    public void Dispose()
    {
    }
}