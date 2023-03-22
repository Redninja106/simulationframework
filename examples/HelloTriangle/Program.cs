using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Shaders;
using System.Numerics;

IRenderingContext renderer = null!;
IBuffer<(Vector3, ColorF)> verts = null!;
IShader vs = new VertexShader();
IShader fs = new FragmentShader();

Simulation simulation = Simulation.Create(Initialize, Render);
simulation.Run();

void Initialize(AppConfig config)
{
    renderer = Graphics.CreateRenderingContext();
    verts = Graphics.CreateBuffer(new (Vector3, ColorF)[]
    {
        (new(-0.5f, -0.5f, 1), ColorF.Red),
        (new(0.0f, 0.5f, 1), ColorF.Green),
        (new(0.5f, -0.5f, 1), ColorF.Blue)
    });
}

void Render(ICanvas canvas)
{
    renderer.CullMode = CullMode.None;

    renderer.SetVertexBuffer(verts);
    
    renderer.SetVertexShader(vs);
    renderer.SetFragmentShader(fs);

    renderer.DrawPrimitives(PrimitiveKind.Triangles, 3);
}
struct VertexShader : IShader
{
    [Input(InputSemantic.VertexElement)]
    (Vector3 position, ColorF color) vertex;

    [Output]
    ColorF color;

    [Output(OutputSemantic.Position)]
    Vector4 outPosition;

    public void Main()
    {
        outPosition = new(vertex.position.X, vertex.position.Y, vertex.position.Z, 1.0f);
        color = vertex.color;
    }
}

struct FragmentShader : IShader
{
    [Input(InputSemantic.Position, LinkageName = "outPosition")]
    Vector4 position;

    [Input]
    ColorF color;

    [Output(OutputSemantic.Color)]
    ColorF outColor;

    public void Main()
    {
        outColor = color;
    }
}