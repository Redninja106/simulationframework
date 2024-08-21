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
            try
            {
                canvas.Fill(shaders[i]);
                canvas.DrawRect(0, 0, 1, 1);
                canvas.Flush();
                Console.WriteLine("Test Succeeded: " + shaders[i].GetType().Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Test Failed: " + shaders[i].GetType().Name);
                Console.WriteLine(ex.ToString());
                success = false;
            }
        }
        return success;
    }
}
