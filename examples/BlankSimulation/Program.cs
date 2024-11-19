using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing.Shaders.Compiler;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

Start<Program>();

partial class Program : Simulation
{
    ColorF[,] myArray = new ColorF[100, 100];

    public override unsafe void OnInitialize()
    {
        ShaderCompiler.DumpShaders = true;
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.Black);

        canvas.Fill(Color.Red);

        Graphics.Dispatch(new ComputeShaderForMyArray() { array = myArray }, 100, 100, 1);
        canvas.Fill(new ShaderThatUsesArray() { array = myArray });

        canvas.DrawRect(0, 0, canvas.Width, canvas.Height);
    }
}

class ComputeShaderForMyArray : ComputeShader
{
    public ColorF[,] array;

    public override void RunThread(int i, int j, int k)
    {
        array[i, j] = new ColorF(i / 100f, j / 100f, 0);
    }
}

class ShaderThatUsesArray : CanvasShader
{
    public ColorF[,] array;

    public override ColorF GetPixelColor(Vector2 position)
    {
        return array[(int)position.X, (int)position.Y];
    }
}
