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
    IBuffer<uint> indexBuffer;
    IRenderer renderer;

    ITexture<float> depthTarget;

    Vector3 lightPosition = Vector3.UnitY * 2;

    float camXRotation, camYRotation, camZoom;
    float xRotation, yRotation;
    bool spin;

    public override void OnInitialize(AppConfig config)
    {
        ObjLoader.Load("blender_monkey.obj", out Vertex[] verts, out uint[] inds);

        vertexBuffer = Graphics.CreateBuffer<Vertex>(verts);
        renderer = Graphics.CreateRenderer();
        // indexBuffer = Graphics.CreateBuffer<uint>(inds);

        depthTarget = Graphics.CreateTexture<float>(1920, 1080);
    }

    public override void OnRender(ICanvas canvas)
    {
        ImGuiNET.ImGui.ShowDemoWindow();

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
        {
            yRotation += Time.DeltaTime;
        }

        if (Keyboard.IsKeyPressed(Key.Space))
        {
            spin = !spin;
        }

        VertexShader vertexShader = new();
        vertexShader.uniforms.World = Matrix4x4.CreateRotationX(xRotation) * Matrix4x4.CreateRotationY(yRotation);
        vertexShader.uniforms.View = Matrix4x4.CreateLookAt(Vector3.Transform(Vector3.UnitZ * MathF.Pow(1.1f, camZoom), Matrix4x4.CreateRotationX(camXRotation) * Matrix4x4.CreateRotationY(camYRotation)), Vector3.Zero, Vector3.UnitY);
        vertexShader.uniforms.Proj = Matrix4x4.CreatePerspectiveFieldOfView(MathF.PI / 3f, 16f / 9f, 0.1f, 100f);

        FragmentShader fragShader = new();
        fragShader.lightPosition = this.lightPosition;

        renderer.RenderTarget = Graphics.DefaultRenderTarget;
        renderer.DepthTarget = Graphics.DefaultDepthTarget;
        renderer.ClearRenderTarget(Color.FromHSV(0,0,.1f));
        renderer.ClearDepthTarget(1.0f);
        renderer.SetViewport(new(renderer.RenderTarget.Width, renderer.RenderTarget.Height, 0, 0));
        
        renderer.CullMode = CullMode.Front;

        renderer.SetVertexBuffer(vertexBuffer);
        // renderer.SetIndexBuffer(indexBuffer);

        renderer.SetVertexShader(vertexShader);
        renderer.SetFragmentShader(fragShader);

        renderer.DrawPrimitives(PrimitiveKind.Triangles, vertexBuffer.Length);
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

    struct VertexShaderUniforms
    {
        public Matrix4x4 World;
        public Matrix4x4 View;
        public Matrix4x4 Proj;
    }

    struct VertexShaderOutput
    {
    }

    struct VertexShader : IShader
    {
        [Uniform]
        public VertexShaderUniforms uniforms;

        [Input] 
        Vertex vertex;

        [Output(OutputSemantic.Position)]
        public Vector4 position;
        [Output]
        public Vector2 uv;
        [Output]
        public Vector3 normal;
        [Output]
        public Vector3 fragPos;

        public void Main()
        {
            //if (position.X < 12)
            //{
            //    position = Vector4.Zero;
            //}

            //for (int i = 0; i < 10; i++)
            //{
            //    this.fragPos += normal * 2;
            //}


            position = new Vector4(vertex.position, 1);

            position = Vector4.Transform(position, uniforms.World);
            position = Vector4.Transform(position, uniforms.View);
            position = Vector4.Transform(position, uniforms.Proj);

            /*ShaderIntrinsics.Hlsl(@"
    // __output.position = float4(__input.vertex.position, 1);
    __output.position = __input.vertex.position * uniforms.World;
    __output.position = __input.vertex.position * uniforms.View;
    __output.position = __input.vertex.position * uniforms.Proj;

");
            */
            uv = vertex.uv;

            normal = Vector3.TransformNormal(vertex.normal, uniforms.World);

            Vector4 fragPos = Vector4.Transform(vertex.position, uniforms.World);
            this.fragPos = new(fragPos.X, fragPos.Y, fragPos.Z);
        }
    }

    struct FragmentShaderInput
    {
        [Input(InputSemantic.Position)]
        public Vector4 position;
        [Input]
        public Vector2 uv;
        [Input]
        public Vector3 normal;
        [Input]
        public Vector3 fragPos;
    }

    struct FragmentShader : IShader
    {
        [Output(OutputSemantic.Color)]
        private Vector4 color;


        [Input(InputSemantic.Position)]
        public Vector4 position;
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
            ShaderIntrinsics.Hlsl(@"
// hello world
");

            Vector3 lightDirection = Vector3.Normalize(fragPos - lightPosition);

            var brightness = MathF.Max(0, Vector3.Dot(normal, lightDirection));

            brightness += .25f;

            color = new(brightness, brightness, brightness, 1);
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