using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders.Compiler;
using SimulationFramework.Drawing.Shaders;
using System.Numerics;

Start<Program>();

partial class Program : Simulation
{
    Vertex[] vertices = [
        new Vertex(new(-0.5f, -0.5f, -0.5f), new(0.0f, 0.0f)),
        new Vertex(new(0.5f, -0.5f, -0.5f), new(1.0f, 0.0f)),
        new Vertex(new(0.5f, 0.5f, -0.5f), new(1.0f, 1.0f)),
        new Vertex(new(0.5f, 0.5f, -0.5f), new(1.0f, 1.0f)),
        new Vertex(new(-0.5f, 0.5f, -0.5f), new(0.0f, 1.0f)),
        new Vertex(new(-0.5f, -0.5f, -0.5f), new(0.0f, 0.0f)),
        new Vertex(new(-0.5f, -0.5f, 0.5f), new(0.0f, 0.0f)),
        new Vertex(new(0.5f, -0.5f, 0.5f), new(1.0f, 0.0f)),
        new Vertex(new(0.5f, 0.5f, 0.5f), new(1.0f, 1.0f)),
        new Vertex(new(0.5f, 0.5f, 0.5f), new(1.0f, 1.0f)),
        new Vertex(new(-0.5f, 0.5f, 0.5f), new(0.0f, 1.0f)),
        new Vertex(new(-0.5f, -0.5f, 0.5f), new(0.0f, 0.0f)),
        new Vertex(new(-0.5f, 0.5f, 0.5f), new(1.0f, 0.0f)),
        new Vertex(new(-0.5f, 0.5f, -0.5f), new(1.0f, 1.0f)),
        new Vertex(new(-0.5f, -0.5f, -0.5f), new(0.0f, 1.0f)),
        new Vertex(new(-0.5f, -0.5f, -0.5f), new(0.0f, 1.0f)),
        new Vertex(new(-0.5f, -0.5f, 0.5f), new(0.0f, 0.0f)),
        new Vertex(new(-0.5f, 0.5f, 0.5f), new(1.0f, 0.0f)),
        new Vertex(new(0.5f, 0.5f, 0.5f), new(1.0f, 0.0f)),
        new Vertex(new(0.5f, 0.5f, -0.5f), new(1.0f, 1.0f)),
        new Vertex(new(0.5f, -0.5f, -0.5f), new(0.0f, 1.0f)),
        new Vertex(new(0.5f, -0.5f, -0.5f), new(0.0f, 1.0f)),
        new Vertex(new(0.5f, -0.5f, 0.5f), new(0.0f, 0.0f)),
        new Vertex(new(0.5f, 0.5f, 0.5f), new(1.0f, 0.0f)),
        new Vertex(new(-0.5f, -0.5f, -0.5f), new(0.0f, 1.0f)),
        new Vertex(new(0.5f, -0.5f, -0.5f), new(1.0f, 1.0f)),
        new Vertex(new(0.5f, -0.5f, 0.5f), new(1.0f, 0.0f)),
        new Vertex(new(0.5f, -0.5f, 0.5f), new(1.0f, 0.0f)),
        new Vertex(new(-0.5f, -0.5f, 0.5f), new(0.0f, 0.0f)),
        new Vertex(new(-0.5f, -0.5f, -0.5f), new(0.0f, 1.0f)),
        new Vertex(new(-0.5f, 0.5f, -0.5f), new(0.0f, 1.0f)),
        new Vertex(new(0.5f, 0.5f, -0.5f), new(1.0f, 1.0f)),
        new Vertex(new(0.5f, 0.5f, 0.5f), new(1.0f, 0.0f)),
        new Vertex(new(0.5f, 0.5f, 0.5f), new(1.0f, 0.0f)),
        new Vertex(new(-0.5f, 0.5f, 0.5f), new(0.0f, 0.0f)),
        new Vertex(new(-0.5f, 0.5f, -0.5f), new(0.0f, 1.0f))
    ];

    public override void OnInitialize()
    {
        ShaderCompiler.DumpShaders = true;
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.Black);

        var canvasShader = new TheCanvasShader()
        {
        };

        var vertexShader = new CubeVertexShader()
        {
            world = Matrix4x4.CreateRotationY(Time.TotalTime * Angle.ToRadians(60)),
            view = Matrix4x4.CreateLookAtLeftHanded(Vector3.One, Vector3.Zero, Vector3.UnitY),
            proj = Matrix4x4.CreatePerspectiveFieldOfViewLeftHanded(MathF.PI / 3f, canvas.Width / (float)canvas.Height, 0.1f, 100f)
        };

        canvas.Fill(canvasShader, vertexShader);
        canvas.DrawTriangles<Vertex>(vertices);
    }
}

struct Vertex
{
    public Vector3 Position;
    public Vector2 Texture;

    public Vertex(Vector3 position, Vector2 texture)
    {
        this.Position = position;
        this.Texture = texture;
    }
}

class TheCanvasShader : CanvasShader
{
    [VertexShaderOutput]
    Vector2 uv;

    public override ColorF GetPixelColor(Vector2 position)
    {
        return new(uv.X, uv.Y, 0, 1);
    }
}

class CubeVertexShader : VertexShader
{
    [VertexData]
    Vertex vertex;

    public Matrix4x4 world;
    public Matrix4x4 view;
    public Matrix4x4 proj;

    [VertexShaderOutput]
    Vector2 uv;

    [UseClipSpace]
    public override Vector4 GetVertexPosition()
    {
        Vector4 result = new(vertex.Position, 1);
        result = Vector4.Transform(result, world);
        result = Vector4.Transform(result, view);
        result = Vector4.Transform(result, proj);

        uv = new(vertex.Texture.X, vertex.Texture.Y);

        return result;
    }
}