using SimulationFramework;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing;
using System.Runtime.InteropServices;

using var sim = new BlankSimulation();
sim.RunWindowed("This is a Blank Simulation!", 1920, 1080);

internal class BlankSimulation : Simulation
{
    IShader vertexShader = null;
    IShader fragmentShader = null;
    IBuffer<Vector3> vertexBuffer;

    public override void OnInitialize(AppConfig config)
    {
        //vertexShader = Graphics.CreateShader(ShaderKind.Vertex, @"
        //cbuffer cb : register(b0)
        //{
        //    float3 offset;
        //}

        //float4 main(float3 pos: position) : SV_Position
        //{
        //    return float4(pos + offset, 1);
        //}");
        vertexShader = Graphics.CreateShader(ShaderKind.Vertex, @"
        cbuffer cb : register(b0)
        {
            float4x4 world;
            float4x4 view;
            float4x4 proj;
        }

        float4 main(float3 pos: position) : SV_Position
        {
            float4 position = float4(pos, 1);

            position = mul(world, position);
            position = mul(view,  position);
            position = mul(proj,  position);

            return position;
        }");

        fragmentShader = Graphics.CreateShader(ShaderKind.Fragment, @"
Texture2D    texture_texture : register(t0);
SamplerState texture_sampler : register(s0);

cbuffer cb : register(b0)
{
    float3 color;
}

float4 main(float4 pos: SV_Position) : SV_Target
{
    return float4(color, 1);
}");

        var vertices = new List<Vector3>();
        for (int i = 0; i < 16; i++)
        {
            float r1 = (i / 16f) * MathF.Tau;
            float r2 = ((i+1) / 16f) * MathF.Tau;
            vertices.Add(new Vector3(MathF.Cos(r1), MathF.Sin(r1), 0));
            vertices.Add(new Vector3(MathF.Cos(r2), MathF.Sin(r2), 0));
            vertices.Add(new Vector3(0, 0, 0));
        }
        vertexBuffer = Graphics.CreateBuffer<Vector3>(vertices.Count);
        vertexBuffer.SetData(CollectionsMarshal.AsSpan(vertices));
    }

    Vector3 cameraPosition;
    Vector2 cameraRotation;

    Vector3 RotateToCamera(Vector3 vector)
    {
        return System.Numerics.Vector3.Transform(vector, System.Numerics.Matrix4x4.CreateRotationX(cameraRotation.X) * System.Numerics.Matrix4x4.CreateRotationY(cameraRotation.Y) );
    }

    public override void OnRender(ICanvas canvas)
    {
        if (Keyboard.IsKeyDown(Key.LeftArrow)) cameraRotation.Y += MathF.PI * Time.DeltaTime;
        if (Keyboard.IsKeyDown(Key.RightArrow)) cameraRotation.Y -= MathF.PI * Time.DeltaTime;
        if (Keyboard.IsKeyDown(Key.UpArrow)) cameraRotation.X -= MathF.PI * Time.DeltaTime;
        if (Keyboard.IsKeyDown(Key.DownArrow)) cameraRotation.X += MathF.PI * Time.DeltaTime;
        
        if (Keyboard.IsKeyDown(Key.W))      cameraPosition += Time.DeltaTime * 5 * RotateToCamera(Vector3.UnitZ);
        if (Keyboard.IsKeyDown(Key.A))      cameraPosition += Time.DeltaTime * 5 * RotateToCamera(Vector3.UnitX);
        if (Keyboard.IsKeyDown(Key.S))      cameraPosition -= Time.DeltaTime * 5 * RotateToCamera(Vector3.UnitZ);
        if (Keyboard.IsKeyDown(Key.D))      cameraPosition -= Time.DeltaTime * 5 * RotateToCamera(Vector3.UnitX);
        if (Keyboard.IsKeyDown(Key.C))      cameraPosition -= Time.DeltaTime * 5 * RotateToCamera(Vector3.UnitY);
        if (Keyboard.IsKeyDown(Key.Space))  cameraPosition += Time.DeltaTime * 5 * RotateToCamera(Vector3.UnitY);
        
        var renderer = Graphics.GetRenderer();
        renderer.SetRenderTarget(Graphics.GetFrameTexture());
        renderer.Clear(Color.Black);
        renderer.UseBuffers(vertexBuffer, null);
        renderer.UseShader(fragmentShader);
        renderer.UseShader(vertexShader);
        fragmentShader.SetVariable("color", new Vector3(MathF.Cos(Time.TotalTime), MathF.Sin(Time.TotalTime), .5f));

        //fragmentShader.SetTexture("texture", null, TileMode.Repeat);

        vertexShader.SetVariable("world", System.Numerics.Matrix4x4.CreateRotationY(1*Time.TotalTime));

        vertexShader.SetVariable(
            "view", System.Numerics.Matrix4x4.CreateLookAt(
                cameraPosition,
                cameraPosition + RotateToCamera(Vector3.UnitZ),
                new(0, 1, 0)
                )
            );
        vertexShader.SetVariable("proj", System.Numerics.Matrix4x4.CreatePerspectiveFieldOfView(MathF.PI / 2f, 1920f / 1080f, 0.01f, 100f));
        renderer.DrawPrimitives(PrimitiveKind.Triangles, vertexBuffer.Size / 3, 0);
    }

    public override void OnUnitialize()
    {
    }
}
