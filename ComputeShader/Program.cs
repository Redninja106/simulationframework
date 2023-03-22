using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Shaders;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;

const int THREADS = 500;

IBuffer<float>? buffer = null;

var s = Simulation.Create(Init, Render);
s.Run();

void Init(AppConfig config)
{
    float[] bufferData = Enumerable.Range(0, THREADS).Select(i => (float)i).ToArray();
    buffer = Graphics.CreateBuffer(bufferData);

    ComputeShader shader = new() { buffer = buffer };

    Graphics.DispatchComputeShader(shader, THREADS);

    var data = buffer.GetData();
    var data2 = buffer.GetData();

    Graphics.DispatchComputeShader(shader, THREADS);

    var data3 = buffer.GetData();

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

    [Input(InputSemantic.ThreadIndex)]
    readonly int threadID;

    [ShaderMethod]
    public void Main()
    {
        buffer[threadID] *= 2;
    }
}