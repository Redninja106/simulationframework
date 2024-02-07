using ImGuiNET;
using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Input;

#nullable disable

Start<Program>();

partial class Program : Simulation
{
    ISound ambiance;
    ISound pop;

    SoundPlayback ambiancePlayback;

    public override void OnInitialize()
    {
        Audio.MasterVolume = 0.1f;
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

        float v = Audio.MasterVolume;
        ImGui.SliderFloat("Volume", ref v, 0, 1);
        Audio.MasterVolume = v;

        if (Keyboard.IsKeyPressed(Key.Space) || Mouse.ScrollWheelDelta != 0)
        {
            pop.Play();
        }
    }
}