using System;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using SimulationFramework.Drawing;
using StbTrueTypeSharp;
using static StbTrueTypeSharp.StbTrueType;

namespace SimulationFramework.OpenGL.Fonts;
internal unsafe class GLFont : IFont
{
    private byte[] pinnedData;

    public string Name { get; }
    public bool SupportsBold => bold != null;
    public bool SupportsItalic => italic != null;

    private SDFFontAtlas regular;
    private SDFFontAtlas? bold;
    private SDFFontAtlas? italic;

    public GLFont(GLGraphics provider, ReadOnlySpan<byte> ttfData)
    {
        pinnedData = GC.AllocateArray<byte>(ttfData.Length, true);
        ttfData.CopyTo(pinnedData);

        byte* data = (byte*)Unsafe.AsPointer(ref MemoryMarshal.GetArrayDataReference(pinnedData));

        regular = LoadFontWithFlags(data, 0) ?? throw new("could not load regular font style!");
        bold = LoadFontWithFlags(data, 1); // 1 = bold
        italic = LoadFontWithFlags(data, 2); // 2 = italic
    }

    private SDFFontAtlas? LoadFontWithFlags(byte* data, int flags)
    {
        int offset = GetFontOffsetWithFlags(data, flags);

        if (offset < 0)
        {
            return null;
        }

        stbtt_fontinfo fontinfo = new();
        if (stbtt_InitFont(fontinfo, data, offset) == 0)
        {
            throw new Exception("Error initializing font!");
        }
        return new SDFFontAtlas(fontinfo, 64);

    }

    private unsafe static int GetFontOffsetWithFlags(byte* data, int flags)
    {
        for (int i = 0; ; i++)
        {
            int offset = stbtt_GetFontOffsetForIndex(data, i);
            if (offset < 0)
            {
                break;
            }
            if (CheckFlags(data, (uint)offset, flags))
            {
                return offset;
            }
        }
        return -1;
    }

    private unsafe static bool CheckFlags(byte* fc, uint offset, int flags)
    {
        if (stbtt__isfont(fc + offset) == 0)
        {
            return false;
        }

        if (flags != 0)
        {
            uint num2 = stbtt__find_table(fc, offset, "head");
            if ((ttUSHORT(fc + num2 + 44) & 7) != (flags & 7))
            {
                return false;
            }
        }
        return true;
    }

    public Rectangle MeasureText(ReadOnlySpan<char> text, float size, TextStyle style, float maxLength, out int charsMeasured)
    {
        SDFFontAtlas atlas = GetAtlas(style);
        Rectangle result = default;

        Vector2 baseline = Vector2.Zero;
        for (int i = 0; i < text.Length; i++)
        {
            baseline = atlas.GetCodepoint(text[i], size, baseline, out Rectangle _, out Rectangle destination);


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

    internal SDFFontAtlas GetAtlas(TextStyle style)
    {
        if ((style & TextStyle.Bold) != 0)
        {
            return bold ?? regular;
        }
        if ((style & TextStyle.Italic) != 0)
        {
            return italic ?? regular;
        }
        return regular;
    }

    public void Dispose()
    {
    }

    public Rectangle GetCodepointRectangle(int codepoint, float size, TextStyle style, out float xAdvance)
    {
        var atlas = GetAtlas(style);
        var baseline = atlas.GetCodepoint(codepoint, size, Vector2.Zero, out _, out Rectangle destination);
        xAdvance = baseline.X;
        return destination;
    }
}
