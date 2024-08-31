using SimulationFramework.Drawing;
using System;
using System.Numerics;

namespace SimulationFramework.OpenGL.Geometry;

internal class TextureGeometryEffect : GeometryEffect
{
    public GLTexture texture;
    internal ColorF tint;

    public TextureGeometryEffect(GLTexture texture, ColorF tint, ShaderProgram program) : base(program)
    {
        this.texture = texture;
        this.tint = tint;
    }

    public override unsafe void Apply(GLCanvas canvas, Matrix4x4 matrix)
    {
        Program.Use();
        var loc = Program.GetUniformLocation("transform"u8);
        glUniformMatrix4fv(loc, 1, 0, (float*)&matrix);
        glActiveTexture(GL_TEXTURE0);
        glBindTexture(GL_TEXTURE_2D, this.texture.GetID());
        glUniform1i(Program.GetUniformLocation("textureSampler"u8), 0);
        glUniform4f(Program.GetUniformLocation("tint"u8), tint.R, tint.G, tint.B, tint.A);
    }

    public override bool Equals(object? obj)
    {
        return obj is TextureGeometryEffect geometryEffect && 
            texture == geometryEffect.texture && 
            tint == geometryEffect.tint;
    }
}
