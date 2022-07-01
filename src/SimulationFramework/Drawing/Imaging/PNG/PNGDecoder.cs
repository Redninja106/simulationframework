using SimulationFramework.Drawing.Imaging;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Imaging.PNG;

public class PNGDecoder : IDisposable
{
    public PNGMetadata Metadata { get; private set; }

    private Stream stream;
    private MemoryStream dataStream = new();
    private bool isAtEnd;

    public PNGDecoder(Stream stream)
    {
        this.stream = stream;

        Decode();
    }

    private void Decode()
    {
        ReadPngSignature(stream);

        while (!isAtEnd)
        {
            DecodeChunk();
        }
    }

    private void DecodeChunk()
    {
        var dataSize = stream.Read<int>(Endian.Big);
        var chunkType = stream.ReadUTF8(4);

        switch (chunkType)
        {
            case "IHDR":
                DecodeHeader(dataSize);
                break;
            case "IDAT":
                DecodeData(dataSize);
                break;
            case "IEND":
                DecodeEnd(dataSize);
                break;
            default:
                DecodeUnknown(dataSize);
                break;
        }

        var crc = stream.Read<int>(Endian.Big);
    }

    private void DecodeUnknown(int dataSize)
    {
        stream.Position += dataSize;
    }

    private void DecodeHeader(int size)
    {
        var width = stream.Read<uint>(Endian.Big);
        var height = stream.Read<uint>(Endian.Big);
        var bitDepth = stream.Read<byte>(Endian.Big);
        var colorType = stream.Read<byte>(Endian.Big);
        var compressionMethod = stream.Read<byte>(Endian.Big);
        var filterMethod = stream.Read<byte>(Endian.Big);
        var interlaceMethod = stream.Read<byte>(Endian.Big);

        Metadata = new PNGMetadata(width, height, bitDepth, colorType, compressionMethod, filterMethod, interlaceMethod);
    }

    private void DecodeData(int size)
    {
        var bytes = new byte[size];
        stream.Read(bytes, 0, size);
        dataStream.Write(bytes);
    }

    private void CheckCRC()
    {
        var position = stream.Position;
    }

    private void DecodeEnd(int size)
    {
        if (size != 0)
            throw new Exception();

        isAtEnd = true;
    }

    public void GetColors(Span<Color> colors)
    {
        switch (Metadata.ColorType)
        {
            case 6:
                if (Metadata.BitDepth == 8)
                {
                    GetColors(new(4, 8), (pixelInfo, pixel) =>
                    {
                        return new Color(pixel[0], pixel[1], pixel[2], pixel[3]);
                    }, colors);
                }
                else if (Metadata.BitDepth == 16)
                {
                    GetColors(new(4, Metadata.BitDepth), (pixelInfo, pixel) =>
                    {
                        return new Color(pixel[0]);
                    }, colors);
                }
                else
                {
                    throw new Exception();
                }
                break;
            default:
                throw new Exception("unknown color type");
        }
    }

    private static void ReadPngSignature(Stream stream)
    {
        Span<byte> bytes = stackalloc byte[] { 137, 80, 78, 71, 13, 10, 26, 10 };

        for (int i = 0; i < bytes.Length; i++)
        {
            var b = stream.ReadByte();
            if (b != bytes[i])
            {
                throw new InvalidDataException("Stream is not a valid PNG file.");
            }
        }
    }

    private void GetColors(PixelInfo pixelInfo, ColorConverter colorConverter, Span<Color> colors)
    {
        dataStream.Position = 0;
        using var compressedStream = new ZLibStream(dataStream, CompressionMode.Decompress, true);

        if (Metadata.InterlaceMethod == 1)
        {
            throw new Exception();
        }
        else
        {
            var unfilteredColors = new MemoryStream();
            var pixelCount = Metadata.Width * Metadata.Height;
            var scanlineCount = Metadata.Height;
            var scanlineSize = (int)(pixelInfo.GetPixelSize() * Metadata.Width);
            var scanline = new byte[scanlineSize];
            var prevScanline = new byte[scanlineSize];

            for (int i = 0; i < scanlineCount; i++)
            {
                var filterType = (byte)compressedStream.ReadByte();
                (scanline, prevScanline) = (prevScanline, scanline);

                int bytesRead = 0;
                while (bytesRead < scanlineSize)
                {
                    bytesRead += compressedStream.Read(scanline, bytesRead, scanlineSize - bytesRead);
                }

                FilterScanline(filterType, scanline, prevScanline, pixelInfo);
                unfilteredColors.Write(scanline.AsSpan());
            }

            Span<byte> col = stackalloc byte[pixelInfo.GetPixelSize()];

            unfilteredColors.Position = 0;
            for (int i = 0; i < pixelCount; i++)
            {
                unfilteredColors.Read(col);
                colors[i] = colorConverter(pixelInfo, col);
            }
        }
    }

    private void FilterScanline(byte filterType, byte[] scanline, byte[] prevScanline, PixelInfo pixelInfo)
    {
        var pixelSize = pixelInfo.GetPixelSize();

        for (int i = 0; i < scanline.Length; i++)
        {
            var a = (byte)(i - pixelSize < 0 ? 0 : scanline[i - pixelSize]);
            var b = prevScanline[i];
            var c = (byte)(i - pixelSize < 0 ? 0 : prevScanline[i - pixelSize]);

            switch (filterType)
            {
                default:
                case 0:
                    break;
                case 1:
                    scanline[i] = (byte)((scanline[i] + a) % 256);
                    break;
                case 2:
                    scanline[i] = (byte)((scanline[i] + b) % 256);
                    break;
                case 3:
                    // Recon(x) = Filt(x) + floor((Recon(a) + Recon(b)) / 2)
                    scanline[i] = (byte)((scanline[i] + (a + b) / 2) % 256);
                    break;
                case 4:
                    // Recon(x) = Filt(x) + PaethPredictor(Recon(a), Recon(b), Recon(c))
                    scanline[i] = (byte)((scanline[i] + PaethPredictor(a, b, c)) % 256);
                    break;
            }
        }
    }

    private static byte PaethPredictor(byte a, byte b, byte c)
    {
        byte Pr;
        var p = a + b - c;
        var pa = MathF.Abs(p - a);
        var pb = MathF.Abs(p - b);
        var pc = MathF.Abs(p - c);

        if (pa <= pb && pa <= pc)
        {
            Pr = a;
        }
        else if (pb <= pc)
        {
            Pr = b;
        }
        else
        {
            Pr = c;
        }

        return Pr;
    }

    public void Dispose()
    {
        stream.Dispose();
    }

    private delegate Color ColorConverter(PixelInfo pixelInfo, Span<byte> bytes);

    record struct PixelInfo(int Channels, int BitDepth)
    {
        public int GetPixelSize()
        {
            return BitDepth / 8 * Channels;
        }
    }
}