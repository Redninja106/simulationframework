using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Shaders;
using System.Numerics;

IRenderer renderer = null!;
IBuffer<Vector3> verts = null!;
IShader vs = new VertexShader();
IShader fs = new FragmentShader();

Simulation simulation = Simulation.Create(Initialize, Render);
simulation.Run();

void Initialize(AppConfig config)
{
    renderer = Graphics.CreateRenderer();
    verts = Graphics.CreateBuffer<Vector3>(new Vector3[]{ new(-0.5f, -0.5f, 1), new(0.0f, 0.5f, 1), new(0.5f, -0.5f, 1), });
}

void Render(ICanvas canvas)
{
    renderer.CullMode = CullMode.None;

    renderer.ClearRenderTarget(Color.Black);
    renderer.SetViewport(new(0, 0, renderer.RenderTarget!.Width, renderer.RenderTarget!.Height));

    renderer.SetVertexBuffer(verts);

    renderer.SetVertexShader(vs);
    renderer.SetFragmentShader(fs);
    
    renderer.DrawPrimitives(PrimitiveKind.Triangles, 1);
}

struct VertexShader : IShader
{
    [Input]
    Vector3 position;

    [Output(OutputSemantic.Position)]
    Vector4 outPosition;

    public void Main()
    {
        outPosition = new(position.X, position.Y, position.Z, 1.0f);
    }
}

struct FragmentShader : IShader
{
    [Input(InputSemantic.Position, LinkageName = "outPosition")]
    Vector3 position;

    [Output(OutputSemantic.Color)]
    ColorF outColor;

    public void Main()
    {
        outColor = new(1, 0, 0, 1);
    }
}