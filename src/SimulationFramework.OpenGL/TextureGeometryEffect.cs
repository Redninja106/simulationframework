using SimulationFramework.Drawing;
using System.Numerics;

namespace SimulationFramework.OpenGL;

internal class TextureGeometryEffect : GeometryEffect
{
    public GLTexture texture;

    private const string vert = @"
#version 330 core
layout (location = 0) in vec2 aPos;
layout (location = 1) in vec2 aTex;

uniform mat4 transform;

out vec2 tex;

void main()
{
    gl_Position = transform * vec4(aPos.xy, 0, 1.0);
    tex = aTex;
}
";


    private const string frag = @"
#version 330 core
out vec4 FragColor;

uniform sampler2D textureSampler;
uniform vec4 tint;

in vec2 tex;

void main()
{
    FragColor = tint * texture(textureSampler, tex);
} 
";

    uint program;
    internal ColorF tint;

    public TextureGeometryEffect()
    {
        program = MakeProgram(vert, frag);
    }

    public override unsafe void ApplyState(CanvasState state, Matrix4x4 matrix)
    {
        var loc = glGetUniformLocation(program, ToPointer("transform"u8));
        glUniformMatrix4fv(loc, 1, 0, (float*)&matrix);
        glActiveTexture(GL_TEXTURE0);
        glBindTexture(GL_TEXTURE_2D, this.texture.GetID());
        glUniform1i(glGetUniformLocation(program, ToPointer("textureSampler"u8)), 0);
        glUniform4f(glGetUniformLocation(program, ToPointer("tint"u8)), tint.R, tint.G, tint.B, tint.A);
    }

    private unsafe byte* ToPointer(ReadOnlySpan<byte> str)
    {
        fixed (byte* a = str)
        {
            return a;
        }
    }

    public override void Use()
    {
        glUseProgram(program);
    }
}