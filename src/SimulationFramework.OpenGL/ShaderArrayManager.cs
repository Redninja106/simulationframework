using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SimulationFramework.OpenGL;
internal class ShaderArrayManager
{
    private readonly GLGraphics graphics;
    private readonly ConditionalWeakTable<Array, ShaderArray> entries = [];

    public ShaderArrayManager(GLGraphics graphics)
    {
        this.graphics = graphics;
    }

    public void UploadArray(Array array)
    {
        ShaderArray shaderArray = this.Get(array);
        ShaderTypeLayout layout = ShaderTypeLayout.Get(array.GetType().GetElementType()!);

        GCHandle handle = GCHandle.Alloc(array, GCHandleType.Pinned);
        try
        {
            unsafe
            {
                void* arrayData = Unsafe.AsPointer(ref MemoryMarshal.GetArrayDataReference(array));
                shaderArray.WriteData(layout, arrayData, array.Length);
            }
        }
        finally
        {
            handle.Free();
        }
    }

    public ShaderArray Get(Array array)
    {
        return entries.GetValue(array, CreateShaderArray);
    }

    private ShaderArray CreateShaderArray(Array array)
    {
        if (graphics.HasGLES31)
        {
            return new SSBOShaderArray();
        }
        else
        {
            return new TextureShaderArray();
        }
    }

    public unsafe void SyncArray(Array array)
    {
        // only need to sync if array is on the gpu & modified
        if (entries.TryGetValue(array, out ShaderArray? shaderArray))
        {
            if (!shaderArray.Synchronized)
            {
                ref byte data = ref MemoryMarshal.GetArrayDataReference(array);
                void* dataPtr = Unsafe.AsPointer(ref data);

                ShaderTypeLayout layout = ShaderTypeLayout.Get(array.GetType().GetElementType()!);
                shaderArray.ReadData(layout, dataPtr, array.Length);
            }
        }
    }

    public static Array? ResolveArray(object? bufferObject)
    {
        if (bufferObject is null)
        {
            return null;
        }

        if (bufferObject is Array array)
        {
            return array;
        }

        Type bufferType = bufferObject.GetType();
        if (bufferType.IsConstructedGenericType &&
            bufferType.GetGenericTypeDefinition() == typeof(ImmutableArray<>))
        {
            unsafe
            {
                // grab the array from immutable array
                return Unsafe.As<object, BoxedImmutableArray>(ref bufferObject).array;
            }
        }

        return null;
    }

    public bool TryGet(Array array, [NotNullWhen(true)] out ShaderArray? shaderArray)
    {
        return entries.TryGetValue(array, out shaderArray);
    }

    // Don't change this!!!
    // ImmutableArray<T> has a documented layout, this matches it.
    private class BoxedImmutableArray
    {
        public Array? array;
    }
}
