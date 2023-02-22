using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Shaders;
using System.Numerics;

const int THREADS = 100;

IBuffer<float>? buffer = null;

var s = Simulation.Create(Init, Render);
s.Run();

void Init(AppConfig config)
{
    float[] bufferData = Enumerable.Range(0, THREADS).Select(i => (float)i).ToArray();
    buffer = Graphics.CreateBuffer<float>(bufferData);

    ComputeShader shader = new() { buffer = buffer };

    Graphics.DispatchComputeShader(shader, 100, 100);
    Graphics.ImmediateQueue.Flush();

    var data = buffer.GetData();

    foreach (var value in data)
    {
        Console.WriteLine(value);
    }
}

void Render(ICanvas? canvas)
{
}

struct ComputeShader : IShader
{
    [Uniform] 
    public IBuffer<float> buffer;

    [Uniform]
    public float DeltaTime;

    [Input(InputSemantic.ThreadID)]
    readonly int threadID;

    public void Main()
    {
        buffer[threadID] *= 2;
    }
}