using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Disassembler;

internal static class StreamExtensions
{
    public static T Read<T>(this Stream stream) where T : unmanaged
    {
        T result = default;

        unsafe
        {
            var resultBytes = new Span<byte>(Unsafe.AsPointer(ref result), sizeof(T));

            stream.Read(resultBytes);
        }

        return result;
    }
}