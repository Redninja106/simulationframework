using Silk.NET.OpenAL;
using System.Net.NetworkInformation;

namespace SimulationFramework.Desktop.Audio;
internal sealed unsafe class DesktopSoundPlayback : SoundPlayback, IDisposable
{
    private readonly uint source;
    private readonly DesktopAudioProvider provider;
    private readonly DesktopSound sound;

    public override float Volume
    {
        get
        {
            provider.al.GetSourceProperty(source, SourceFloat.Pitch, out float gain);
            return gain;
        }
        set
        {
            if (value is < 0 or > 1)
                throw new ArgumentOutOfRangeException(nameof(Volume), "Volume must be between 0 and 1!");
            provider.al.SetSourceProperty(source, SourceFloat.Gain, value);
        }
    }

    public override float PitchMultiplier
    {
        get
        {
            provider.al.GetSourceProperty(source, SourceFloat.Pitch, out float pitch);
            return pitch;
        }
        set
        {
            if (value is < .5f or > 2f)
                throw new ArgumentOutOfRangeException(nameof(Volume), "pitch must be between .5 and 2!");
            provider.al.SetSourceProperty(source, SourceFloat.Pitch, value);
        }
    }


    public override bool IsStopped
    {
        get
        {
            provider.al.GetSourceProperty(source, GetSourceInteger.SourceState, out int sourceState);
            return (SourceState)sourceState == SourceState.Stopped;
        }
    }

    public override bool IsPaused
    {
        get
        {
            provider.al.GetSourceProperty(source, GetSourceInteger.SourceState, out int sourceState);
            return (SourceState)sourceState == SourceState.Paused;
        }
    }

    public DesktopSoundPlayback(DesktopAudioProvider provider, DesktopSound sound, float volume, bool loop)
    {
        sound.activePlaybacks.Add(this);
        this.provider = provider;
        this.sound = sound;

        source = provider.al.GenSource();
        if (source == 0)
        {
            throw new Exception($"alGenSource() returned 0! (error: {provider.al.GetError()})");
        }

        if (loop)
        {
            provider.al.SetSourceProperty(source, SourceBoolean.Looping, loop);
        }

        this.Volume = volume;

        fixed (uint* bufferPtr = &sound.buffer)
        {
            provider.al.SourceQueueBuffers(source, 1, bufferPtr);
        }
        provider.al.SourcePlay(source);
    }

    public override void Dispose()
    {
        _ = sound.activePlaybacks.Remove(this);
        if (!provider.disposed)
        {
            provider.al.DeleteSource(source);
        }
    }

    public override void Pause()
    {
        provider.al.SourcePause(source);
    }

    public override void Resume()
    {
        provider.al.SourcePlay(source);
    }

    public override void Stop()
    {
        provider.al.SourceStop(source);
        sound.activePlaybacks.Remove(this);
        Dispose();
    }

    public override void Restart()
    {
        provider.al.SourceRewind(source);
        provider.al.SourcePlay(source);
    }
}
