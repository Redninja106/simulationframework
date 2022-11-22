﻿using SimulationFramework.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;


internal class ValidatorRenderer : IRenderer
{
    private const string PRIMITIVE_COUNT_NEGATIVE = "Primitive count cannot be negative!";

    private readonly IRenderer baseRenderer;
    private int? vertexBufferLength;
    private int? indexBufferLength;
    private int? instanceBufferLength;
    private Rectangle currentViewport = new(0, 0, 0, 0);
    private int stateStackHeight = 0;

    public ValidatorRenderer(IRenderer baseRenderer)
    {
        this.baseRenderer = baseRenderer;
    }

    public ITexture<Color>? RenderTarget 
    { 
        get
        {
            return baseRenderer.RenderTarget;
        }
        set
        {
            baseRenderer.RenderTarget = value;
        }
    }

    public ITexture<float>? DepthTarget
    {
        get
        {
            return baseRenderer.DepthTarget;
        }
        set
        {
            baseRenderer.DepthTarget = value;
        }
    }

    public ITexture<byte>? StencilTarget
    {
        get
        {
            return baseRenderer.StencilTarget;
        }
        set
        {
            baseRenderer.StencilTarget = value;
        }
    }

    public CullMode CullMode
    {
        get
        {
            return baseRenderer.CullMode;
        }
        set
        {
            ValidateEnumValue(value);
            baseRenderer.CullMode = value;
        }
    }

    public float DepthBias
    {
        get
        {
            return baseRenderer.DepthBias;
        }
        set
        {
            Validate(!float.IsFinite(value), nameof(DepthBias));
            baseRenderer.DepthBias = value;
        }
    }

    public bool Wireframe
    {
        get
        {
            return baseRenderer.Wireframe;
        }
        set
        {
            baseRenderer.Wireframe = value;
        }
    }


    public void Clip(Rectangle? rectangle)
    {
        if (rectangle is not null)
        {
            Validate(currentViewport.ContainsPoint(rectangle.Value.GetAlignedPoint(Alignment.TopLeft)), nameof(rectangle));
            Validate(currentViewport.ContainsPoint(rectangle.Value.GetAlignedPoint(Alignment.BottomRight)), nameof(rectangle));
        }
        
        baseRenderer.Clip(rectangle);
    }

    public void DrawPrimitives(PrimitiveKind kind, int count, int vertexOffset)
    {
        ValidateEnumValue(kind);
        
        Validate(count >= 0, nameof(count), PRIMITIVE_COUNT_NEGATIVE);
        Validate(vertexOffset >= 0, nameof(vertexOffset), "Vertex offset cannot be negative!");

        Validate(vertexOffset + Graphics.GetVertexCount(kind, count) < vertexBufferLength, nameof(vertexOffset), "Vertex offset + Vertex count cannot be >= the length of the vertex buffer!");

        Validate(vertexBufferLength is not null, nameof(vertexBufferLength));

        baseRenderer.DrawPrimitives(kind, count, vertexOffset);
    }

    public void DrawPrimitivesIndexed(PrimitiveKind kind, int count, int vertexOffset, int indexOffset)
    {
        ValidateEnumValue(kind);

        Validate(count >= 0, nameof(count), PRIMITIVE_COUNT_NEGATIVE);
        Validate(vertexOffset >= 0, nameof(count), PRIMITIVE_COUNT_NEGATIVE);

        Validate(vertexOffset >= 0 && vertexOffset + Graphics.GetVertexCount(kind, count) < vertexBufferLength, nameof(vertexOffset));

        Validate(vertexBufferLength is not null, nameof(vertexBufferLength));
        Validate(indexBufferLength is not null, nameof(indexBufferLength));

        baseRenderer.DrawPrimitivesIndexed(kind, count, vertexOffset, indexOffset);
    }

    private void ValidateEnumValue<T>(T value, [CallerArgumentExpression(nameof(value))] string? paramName = null) where T : struct, Enum
    {
        Debug.Assert(Enum.IsDefined(value), Exceptions.InvalidEnumArgument(paramName));
    }

    private void Validate(bool condition, string? paramName, [CallerArgumentExpression("condition")] string? message = null)
    {
        if (!condition)
        {
            throw new ArgumentException(message, paramName);
        }
    }

    public void PopState()
    {
        if (stateStackHeight <= 0)
        {
            throw new InvalidOperationException();
        }

        stateStackHeight--;
        baseRenderer.PopState();
    }

    public void PushState()
    {
        stateStackHeight++;
        baseRenderer.PushState();
    }

    public void ResetState()
    {
        baseRenderer.ResetState();
    }

    public void SetIndexBuffer(IBuffer<uint>? indexBuffer)
    {
        indexBufferLength = indexBuffer?.Length;
        baseRenderer.SetIndexBuffer(indexBuffer);
    }

    public void SetVertexBuffer<T>(IBuffer<T>? vertexBuffer) where T : unmanaged
    {
        this.vertexBufferLength = vertexBuffer?.Length;
        baseRenderer.SetVertexBuffer(vertexBuffer);
    }

    public void SetInstanceBuffer<T>(IBuffer<T>? instanceBuffer) where T : unmanaged
    {
        this.instanceBufferLength = instanceBuffer?.Length;
        baseRenderer.SetInstanceBuffer(instanceBuffer);
    }

    public void DrawPrimitivesInstanced(PrimitiveKind kind, int count, int instanceCount, int vertexOffset = 0, int instanceOffset = 0)
    {
        ValidateEnumValue(kind);

        Validate(count >= 0, nameof(count));
        Validate(vertexOffset >= 0, nameof(count));
        Validate(instanceCount >= 0, nameof(instanceCount));

        Validate(vertexOffset >= 0 && vertexOffset + Graphics.GetVertexCount(kind, count) < vertexBufferLength, nameof(vertexOffset));
        Validate(instanceOffset >= 0 && instanceOffset + instanceCount < vertexBufferLength, nameof(vertexOffset));

        Validate(vertexBufferLength is not null, nameof(vertexBufferLength));
        Validate(indexBufferLength is not null, nameof(indexBufferLength));

        baseRenderer.DrawPrimitivesInstanced(kind, count, instanceCount, vertexOffset, instanceOffset);
    }

    public void DrawPrimitivesIndexedInstanced(PrimitiveKind kind, int count, int instanceCount, int vertexOffset = 0, int indexOffset = 0, int instanceOffset = 0)
    {
        throw new NotImplementedException();
    }

    public void Flush()
    {
        throw new NotImplementedException();
    }

    public void SetVertexShader(IShader? shader)
    {
        throw new NotImplementedException();
    }

    public void SetGeometryShader(IShader? shader)
    {
        throw new NotImplementedException();
    }

    public void SetFragmentShader(IShader? shader)
    {
        throw new NotImplementedException();
    }

    public void SetViewport(Rectangle viewport)
    {
        throw new NotImplementedException();
    }

    public void ClearRenderTarget(Color color)
    {
        throw new NotImplementedException();
    }

    public void ClearDepthTarget(float depth)
    {
        throw new NotImplementedException();
    }

    public void ClearStencilTarget(byte stencil)
    {
        throw new NotImplementedException();
    }
}