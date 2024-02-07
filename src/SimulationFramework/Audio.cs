using SimulationFramework.Components;
using System.IO;

namespace SimulationFramework;

/// <summary>
/// Provides audio functionality to the simulation.
/// </summary>
public static class Audio
{
    private static IAudioProvider Provider => Application.GetComponent<IAudioProvider>();

    /// <summary>
    /// Controls the volume of all sounds.
    /// </summary>
    public static float MasterVolume
    {
        get => Provider.MasterVolume;
        set
        {
            if (value < 0)
                throw new InvalidOperationException("Volume cannot be less than zero!");

            Provider.MasterVolume = value;
        }
    }

    /// <summary>
    /// Loads a sound from a file. Supported file formats are: <c>.wav</c>,<c>.mp3</c>, and <c>.ogg</c>.
    /// </summary>
    public static ISound LoadSound(string file)
    {
        SoundFileKind kind;
        if (file.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
        {
            kind = SoundFileKind.Wav;
        }
        else if (file.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
        {
            kind = SoundFileKind.Mp3;
        }
        else if (file.EndsWith(".ogg", StringComparison.OrdinalIgnoreCase))
        {
            kind = SoundFileKind.Ogg;
        }
        else
        {
            throw new ArgumentException("File type cannot be determined from file name.");
        }

        return LoadSound(file, kind);
    }

    /// <summary>
    /// Loads a sound from a file.
    /// </summary>
    public static ISound LoadSound(string file, SoundFileKind kind)
    {
        var data = File.ReadAllBytes(file);
        return LoadSound(data, kind);
    }

    /// <summary>
    /// Loads a sound file from its raw data.
    /// </summary>
    public static ISound LoadSound(ReadOnlySpan<byte> encodedData, SoundFileKind kind)
    {
        return Provider.LoadSound(encodedData, kind);
    }
}
