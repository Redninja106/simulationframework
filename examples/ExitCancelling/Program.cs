using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Input;
using SimulationFramework.Messaging;

Start<Program>();

partial class Program : Simulation
{
    public override void OnInitialize()
    {
        Application.Exiting += Application_Exiting;
    }

    private void Application_Exiting(ExitMessage message)
    {
        Console.WriteLine("Simulation is exiting!");
        if (message.IsCancellable)
        {
            Application.CancelExit();
        }
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.Black);

        if (Keyboard.IsKeyReleased(Key.Space))
        {
            Application.Exit(true);
        }

        if (Keyboard.IsKeyReleased(Key.Escape))
        {
            Application.Exit(false);
        }
    }
}