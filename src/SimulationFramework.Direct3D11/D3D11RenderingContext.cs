using SimulationFramework.Drawing.Direct3D11.Textures;
using SimulationFramework.Shaders;
using SimulationFramework.Shaders.Compiler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;
using Vortice.Direct3D11.Debug;
using Vortice.DXGI;
using Vortice.Mathematics;

namespace SimulationFramework.Drawing.Direct3D11;

internal sealed class D3D11RenderingContext : D3D11Object, IRenderingContext
{
    public ITexture<Color>? RenderTarget 
    {
        get => (ITexture<Color>)this.renderTarget!;
        set => this.renderTarget = (IResourceProvider<ID3D11RenderTargetView>)value!;
    }

    public ITexture<float>? DepthTarget
    {
        get => (ITexture<float>)this.depthTarget!;
        set => this.depthTarget = (IResourceProvider<ID3D11DepthStencilView>)value!;
    }

    public ITexture<byte>? StencilTarget { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public GraphicsQueueBase D3DQueue { get; private set; }

    public IGraphicsQueue Queue { get => D3DQueue; set => D3DQueue = value as GraphicsQueueBase; }

    public CullMode CullMode
    {
        get
        {
            return this.rasterizerInfo.CullMode;
        }
        set
        {
            Debug.Assert(Enum.IsDefined(value));
            this.rasterizerInfo.CullMode = value;
        }
    }

    public bool Wireframe
    {
        get
        {
            return this.rasterizerInfo.Wireframe;
        }
        set
        {

            this.rasterizerInfo.Wireframe = value;
        }
    }

    public bool BlendEnabled { get; set; }
    public ColorF BlendConstant { get; set; }
    public byte StencilTestValue { get; set; }

    private IShader? vertexShader;
    private IShader? fragmentShader;

    private Rectangle? clipRect;
    private (Rectangle bounds, float minDepth, float maxDepth)? viewport;
    private bool isAutoViewport;

    private IResourceProvider<ID3D11Buffer>? vertexBuffer;
    private IResourceProvider<ID3D11Buffer>? indexBuffer;
    private IResourceProvider<ID3D11Buffer>? instanceBuffer;

    private IResourceProvider<ID3D11RenderTargetView>? renderTarget;
    private IResourceProvider<ID3D11DepthStencilView>? depthTarget;

    private ColorF blendConstant;

    private BlendStateProvider.BlendStateInfo blendInfo;
    private RasterizerStateProvider.RasterizerStateInfo rasterizerInfo;
    private DepthStencilStateProvider.DepthStencilInfo depthStencilInfo;

    public D3D11RenderingContext(DeviceResources resources, GraphicsQueueBase queue) : base(resources)
    {
        this.D3DQueue = queue;
        RenderTarget = Graphics.DefaultRenderTarget;

        depthStencilInfo = new()
        {
            depthComparison = DepthStencilComparison.LessThanOrEqual,
        };
    }


    public void BindForDrawing(PrimitiveKind primitiveKind)
    {
        BindTargets();
        BindBuffers();
        BindShaders();
        BindStates();

        D3DQueue.DeviceContext.IASetPrimitiveTopology(primitiveKind.AsPrimitiveTopology());
    }

    private void BindTargets()
    {
        var context = D3DQueue.DeviceContext;

        ID3D11RenderTargetView? renderTargetResource = null;
        ID3D11DepthStencilView? depthTargetResource = null;
        
        renderTarget?.GetResource(out renderTargetResource);
        depthTarget?.GetResource(out depthTargetResource);

        var rtArray = new ID3D11RenderTargetView[1];
        context.OMGetRenderTargets(1, rtArray, out var currentDepthStencil);
        var currentRenderTarget = rtArray[0];

        if (currentRenderTarget != renderTargetResource || currentDepthStencil != depthTargetResource)
        {
            renderTarget?.NotifyBound(D3DQueue, BindingUsage.RenderTarget, true);
            depthTarget?.NotifyBound(D3DQueue, BindingUsage.DepthStencilTarget, true);

            context.OMSetRenderTargets(renderTargetResource!, depthTargetResource);
        }

        // dispose wrapper objects
        currentRenderTarget?.Dispose();
        currentDepthStencil?.Dispose();
    }

    private void BindBuffers()
    {
        var context = D3DQueue.DeviceContext;

        if (vertexBuffer is not null)
        {
            vertexBuffer.NotifyBound(D3DQueue, BindingUsage.VertexBuffer, false);
            vertexBuffer.GetResource(out var vb);
            context.IASetVertexBuffer(0, vb, vb.Description.StructureByteStride);
        }

        if (indexBuffer is not null)
        {
            indexBuffer.NotifyBound(D3DQueue, BindingUsage.IndexBuffer, false);
            indexBuffer.GetResource(out var ib);

            Format format = indexBuffer switch
            {
                IBuffer<uint> => Format.R32_UInt,
                IBuffer<ushort> => Format.R16_UInt,
                _ => throw new Exception()
            };

            context.IASetIndexBuffer(ib, format, 0);
        }

        if (instanceBuffer is not null)
        {
            instanceBuffer.NotifyBound(D3DQueue, BindingUsage.VertexBuffer, false);
            instanceBuffer.GetResource(out var ib);
            context.IASetVertexBuffer(1, ib, ib.Description.StructureByteStride);
        }
    }

    private void BindShaders()
    {
        var context = D3DQueue.DeviceContext;

        if (vertexShader is null)
            throw new InvalidOperationException("Cannot draw without a vertex shader");
        if (fragmentShader is null)
            throw new InvalidOperationException("Cannot draw without a fragment shader");

        var d3dVertexShader = Resources.ShaderProvider.GetVertexShader(this.vertexShader.GetType());

        context.IASetInputLayout(Resources.InputLayoutProvider.GetInputLayout(d3dVertexShader, instanceBuffer is not null));

        d3dVertexShader.Update(this.vertexShader);
        d3dVertexShader.Apply(context);

        var d3dFragmentShader = Resources.ShaderProvider.GetFragmentShader(this.fragmentShader.GetType(), d3dVertexShader.Compilation.OutputSignature);
        d3dFragmentShader.Update(this.fragmentShader);
        d3dFragmentShader.Apply(context);
    }

    private void BindStates()
    {
        var context = D3DQueue.DeviceContext;

        context.OMSetBlendState(Resources.BlendStateProvider.GetBlendState(this.blendInfo), new(this.blendConstant.ToVector4()));
        context.RSSetState(Resources.RasterizerStateProvider.GetRasterizerState(this.rasterizerInfo with { ClipEnable = this.clipRect is not null }));
        
        depthStencilInfo.depthEnabled = depthTarget is not null;
        context.OMSetDepthStencilState(Resources.DepthStencilStateProvider.GetDepthStencilState(this.depthStencilInfo));

        if (this.clipRect is not null)
        {
            context.RSSetScissorRect(
                (int)this.clipRect.Value.X,
                (int)this.clipRect.Value.Y,
                (int)this.clipRect.Value.Width,
                (int)this.clipRect.Value.Height
                );
        }

        // set default viewport
        if (isAutoViewport || this.viewport is null)
        {
            viewport = new(new(0, 0, RenderTarget.Width, RenderTarget.Height), 0, 1);
            isAutoViewport = true;
        }

        context.RSSetViewport(
            this.viewport.Value.bounds.X,
            this.viewport.Value.bounds.Y,
            this.viewport.Value.bounds.Width,
            this.viewport.Value.bounds.Height,
            this.viewport.Value.minDepth,
            this.viewport.Value.maxDepth
            );
    }

    public override void Dispose()
    {
        base.Dispose();
    }

    public void SetClipRectangle(Rectangle? rectangle)
    {
        clipRect = rectangle;

        if (rectangle is not null) 
        {
            Rectangle rect = rectangle.Value;
            this.D3DQueue.DeviceContext.RSSetScissorRect((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
        }
        else
        {
            this.D3DQueue.DeviceContext.RSSetScissorRects(0, Array.Empty<Rectangle>());
        }
    }

    public void ResetState()
    {
    }

    public void Flush()
    {
        this.D3DQueue.Flush();
    }

    public void SetVertexShader(IShader? shader)
    {
        this.vertexShader = shader!;
    }

    public void SetGeometryShader(IShader? shader)
    {
        throw new NotImplementedException();
    }

    public void SetFragmentShader(IShader? shader)
    {
        this.fragmentShader = shader;
    }

    public void ClearRenderTarget(Color color)
    {
        if (this.renderTarget is null)
            throw new InvalidOperationException("No Depth Target is set");


        D3DQueue.DeviceContext.ClearRenderTargetView(this.renderTarget.GetResource(), new(color.ToVector4()));
    }

    public void ClearDepthTarget(float depth)
    {
        if (this.depthTarget is null)
            throw new InvalidOperationException("No Depth Target is set");

        D3DQueue.DeviceContext.ClearDepthStencilView(this.depthTarget.GetResource(), DepthStencilClearFlags.Depth, depth, 0);
    }

    public void ClearStencilTarget(byte stencil)
    {
        throw new NotImplementedException();
    }

    public void DrawGeometry(IGeometry geometry)
    {
        throw new NotImplementedException();
    }

    public void Submit(IGraphicsQueue deferredQueue)
    {
        throw new NotImplementedException();
    }

    public void SetVertexBuffer<T>(IBuffer<T>? vertexBuffer) where T : unmanaged
    {
        if (vertexBuffer is null)
        {
            D3DQueue.DeviceContext.IASetVertexBuffer(0, null!, 0);
            return;
        }

        if (vertexBuffer is not Buffer<T> d3dBuffer)
            throw new ArgumentException(null, nameof(vertexBuffer));

        d3dBuffer.NotifyBound(this.D3DQueue, BindingUsage.VertexBuffer, false);

        d3dBuffer.GetResource(out ID3D11Buffer buffer);
        D3DQueue.DeviceContext.IASetVertexBuffer(0, buffer, d3dBuffer.Stride);
    }

    public void SetInstanceBuffer<T>(IBuffer<T>? instanceBuffer) where T : unmanaged
    {
        throw new NotImplementedException();
    }

    public void DrawPrimitives(PrimitiveKind kind, int vertexCount, int vertexOffset = 0)
    {
        BindForDrawing(kind);

        D3DQueue.DeviceContext.Draw(vertexCount, vertexOffset);
    }

    public void DrawIndexedPrimitives(PrimitiveKind kind, int indexCount, int indexOffset = 0, int vertexOffset = 0)
    {
        BindForDrawing(kind);

        D3DQueue.DeviceContext.DrawIndexed(indexCount, indexOffset, vertexOffset);
    }


    public void SetStencilMode(DepthStencilComparison comparison, StencilOperation pass, StencilOperation fail)
    {
        throw new NotImplementedException();
    }

    public void DrawInstancedPrimitives(PrimitiveKind kind, int vertexCount, int instanceCount, int vertexOffset = 0, int instanceOffset = 0)
    {
        throw new NotImplementedException();
    }

    public void DrawIndexedInstancedPrimitives(PrimitiveKind kind, int indexCount, int instanceCount, int indexOffset = 0, int instanceOffset = 0)
    {
        throw new NotImplementedException();
    }

    public void DrawInstancedGeometry(IGeometry geometry, int instanceCount)
    {
        throw new NotImplementedException();
    }

    public void SetIndexBuffer<T>(IBuffer<T>? indexBuffer) where T : unmanaged
    {
        if (indexBuffer == this.indexBuffer)
        {
            return;
        }

        if (indexBuffer is null)
        {
            this.indexBuffer = null;
            return;
        }

        var d3dBuffer = indexBuffer as IResourceProvider<ID3D11Buffer> ?? throw D3DExceptions.InvalidBuffer(nameof(indexBuffer));

        if (Unsafe.SizeOf<T>() is not 2 or 4)
        {
            throw new InvalidOperationException("Index buffers must be either 16-bit or 32 bit");
        }

        if (typeof(T) != typeof(uint) && typeof(T) != typeof(ushort))
        {
            Console.WriteLine($"Warning: index buffer type is not uint or ushort.");
        }

        this.indexBuffer = d3dBuffer;
    }

    public void SetBlendEnabled(bool enabled)
    {
        blendInfo.BlendEnabled = enabled;
    }

    public void SetBlendMode(BlendMode sourceBlend, BlendMode destinationBlend, BlendOperation operation = BlendOperation.Add)
    {
        blendInfo.SourceBlend = sourceBlend;
        blendInfo.DestinationBlend = destinationBlend;
        blendInfo.BlendOperation = operation;
    }

    public void SetAlphaBlendMode(BlendMode sourceBlend, BlendMode destinationBlend, BlendOperation operation = BlendOperation.Add)
    {
        blendInfo.SourceBlendAlpha = sourceBlend;
        blendInfo.DestinationBlendAlpha = destinationBlend;
        blendInfo.BlendOperationAlpha = operation;
    }

    public void SetStencilMasks(byte readMask, byte writeMask)
    {
        depthStencilInfo.stencilReadMask = readMask;
        depthStencilInfo.stencilWriteMask = writeMask;
    }

    public void DrawInstancedPrimitives(PrimitiveKind kind, IBuffer<DrawCommand> commands, int commandOffset = 0, int? commandCount = null)
    {
        throw new NotImplementedException();
    }

    public void DrawIndexedInstancedPrimitives(PrimitiveKind kind, IBuffer<IndexedDrawCommand> commands, int commandOffset = 0, int? commandCount = null)
    {
        throw new NotImplementedException();
    }

    public void SetViewport(Rectangle bounds, float minDepth = 0, float maxDepth = 1)
    {
        this.viewport = new(bounds, minDepth, maxDepth);
        isAutoViewport = false;
    }

    public void SetDepthMode(DepthStencilComparison comparison, bool readOnly, float depthBias)
    {
        depthStencilInfo.depthComparison = comparison;
        depthStencilInfo.depthReadOnly = readOnly;
        depthStencilInfo.depthBias = depthBias;
    }
}