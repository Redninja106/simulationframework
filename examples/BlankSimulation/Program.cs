using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Implementations.Canvas;
using SimulationFramework.Shaders;
using SimulationFramework.Shaders.Compiler;
using System.Numerics;

RendererCanvas canvas = null!;
IBuffer<(Vector3, Vector3, Vector2)> vb = null!;
IBuffer<uint> ib = null!;
IRenderingContext renderingContext = null!;
FreeCamera freeCamera = null!;
Simulation.Create(Initialize, Render).Run();
void Initialize(AppConfig config)
{
    freeCamera = new(75f, Graphics.DefaultRenderTarget.Width / (float)Graphics.DefaultRenderTarget.Height);
    renderingContext = Graphics.CreateRenderingContext();
    GenerateMesh();
}

void GenerateMesh()
{
    List<(Vector3 position, Vector3 normal, Vector2 uv)> vertices = new();
    List<uint> indices = new();

    const int width = 128, height = 128;
    float scale = .1f;

    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            float h = 0;
            h += MathF.Cos((x * scale) + (y * scale) + Time.TotalTime);
            h += MathF.Sin((x * scale) - (y * scale) + Time.TotalTime);
            Vector3 position = new(x * scale, h, y * scale);
            Vector2 uv = new(x / (float)width, y / (float)height);

            vertices.Add((position, Vector3.Zero, uv));
        }
    }

    for (uint y = 0; y < height - 1; y++)
    {
        for (uint x = 0; x < width - 1; x++)
        {
            indices.Add(y * width + x);
            indices.Add(y * width + (x+1));
            indices.Add((y+1) * width + x);

            indices.Add(y * width + (x+1));
            indices.Add((y+1) * width + (x+1));
            indices.Add((y+1) * width + x);
        }
    }

    for (int i = 0; i < indices.Count; i += 3)
    {
        var a = vertices[(int)indices[i + 0]];
        var b = vertices[(int)indices[i + 1]];
        var c = vertices[(int)indices[i + 2]];

        Vector3 edge1 = Vector3.Normalize(b.position - a.position);
        Vector3 edge2 = Vector3.Normalize(c.position - a.position);

        Vector3 normal = -Vector3.Cross(edge1, edge2);

        a.normal += normal;
        b.normal += normal;
        c.normal += normal;

        vertices[(int)indices[i + 0]] = a;
        vertices[(int)indices[i + 1]] = b;
        vertices[(int)indices[i + 2]] = c;
    }

    for (int i = 0; i < vertices.Count; i++)
    {
        var vertex = vertices[i];
        
        vertex.normal = vertex.normal.Normalized();
        
        vertices[i] = vertex;
    }

    if (vb is null)
    {
        vb = Graphics.CreateBuffer(vertices.ToArray());
    }
    else
    {
        vb.Update(vertices.ToArray());
    }

    if (ib is null)
    {
        ib = Graphics.CreateBuffer(indices.ToArray());
    }
    else
    {
        ib.Update(indices.ToArray());
    }


}

void Render(ICanvas c)
{
    GenerateMesh();

    renderingContext.RenderTarget = Graphics.DefaultRenderTarget;
    renderingContext.DepthTarget = Graphics.DefaultDepthTarget;

    renderingContext.ClearRenderTarget(Color.Black);
    renderingContext.ClearDepthTarget(1.0f);

    freeCamera.Update();

    freeCamera.AspectRatio = Graphics.DefaultRenderTarget.Width / (float)Graphics.DefaultRenderTarget.Height;

    VS vs = new() { CameraTransform = freeCamera.GetTransformMatrix() };
    FS fs = new() {  };

    renderingContext.SetVertexShader(vs);
    renderingContext.SetFragmentShader(fs);
    renderingContext.SetVertexBuffer(vb);
    renderingContext.SetIndexBuffer(ib);
    renderingContext.DrawIndexedPrimitives(PrimitiveKind.Triangles, ib.Length);
}

struct VS : IShader
{
    [Input(InputSemantic.VertexElement)]
    (Vector3 position, Vector3 normal, Vector2 uv) vertex;

    [Uniform]
    public Matrix4x4 CameraTransform;

    [Output]
    Vector2 uv;
    [Output]
    Vector3 normal;
    [Output(OutputSemantic.Position)]
    Vector4 position;

    public void Main()
    {
        position = Vector4.Transform(new Vector4(vertex.position, 1), CameraTransform);
        uv = vertex.uv;
        normal = vertex.normal;
    }
}

struct FS : IShader
{
    [Input]
    Vector2 uv;
    [Input]
    Vector3 normal;
    [Input(InputSemantic.Position)]
    Vector4 position;
    [Output(OutputSemantic.Color)]
    ColorF color;

    [ShaderMethod(Kind = ShaderKind.Fragment)]
    public void Main()
    {
        Vector3 low = new Vector3(170 / 255f, 124 / 255f, 65 / 255f);
        Vector3 high = new Vector3(246 / 255f, 198 / 255f, 127 / 255f);

        float brightness = Vector3.Dot(normal.Normalized(), Vector3.One.Normalized()) * .5f + .5f;

        float t = MathF.Sin((uv.X + uv.Y) * 100);
        t = t * t;
        t = t * t;
        t = t * t;
        t = t * t;
        color = new(ColorF.Lerp(new(high), new(low), t).ToVector3() * brightness);

        // color = new ColorF(new Vector3(brightness));

        // color = new(normal * .5f + Vector3.One * .5f);
    }
}

