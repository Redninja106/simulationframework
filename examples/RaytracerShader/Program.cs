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
        SetFixedResolution(1600, 900, new Color(25, 25, 25));
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

        shader.cameraRotationMatrix = Matrix4x4.CreateRotationX(cameraRotX) * Matrix4x4.CreateRotationY(cameraRotY);

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
        shader.cameraPosition += Vector3.Transform(delta, shader.cameraRotationMatrix);

        shader.cameraRotationMatrix = Matrix4x4.Transpose(shader.cameraRotationMatrix);
        // draw a fullscreen rect, fill with the shader
        canvas.Fill(shader);
        canvas.DrawRect(0, 0, canvas.Width, canvas.Height);
    }
}

class RayTracerShader : CanvasShader
{
    public float width, height;
    public Vector3 cameraPosition;
    public Matrix4x4 cameraRotationMatrix;
    public float vfov = 60;

    public override ColorF GetPixelColor(Vector2 position)
    {
        Vector2 uv = new(position.X / width, position.Y / height);
        Vector2 vp = new(uv.X * 2f - 1f, uv.Y * 2f - 1f);
        vp.X *= width / height;

        Vector3 origin = cameraPosition;
        Vector4 direction4 = ShaderIntrinsics.Multiply(new(vp * MathF.Tan(Angle.ToRadians(vfov / 2f)), 1, 1), cameraRotationMatrix);
        Vector3 direction = new(direction4.X, direction4.Y, direction4.Z);

        Vector3 normal = Vector3.Zero;
        if (RaySphereIntersect(origin, direction, new Vector3(0, 0, 1), .5f, out normal) == 1)
        {
            return new ColorF(new Vector3(.1f + MathF.Max(0, .9f * Vector3.Dot(normal, new Vector3(-1, -1, -1).Normalized()))));
        }
        else
        {
            return ColorF.Lerp(ColorF.White, ColorF.Blue, direction.Y);
        }
    }

    private float RaySphereIntersect(Vector3 origin, Vector3 direction, Vector3 position, float radius, out Vector3 normal)
    {
        var offset = origin - position;
        var a = Vector3.Dot(direction, direction);
        var halfB = Vector3.Dot(offset, direction);
        var c = Vector3.Dot(offset, offset) - radius * radius;

        var discriminant = halfB * halfB - a * c;

        if (discriminant < 0)
        {
            normal = default;
            return 0;
        }

        var sqrtD = MathF.Sqrt(discriminant);

        var t = (-halfB - sqrtD) / a;

        if (discriminant is not 0)
        {
            t = (-halfB - sqrtD) / a;
        }
        normal = (origin + direction * t) - position;
        normal = normal.Normalized();

        return 1;
    }
}