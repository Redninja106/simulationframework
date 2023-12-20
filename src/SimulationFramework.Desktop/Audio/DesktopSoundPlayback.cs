﻿using Silk.NET.OpenAL;
using SimulationFramework.Desktop.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Desktop.Audio;
internal unsafe class DesktopSoundPlayback : SoundPlayback, IDisposable
{
    uint source;
    DesktopAudioProvider provider;

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

    public DesktopSoundPlayback(DesktopAudioProvider provider, DesktopSound sound)
    {
        this.provider = provider;
        source = provider.al.GenSource();
        fixed (uint* bufferPtr = &sound.buffer)
        {
            provider.al.SourceQueueBuffers(source, 1, bufferPtr);
        }
        provider.al.SourcePlay(source);
    }


    public override void Dispose()
    {
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
}