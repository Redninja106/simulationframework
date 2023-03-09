// See https://aka.ms/new-console-template for more information
using ImGuiNET;
using SimulationFramework;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Direct3D11;
using SimulationFramework.ImGuiNET;
using SimulationFramework.Shaders;
using System.Numerics;

IRenderingContext renderer = null!;
IBuffer<Vector2> quadBuffer = null!;
Vector3 position = Vector3.Zero;
float camRotationX = 0, camRotationY = 0;

float lightRotationX = 0, lightRotationY = 0;
Sphere sphere = new() { Radius = .5f };

Vector2[] quad = new Vector2[]
{
    new(-1, +1),
    new(+1, +1),
    new(-1, -1),
    new(+1, -1),
};

var s = Simulation.Create(Initialize, Render);
s.Run(new DesktopPlatform().WithExtension(new ImGuiComponent()));

void Initialize(AppConfig config)
{
    renderer = Graphics.CreateRenderer();
    quadBuffer = Graphics.CreateBuffer(quad.AsSpan());
}

void Render(ICanvas canvas)
{
    LayoutImGui();

    renderer.RenderTarget = Graphics.DefaultRenderTarget;
    renderer.ClearRenderTarget(Color.FromHSV(0, 0, .1f));
    HandleInput();

    VertexShader vs = new();
    FragmentShader fs = new()
    {
        width = Graphics.DefaultRenderTarget.Width,
        height = Graphics.DefaultRenderTarget.Height,
        cameraMatrix = CreateRotationMatrix(camRotationX, camRotationY) * Matrix4x4.CreateTranslation(position),
        lightDirection = Vector3.Transform(-Vector3.UnitZ, CreateRotationMatrix(lightRotationX, lightRotationY)),
        sphere = sphere,
    };

    renderer.SetVertexBuffer(quadBuffer);
    renderer.SetVertexShader(vs);
    renderer.SetFragmentShader(fs);
    renderer.DrawPrimitives(PrimitiveKind.TriangleStrip, 4);
}

void HandleInput()
{
    if (Mouse.IsButtonDown(MouseButton.Right))
    {
        camRotationX -= Mouse.DeltaPosition.Y * MathF.PI * 0.001f;
        camRotationY -= Mouse.DeltaPosition.X * MathF.PI * 0.001f;
    }

    Vector3 delta = Vector3.Zero;

    if (Keyboard.IsKeyDown(Key.W)) delta += Vector3.UnitZ;
    if (Keyboard.IsKeyDown(Key.A)) delta -= Vector3.UnitX;
    if (Keyboard.IsKeyDown(Key.S)) delta -= Vector3.UnitZ;
    if (Keyboard.IsKeyDown(Key.D)) delta += Vector3.UnitX;
    if (Keyboard.IsKeyDown(Key.C)) delta -= Vector3.UnitY;
    if (Keyboard.IsKeyDown(Key.Space)) delta += Vector3.UnitY;

    position += Vector3.Transform(delta * Time.DeltaTime, CreateRotationMatrix(camRotationX, camRotationY));
}

Matrix4x4 CreateRotationMatrix(float rx, float ry)
{
    return Matrix4x4.CreateRotationX(rx) * Matrix4x4.CreateRotationY(ry);
}

void LayoutImGui()
{
    ImGui.DragFloat3("position", ref position, 0.01f);
    ImGui.SliderAngle("camera yaw", ref camRotationX, -90, 90);
    ImGui.SliderAngle("camera pitch", ref camRotationY);
    ImGui.Separator();
    ImGui.SliderAngle("light yaw", ref lightRotationX, -90, 90);
    ImGui.SliderAngle("light pitch", ref lightRotationY);
    ImGui.Separator();
    ImGui.DragFloat3("sphere position", ref sphere.Position, 0.01f);
    ImGui.DragFloat("sphere radius", ref sphere.Radius, 0.01f);
}

struct VertexShader : IShader
{
    [Input(InputSemantic.Vertex)]
    Vector2 vertexPosition;

    [Output(OutputSemantic.Position)]
    Vector4 position;

    public void Main()
    {
        position = new(vertexPosition, 0, 1);
    }
}

struct FragmentShader : IShader
{
    [Input(InputSemantic.Position)]
    Vector4 screenPosition;

    [Output(OutputSemantic.Color)]
    ColorF outputColor;

    [Uniform]
    public int width, height;
    [Uniform]
    public Matrix4x4 cameraMatrix;
    [Uniform]
    public Vector3 lightDirection;
    [Uniform]
    public Sphere sphere;

    public void Main()
    {
        Vector2 uv = new(screenPosition.X / width, screenPosition.Y / height);

        Vector2 vp = uv - new Vector2(.5f);
        vp.X *= width / (float)height;
        vp.Y *= -1;

        var ray = GetRay(vp);
        ray.Position = Vector3.Transform(ray.Position, cameraMatrix);
        ray.Direction = Vector3.TransformNormal(ray.Direction, cameraMatrix);

        Sphere s = sphere;
        if (!s.Hit(ray, out var t)) 
        {
            outputColor = new(0, 0, 0, 1);
            return;
        }

        var hitPosition = ray.At(t);
        var normal = Vector3.Normalize(hitPosition - s.Position);

        var diffuse = MathF.Max(0, Vector3.Dot(normal, lightDirection));
        var ambient = .1f;

        outputColor = new(new Vector3(diffuse + ambient));
        outputColor.R = MathF.Pow(outputColor.R, 1f / 2.2f);
        outputColor.G = MathF.Pow(outputColor.G, 1f / 2.2f);
        outputColor.B = MathF.Pow(outputColor.B, 1f / 2.2f);
    }

    Ray GetRay(Vector2 vp)
    {
        return new()
        {
            Position = Vector3.Zero,
            Direction = new(vp, 1),
            minT = 0,
            maxT = float.PositiveInfinity
        };
    }
}

struct Ray
{
    public Vector3 Position;
    public Vector3 Direction;
    public float minT;
    public float maxT;

    public Vector3 At(float t)
    {
        return Position + Direction * t;
    }
}

struct Sphere
{
    public Vector3 Position;
    public float Radius;

    public Sphere(Vector3 position, float radius)
    {
        this.Position = position;
        this.Radius = radius;
    }

    public bool Hit(Ray ray, out float t)
    {
        t = 0;
        var oc = ray.Position - this.Position;
        var a = ray.Direction.LengthSquared();
        var halfB = Vector3.Dot(oc, ray.Direction);
        var c = oc.LengthSquared() - Radius * Radius;
        var discriminant = (halfB * halfB) - (a * c);

        if (discriminant < 0)
            return false;

        var sqrtd = MathF.Sqrt(discriminant);

        t = (-halfB - sqrtd) / a;
        
        return (t > ray.minT && t < ray.maxT);
    }
}