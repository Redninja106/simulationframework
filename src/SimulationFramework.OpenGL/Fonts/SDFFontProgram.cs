namespace SimulationFramework.OpenGL.Fonts;

class SDFFontProgram : ShaderProgram
{
    const string vert = @"
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
    const string frag = @"
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

    public SDFFontProgram() : base(vert, frag)
    {
    }
}