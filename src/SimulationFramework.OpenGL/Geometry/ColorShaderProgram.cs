namespace SimulationFramework.OpenGL.Geometry;

class ColorShaderProgram : ShaderProgram
{

    private const string vert = @"
#version 330 core
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
#version 330 core
out vec4 FragColor;

in vec4 col;

void main()
{
    FragColor = col;
} 
";

    public ColorShaderProgram() : base(vert, frag)
    {
    }
}
