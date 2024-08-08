using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing.Shaders.Compiler;
using SimulationFramework.Input;
using SimulationFramework.OpenGL;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

Start<Program>();

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

partial class Program : Simulation
{
    Vertex[] vertices = [
        new Vertex(new(-0.5f, -0.5f, -0.5f), new(0.0f, 0.0f)),
        new Vertex(new( 0.5f, -0.5f, -0.5f), new(1.0f, 0.0f)),
        new Vertex(new( 0.5f,  0.5f, -0.5f), new(1.0f, 1.0f)),
        new Vertex(new( 0.5f,  0.5f, -0.5f), new(1.0f, 1.0f)),
        new Vertex(new(-0.5f,  0.5f, -0.5f), new(0.0f, 1.0f)),
        new Vertex(new(-0.5f, -0.5f, -0.5f), new(0.0f, 0.0f)),
        new Vertex(new(-0.5f, -0.5f,  0.5f), new(0.0f, 0.0f)),
        new Vertex(new( 0.5f, -0.5f,  0.5f), new(1.0f, 0.0f)),
        new Vertex(new( 0.5f,  0.5f,  0.5f), new(1.0f, 1.0f)),
        new Vertex(new( 0.5f,  0.5f,  0.5f), new(1.0f, 1.0f)),
        new Vertex(new(-0.5f,  0.5f,  0.5f), new(0.0f, 1.0f)),
        new Vertex(new(-0.5f, -0.5f,  0.5f), new(0.0f, 0.0f)),
        new Vertex(new(-0.5f,  0.5f,  0.5f), new(1.0f, 0.0f)),
        new Vertex(new(-0.5f,  0.5f, -0.5f), new(1.0f, 1.0f)),
        new Vertex(new(-0.5f, -0.5f, -0.5f), new(0.0f, 1.0f)),
        new Vertex(new(-0.5f, -0.5f, -0.5f), new(0.0f, 1.0f)),
        new Vertex(new(-0.5f, -0.5f,  0.5f), new(0.0f, 0.0f)),
        new Vertex(new(-0.5f,  0.5f,  0.5f), new(1.0f, 0.0f)),
        new Vertex(new( 0.5f,  0.5f,  0.5f), new(1.0f, 0.0f)),
        new Vertex(new( 0.5f,  0.5f, -0.5f), new(1.0f, 1.0f)),
        new Vertex(new( 0.5f, -0.5f, -0.5f), new(0.0f, 1.0f)),
        new Vertex(new( 0.5f, -0.5f, -0.5f), new(0.0f, 1.0f)),
        new Vertex(new( 0.5f, -0.5f,  0.5f), new(0.0f, 0.0f)),
        new Vertex(new( 0.5f,  0.5f,  0.5f), new(1.0f, 0.0f)),
        new Vertex(new(-0.5f, -0.5f, -0.5f), new(0.0f, 1.0f)),
        new Vertex(new( 0.5f, -0.5f, -0.5f), new(1.0f, 1.0f)),
        new Vertex(new( 0.5f, -0.5f,  0.5f), new(1.0f, 0.0f)),
        new Vertex(new( 0.5f, -0.5f,  0.5f), new(1.0f, 0.0f)),
        new Vertex(new(-0.5f, -0.5f,  0.5f), new(0.0f, 0.0f)),
        new Vertex(new(-0.5f, -0.5f, -0.5f), new(0.0f, 1.0f)),
        new Vertex(new(-0.5f,  0.5f, -0.5f), new(0.0f, 1.0f)),
        new Vertex(new( 0.5f,  0.5f, -0.5f), new(1.0f, 1.0f)),
        new Vertex(new( 0.5f,  0.5f,  0.5f), new(1.0f, 0.0f)),
        new Vertex(new( 0.5f,  0.5f,  0.5f), new(1.0f, 0.0f)),
        new Vertex(new(-0.5f,  0.5f,  0.5f), new(0.0f, 0.0f)),
        new Vertex(new(-0.5f,  0.5f, -0.5f), new(0.0f, 1.0f))
    ];

    ITexture logo;

    public override void OnInitialize()
    {
        ShaderCompiler.DumpShaders = true;
        logo = Graphics.LoadTexture("logo-512x512.png");
        logo.WrapModeY = logo.WrapModeX = TileMode.Repeat;
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.Black);

        var canvasShader = new CubeCanvasShader()
        {
            tex = logo,
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

class CubeCanvasShader : CanvasShader
{
    [VertexShaderOutput]
    Vector2 uv;

    public ITexture tex;

    public override ColorF GetPixelColor(Vector2 position)
    {
        ColorF x = tex.SampleUV(uv * 10);
        if (x.A < 0.001f)
        {
            ShaderIntrinsics.Discard();
        }
        return x;
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

        uv = vertex.Texture;

        return result;
    }
}