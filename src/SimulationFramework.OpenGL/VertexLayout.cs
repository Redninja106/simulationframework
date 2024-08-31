using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;

namespace SimulationFramework.OpenGL;

class VertexLayout
{
    private static Dictionary<Type, VertexLayout> layouts = [];

    private VertexLayout(Type type)
    {
        int offset = 0; 
        List<VertexAttribute> attributes = [];
        AddAttributes(type);
        this.attributes = attributes.ToArray();
        VertexSize = offset;

        void AddAttributes(Type type)
        {
            if (!type.IsValueType)
            {
                throw new InvalidOperationException($"invalid type in vertex: {type.Name}!");
            }
            else if (type == typeof(float))
            {
                attributes.Add(new(1, GL_FLOAT, offset));
                offset += sizeof(float);
                return;
            }
            else if (type == typeof(Vector2))
            {
                attributes.Add(new(2, GL_FLOAT, offset));
                offset += 2 * sizeof(float);
                return;
            }
            else if (type == typeof(Vector3))
            {
                attributes.Add(new(3, GL_FLOAT, offset));
                offset += 3 * sizeof(float);
                return;
            }
            else if (type == typeof(Vector4) || type == typeof(ColorF))
            {
                attributes.Add(new(4, GL_FLOAT, offset));
                offset += 4 * sizeof(float);
                return;
            }
            else if (type == typeof(int))
            {
                attributes.Add(new(1, GL_INT, offset));
                offset += sizeof(int);
                return;
            }
            else if (type == typeof(uint))
            {
                attributes.Add(new(1, GL_UNSIGNED_INT, offset));
                offset += sizeof(uint);
                return;
            }
            else if (type == typeof(Color))
            {
                attributes.Add(new(4, GL_UNSIGNED_BYTE, offset));
                offset += 4 * sizeof(byte);
                return;
            }
            else if (type.IsPrimitive)
            {
                throw new($"unsupported primitive in vertex layout: {type}");
            }
            else
            {
                foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    AddAttributes(field.FieldType); ;
                }
            }
        }
    }

    private VertexAttribute[] attributes;
    public int VertexSize { get; }

    public unsafe void Bind()
    {
        for (int i = 0; i < attributes.Length; i++)
        {
            VertexAttribute attr = attributes[i];
            glEnableVertexAttribArray((uint)i);
            glVertexAttribPointer((uint)i, attr.channels, attr.attribType, (byte)GL_FALSE, VertexSize, (void*)attr.offset);
        }
    }

    public static VertexLayout Get(Type type)
    {
        if (layouts.TryGetValue(type, out var layout))
        {
            return layout;
        }
        else
        {
            layout = new(type);
            layouts[type] = layout;
            return layout;
        }
    }

    struct VertexAttribute
    {
        public int channels;
        public uint attribType;
        public int offset;

        public VertexAttribute(int channels, uint attribType, int offset)
        {
            this.channels = channels;
            this.attribType = attribType;
            this.offset = offset;
        }
    }
}