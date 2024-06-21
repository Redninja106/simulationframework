using System.ComponentModel;

namespace SimulationFramework;

/// <summary>
/// Represents a playable sound.
/// </summary>
public interface ISound : IDisposable
{
    /// <summary>
    /// The sample rate of the sound.
    /// </summary>
    int SampleRate { get; }

    /// <summary>
    /// The duration of the sound, in seconds.
    /// </summary>
    float Duration { get; }

    /// <summary>
    /// Whether the sound is stereo (dual-channel) or mono (single channel).
    /// </summary>
    bool IsStereo { get; }

    /// <summary>
    /// Whether the sound is 16 bit or 8 bit.
    /// </summary>
    [Obsolete("Will be removed.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    bool Is16Bit { get; }

    /// <summary>
    /// Begins to play the sound. The playback will stop once the sound has been fully played.
    /// </summary>
    /// <returns>A <see cref="SoundPlayback"/> instance representing the started playback.</returns>
    SoundPlayback Play(float volume = 1);

    /// <summary>
    /// Begins to loop the sound. A looped sound plays repeatedly until stopped.
    /// </summary>
    /// <returns>A <see cref="SoundPlayback"/> instance representing the started playback.</returns>
    SoundPlayback Loop(float volume = 1);
}
