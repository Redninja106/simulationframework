using SimulationFramework;
using SimulationFramework.Drawing;

namespace BlankSimulation;

internal class BlankSimulation : Simulation
{
    public override void OnInitialize(AppConfig config)
    {
    }

    float h, s, v;

    public override void OnRender(ICanvas canvas)
    {
        if (Keyboard.IsKeyPressed(Key.Space))
        {
            Console.WriteLine("H:");
            h = float.Parse(Console.ReadLine());
            Console.WriteLine("S:");
            s = float.Parse(Console.ReadLine());
            Console.WriteLine("V:");
            v = float.Parse(Console.ReadLine());
        }

        canvas.Clear(Color.FromHSV(h /  360f, s / 100, v / 100));
    }
}