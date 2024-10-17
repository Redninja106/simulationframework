using ImGuiNET;
using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing.Shaders.Compiler;
using SimulationFramework.Input;
using SimulationFramework.OpenGL;
using System;
using System.Numerics;
using System.Runtime.Intrinsics;

Start<Program>();

partial class Program : Simulation
{
    RayTracerShader shader = new();
    public float cameraRotX, cameraRotY;
    private float cameraSpeed = 5;
    public override void OnInitialize()
    {
        SetFixedResolution(640, 480, new Color(25, 25, 25));

        // for debugging
        ShaderCompiler.DumpShaders = true;

        // setup scene
        shader.spheres = new Sphere[20];

        // the first sphere is the big floor one
        shader.spheres[0] = new(new(0, 1000, 0, 1000), ColorF.Gray);

        // the rest are random
        for (int i = 1; i < shader.spheres.Length; i++)
        {
            shader.spheres[i] = new(
                new Vector4(
                    Random.Shared.NextSingle(-20, 20),
                    Random.Shared.NextSingle(-4, 0),
                    Random.Shared.NextSingle(-20, 20),
                    Random.Shared.NextSingle(.1f, 7)
                ),
                ColorF.FromHSV(Random.Shared.NextSingle(), 1, 1)
                );
            shader.spheres[i].position.Y -= shader.spheres[i].radius;
        }
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Antialias(true);

        // fov slider
        ImGui.SliderFloat("vfov", ref shader.vfov, 30, 120);
        ImGui.SliderFloat("camera speed", ref cameraSpeed, 1, 10);
        ImGui.Text(Performance.Framerate.ToString());

        // set shader width, height, and time
        shader.width = canvas.Width;
        shader.height = canvas.Height;
        shader.time = Time.TotalTime;

        // hold lmb to look around
        if (Mouse.IsButtonDown(MouseButton.Left))
        {
            cameraRotX -= Mouse.DeltaPosition.Y * 0.0005f * MathF.PI;
            cameraRotY += Mouse.DeltaPosition.X * 0.0005f * MathF.PI;
            Mouse.Visible = false;
        }
        else
        {
            Mouse.Visible = true;
        }

        // easy CPU debugging!
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

        shader.cameraPosition += Vector3.Transform(cameraSpeed * delta, shader.cameraTransform);

        if (ImGui.Button("add"))
        {
            shader.spheres = [.. shader.spheres, new()];
        }

        for (int i = 0; i < shader.spheres.Length; i++)
        {
            ImGui.PushID(i);
            ImGui.Separator();
            ImGui.DragFloat3("position", ref shader.spheres[i].position);
            ImGui.DragFloat("radius", ref shader.spheres[i].radius);
            ImGui.ColorEdit4("radius", ref shader.spheres[i].color);
            if (ImGui.Button("remove"))
            {
                shader.spheres = [..shader.spheres[..i], ..shader.spheres[(i+1)..]];
            }
            ImGui.PopID();
        }

        // draw a fullscreen rect, fill with the shader
        canvas.Clear(Color.Black);
        canvas.Fill(shader);
        canvas.DrawRect(0, 0, canvas.Width, canvas.Height);
        //canvas.DrawCircle(Mouse.Position, 100);
    }
}

class RayTracerShader : CanvasShader
{
    public float width, height;
    public Vector3 cameraPosition;
    public Matrix4x4 cameraTransform;
    public float vfov = 60;
    public Sphere[] spheres = [];
    public float time;

    public override ColorF GetPixelColor(Vector2 position)
    {
        Vector2 uv = new(position.X / width, position.Y / height);
        Vector2 vp = new(uv.X * 2f - 1f, uv.Y * 2f - 1f);
        vp.X *= width / height;

        Vector3 origin = cameraPosition;
        
        const int antialias = 15; // sample # = antialias^2
        
        ColorF color = default;
        for (int y = 0; y < antialias; y++)
        {
            for (int x = 0; x < antialias; x++)
            {
                //Vector2 sampleOffset = new((x + .5f) / antialias, (y + .5f) / antialias); // evenly spaced sample distribution
                Vector3 sampleOffset = Random3(Random3(new((vp.X + x), (vp.Y * y), time))); // random sample distribution
                
                Vector2 samplePos = new Vector2(
                    vp.X * width * .5f + sampleOffset.X,
                    vp.Y * height * .5f + sampleOffset.Y
                    );
                samplePos /= new Vector2(width * .5f, height * .5f);
                Vector4 direction4 = ShaderIntrinsics.Transform(new Vector4(samplePos * MathF.Tan(Angle.ToRadians(vfov / 2f)), 1, 1), cameraTransform);
                color += RayColor(origin, new(direction4.X, direction4.Y, direction4.Z));
            }
        }

        color /= antialias * antialias;

        return color with { A = 1.0f };
    }

    private ColorF RayColor(Vector3 origin, Vector3 direction)
    {
        const int MaxBounces = 4;

        Vector4 color = Vector4.One;
        for (int i = 0; i < MaxBounces; i++)
        {
            bool hit = false;
            Vector3 normal = default;
            float closestT = float.PositiveInfinity;
            Vector4 closestCol = default;
            for (int j = 0; j < spheres.Length; j++)
            {
                Sphere s = spheres[j];
                s.position -= cameraPosition;
                if (RaySphereIntersect(origin, direction, s, out Vector3 n, out float t))
                {
                    if (t < closestT)
                    {
                        hit = true;
                        normal = n;
                        closestT = t;
                        closestCol = s.color;
                    }
                }
            }

            if (hit)
            {
                color *= closestCol;
                origin += direction * closestT + normal * 0.001f;

                direction = ShaderIntrinsics.Normalize(normal * 1.00001f + RandomUnit3(normal + direction + new Vector3(time)));

                // for reflective balls uncomment:
                direction = ShaderIntrinsics.Reflect(direction, normal);
            }
            else
            {
                color *= ColorF.Lerp(ColorF.WhiteSmoke, ColorF.SkyBlue, direction.Y).ToVector4();
                return new(color);
            }
        }
        return new(color);
    }

    private bool RaySphereIntersect(Vector3 origin, Vector3 direction, Sphere sphere, out Vector3 normal, out float t)
    {
        var offset = origin - sphere.position;
        var a = Vector3.Dot(direction, direction);
        var halfB = Vector3.Dot(offset, direction);
        var c = Vector3.Dot(offset, offset) - sphere.radius * sphere.radius;

        var discriminant = halfB * halfB - a * c;

        if (discriminant < 0)
        {
            normal = default;
            t = float.PositiveInfinity;
            return false;
        }

        var sqrtD = MathF.Sqrt(discriminant);

        t = (-halfB - sqrtD) / a;

        if (t < 0)
        {
            t = (-halfB + sqrtD) / a;

            if (t < 0)
            {
                normal = default;
                return false;
            }
        }

        normal = (origin + direction * t) - sphere.position;
        normal = normal.Normalized();

        return true;
    }

    private Vector3 RandomUnit3(Vector3 p)
    {
        Vector3 rand = p * 2f - Vector3.One;
        do
        {
            rand = Random3(rand) * 2f - Vector3.One;
        }
        while (rand.LengthSquared() >= 1);

        return rand.Normalized();
    }

    private Vector3 Random3(Vector3 p)
    {
        p = ShaderIntrinsics.Fract(p * new Vector3(443.897f, 441.423f, .0973f));
        p += new Vector3(ShaderIntrinsics.Dot(p, new Vector3(p.Y, p.X, p.Z) + new Vector3(19.19f)));
        return ShaderIntrinsics.Fract((new Vector3(p.X,p.X,p.Y) + new Vector3(p.Y,p.Z,p.Z)) * new Vector3(p.Z, p.Y, p.X));
    }
}

struct Sphere
{
    public Vector3 position;
    public float radius;
    public Vector4 color;

    public Sphere(Vector4 data, ColorF color)
    {
        position = new(data.X, data.Y, data.Z);
        radius = data.W;
        this.color = color.ToVector4();
    }
}