using Microsoft.VisualBasic;
using NAudio;
using NAudio.Vorbis;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Silk.NET.OpenAL;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Desktop.Audio;
internal sealed class DesktopSound : ISound
{
    private readonly DesktopAudioProvider provider;
    // internal readonly List<DesktopSoundPlayback> activePlaybacks;
    internal readonly uint buffer;

    public int SampleRate { get; }
    public float Duration { get; }
    public bool IsStereo { get; }
    public bool Is16Bit { get; }

    public unsafe DesktopSound(DesktopAudioProvider provider, ReadOnlySpan<byte> encodedData, SoundFileKind fileKind)
    {
        this.provider = provider;

        fixed (byte* encodedDataPtr = encodedData)
        {
            var stream = new UnmanagedMemoryStream(encodedDataPtr, encodedData.Length);

            WaveStream reader = fileKind switch
            {
                SoundFileKind.Wav => new WaveFileReader(stream),
                SoundFileKind.Mp3 => new Mp3FileReader(stream),
                SoundFileKind.Ogg => new VorbisWaveReader(stream),
                _ => throw new NotSupportedException("Unsupported audio file kind."),
            };

            if (reader.WaveFormat.BitsPerSample is 32)
            {
                reader = new Wave32To16Stream(reader);
            }
            else if (reader.WaveFormat.BitsPerSample is 24)
            {
                reader = new Wave24To16Stream(reader);
            }

            var data = new byte[stream.Length / reader.BlockAlign * reader.BlockAlign];
            reader.Read(data.AsSpan());

            buffer = provider.al.GenBuffer();

            if (buffer == 0)
            {
                throw new Exception($"alGenBuffer returned null! error: {provider.al.GetError()}");
            }

            fixed (byte* dataPtr = &data[0])
            {
                provider.al.BufferData(buffer, GetBufferFormat(reader.WaveFormat), dataPtr, data.Length, reader.WaveFormat.SampleRate);
                if (provider.al.GetError() != AudioError.NoError)
                {
                    throw new Exception($"error on alBufferData! {provider.al.GetError()}");
                }
            }

            var format = reader.WaveFormat;

            SampleRate = format.SampleRate;
            IsStereo = format.Channels == 2;
            Is16Bit = format.BitsPerSample == 16;

            int sampleSize = Is16Bit ? 2 : 1;
            int sampleCount = data.Length / sampleSize;

            float duration = sampleCount / SampleRate;

            if (IsStereo)
                duration *= .5f;

            Duration = duration;
        }
    }

    private static BufferFormat GetBufferFormat(WaveFormat format)
    {
        if (format.Channels is 1)
        {
            if (format.BitsPerSample == 16)
            {
                return BufferFormat.Mono16;
            }
            else if (format.BitsPerSample == 8)
            {
                return BufferFormat.Mono8;
            }
        }
        else if (format.Channels is 2)
        {
            if (format.BitsPerSample == 16)
            {
                return BufferFormat.Stereo16;
            }
            else if (format.BitsPerSample == 8)
            {
                return BufferFormat.Stereo8;
            }
        }

        throw new Exception("Audio format not supported!");
    }

    public SoundPlayback Play()
    {
        var newPlayback = new DesktopSoundPlayback(provider, this);
        return newPlayback;
    }

    public void Dispose()
    {
        provider.al.DeleteBuffer(this.buffer);
    }
}

internal sealed class Wave24To16Stream : WaveStream
{
    public override WaveFormat WaveFormat { get; }
    
    private WaveStream source;
    private long length;
    private long position;

    public Wave24To16Stream(WaveStream source)
    {
        this.source = source;

        WaveFormat = new WaveFormat(source.WaveFormat.SampleRate, 16, source.WaveFormat.Channels);

        length = source.Length / 3 * 2;
        position = source.Position / 3 * 2;
    }

    /// <summary>
    /// Returns the stream length
    /// </summary>
    public override long Length
    {
        get
        {
            return length;
        }
    }

    /// <summary>
    /// Gets or sets the current position in the stream
    /// </summary>
    public override long Position
    {
        get
        {
            return position;
        }
        set
        {
            throw new();
        }
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        byte[] blockBuffer = new byte[6];

        int bytesWritten = 0;
        for (int i = 0; i < count / 4; i++)
        {
            int loc = offset + i * 4;
            source.Read(blockBuffer.AsSpan());

            buffer[loc + 0] = blockBuffer[1];
            buffer[loc + 1] = blockBuffer[2];
            buffer[loc + 2] = blockBuffer[4];
            buffer[loc + 3] = blockBuffer[5];
            bytesWritten += 4;
        }
        return bytesWritten;
    }
}
