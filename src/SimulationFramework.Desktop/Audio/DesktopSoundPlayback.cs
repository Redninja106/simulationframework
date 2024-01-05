using Silk.NET.OpenAL;

namespace SimulationFramework.Desktop.Audio;
internal sealed unsafe class DesktopSoundPlayback : SoundPlayback, IDisposable
{
    private readonly uint source;
    private readonly DesktopAudioProvider provider;

    public float Volume
    {
        get
        {
            provider.al.GetSourceProperty(source, SourceFloat.Gain, out float gain);
            return gain;
        }
        set
        {
            if (value is < 0 or > 1)
                throw new ArgumentOutOfRangeException(nameof(Volume), "Volume must be between 0 and 1!");
            provider.al.SetSourceProperty(source, SourceFloat.Gain, value);
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

    public DesktopSoundPlayback(DesktopAudioProvider provider, DesktopSound sound)
    {
        this.provider = provider;
        source = provider.al.GenSource();
        if (source == 0)
        {
            throw new Exception($"alGenSource() returned 0! (error: {provider.al.GetError()})");
        }
        fixed (uint* bufferPtr = &sound.buffer)
        {
            provider.al.SourceQueueBuffers(source, 1, bufferPtr);
        }
        provider.al.SourcePlay(source);
    }

    public override void Dispose()
    {
        GC.SuppressFinalize(this);
        provider.al.DeleteSource(source);
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
    }

    public override void Restart()
    {
        provider.al.SourceRewind(source);
        provider.al.SourcePlay(source);
    }

    ~DesktopSoundPlayback()
    {
        this.Dispose();
    }
}
