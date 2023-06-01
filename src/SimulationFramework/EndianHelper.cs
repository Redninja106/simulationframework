using System;
using System.Runtime.InteropServices;

namespace SimulationFramework;

internal static class EndianHelper
{
    public static bool IsLittleEndian => BitConverter.IsLittleEndian;
    public static bool IsBigEndian => !IsLittleEndian;

    public static void MakeBigEndian<T>(ref T value) where T : unmanaged
    {
        if (IsLittleEndian)
        {
            SwapEndianness(GetBytes(ref value));
        }
    }

    public static void MakeBigEndian(Span<byte> bytes)
    {
        if (IsLittleEndian)
        {
            SwapEndianness(bytes);
        }
    }

    public static void MakeLttleEndian<T>(ref T value) where T : unmanaged
    {
        if (IsBigEndian)
        {
            SwapEndianness(GetBytes(ref value));
        }
    }

    public static void MakeLttleEndian(Span<byte> bytes)
    {
        if (IsBigEndian)
        {
            SwapEndianness(bytes);
        }
    }

    public static void SwapEndianness(Span<byte> values)
    {
        for (int i = 0; i < values.Length / 2; i++)
        {
            (values[i], values[^(i + 1)]) = (values[^(i + 1)], values[i]);
        }
    }

    private static Span<byte> GetBytes<T>(ref T value) where T : unmanaged
    {
        Span<T> span = MemoryMarshal.CreateSpan(ref value, 1);
        Span<byte> bytes = MemoryMarshal.AsBytes(span);
        return bytes;
    }
}