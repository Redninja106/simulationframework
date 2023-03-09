using Grass;
using ImGuiNET;
using SimulationFramework;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing;
using SimulationFramework.ImGuiNET;
using SimulationFramework.Shaders;
using System.Numerics;

GrassRenderer grassRenderer = null!;
FreeCamera freeCam = null!;
ColorF floorColor = default;
IBuffer<Vector3> floor = null!;
IRenderingContext floorRenderer = null!;

Simulation.Create(Init, Render).Run(new DesktopPlatform().WithExtension(new ImGuiComponent()));

void Init(AppConfig config)
{
    freeCam = new(90f, Graphics.DefaultRenderTarget.Width / (float)Graphics.DefaultRenderTarget.Height)
    {
        Position = Vector3.One,
        Pitch = MathHelper.DegreesToRadians(45),
        Yaw = MathHelper.DegreesToRadians(225)
    };

    floorRenderer = Graphics.CreateRenderer();
    floor = Graphics.CreateBuffer(new Vector3[] 
    {  
        new(-1, 0, +1),
        new(+1, 0, +1),
        new(-1, 0, -1),
        new(+1, 0, -1),
    });

    grassRenderer = new(new(100000, 0.1f, 0.15f));
    grassRenderer.GrassTopColor = new(ColorF.LawnGreen.ToVector3() * .75f);
    floorColor = grassRenderer.GrassBottomColor = new(ColorF.LawnGreen.ToVector3() * .5f);
    
}

void Render(ICanvas canvas)
{
    Layout();
    Update();

    var cameraMatrix = freeCam.ViewMatrix * freeCam.ProjectionMatrix;

    floorRenderer.RenderTarget = Graphics.DefaultRenderTarget;
    floorRenderer.DepthTarget = Graphics.DefaultDepthTarget;

    floorRenderer.ClearRenderTarget(Color.DeepSkyBlue);
    floorRenderer.ClearDepthTarget(1.0f);
    floorRenderer.CullMode = CullMode.None;

    FloorFragmentShader floorFS = new(floorColor);
    FloorVertexShader floorVS = new(cameraMatrix);

    floorRenderer.SetFragmentShader(floorFS);
    floorRenderer.SetVertexShader(floorVS);

    floorRenderer.SetVertexBuffer(floor);

    floorRenderer.DrawPrimitives(PrimitiveKind.TriangleStrip, floor.Length);

    grassRenderer.TransformMatrix = cameraMatrix;
    grassRenderer.Render();
}

void Update()
{
    freeCam.AspectRatio = Graphics.DefaultRenderTarget.Width / (float)Graphics.DefaultRenderTarget.Height;
    freeCam.Update();
}

void Layout()
{
    ImGuiHelper.ColorEdit3("Floor Color", ref floorColor);
    ImGuiHelper.ColorEdit3("Grass Top Color", ref grassRenderer.GrassTopColor);
    ImGuiHelper.ColorEdit3("Grass Bottom Color", ref grassRenderer.GrassBottomColor);

    freeCam.Layout();
}

struct FloorVertexShader : IShader
{
    [Input(InputSemantic.Vertex)]
    Vector3 vertexPosition;

    [Output(OutputSemantic.Position)]
    Vector4 position;

    [Uniform]
    public Matrix4x4 TransformMatrix;

    public FloorVertexShader(Matrix4x4 transformMatrix)
    {
        this.TransformMatrix = transformMatrix;
    }

    public void Main()
    {
        position = new(vertexPosition, 1);
        position = Vector4.Transform(position, TransformMatrix);
    }
}

struct FloorFragmentShader : IShader
{
    [Output(OutputSemantic.Color)]
    ColorF outputColor;

    [Uniform]
    public ColorF Color;

    public FloorFragmentShader(ColorF color) : this()
    {
        this.Color = color;
    }

    public void Main()
    {
        outputColor = Color;
    }
}