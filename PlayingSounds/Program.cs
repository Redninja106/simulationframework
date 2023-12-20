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
    }
}