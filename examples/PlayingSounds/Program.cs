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

    float ambianceVolume = 1;
    float popVolume = 1;

    public override void OnInitialize()
    {
        Audio.MasterVolume = 0.1f;
        ambiance = Audio.LoadSound("ambiance.wav");
        ambiancePlayback = ambiance.Play();
        
        pop = Audio.LoadSound("pop.wav");

        SetFixedResolution(100, 100, Color.Black);
        
        ambiancePlayback = ambiance.Loop();
    }

    public override void OnRender(ICanvas canvas)
    {
        float v = Audio.MasterVolume;
        ImGui.SliderFloat("Volume", ref v, 0, 1);
        Audio.MasterVolume = v;

        ImGui.SliderFloat("Ambiance", ref ambianceVolume, 0, 1);
        ambiancePlayback.Volume = ambianceVolume;

        ImGui.SliderFloat("Pop", ref popVolume, 0, 1);

        if (Keyboard.IsKeyPressed(Key.Space) || Mouse.ScrollWheelDelta != 0)
        {
            var playback = pop.Play(popVolume);
            playback.PitchMultiplier = 1f - .1f * Random.Shared.NextSingle();
        }
    }
}