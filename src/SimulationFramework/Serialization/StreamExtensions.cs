using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Serialization;
internal static class StreamExtensions
{
    public static T Read<T>(this Stream stream, Endian endianness = Endian.Litte) where T : unmanaged
    {
        T result = default;


        unsafe
        {
            var resultBytes = new Span<byte>(Unsafe.AsPointer(ref result), sizeof(T));

            stream.Read(resultBytes);

            if (endianness == Endian.Big)
            {
                SwapEndianness(resultBytes);
            }
        }

        return result;
    }

    public static string ReadUTF8(this Stream stream, int length)
    {
        Span<byte> bytes = stackalloc byte[length];
        stream.Read(bytes);
        return Encoding.UTF8.GetString(bytes);
    }

    static void SwapEndianness(Span<byte> values)
    {
        for (int i = 0; i < values.Length / 2; i++)
        {
            (values[i], values[^(i + 1)]) = (values[^(i + 1)], values[i]);
        }
    }
}

internal enum Endian
{
    Litte,
    Big
}
