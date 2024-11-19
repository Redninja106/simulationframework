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
    public override unsafe void OnInitialize()
    {
        ShaderCompiler.DumpShaders = true;
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.Black);
        canvas.Fill(new Shader());
        canvas.DrawRect(1, 1, 1, 1);
    }
}

class Shader : CanvasShader
{
    int[,] array = new int[100, 100];

    public override ColorF GetPixelColor(Vector2 position)
    {
        ColorF r = ColorF.Red;

        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                r.R += array[i, j];
            }
        }

        return r;
    }
}
