using NAudio.Wave;

namespace SimulationFramework.Desktop.Audio;

internal sealed class Wave24To16Stream : WaveStream
{
    public override WaveFormat WaveFormat { get; }
    
    private readonly WaveStream source;
    private readonly long length;
    private readonly long position;

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
