namespace SimulationFramework.OpenGL.Geometry;

class TextureProgram : ShaderProgram
{
    private const string vert = @"
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
precision highp float;

out vec4 FragColor;

uniform sampler2D textureSampler;
uniform vec4 tint;

in vec2 tex;

void main()
{
    vec4 color = tint * texture(textureSampler, tex);
    if (color.a < 0.001) {
        discard;
    }
    FragColor = color;
} 
";
    public TextureProgram(string shaderVersion) : base(shaderVersion, vert, frag)
    {
    }
}