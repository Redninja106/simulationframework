using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Utilities;

public class CollectionsHelper
{
    public const int MAX_STACK_SIZE = 1024;

    public static unsafe void EnumerableAsSpan<T, TArg>(IEnumerable<T> enumerable, TArg state, SpanAction<T, TArg> action) where T : unmanaged
    {
        // if we were given a list, we can take a shortcut by just converting it to a span directly
        // otherwise we need make a copy of the collection so that we can pass it as a span.
        if (enumerable is List<T> polygonList)
        {
            action(CollectionsMarshal.AsSpan(polygonList), state);
            return;
        }

        var count = enumerable.Count();

        // if enumerable is empty just use an empty span
        if (count == 0)
        {
            action(Span<T>.Empty, state);
            return;
        }
            
        // if collection is too big for stack, allocate on heap
        if (Unsafe.SizeOf<T>() * count > MAX_STACK_SIZE)
        {
            T* buffer = (T*)NativeMemory.Alloc((nuint)count, (nuint)Unsafe.SizeOf<T>());
            EnumerableAsSpanContinued(enumerable, buffer, count, ref state, action);
            NativeMemory.Free(buffer);
        }
        else
        {
            T* buffer = stackalloc T[count];
            EnumerableAsSpanContinued(enumerable, buffer, count, ref state, action);
        }
    }

    private static unsafe void EnumerableAsSpanContinued<T, TArg>(IEnumerable<T> enumerable, T* ptr, int count, ref TArg state, SpanAction<T, TArg> action) where T : unmanaged
    {
        var enumerator = enumerable.GetEnumerator();

        int i = 0;
        while (enumerator.MoveNext())
        {
            ptr[i++] = enumerator.Current;
        }
        enumerator.Reset();

        action(new Span<T>(ptr, count), state);
    }
}
