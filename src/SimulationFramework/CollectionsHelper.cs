using System.Buffers;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SimulationFramework;

/// <summary>
/// Provides utilities for copying data between different kinds of collections.
/// </summary>
public static class CollectionsHelper
{
    /// <summary>
    /// A reasonable limit for stack allocation size.
    /// </summary>
    public const int MAX_STACK_SIZE = 1024;

    /// <summary>
    /// Copies an IEnumerable to a block of unmanaged memory and provides that memory to <paramref name="action"/>.
    /// </summary>
    /// <typeparam name="T">The type of element to copy.</typeparam>
    /// <typeparam name="TArg">The state object type.</typeparam>
    /// <param name="enumerable">The source enumerable.</param>
    /// <param name="state">A state object passed to <paramref name="action"/>.</param>
    /// <param name="action">The delegate to provide with the un</param>
    public static unsafe void EnumerableAsSpan<T, TArg>(IEnumerable<T> enumerable, TArg state, SpanAction<T, TArg> action) where T : unmanaged
    {
        // if we were given a list, we can take a shortcut by just converting it to a span directly
        // otherwise we need make a copy of the collection so that we can pass it as a span.
        if (enumerable is List<T> list)
        {
            action(CollectionsMarshal.AsSpan(list), state);
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
        using var enumerator = enumerable.GetEnumerator();

        int i = 0;
        while (enumerator.MoveNext())
        {
            ptr[i++] = enumerator.Current;
        }

        action(new Span<T>(ptr, count), state);
    }

    /// <summary>
    /// Gets an <see cref="IEnumerable{T}"/> for the provided unmanaged array.
    /// </summary>
    /// <typeparam name="T">The type of element to enumerate.</typeparam>
    /// <param name="elements">A pointer to the array.</param>
    /// <param name="length">The length of the array, in elements.</param>
    public static unsafe IEnumerable<T> AsEnumerableUnsafe<T>(T* elements, int length) where T : unmanaged
    {
        return new MemoryEnumerable<T>(elements, length);
    }

    /// <summary>
    /// Gets an <see cref="IEnumerator{T}"/> for the provided unmanaged array.
    /// </summary>
    /// <typeparam name="T">The type of element to enumerate.</typeparam>
    /// <param name="elements">A pointer to the array.</param>
    /// <param name="length">The length of the array, in elements.</param>
    /// <returns></returns>
    public static unsafe IEnumerator<T> GetEnumeratorUnsafe<T>(T* elements, int length) where T : unmanaged
    {
        return new MemoryEnumerator<T>(elements, length);
    }


    private unsafe readonly struct MemoryEnumerable<T> : IEnumerable<T> where T : unmanaged
    {
        private readonly T* memory;
        private readonly int length;

        public MemoryEnumerable(T* memory, int length)
        {
            this.memory = memory;
            this.length = length;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<T> GetEnumerator()
        {
            return new MemoryEnumerator<T>(memory, length);
        }
    }

    private unsafe struct MemoryEnumerator<T> : IEnumerator<T> where T : unmanaged
    {
        private readonly T* memory;
        private readonly int length;
        private int index;

        public T Current => memory[index];
        object IEnumerator.Current => Current;

        public MemoryEnumerator(Span<T> span) : this((T*)Unsafe.AsPointer(ref span[0]), span.Length)
        {
        }

        public MemoryEnumerator(T* memory, int length)
        {
            this.memory = memory;
            this.length = length;
            this.index = 0;
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            index++;

            return index >= length;
        }

        public void Reset()
        {
            index = 0;
        }
    }
}
