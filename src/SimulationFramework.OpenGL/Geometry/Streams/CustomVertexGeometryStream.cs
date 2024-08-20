using SimulationFramework.Drawing.Shaders.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;

namespace SimulationFramework.OpenGL.Geometry.Streams;

class CustomVertexGeometryStream : GeometryStream
{
    internal List<byte> vertexData = [];
    private uint vao;
    private ShaderType vertexType;
    private int vertexSize;

    public unsafe CustomVertexGeometryStream(ShaderType vertexType)
    {
        var graphics = Application.GetComponent<GLGraphics>();
        this.vertexType = vertexType;

        fixed (uint* vaoPtr = &vao)
        {
            glGenVertexArrays(1, vaoPtr);
            glBindVertexArray(vao);
            vertexSize = ShaderType.CalculateTypeSize(vertexType);
        }
    }

    public override int GetVertexSize()
    {
        return vertexSize;
    }

    public override void WriteVertex(Vector2 position)
    {
        throw new NotSupportedException();
    }

    public override ReadOnlySpan<byte> GetData()
    {
        return CollectionsMarshal.AsSpan(this.vertexData);
    }

    public void WriteVertex<TVertex>(TVertex vertex)
        where TVertex : unmanaged
    {
        var span = new Span<TVertex>(ref vertex);
        var bytes = MemoryMarshal.AsBytes(span);
        vertexData.AddRange(bytes);
    }

    public override int GetVertexCount()
    {
        return vertexData.Count / vertexSize;
    }

    public override void Clear()
    {
        vertexData.Clear();
    }

    public override void BindVertexArray()
    {
        glBindVertexArray(vao);
        int location = 0, offset = 0;
        SetVertexLayout(vertexType, vertexSize, ref location, ref offset);
    }


    public static unsafe void SetVertexLayout(ShaderType type, int vertexSize, ref int location, ref int offset)
    {
        if (type is ShaderStructureType structType)
        {
            foreach (var field in structType.structure.fields)
            {
                SetVertexLayout(field.Type, vertexSize, ref location, ref offset);
            }
        }
        else if (type.GetPrimitiveKind() is PrimitiveKind primitiveKind)
        {
            int channels = primitiveKind switch
            {
                PrimitiveKind.Float2 or PrimitiveKind.UInt2 or PrimitiveKind.Int2 => 2,
                PrimitiveKind.Float3 or PrimitiveKind.UInt3 or PrimitiveKind.Int3 => 3,
                PrimitiveKind.Float4 or PrimitiveKind.UInt4 or PrimitiveKind.Int4 => 4,
                _ => 1,
            };

            uint attribType = primitiveKind switch
            {
                PrimitiveKind.Int or PrimitiveKind.Int2 or PrimitiveKind.Int3 or PrimitiveKind.Int4 => GL_INT,
                PrimitiveKind.Float or PrimitiveKind.Float2 or PrimitiveKind.Float3 or PrimitiveKind.Float4 => GL_FLOAT,
                PrimitiveKind.UInt or PrimitiveKind.UInt2 or PrimitiveKind.UInt3 or PrimitiveKind.UInt4 => GL_UNSIGNED_INT,
            };

            uint loc = (uint)location++;
            glEnableVertexAttribArray(loc);
            glVertexAttribPointer(loc, channels, attribType, (byte)GL_FALSE, vertexSize, (void*)offset);
            offset += channels * 4;
        }
        else
        {
            throw new NotSupportedException();
        }
    }
}
