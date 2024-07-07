using ImGuiNET;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Input;
using System.Numerics;

Start<Program>();

partial class Program : Simulation
{
    RayTracerShader shader = new();
    public float cameraRotX, cameraRotY;
    private float turnSpeed = 5;
    public override void OnInitialize()
    {
        // SetFixedResolution(1600, 900, new Color(25, 25, 25));
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Antialias(false);

        // fov slider
        ImGuiNET.ImGui.SliderFloat("vfov", ref shader.vfov, 30, 120);
        ImGuiNET.ImGui.SliderFloat("camera speed", ref turnSpeed, 1, 10);

        // set shader width and height
        shader.width = canvas.Width;
        shader.height = canvas.Height;

        // hold rmb to look around
        if (Mouse.IsButtonDown(MouseButton.Left))
        {
            cameraRotX -= Mouse.DeltaPosition.Y * turnSpeed * 0.001f * MathF.PI;
            cameraRotY += Mouse.DeltaPosition.X * turnSpeed * 0.001f * MathF.PI;
            Mouse.Visible = false;
        }
        else
        {
            Mouse.Visible = true;
        }

        if (Mouse.IsButtonDown(MouseButton.Right))
        {
            shader.GetPixelColor(Mouse.Position);
        }

        shader.cameraTransform = Matrix4x4.CreateRotationX(cameraRotX) * Matrix4x4.CreateRotationY(cameraRotY);
        
        Vector3 delta = Vector3.Zero;
        if (Keyboard.IsKeyDown(Key.W))
            delta.Z += Time.DeltaTime;
        if (Keyboard.IsKeyDown(Key.S))
            delta.Z -= Time.DeltaTime;
        if (Keyboard.IsKeyDown(Key.D))
            delta.X += Time.DeltaTime;
        if (Keyboard.IsKeyDown(Key.A))
            delta.X -= Time.DeltaTime;
        if (Keyboard.IsKeyDown(Key.Space))
            delta.Y -= Time.DeltaTime;
        if (Keyboard.IsKeyDown(Key.C))
            delta.Y += Time.DeltaTime;

        shader.cameraPosition += Vector3.Transform(delta, shader.cameraTransform);

        if (ImGui.Button("add"))
        {
            shader.spheres = [.. shader.spheres, new()];
        }

        for (int i = 0; i < shader.spheres.Length; i++)
        {
            ImGui.PushID(i);
            ImGui.Separator();
            ImGui.DragFloat4("sphere", ref shader.spheres[i].data);
            if (ImGui.Button("remove"))
            {
                shader.spheres = [..shader.spheres[..i], ..shader.spheres[(i+1)..]];
            }
            ImGui.PopID();
        }

        // draw a fullscreen rect, fill with the shader
        canvas.Fill(new MyShader());
        canvas.DrawRect(0, 0, canvas.Width, canvas.Height);
    }
}

class RayTracerShader : CanvasShader
{
    public float width, height;
    public Vector3 cameraPosition;
    public Matrix4x4 cameraTransform;
    public float vfov = 60;
    public Sphere[] spheres = [];
    int count = 1;

    public override ColorF GetPixelColor(Vector2 position)
    {
        Vector2 uv = new(position.X / width, position.Y / height);
        Vector2 vp = new(uv.X * 2f - 1f, uv.Y * 2f - 1f);
        vp.X *= width / height;

        Vector3 origin = cameraPosition;
        Vector4 direction4 = ShaderIntrinsics.Transform(new(vp * MathF.Tan(Angle.ToRadians(vfov / 2f)), 1, 1), cameraTransform);
        Vector3 direction = new(direction4.X, direction4.Y, direction4.Z);
        
        bool hit = false;
        Vector3 normal = default;
        for (int i = 0; i < count; i++)
        {
            if (RaySphereIntersect(origin, direction, new(new(0,0,0,1)), out normal) == 1)
            {
                hit = true;
            }
        }

        if (hit)
        {
            return new ColorF(new Vector3(.1f + MathF.Max(0, .9f * Vector3.Dot(normal, new Vector3(-1, -1, -1).Normalized()))));
        }
        else
        {
            return ColorF.Lerp(ColorF.White, ColorF.Blue, direction.Y);
        }
    }

    private float RaySphereIntersect(Vector3 origin, Vector3 direction, Sphere sphere, out Vector3 normal)
    {
        var offset = origin - sphere.position;
        var a = Vector3.Dot(direction, direction);
        var halfB = Vector3.Dot(offset, direction);
        var c = Vector3.Dot(offset, offset) - sphere.radius * sphere.radius;

        var discriminant = halfB * halfB - a * c;

        if (discriminant < 0)
        {
            normal = default;
            return 0;
        }

        var sqrtD = MathF.Sqrt(discriminant);

        var t = (-halfB - sqrtD) / a;

        if (t < 0)
        {
            t = (-halfB + sqrtD) / a;
        }

        if (t < 0)
        {
            normal = default;
            return 0;
        }

        normal = (origin + direction * t) - sphere.position;
        normal = normal.Normalized();

        return 1;
    }
}

class MyShader : CanvasShader
{
    public override ColorF GetPixelColor(Vector2 position)
    {
        return new(position.X / 2000, position.Y / 1000, 0, 1);
    }
}

struct Sphere
{
    public Vector4 data;

    public Sphere(Vector4 data)
    {
        this.data = data;
    }

    public Vector3 position => new(data.X, data.Y, data.Z);
    public float radius => data.W;
}

struct Ray
{
    public Vector3 origin;
    public Vector3 direction;
    public float tMax;
}