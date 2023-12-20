using SimulationFramework.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// 
/// </summary>
public static class Audio
{
    private static IAudioProvider Provider => Application.GetComponent<IAudioProvider>();

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

    public static ISound LoadSound(string file, SoundFileKind kind)
    {
        var data = File.ReadAllBytes(file);
        return LoadSound(data, kind);
    }


    public static ISound LoadSound(ReadOnlySpan<byte> encodedData, SoundFileKind kind)
    {
        return Provider.LoadSound(encodedData, kind);
    }
}

public enum SoundFileKind
{
    Ogg,
    Mp3,
    Wav
}