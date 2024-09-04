using System;
using System.Collections.Generic;
using System.Linq;
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
                attributes.Add(new(1, GL_FLOAT, offset, false));
                offset += sizeof(float);
                return;
            }
            else if (type == typeof(Vector2))
            {
                attributes.Add(new(2, GL_FLOAT, offset, false));
                offset += 2 * sizeof(float);
                return;
            }
            else if (type == typeof(Vector3))
            {
                attributes.Add(new(3, GL_FLOAT, offset, false));
                offset += 3 * sizeof(float);
                return;
            }
            else if (type == typeof(Vector4) || type == typeof(ColorF))
            {
                attributes.Add(new(4, GL_FLOAT, offset, false));
                offset += 4 * sizeof(float);
                return;
            }
            else if (type == typeof(int))
            {
                attributes.Add(new(1, GL_INT, offset, false));
                offset += sizeof(int);
                return;
            }
            else if (type == typeof(uint))
            {
                attributes.Add(new(1, GL_UNSIGNED_INT, offset, false));
                offset += sizeof(uint);
                return;
            }
            else if (type == typeof(Color))
            {
                attributes.Add(new(4, GL_UNSIGNED_BYTE, offset, true));
                offset += 4 * sizeof(byte);
                return;
            }
            else if (type == typeof(Matrix3x2))
            {
                attributes.AddRange([
                    new(2, GL_FLOAT, offset + 0 * sizeof(float), false),
                    new(2, GL_FLOAT, offset + 2 * sizeof(float), false),
                    new(2, GL_FLOAT, offset + 4 * sizeof(float), false),
                    ]);
                offset += 6 * sizeof(float);
                return;
            }
            else if (type == typeof(Matrix4x4))
            {
                attributes.AddRange([
                    new(4, GL_FLOAT, offset + 0 * sizeof(float), false),
                    new(4, GL_FLOAT, offset + 4 * sizeof(float), false),
                    new(4, GL_FLOAT, offset + 8 * sizeof(float), false),
                    new(4, GL_FLOAT, offset + 12 * sizeof(float), false)
                    ]);
                offset += 16 * sizeof(float);
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
    public int AttributeCount => attributes.Length;

    public unsafe void Bind(int baseIndex, bool instanceBuffer)
    {
        for (int i = 0; i < attributes.Length; i++)
        {
            VertexAttribute attr = attributes[i];
            uint location = (uint)(baseIndex + i);
            glEnableVertexAttribArray(location);
            glVertexAttribPointer(location, attr.channels, attr.attribType, (byte)(attr.normalized ? GL_TRUE : GL_FALSE), VertexSize, (void*)attr.offset);
            glVertexAttribDivisor(location, (uint)(instanceBuffer ? 1 : 0));
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
        public bool normalized;

        public VertexAttribute(int channels, uint attribType, int offset, bool normalized)
        {
            this.channels = channels;
            this.attribType = attribType;
            this.offset = offset;
            this.normalized = normalized;
        }
    }
}