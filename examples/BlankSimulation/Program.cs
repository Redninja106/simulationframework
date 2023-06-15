using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Input;
using SimulationFramework.Messaging;

Simulation.Start<MySimulation>();

class MySimulation : Simulation
{
    public override void OnInitialize()
    {
        
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.Red);

        if (Keyboard.IsKeyPressed(Key.Space))
        {
            if (Window.IsFullscreen)
            {
                Window.ExitFullscreen();
            }
            else
            {
                Window.EnterFullscreen(null);
            }
        }
    }
}