using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShaderIntegration;
internal class ShaderTestRunner
{
    private CanvasShader[] shaders;

    public ShaderTestRunner(CanvasShader[] shaders)
    {
        this.shaders = shaders;
    }

    public bool RunTests(ICanvas canvas)
    {
        bool success = true;
        for (int i = 0; i < shaders.Length; i++)
        {
            canvas.Fill(shaders[i]);
            canvas.DrawRect(0, 0, 1, 1);
            canvas.Flush();
            Console.WriteLine("Test Succeeded: " + shaders[i].GetType().Name);
        }
        return success;
    }

    public void RunTest<TShader>(ICanvas canvas)
        where TShader : CanvasShader, new()
    {
        canvas.Fill(new TShader());
        canvas.DrawRect(0, 0, 1, 1);
        canvas.Flush();
        Console.WriteLine("Test Succeeded: " + typeof(TShader).Name);
    }
}
