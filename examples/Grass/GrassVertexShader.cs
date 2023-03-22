using SimulationFramework;
using SimulationFramework.Shaders;
using System.Numerics;

namespace Grass;

struct GrassVertexShader : IShader
{
    [Input(InputSemantic.VertexElement)]
    public GrassVertex input;

    [Output]
    float scaledHeight;

    [Output(OutputSemantic.Position)]
    public Vector4 position;

    [Uniform]
    public Matrix4x4 TransformMatrix;

    [Uniform]
    public float Time;

    [Uniform]
    public Vector3 CameraPosition;

    [Uniform]
    public float windHeading;

    public void Main()
    {
        Vector3 cameraDir = CameraPosition - input.Position;

        Vector3 offset = new(MathF.Cos(windHeading), 0, MathF.Sin(windHeading));
        offset += cameraDir;
        offset *= MathF.Cos(Time + input.RandomValue * (MathF.Tau / 6f));

        offset += CameraPosition;// Vector3.UnitY * cameraDir.Length();
            
        input.Position += offset * .01f * input.PivotWeight;

        scaledHeight = input.Position.Y / input.height;

        position = new(input.Position, 1);
        position = Vector4.Transform(position, TransformMatrix);
    }
}
