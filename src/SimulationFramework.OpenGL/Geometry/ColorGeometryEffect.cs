using SimulationFramework.Drawing;
using System.Numerics;

namespace SimulationFramework.OpenGL.Geometry;

class ColorGeometryEffect : GeometryEffect
{
    public ColorGeometryEffect(ShaderProgram program) : base(program)
    {
    }

    public override void Apply(GLCanvas canvas, Matrix4x4 projection)
    {
        Program.Use();
        unsafe
        {
            glUniformMatrix4fv(Program.GetUniformLocation("transform"u8), 1, 0, (float*)&projection);
        }
    }

    public override bool Equals(object? obj)
    {
        return obj is ColorGeometryEffect;
    }
}