using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders.Compiler;
using SimulationFramework.Drawing.Shaders;
using System.Numerics;

Start<Program>();

partial class Program : Simulation
{
    Vertex[] vertices = [
        new Vertex(new(0, .5f, 0), ColorF.Red),
        new Vertex(new(.5f, -.5f, 0), ColorF.Lime),
        new Vertex(new(-.5f, -.5f, 0), ColorF.Blue),
    ];

    public override void OnInitialize()
    {
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.Black);

        CanvasShader canvasShader = new TriangleCanvasShader();
        VertexShader vertexShader = new TriangleVertexShader();

        canvas.Fill(canvasShader, vertexShader);
        canvas.DrawTriangles<Vertex>(vertices);
    }
}

struct Vertex
{
    public Vector3 Position;
    public ColorF Color;

    public Vertex(Vector3 position, ColorF color)
    {
        this.Position = position;
        this.Color = color;
    }
}

class TriangleCanvasShader : CanvasShader
{
    [VertexShaderOutput]
    ColorF color;

    public override ColorF GetPixelColor(Vector2 position)
    {
        return color;
    }
}

class TriangleVertexShader : VertexShader
{
    [VertexData]
    Vertex vertex;

    [VertexShaderOutput]
    ColorF color;

    [UseClipSpace]
    public override Vector4 GetVertexPosition()
    {
        Vector4 result = new(vertex.Position, 1);
        color = vertex.Color;
        return result;
    }
}