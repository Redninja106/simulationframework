namespace SimulationFramework.OpenGL.Geometry;

class ColorShaderProgram : ShaderProgram
{

    private const string vert = @"
layout (location = 0) in vec2 aPos;
layout (location = 1) in vec4 aCol;

uniform mat4 transform;

out vec4 col;

void main()
{
    gl_Position = transform * vec4(aPos.xy, 0, 1.0);
    col = aCol;
}
";


    private const string frag = @"
precision highp float;

out vec4 FragColor;

in vec4 col;

void main()
{
    FragColor = col;
} 
";

    public ColorShaderProgram(string shaderVersion) : base(shaderVersion, vert, frag)
    {
    }
}
