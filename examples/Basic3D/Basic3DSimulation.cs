using ImGuiNET;
using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.ImGuiNET;
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
    static readonly Vertex[] cube = new Vertex[]
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

    IBuffer<Vertex> modelBuffer;
    IBuffer<Vertex> cubeBuffer;

    IRenderingContext renderer;

    ITexture<Color> concrete;

    Vector3 lightPosition = Vector3.UnitY * 2;

    float camXRotation, camYRotation, camZoom;
    float xRotation, yRotation;
    bool spin, demoWindow;

    public override void OnInitialize(AppConfig config)
    {
        ObjLoader.Load("F22A.obj", out Vertex[] verts, out _);
        modelBuffer = Graphics.CreateBuffer<Vertex>(verts);

        cubeBuffer = Graphics.CreateBuffer<Vertex>(cube);

        concrete = Graphics.LoadTexture("texture.png");
        
        renderer = Graphics.CreateRenderer();
    }

    public override void OnRender(ICanvas canvas)
    {
        ImGui.Image(concrete.GetImGuiTextureID(), new(500));

        if (demoWindow)
            ImGui.ShowDemoWindow();

        ProcessInput();

        CameraTransforms camera = default;
        camera.View = Matrix4x4.CreateLookAt(Vector3.Transform(Vector3.UnitZ * MathF.Pow(1.1f, camZoom), Matrix4x4.CreateRotationX(camXRotation) * Matrix4x4.CreateRotationY(camYRotation)), Vector3.Zero, Vector3.UnitY);
        camera.Proj = Matrix4x4.CreatePerspectiveFieldOfView(MathF.PI / 3f, 16f / 9f, 0.1f, 100f);

        camera.World = Matrix4x4.CreateRotationX(xRotation) * Matrix4x4.CreateRotationY(yRotation);
        VertexShader vertexShader = new() { camera = camera };
        FragmentShader fragShader = new();
        fragShader.lightPosition = this.lightPosition;
        fragShader.texture = concrete;
        fragShader.sampler = TextureSampler.Linear;

        renderer.RenderTarget = Graphics.DefaultRenderTarget;
        // renderer.DepthTarget = Graphics.DefaultDepthTarget;
        renderer.ClearRenderTarget(Color.FromHSV(0, 0, .1f));
        // renderer.ClearDepthTarget(1.0f);
        renderer.SetViewport(new(renderer.RenderTarget.Width, renderer.RenderTarget.Height, 0, 0));
        
        renderer.CullMode = CullMode.None;

        renderer.SetVertexBuffer(modelBuffer);

        renderer.SetVertexShader(vertexShader);
        renderer.SetFragmentShader(fragShader);

        renderer.DrawPrimitives(PrimitiveKind.Triangles, modelBuffer.Length);

        renderer.SetVertexBuffer(cubeBuffer);
        camera.World = Matrix4x4.CreateTranslation(lightPosition);
        renderer.SetVertexShader(new LightVertexShader() { camera = camera });
        renderer.SetFragmentShader(new LightFragmentShader());

        renderer.DrawPrimitives(PrimitiveKind.Triangles, cubeBuffer.Length);
    }

    private void ProcessInput()
    {
        if (Keyboard.IsKeyPressed(Key.F1))
            demoWindow = !demoWindow;

        if (Mouse.IsButtonDown(MouseButton.Right))
        {
            camYRotation += Mouse.DeltaPosition.X * 0.01f;
            camXRotation += Mouse.DeltaPosition.Y * 0.01f;
        }

        camZoom -= Mouse.ScrollWheelDelta;

        if (Keyboard.IsKeyDown(Key.Plus))
            camZoom -= Time.DeltaTime * 5;

        if (Keyboard.IsKeyDown(Key.Minus))
            camZoom += Time.DeltaTime * 5;

        if (Keyboard.IsKeyDown(Key.A))
            yRotation += Time.DeltaTime;

        if (Keyboard.IsKeyDown(Key.D))
            yRotation -= Time.DeltaTime;

        if (Keyboard.IsKeyDown(Key.W))
            xRotation -= Time.DeltaTime;

        if (Keyboard.IsKeyDown(Key.S))
            xRotation += Time.DeltaTime;

        if (Keyboard.IsKeyDown(Key.RShift))
            lightPosition.Z += Time.DeltaTime * 2;

        if (Keyboard.IsKeyDown(Key.RCtrl))
            lightPosition.Z -= Time.DeltaTime * 2;

        if (Keyboard.IsKeyDown(Key.LeftArrow))
            lightPosition.X -= Time.DeltaTime * 2;

        if (Keyboard.IsKeyDown(Key.RightArrow))
            lightPosition.X += Time.DeltaTime * 2;

        if (Keyboard.IsKeyDown(Key.UpArrow))
            lightPosition.Y += Time.DeltaTime * 2;

        if (Keyboard.IsKeyDown(Key.DownArrow))
            lightPosition.Y -= Time.DeltaTime * 2;

        if (spin)
            yRotation += Time.DeltaTime;

        if (Keyboard.IsKeyPressed(Key.Space))
            spin = !spin;
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

        public override string ToString()
        {
            return $"(pos {position}, uv: {uv}, normal: {normal})";
        }
    }

    struct CameraTransforms
    {
        public Matrix4x4 World;
        public Matrix4x4 View;
        public Matrix4x4 Proj;
    }

    struct VertexShader : IShader
    {
        [Uniform]
        public CameraTransforms camera;

        [Input(InputSemantic.Vertex)]
        Vertex vertex;

        [Output]
        public Vector2 uv;
        [Output]
        public Vector3 normal;
        [Output]
        public Vector3 fragPos;

        [Output(OutputSemantic.Position)]
        public Vector4 position;
        public void Main()
        {
            position = new Vector4(vertex.position, 1);
            position = Vector4.Transform(position, camera.World);
            position = Vector4.Transform(position, camera.View);
            position = Vector4.Transform(position, camera.Proj);

            uv = vertex.uv;
            normal = Vector3.TransformNormal(vertex.normal, camera.World);

            Vector4 fragPos = Vector4.Transform(new Vector4(vertex.position, 1), camera.World);
            this.fragPos = new(fragPos.X, fragPos.Y, fragPos.Z);
        }
    }

    struct FragmentShader : IShader
    {
        [Uniform]
        public ITexture<Color> texture;
        [Uniform]
        public TextureSampler sampler;

        [Output(OutputSemantic.Color)]
        private Vector4 color;

        [Input]
        public Vector2 uv;
        [Input]
        public Vector3 normal;
        [Input]
        public Vector3 fragPos;

        [Uniform] 
        public Vector3 lightPosition;
        [Uniform] 
        public Vector3 cameraPosition;

        public void Main()
        {
            Vector3 lightDirection = Vector3.Normalize(lightPosition - fragPos);

            var albedo = texture.Sample(uv, sampler).ToVector3();
            
            var diffuse = MathF.Max(0, Vector3.Dot(normal, lightDirection));
            var ambient = .25f;

            color = new(albedo, 1);
        }
    }

    struct LightVertexShader : IShader
    {
        [Uniform]
        public CameraTransforms camera;

        [Input(InputSemantic.Vertex)]
        Vertex vertex;

        [Output(OutputSemantic.Position)]
        Vector4 position;

        public void Main()
        {
            position = new(vertex.position, 1);
            position = Vector4.Transform(position, camera.World);
            position = Vector4.Transform(position, camera.View);
            position = Vector4.Transform(position, camera.Proj);
        }
    }

    struct LightFragmentShader : IShader
    {
        [Output(OutputSemantic.Color)]
        ColorF color;

        public void Main()
        {
            color = new(1, 1, 1, 1);
        }
    }


    class ObjLoader
    {
        public static void Load(string fileName, out Vertex[] vertices, out uint[] indices)
        {
            List<Vector3> positions = new();
            List<Vector2> uvs = new();
            List<Vector3> normals = new();

            List<Vertex> verticesList = new();
            List<uint> indicesList = new();

            void AddVertex(int pos, int uv, int normal)
            {
                Vertex vertex = new(positions[pos-1], uvs[uv-1], normals[normal-1]);
                
                //if (!verticesList.Contains(vertex))
                //{
                    verticesList.Add(vertex);
                //}

                //indicesList.Add((uint)verticesList.IndexOf(vertex));
            }

            var lines = File.ReadAllLines(fileName);
            foreach (var line in lines)
            {
                var parts = line.Split(' ');

                switch (parts[0])
                {
                    case "v":
                        positions.Add(new(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3])));
                        break;
                    case "vt":
                        uvs.Add(new(float.Parse(parts[1]), float.Parse(parts[2])));
                        break;
                    case "vn":
                        normals.Add(new(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3])));
                        break;
                    case "f":
                        var v1 = parts[1].Split('/').Select(int.Parse).ToArray();
                        var v2 = parts[2].Split('/').Select(int.Parse).ToArray();
                        var v3 = parts[3].Split('/').Select(int.Parse).ToArray();

                        AddVertex(v1[0], v1[1], v1[2]);
                        AddVertex(v2[0], v2[1], v2[2]);
                        AddVertex(v3[0], v3[1], v3[2]);
                        break;  
                    default:
                        break;
                }
            }

            vertices = verticesList.ToArray();
            indices = indicesList.ToArray();
        }
    }
}