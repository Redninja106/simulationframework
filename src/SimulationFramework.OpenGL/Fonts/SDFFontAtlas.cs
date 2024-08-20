using System.Collections.Generic;
using System.Numerics;
using static StbTrueTypeSharp.StbTrueType;

namespace SimulationFramework.OpenGL.Fonts;

// TODO: dynamic atlas size
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
