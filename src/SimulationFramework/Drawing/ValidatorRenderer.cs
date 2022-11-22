using SimulationFramework.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;


internal class ValidatorRenderer : IRenderer
{
    private readonly IRenderer baseRenderer;

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

    public float DepthBias { get; set; }

    public void Clip(Rectangle? rectangle)
    {
        baseRenderer.Clip(rectangle);
    }

    public void DrawIndexedPrimitives(PrimitiveKind kind, int count, int vertexOffset, int indexOffset)
    {
        ValidateEnumValue(kind);

        ValidateStateForDrawing();

        baseRenderer.DrawIndexedPrimitives(kind, count, vertexOffset, indexOffset);
    }

    public void DrawPrimitives(PrimitiveKind kind, int count, int offset)
    {
        ValidateEnumValue(kind);

        ValidateStateForDrawing();

        baseRenderer.DrawPrimitives(kind, count, offset);
    }

    private void ValidateStateForDrawing()
    {
    }

    private void ValidateEnumValue<T>(T value) where T : struct, Enum
    {
        Assert(Enum.IsDefined(value), "Invalid " + typeof(T).Name + " value!");
    }

    private void Assert(bool condition, [CallerArgumentExpression("condition")] string? message = null)
    {
        if (!condition)
        {
            throw new Exception(message ?? "Assert failed");
        }
    }

    public void PopState()
    {
        throw new NotImplementedException();
    }

    public void PushState()
    {
        throw new NotImplementedException();
    }

    public void ResetState()
    {
        throw new NotImplementedException();
    }

    public void SetIndexBuffer(IBuffer<uint>? indexBuffer)
    {
        throw new NotImplementedException();
    }

    public void SetIndexBufferShort(IBuffer<ushort>? indexBuffer)
    {
        throw new NotImplementedException();
    }

    public void SetVertexBuffer<T>(IBuffer<T>? vertexBuffer) where T : unmanaged
    {
        throw new NotImplementedException();
    }

    public void SetVertexShader<T>(T? vertexShader) where T : struct, IShader
    {
        throw new NotImplementedException();
    }

    public void SetViewport(Rectangle viewport, float minDepth, float maxDepth)
    {
        throw new NotImplementedException();
    }

    public void SetInstanceBuffer<T>(IBuffer<T>? instanceBuffer) where T : unmanaged
    {
        throw new NotImplementedException();
    }

    public void DrawInstancedPrimitives(PrimitiveKind kind, int count, int instanceCount, int vertexOffset = 0, int instanceOffset = 0)
    {
        throw new NotImplementedException();
    }

    public void DrawIndexedInstancedPrimitives(PrimitiveKind kind, int count, int instanceCount, int vertexOffset = 0, int indexOffset = 0, int instanceOffset = 0)
    {
        throw new NotImplementedException();
    }

    public void Flush()
    {
        throw new NotImplementedException();
    }

    public void SetVertexShader(IShader shader)
    {
        throw new NotImplementedException();
    }

    public void SetGeometryShader(IShader shader)
    {
        throw new NotImplementedException();
    }

    public void SetFragmentShader(IShader shader)
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