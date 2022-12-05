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

    public DepthStencilComparison DepthComparison { get; set; }
    public bool WriteDepth { get; set; }

    public void Clip(Rectangle? rectangle)
    {
        if (rectangle is not null)
        {
            Validate(currentViewport.ContainsPoint(rectangle.Value.GetAlignedPoint(Alignment.TopLeft)), nameof(rectangle));
            Validate(currentViewport.ContainsPoint(rectangle.Value.GetAlignedPoint(Alignment.BottomRight)), nameof(rectangle));
        }
        
        baseRenderer.Clip(rectangle);
    }

    public void DrawPrimitives(PrimitiveKind kind, int count)
    {
        ValidateEnumValue(kind);
        
        Validate(count >= 0, nameof(count), PRIMITIVE_COUNT_NEGATIVE);

        Validate(vertexBufferLength is not null, nameof(vertexBufferLength));

        baseRenderer.DrawPrimitives(kind, count);
    }

    public void DrawPrimitivesIndexed(PrimitiveKind kind, int count)
    {
        ValidateEnumValue(kind);

        Validate(count >= 0, nameof(count), PRIMITIVE_COUNT_NEGATIVE);

        Validate(vertexBufferLength is not null, nameof(vertexBufferLength));
        Validate(indexBufferLength is not null, nameof(indexBufferLength));

        baseRenderer.DrawPrimitivesIndexed(kind, count);
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

    public void SetIndexBuffer(IBuffer<uint>? indexBuffer, int offset = 0)
    {
        indexBufferLength = indexBuffer?.Length;
        baseRenderer.SetIndexBuffer(indexBuffer, offset);
    }

    public void SetVertexBuffer<T>(IBuffer<T>? vertexBuffer, int offset = 0) where T : unmanaged
    {
        this.vertexBufferLength = vertexBuffer?.Length;
        baseRenderer.SetVertexBuffer(vertexBuffer, offset);
    }

    public void SetInstanceBuffer<T>(IBuffer<T>? instanceBuffer, int offset = 0) where T : unmanaged
    {
        this.instanceBufferLength = instanceBuffer?.Length;
        baseRenderer.SetInstanceBuffer(instanceBuffer, offset);
    }

    public void DrawPrimitivesInstanced(PrimitiveKind kind, int primtiveCount, int instanceCount)
    {
        ValidateEnumValue(kind);

        Validate(primtiveCount >= 0, nameof(primtiveCount));
        Validate(instanceCount >= 0, nameof(instanceCount));

        Validate(vertexBufferLength is not null, nameof(vertexBufferLength));
        Validate(indexBufferLength is not null, nameof(indexBufferLength));

        baseRenderer.DrawPrimitivesInstanced(kind, primtiveCount, instanceCount);
    }

    public void DrawPrimitivesIndexedInstanced(PrimitiveKind kind, int primitiveCount, int instanceCount)
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

    public void DrawGeometry(IGeometry geometry)
    {
        throw new NotImplementedException();
    }

    public void DrawGeometryInstanced(IGeometry geometry, int instanceCount)
    {
        throw new NotImplementedException();
    }
}