using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Shaders;
using System.Numerics;

IRenderer renderer = null!;
IBuffer<Vector3> verts = null!;
IShader vs = new VertexShader();
IShader gs = new GeometryShader();
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
    renderer.SetGeometryShader(gs);
    renderer.SetFragmentShader(fs);
    
    renderer.DrawPrimitives(PrimitiveKind.Triangles, 1);
}

struct VertexShader : IShader
{
    [ShaderInput]
    Vector3 position;

    [ShaderOutput(OutputSemantic.Position)]
    Vector4 outPosition;

    public void Main()
    {
        outPosition = new(position.X, position.Y, position.Z, 1.0f);
    }
}

struct GeometryShader : IShader
{
    [ShaderInput(InputSemantic.Position)]
    TrianglePrimitive<Vector3> input;

    [ShaderOutput, MaxVertices(4)]
    TriangleStream<Vector3> output;

    public void Main()
    {
        output.EmitVertex(input.VertexA);
        output.EmitVertex(input.VertexB);
        output.EmitVertex(input.VertexC);
        output.EmitVertex(input.VertexC + input.VertexB - input.VertexA);
        output.EndPrimitive();
    }
}

struct FragmentShader : IShader
{
    [ShaderInput(InputSemantic.Position, LinkageName = "outPosition")]
    Vector3 position;

    [ShaderOutput(OutputSemantic.Color)]
    ColorF outColor;

    public void Main()
    {
        outColor = new(1, 0, 0, 1);
    }
}