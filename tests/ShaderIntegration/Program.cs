using ShaderIntegration;
using ShaderIntegration.Shaders;
using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing.Shaders.Compiler;
using System.Reflection;

Start<Program>();

partial class Program : Simulation
{
    ShaderTestRunner runner;

    public override void OnInitialize()
    {
        ShaderCompiler.DumpShaders = true;

        List<CanvasShader> shaders = [];
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetCustomAttribute<TestAttribute>() != null))
        {
            shaders.Add((CanvasShader)Activator.CreateInstance(type)!);
        }

        runner = new(shaders.ToArray());
    }

    bool ranTests = false;

    public override void OnRender(ICanvas canvas)
    {
        if (ranTests)
        {
            Application.Exit(false);
            return;
        }
        ranTests = true;
        runner.RunTests(canvas);
        canvas.Flush();
        Console.WriteLine(new string('=', 20));
        Console.WriteLine("success!");
        Application.Exit(false);
    }
}