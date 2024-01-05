namespace SimulationFramework.Components;

/// <summary>
/// Provides audio functionality to the application
/// </summary>
public interface IAudioProvider : ISimulationComponent
{
    /// <summary>
    /// Controls the volume of all sounds.
    /// </summary>
    float MasterVolume { get; set; }

    /// <summary>
    /// Loads a sound file from its raw data.
    /// </summary>
    ISound LoadSound(ReadOnlySpan<byte> encodedData, SoundFileKind kind);
}
