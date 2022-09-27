using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.RenderPipeline;
using SimulationFramework.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Basic3D;

internal class Basic3DSimulation : Simulation
{
    static readonly Vertex[] vertices = new Vertex[]
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
    };

    IBuffer<Vertex> vertexBuffer;
    float xRotation, yRotation;
    bool spin;
    public override void OnInitialize(AppConfig config)
    {
        vertexBuffer = Graphics.CreateBuffer(vertices);
    }

    public override void OnRender(ICanvas canvas)
    {
        if (Keyboard.IsKeyDown(Key.A)) yRotation += Time.DeltaTime;
        if (Keyboard.IsKeyDown(Key.D)) yRotation -= Time.DeltaTime;
        if (Keyboard.IsKeyDown(Key.W)) xRotation -= Time.DeltaTime;
        if (Keyboard.IsKeyDown(Key.S)) xRotation += Time.DeltaTime;
        
        if (spin)
        {
            yRotation += Time.DeltaTime;
        }

        if (Keyboard.IsKeyPressed(Key.Space))
        {
            spin = !spin;
        }

        VertexShader vertexShader = new()
        {
            World = Matrix4x4.CreateRotationX(xRotation) * Matrix4x4.CreateRotationY(yRotation),
            View = Matrix4x4.CreateLookAt(Vector3.One * 5, Vector3.Zero, Vector3.UnitY),
            Proj = Matrix4x4.CreatePerspectiveFieldOfView(MathF.PI / 3f, 16f / 9f, 0.1f, 10f)
        };

        FragmentShader fragShader = new();

        var renderer = Graphics.GetRenderer();

        renderer.Clear(Color.Black);

        renderer.SetVertexBuffer(vertexBuffer);
        renderer.SetVertexShader(vertexShader);
        renderer.SetFragmentShader(fragShader);

        renderer.DrawPrimitives(PrimitiveKind.Triangles, 12, 0);
    }

    struct Vertex
    {
        public Vector3 position;
        public Vector2 uv;
        public Vector3 normal;

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

    struct VertexShader : IShader
    {
        public Matrix4x4 World;
        public Matrix4x4 View;
        public Matrix4x4 Proj;

        [ShaderIn]
        private Vertex vertex;

        [ShaderOut]
        private Vector3 normal;

        [ShaderOut]
        private Vector2 uv;

        [ShaderOut(OutSemantic.Position)]
        private Vector4 position;

        public void Main()
        {
            position = new Vector4(vertex.position, 1);

            position = Vector4.Transform(position, World);
            position = Vector4.Transform(position, View);
            position = Vector4.Transform(position, Proj);

            uv = vertex.uv;

            normal = vertex.normal;

            normal = Vector3.TransformNormal(vertex.normal, World);
        }
    }

    struct FragmentShader : IShader
    {
        [ShaderIn]
        private Vector2 uv;

        [ShaderIn]
        private Vector3 normal;

        [ShaderOut(OutSemantic.Color)]
        private Vector4 color;

        public void Main()
        {
            float x = (uv.X - .5f);
            float y = (uv.Y - .5f);
            float c = x * x + y * y;
            var b = Vector3.Dot(normal, Vector3.Normalize(new Vector3(-1, 1, -1)));
            c = b * MathF.Sqrt(c) * .5f + .25f;
            color = new(uv.X, uv.Y, 0, 1);
        }
    }
}