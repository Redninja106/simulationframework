using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Input;

Start<Program>();

partial class Program : Simulation
{
    ISound ambiance;
    ISound pop;

    SoundPlayback ambiancePlayback;

    public override void OnInitialize()
    {
        Audio.MasterVolume = 1f;
        ambiance = Audio.LoadSound("ambiance.wav");
        ambiancePlayback = ambiance.Play();

        pop = Audio.LoadSound("pop.wav");

        SetFixedResolution(100, 100, Color.Black);
    }

    public override void OnRender(ICanvas canvas)
    {
        if (ambiancePlayback.IsStopped)
        {
            ambiancePlayback = ambiance.Play();
        }
    
        if (Keyboard.IsKeyPressed(Key.Space))
        {
            pop.Play();
        }

        Console.WriteLine(Window.Width);
        Console.WriteLine(Window.Height);
    }
}