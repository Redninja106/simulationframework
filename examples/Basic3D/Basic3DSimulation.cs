using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Basic3D;

internal class Basic3DSimulation : Simulation
{
    IShader vertexShader, fragmentShader;
    IBuffer<Vertex> vertexBuffer;
    ITexture texture;

    struct Vertex
    {
        Vector3 position;
        Vector2 uv;
        Vector3 normal;

        public Vertex(Vector3 position, Vector2 uv, Vector3 normal)
        {
            this.position = position;
            this.uv = uv;
            this.normal = normal;
        }

        public Vertex(float x, float y, float z, float u, float v, float nx, float ny, float nz) : this(new(x, y, z), new(u, v), new(nx, ny, nz))
        {
        }
    }

    public override void OnInitialize(AppConfig config)
    {
        vertexShader = Graphics.CreateShader(ShaderKind.Vertex, @"
cbuffer ___cbuffer : register(b0)
{
    float4x4 world;
    float4x4 view;
    float4x4 proj;
};

struct vs_in
{
    float3 position : POSITION;
    float2 tex : TEXCOORD0;
};

struct ps_in
{
    float4 position : SV_POSITION;
    float2 tex : TEXCOORD0;
};

ps_in main(vs_in input)
{
    ps_in result;
    result.position = float4(input.position, 1);
    result.position = mul(result.position, world);
    result.position = mul(result.position, view);
    result.position = mul(result.position, proj);
    
    result.tex = input.tex;

    return result;
}
");
        fragmentShader = Graphics.CreateShader(ShaderKind.Fragment, @"
Texture2D image : register(t0);
SamplerState image_sampler : register(s0);

struct ps_in
{
    float4 position : SV_POSITION;
    float2 tex : TEXCOORD0;
};

float4 main(ps_in input) : SV_Target
{
    return image.Sample(image_sampler, input.tex);
}
");

        vertexBuffer = Graphics.CreateBuffer<Vertex>(36);
        vertexBuffer.SetData(new Vertex[]
        {
            new(-1.0f,  1.0f, -1.0f, 0.0f, 0.0f,  0.0f,  0.0f, -1.0f),
            new( 1.0f,  1.0f, -1.0f, 1.0f, 0.0f,  0.0f,  0.0f, -1.0f),
            new(-1.0f, -1.0f, -1.0f, 0.0f, 1.0f,  0.0f,  0.0f, -1.0f),
            new(-1.0f, -1.0f, -1.0f, 0.0f, 1.0f,  0.0f,  0.0f, -1.0f),
            new( 1.0f,  1.0f, -1.0f, 1.0f, 0.0f,  0.0f,  0.0f, -1.0f),
            new( 1.0f, -1.0f, -1.0f, 1.0f, 1.0f,  0.0f,  0.0f, -1.0f),
            new( 1.0f,  1.0f, -1.0f, 0.0f, 0.0f,  1.0f,  0.0f,  0.0f),
            new( 1.0f,  1.0f,  1.0f, 1.0f, 0.0f,  1.0f,  0.0f,  0.0f),
            new( 1.0f, -1.0f, -1.0f, 0.0f, 1.0f,  1.0f,  0.0f,  0.0f),
            new( 1.0f, -1.0f, -1.0f, 0.0f, 1.0f,  1.0f,  0.0f,  0.0f),
            new( 1.0f,  1.0f,  1.0f, 1.0f, 0.0f,  1.0f,  0.0f,  0.0f),
            new( 1.0f, -1.0f,  1.0f, 1.0f, 1.0f,  1.0f,  0.0f,  0.0f),
            new( 1.0f,  1.0f,  1.0f, 0.0f, 0.0f,  0.0f,  0.0f,  1.0f),
            new(-1.0f,  1.0f,  1.0f, 1.0f, 0.0f,  0.0f,  0.0f,  1.0f),
            new( 1.0f, -1.0f,  1.0f, 0.0f, 1.0f,  0.0f,  0.0f,  1.0f),
            new( 1.0f, -1.0f,  1.0f, 0.0f, 1.0f,  0.0f,  0.0f,  1.0f),
            new(-1.0f,  1.0f,  1.0f, 1.0f, 0.0f,  0.0f,  0.0f,  1.0f),
            new(-1.0f, -1.0f,  1.0f, 1.0f, 1.0f,  0.0f,  0.0f,  1.0f),
            new(-1.0f,  1.0f,  1.0f, 0.0f, 0.0f, -1.0f,  0.0f,  0.0f),
            new(-1.0f,  1.0f, -1.0f, 1.0f, 0.0f, -1.0f,  0.0f,  0.0f),
            new(-1.0f, -1.0f,  1.0f, 0.0f, 1.0f, -1.0f,  0.0f,  0.0f),
            new(-1.0f, -1.0f,  1.0f, 0.0f, 1.0f, -1.0f,  0.0f,  0.0f),
            new(-1.0f,  1.0f, -1.0f, 1.0f, 0.0f, -1.0f,  0.0f,  0.0f),
            new(-1.0f, -1.0f, -1.0f, 1.0f, 1.0f, -1.0f,  0.0f,  0.0f),
            new(-1.0f,  1.0f,  1.0f, 0.0f, 0.0f,  0.0f,  1.0f,  0.0f),
            new( 1.0f,  1.0f,  1.0f, 1.0f, 0.0f,  0.0f,  1.0f,  0.0f),
            new(-1.0f,  1.0f, -1.0f, 0.0f, 1.0f,  0.0f,  1.0f,  0.0f),
            new(-1.0f,  1.0f, -1.0f, 0.0f, 1.0f,  0.0f,  1.0f,  0.0f),
            new( 1.0f,  1.0f,  1.0f, 1.0f, 0.0f,  0.0f,  1.0f,  0.0f),
            new( 1.0f,  1.0f, -1.0f, 1.0f, 1.0f,  0.0f,  1.0f,  0.0f),
            new(-1.0f, -1.0f, -1.0f, 0.0f, 0.0f,  0.0f, -1.0f,  0.0f),
            new( 1.0f, -1.0f, -1.0f, 1.0f, 0.0f,  0.0f, -1.0f,  0.0f),
            new(-1.0f, -1.0f,  1.0f, 0.0f, 1.0f,  0.0f, -1.0f,  0.0f),
            new(-1.0f, -1.0f,  1.0f, 0.0f, 1.0f,  0.0f, -1.0f,  0.0f),
            new( 1.0f, -1.0f, -1.0f, 1.0f, 0.0f,  0.0f, -1.0f,  0.0f),
            new( 1.0f, -1.0f,  1.0f, 1.0f, 1.0f,  0.0f, -1.0f,  0.0f),
        });

        texture = Graphics.LoadTexture("texture.png");
    }

    public override void OnRender(ICanvas canvas)
    {
        vertexShader.SetVariable("world", Matrix4x4.Transpose(Matrix4x4.CreateRotationY(Time.TotalTime)));
        vertexShader.SetVariable("view", Matrix4x4.Transpose(Matrix4x4.CreateLookAt(Vector3.One * 5, Vector3.Zero, Vector3.UnitY)));
        vertexShader.SetVariable("proj", Matrix4x4.Transpose(Matrix4x4.CreatePerspectiveFieldOfView(MathF.PI / 3f, 16f/9f, 0.1f, 10f)));

        fragmentShader.SetTexture("image", texture, TileMode.Mirror);

        var renderer = Graphics.GetRenderer();

        renderer.Clear(Color.Black);

        renderer.VertexBuffer(vertexBuffer);
        renderer.Shader(vertexShader);
        renderer.Shader(fragmentShader);

        //fragmentShader.SetTexture("texture", texture, TileMode.Clamp);

        renderer.DrawPrimitives(PrimitiveKind.Triangles, 12, 0);
    }
}
