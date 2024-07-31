using System;
using System.Collections.Generic;
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

namespace SimulationFramework.OpenGL;
public unsafe class GLFont : IFont
{
    private byte[] pinnedData;

    public string Name { get; }
    public bool SupportsBold => bold != null;
    public bool SupportsItalic => italic != null;

    private SDFFontAtlas regular;
    private SDFFontAtlas? bold;
    private SDFFontAtlas? italic;

    public GLFont(GLGraphicsProvider provider, ReadOnlySpan<byte> ttfData)
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
        if (style.HasFlag(TextStyle.Bold))
        {
            return bold ?? regular;
        }
        if (style.HasFlag(TextStyle.Italic))
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

internal unsafe class SDFFontAtlas
{
    public int Width => atlasWidth;
    public int Height => atlasHeight;

    stbtt_fontinfo fontInfo;
    Dictionary<int, CharInfo> chars = [];
    public int pixelSize;
    uint tex;
    int nextX, nextY;
    int atlasWidth = 1024, atlasHeight = 1024;

    public SDFFontAtlas(stbtt_fontinfo fontInfo, int pixelSize)
    {
        this.fontInfo = fontInfo;
        this.pixelSize = pixelSize;

        // EnsureContains('a'..'z');
        // EnsureContains('A'..'Z');
        // EnsureContains('0'..'9');

        fixed (uint* texPtr = &tex)
        {
            glGenTextures(1, texPtr);
        }

        glBindTexture(GL_TEXTURE_2D, tex);
        glTexImage2D(GL_TEXTURE_2D, 0, (int)GL_R8, atlasWidth, atlasHeight, 0, GL_RED, GL_UNSIGNED_BYTE, null);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, (int)GL_LINEAR);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, (int)GL_LINEAR);
        glBindTexture(GL_TEXTURE_2D, 0);

        for (int i = 'A'; i < 'z'; i++)
        {
            AddCodepoint(i);
        }
    }

    private unsafe CharInfo AddCodepoint(int codepoint)
    {
        int width, height;
        int xoff, yoff;

        float scale = stbtt_ScaleForPixelHeight(fontInfo, pixelSize);
        var sdf = stbtt_GetCodepointSDF(fontInfo, scale, codepoint, 5, 180, 180f / 5f, &width, &height, &xoff, &yoff);

        glPixelStorei(GL_UNPACK_ALIGNMENT, 1);
        glBindTexture(GL_TEXTURE_2D, tex);
        glTexSubImage2D(GL_TEXTURE_2D, 0, nextX, nextY, width, height, GL_RED, GL_UNSIGNED_BYTE, sdf);
        glBindTexture(GL_TEXTURE_2D, 0);

        int advance, leftSideBearing;
        stbtt_GetCodepointHMetrics(fontInfo, codepoint, &advance, &leftSideBearing);

        CharInfo charInfo = new()
        {
            destination = new(xoff, yoff, width, height),
            source = new(nextX, nextY, width, height),
            xAdvance = advance * scale,
        };

        chars.Add(codepoint, charInfo);

        nextX += pixelSize;
        if (nextX >= atlasWidth)
        {
            nextX = 0;
            nextY += pixelSize;
        }

        stbtt_FreeSDF(sdf, null);

        return charInfo;
    }

    public uint GetTextureID()
    {
        return tex;
    }

    public Vector2 GetCodepoint(int codepoint, float size, Vector2 position, out Rectangle source, out Rectangle destination)
    {
        if (!chars.TryGetValue(codepoint, out CharInfo charInfo))
        {
            // TODO: atlas resizing
            charInfo = AddCodepoint(codepoint);
        }

        float scale = size / pixelSize;

        source = charInfo.source;
        destination = charInfo.destination;

        destination.Position *= scale;
        destination.Size *= scale;

        destination.Position += position;

        position.X += charInfo.xAdvance * scale;
        return position;
    }

    internal struct CharInfo
    {
        public Rectangle source;
        public Rectangle destination;
        public float xAdvance;
    }

}

class SDFFontEffect : GeometryEffect
{
    uint program;
    public uint fontAtlas;
    public float textSlant;
    public bool boldThreshold;

    const string VS = @"
#version 330 core
layout (location = 0) in vec2 aPos;
layout (location = 1) in vec2 aTex;

uniform mat4 transform;
uniform float slant;

out vec2 tex;

void main()
{
    gl_Position = transform * vec4(aPos.xy, 0, 1.0);
    tex = aTex;
}
";

    // https://drewcassidy.me/2020/06/26/sdf-antialiasing/
    const string FS = @"
#version 330 core
out vec4 FragColor;

uniform sampler2D textureSampler;
uniform vec4 tint;
uniform float threshold;

in vec2 tex;

void main()
{
    float dist = threshold - texture(textureSampler, tex).r;
    if (dist > 0)
    {
        discard;
    }

    // https://mortoray.com/antialiasing-with-a-signed-distance-field/

    float distInPixels = dist / length(vec2(dFdx(dist), dFdy(dist)));

    FragColor.rgb = tint.rgb;
    FragColor.a = tint.a * clamp(0.5 - distInPixels, 0, 1);
} 
";

    public SDFFontEffect()
    {
        program = MakeProgram(VS, FS);
    }

    public override unsafe void ApplyState(CanvasState state, Matrix4x4 matrix)
    {
        glUniformMatrix4fv(GetUniformLocation(program, "transform"u8), 1, 0, (float*)&matrix);
        glActiveTexture(GL_TEXTURE0);
        glBindTexture(GL_TEXTURE_2D, this.fontAtlas);
        glUniform1i(GetUniformLocation(program, "textureSampler"u8), 0);
        glUniform4f(GetUniformLocation(program, "tint"u8), state.Color.R, state.Color.G, state.Color.B, state.Color.A);
        // TODO: tweak threshold for small fonts instead of making them all bold (lol)
        glUniform1f(GetUniformLocation(program, "threshold"u8), boldThreshold ? 140f / 255f : 130f / 255f);
    }

    public override void Use()
    {
        glUseProgram(program);
    }
}