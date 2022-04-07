using SimulationFramework;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing;

using var sim = new BlankSimulation();
sim.RunWindowed("This is a Blank Simulation!", 1920, 1080);

internal class BlankSimulation : Simulation
{
    IShader vertexShader = null;
    IShader fragmentShader = null;
    IBuffer<Vector3> vertexBuffer;

    Vector3[] vertices = new Vector3[]
    {
        (0,    -.5f, 0),
        (-.5f, .5f,  0),
        (.5f,  .5f,  0),
    };

    public override void OnInitialize(AppConfig config)
    {
        vertexShader = Graphics.CreateShader(ShaderKind.Vertex, @"
float4 main(float3 pos: position) : SV_Position
{
    return float4(pos, 1);
}");
        fragmentShader = Graphics.CreateShader(ShaderKind.Fragment, @"
float4 main(float4 pos: SV_Position) : SV_Target
{
    return float4(pos.x / 1920, pos.y / 1080, 0, 1);
}");

        vertexBuffer = Graphics.CreateBuffer<Vector3>(vertices.Length);
        vertexBuffer.SetData(vertices);
    }

    public override void OnRender(ICanvas canvas)
    {
        var renderer = Graphics.GetRenderer();
        renderer.SetRenderTarget(Graphics.GetFrameTexture());
        renderer.Clear(Color.Black);
        renderer.SetVertexBuffer(vertexBuffer);
        renderer.SetShader(vertexShader);
        renderer.SetShader(fragmentShader);
        vertexShader.SetVariable("modelViewProj", Matrix4x4.Identity);
        renderer.DrawPrimitives(PrimitiveKind.Triangles, 1, 0);
    }

    public override void OnUnitialize()
    {
    }
}