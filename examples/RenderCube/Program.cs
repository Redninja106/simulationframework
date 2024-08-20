using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;
using System.Numerics;

Start<Program>();

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
    IDepthMask depthMask;

    public override void OnInitialize()
    {
        SetFixedResolution(400, 400, Color.Black);

        logo = Graphics.LoadTexture("logo-512x512.png");
        logo.WrapModeY = logo.WrapModeX = WrapMode.Repeat;

        depthMask = Graphics.CreateDepthMask(Window.Width, Window.Height);
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.Black);
        depthMask.Clear(1f);

        var canvasShader = new CubeCanvasShader()
        {
            texture = logo,
        };

        var vertexShader = new CubeVertexShader()
        {
            world = Matrix4x4.CreateRotationY(Time.TotalTime * Angle.ToRadians(60)),
            view = Matrix4x4.CreateLookAt(Vector3.One, Vector3.Zero, Vector3.UnitY),
            proj = Matrix4x4.CreatePerspectiveFieldOfView(MathF.PI / 3f, canvas.Width / (float)canvas.Height, 0.1f, 100f)
        };

        canvas.Fill(canvasShader, vertexShader);
        canvas.Mask(depthMask);
        canvas.WriteMask(depthMask);
        canvas.DrawTriangles<Vertex>(vertices);
    }
}

struct Vertex(Vector3 position, Vector2 texture)
{
    public Vector3 Position = position;
    public Vector2 Texture = texture;
}

class CubeCanvasShader : CanvasShader
{
    [VertexShaderOutput]
    Vector2 uv;

    public ITexture texture;

    public override ColorF GetPixelColor(Vector2 position)
    {
        ColorF x = texture.SampleUV(uv * 10);
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

        uv = new(vertex.Texture.X, vertex.Texture.Y);

        return result;
    }
}