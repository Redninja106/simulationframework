using System.Numerics;
using SimulationFramework.Drawing;
using SimulationFramework.OpenGL.Geometry;

namespace SimulationFramework.OpenGL.Fonts;

class SDFFontEffect : GeometryEffect
{
    public SDFFontAtlas atlas;
    public float textSlant;
    public bool boldThreshold;
    public ColorF color;

    public SDFFontEffect(ColorF color, SDFFontAtlas atlas, ShaderProgram program) : base(program)
    {
        this.atlas = atlas;
        this.color = color;
    }

    public override unsafe void Apply(GLCanvas canvas, Matrix4x4 projection)
    {
        Program.Use();
        glUniformMatrix4fv(Program.GetUniformLocation("transform"u8), 1, 0, (float*)&projection);
        glActiveTexture(GL_TEXTURE0);
        glBindTexture(GL_TEXTURE_2D, this.atlas.GetTextureID());
        glUniform1i(Program.GetUniformLocation("textureSampler"u8), 0);
        glUniform4f(Program.GetUniformLocation("tint"u8), color.R, color.G, color.B, color.A);
        // TODO: tweak threshold for small fonts instead of making them all bold (lol)
        glUniform1f(Program.GetUniformLocation("threshold"u8), boldThreshold ? 140f / 255f : 130f / 255f);
    }

    public override bool CheckStateCompatibility(ref readonly CanvasState state)
    {
        return state.Color == color;
    }
}