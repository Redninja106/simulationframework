using SimulationFramework.Drawing;
using SimulationFramework.OpenGL.Fonts;
using SimulationFramework.OpenGL.Shaders;
using System;

namespace SimulationFramework.OpenGL.Geometry;

class GeometryEffectCollection
{
    private readonly GLGraphics graphics;

    public ColorShaderProgram colorProgram;
    public SDFFontProgram fontProgram;
    public TextureProgram textureProgram;

    public GeometryEffectCollection(GLGraphics graphics)
    {
        this.graphics = graphics;

        colorProgram = new();
        fontProgram = new();
        textureProgram = new();
    }

    public GeometryEffect GetEffectFromCanvasState(ref readonly CanvasState state)
    {
        if (state.Shader != null)
        {
            var program = graphics.GetShaderProgram(state.Shader, state.VertexShader);
            return new ProgrammableShaderEffect(state.Transform, state.Shader, state.VertexShader, program);
        }
        else
        {
            return new ColorGeometryEffect(colorProgram);
        }
    }

    public TextureGeometryEffect GetTextureGeometryEffect(ITexture texture, ColorF tint)
    {
        return new TextureGeometryEffect((GLTexture)texture, tint, textureProgram);
    }

    public SDFFontEffect GetFontEffect(GLFont font, ColorF color, TextStyle style)
    {
        throw new NotImplementedException();
    }
}

class EffectPool
{
    public void Rent()
    {

    }
}