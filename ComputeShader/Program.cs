using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Shaders;
using System.Numerics;

const int THREADS = 100;

IBuffer<float>? bufferA = null;
IBuffer<float>? bufferB = null;
IBuffer<float>? bufferC = null;
var s = Simulation.Create(Init, Render);
s.Run();

void Init(AppConfig config)
{
    float[] bufferData = Enumerable.Range(0, THREADS).Select(i => (float)i).ToArray();
    bufferA = Graphics.CreateBuffer<float>(bufferData);
    bufferB = Graphics.CreateBuffer<float>(bufferData);
    bufferC = Graphics.CreateBuffer<float>(bufferData.Length);
    
    ITexture<Color> rgb;

    Graphics.DefaultRenderTarget

    var p = new Particle[100_000];

    for (int i = 0; i < 100 * 1000; i++)
    {
        p[i].Position += p[i].Velocity * Time.DeltaTime;
    }

    var particleBuffer = Graphics.CreateBuffer(p.AsSpan());

    // create compute shader struct
    ComputeShader shader = new(particleBuffer);

    Graphics.DispatchComputeShader(shader, 100, 100);

    Graphics.ImmediateQueue.Flush();

    var data = bufferC.GetData();

    foreach (var f in data)
    {
        Console.WriteLine(f);
    }
}

void Render(ICanvas? canvas)
{
}

struct ComputeShader : IShader
{
    [Uniform] public IBuffer<float> bufferA;
    [Uniform] public IBuffer<float> bufferB;
    [Uniform] public IBuffer<float> bufferC;

    [Uniform] public ITexture<Color> renderTarget;
    [Uniform] public IBuffer<Particle> particles;

    [Input(InputSemantic.ThreadID)]
    readonly int threadID;

    [Uniform]
    public float DeltaTime;

    public ComputeShader(IBuffer<Particle> particles)
    {
        this.particles = particles;
        DeltaTime = Time.DeltaTime;
    }

    [ThreadGroupSize(100)]
    public void Main()
    {
        particles[threadID].Position += particles[threadID].Velocity * DeltaTime;
    }
}

struct Particle
{
    public Vector3 Position { get; set; }
    public Vector3 Velocity { get; set; }
}