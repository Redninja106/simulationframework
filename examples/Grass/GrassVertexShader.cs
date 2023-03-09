using SimulationFramework;
using SimulationFramework.Shaders;
using System.Numerics;

namespace Grass;

struct GrassVertexShader : IShader
{
    [Input(InputSemantic.Vertex)]
    public GrassVertex input;

    [Output]
    Vector3 fragPos;

    [Output]
    float scaledHeight;

    [Output(OutputSemantic.Position)]
    public Vector4 position;

    [Uniform]
    public Matrix4x4 TransformMatrix;

    [Uniform]
    public float time;

    [Uniform]
    public float windHeading;

    public void Main()
    {
        Vector3 offset = new(MathF.Cos(windHeading), 0, MathF.Sin(windHeading));
        offset *= MathF.Cos(time + input.RandomValue * MathF.Tau);

        input.Position += offset * .01f * input.PivotWeight;

        fragPos = input.Position;
        scaledHeight = fragPos.Y / input.height;

        position = new(input.Position, 1);
        position = Vector4.Transform(position, TransformMatrix);
    }
}
