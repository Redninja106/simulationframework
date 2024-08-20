﻿namespace SimulationFramework.OpenGL.Geometry;

class TextureProgram : ShaderProgram
{
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
    public TextureProgram() : base(vert, frag)
    {
    }
}