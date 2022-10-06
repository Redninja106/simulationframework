using SimulationFramework;
using SimulationFramework.Drawing;
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

    float camXRotation, camYRotation, camZoom;
    float xRotation, yRotation;
    bool spin;

    public override void OnInitialize(AppConfig config)
    {
        vertexBuffer = Graphics.CreateBuffer(vertices);
    }

    public override void OnRender(ICanvas canvas)
    {
        if (Mouse.IsButtonDown(MouseButton.Right))
        {
            camYRotation += Mouse.DeltaPosition.X * 0.01f;
            camXRotation += Mouse.DeltaPosition.Y * 0.01f;
        }

        camZoom -= Mouse.ScrollWheelDelta;

        if (Keyboard.IsKeyDown(Key.A)) 
            yRotation += Time.DeltaTime;

        if (Keyboard.IsKeyDown(Key.D)) 
            yRotation -= Time.DeltaTime;

        if (Keyboard.IsKeyDown(Key.W)) 
            xRotation -= Time.DeltaTime;

        if (Keyboard.IsKeyDown(Key.S)) 
            xRotation += Time.DeltaTime;
        
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
            View = Matrix4x4.CreateLookAt(Vector3.Transform(Vector3.UnitZ * MathF.Pow(1.1f, camZoom), Matrix4x4.CreateRotationX(camXRotation) * Matrix4x4.CreateRotationY(camYRotation)), Vector3.Zero, Vector3.UnitY),
            Proj = Matrix4x4.CreatePerspectiveFieldOfView(MathF.PI / 3f, 16f / 9f, 0.1f, 100f)
        };

        FragmentShader fragShader = new();

        var renderer = Graphics.GetRenderer();

        renderer.Clear(Color.Gray);
        
        renderer.SetViewport(new(canvas.Width, canvas.Height, 0, 0));

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
        private Vector2 uv;

        [ShaderOut(OutSemantic.Position)]
        private Vector4 position;

        [ShaderOut]
        private Vector3 normal;

        public void Main()
        {
            position = new Vector4(vertex.position, 1);

            position = Vector4.Transform(position, World);
            position = Vector4.Transform(position, View);
            position = Vector4.Transform(position, Proj);

            uv = vertex.uv;

            normal = Vector3.TransformNormal(vertex.normal, World);
        }
    }

    struct FragmentShader : IShader
    {
        [ShaderIn]
        private Vector3 normal;

        [ShaderOut(OutSemantic.Color)]
        private Vector4 color;

        [ShaderIn(InSemantic.Position)]
        private Vector4 position;

        [ShaderIn]
        private Vector2 uv;

        //public ITexture Texture;

        public void Main()
        {
            //float x = (uv.X - .5f);
            //float y = (uv.Y - .5f);
            //float c =  x * x + y * y;
            var b = Vector3.Dot(normal, Vector3.Normalize(new Vector3(-1, 1, -1)));
            var c = b * .9f + .1f;
            color = new(c, 0, 0, 1);
            // color = new(normal.X, 0, 0, 1);
            //color = Texture.Sample(uv).ToVector4();
        }
    }
}