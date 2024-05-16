using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WebGPU;
internal static unsafe class MarshalUtils
{
    public static string? StringFromPointer(byte* ptr)
    {
        return Marshal.PtrToStringUTF8((nint)ptr);
    }

    public static byte* AllocString(string? str)
    {
        if (str is null)
            return null;

        var bytes = (byte*)NativeMemory.Alloc((nuint)str.Length + 1);
        Encoding.UTF8.GetBytes(str, new Span<byte>(bytes, str.Length + 1));
        bytes[str.Length] = 0;
        return bytes;
    }

    public static void FreeString(byte* ptr)
    {
        NativeMemory.Free(ptr);
    }

    public static T* SpanToPointer<T>(Span<T> span) where T : unmanaged
    {
        if (span.IsEmpty)
            return null;

        return (T*)Unsafe.AsPointer(ref span[0]);
    }

    interface IAllocator
    {
        nint Alloc(nint size);
        void Free(nint ptr);
    }

    public static ChainedStruct.Native* AllocChain(IChainable? nextInChain)
    {
        if (nextInChain is null)
            return null;

        ChainedStruct.Native* head = null;
        ChainedStruct.Native* prevPtr = null;
        ChainedStruct.Native* currentPtr = null;
        IChainable? current = nextInChain;
        while (current != null)
        {
            prevPtr = currentPtr;
            currentPtr = (ChainedStruct.Native*)NativeMemory.Alloc((nuint)current.SizeInBytes);
            currentPtr->next = null;
            if (prevPtr != null)
                prevPtr->next = currentPtr;
            if (head == null)
                head = currentPtr;
            current.InitNative(currentPtr);
            current = current.Next;
        }

        return head;
    }

    public static void FreeChain(IChainable? chainable, ChainedStruct.Native* nextInChain)
    {
        if (chainable is null || nextInChain is null)
            return;

        IChainable? currentChainable = chainable;
        ChainedStruct.Native* prev = null;
        ChainedStruct.Native* current = nextInChain;
        while (current != null)
        {
            currentChainable?.UninitNative(current);
            
            currentChainable = currentChainable?.Next;

            prev = current;
            current = current->next;
            if (current != null)
                current->next = prev;
        }

        current = nextInChain;
        while (current != null)
        {
            NativeMemory.Free(current);
            current = current->next;
        }
    }

    public static T* AllocArray<T>(T[] array) where T : unmanaged
    {
        return (T*)NativeMemory.Alloc((nuint)array.Length, (nuint)Unsafe.SizeOf<T>());
    }

    public static void FreeArray<T>(T* array) where T : unmanaged
    {
        NativeMemory.Free(array);
    }
}
