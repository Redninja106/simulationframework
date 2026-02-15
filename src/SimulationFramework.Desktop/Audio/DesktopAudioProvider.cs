using SimulationFramework.Components;
using SimulationFramework.Messaging;
using Silk.NET.OpenAL;
using System;

namespace SimulationFramework.Desktop.Audio;
internal unsafe class DesktopAudioProvider : IAudioProvider
{
    public ALContext alc = ALContext.GetApi();
    public AL al = AL.GetApi();
    internal bool disposed;

    Device* device;
    Context* context;
    internal readonly List<DesktopSound> loadedSounds = [];

    public float MasterVolume
    {
        get
        {
            al.GetListenerProperty(ListenerFloat.Gain, out float gain);
            return gain;
        }
        set
        {
            al.SetListenerProperty(ListenerFloat.Gain, value);
        }
    }

    public void Dispose()
    {
        foreach (var sound in loadedSounds)
        {
            sound.Dispose();
        }
        loadedSounds.Clear();

        disposed = true;
        alc.DestroyContext(context);
        alc.CloseDevice(device);
    }

    public void Initialize(MessageDispatcher dispatcher)
    {
        device = alc.OpenDevice(null);

        if (device == null)
        {
            throw new Exception("unable to get openal device");
        }

        context = alc.CreateContext(device, null);
        alc.MakeContextCurrent(context);
    }

    public ISound LoadSound(ReadOnlySpan<byte> encodedData, SoundFileKind kind)
    {
        var sound = new DesktopSound(this, encodedData, kind);
        loadedSounds.Add(sound);
        return sound;
    }
}
