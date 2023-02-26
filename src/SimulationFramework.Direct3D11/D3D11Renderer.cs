using SimulationFramework.Drawing.Direct3D11.Buffers;
using SimulationFramework.Drawing.Direct3D11.Textures;
using SimulationFramework.Shaders;
using SimulationFramework.Shaders.Compiler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;
using Vortice.Direct3D11.Debug;
using Vortice.Mathematics;

namespace SimulationFramework.Drawing.Direct3D11;

internal sealed class D3D11Renderer : D3D11Object, IRenderer
{
    public ITexture<Color> RenderTarget
    {
        get => currentRenderTarget;
        set => SetRenderTarget(value);
    }

    public ITexture<float> DepthTarget
    {
        get => currentDepthTarget;
        set => depthStencilManager.SetDepthTexture(value as D3D11Texture<float>);
    }

    public D3D11QueueBase D3D11Queue { get; private set; }

    public IGraphicsQueue Queue { get => D3D11Queue; set => D3D11Queue = value as D3D11QueueBase; }

    public ITexture<byte> StencilTarget { get; set; }
    public DepthStencilComparison DepthComparison { get; set; }
    public bool WriteDepth { get; set; }

    public CullMode CullMode
    {
        get
        {
            return this.cullMode;
        }
        set
        {
            Debug.Assert(Enum.IsDefined(value));
            this.cullMode = value;
        }
    }

    public bool Wireframe
    {
        get
        {
            return this.wireframe;
        }
        set
        {

            this.wireframe = value;
        }
    }

    public float DepthBias { get; set; }
    public byte StencilReferenceValue { get; set; }
    public byte StencilReadMask { get; set; }
    public byte StencilWriteMask { get; set; }
    public DepthStencilComparison StencilComparison { get; set; }
    public StencilOperation StencilFailOperation { get; set; }
    public StencilOperation StencilPassDepthFailOperation { get; set; }
    public StencilOperation StencilPassOperation { get; set; }
    public bool BlendEnabled { get; set; }
    public ColorF BlendConstant { get; set; }

    private D3D11Texture<Color> currentRenderTarget;
    private D3D11Texture<float> currentDepthTarget;

    private ShaderSignature vsOutputSignature;
    private ShaderSignature gsOutputSignature;

    private IShader vertexShader;
    private IShader geometryShader;
    private IShader fragmentShader;

    private CullMode cullMode;
    private bool wireframe;

    private readonly RasterizerStateManager rasterizerStateManager;
    private readonly DepthStencilManager depthStencilManager;
    private readonly BlendStateManager blendStateManager;

    private ID3D11BlendState blendState;
    private Rectangle? clipRect;

    public D3D11Renderer(DeviceResources resources, D3D11QueueBase queue) : base(resources)
    {
        this.D3D11Queue = queue;
        depthStencilManager = new(resources);
        blendStateManager = new(resources);
        rasterizerStateManager = new(resources);
    }

    public void PreDraw(PrimitiveKind primitiveKind)
    {
        D3D11Queue.DeviceContext.IASetPrimitiveTopology(primitiveKind.AsPrimitiveTopology());

        var vp = D3D11Queue.DeviceContext.RSGetViewport();

        if (vp.Width == 0 || vp.Height == 0)
        {
            D3D11Queue.DeviceContext.RSSetViewport(0, 0, this.currentRenderTarget.Width, this.currentRenderTarget.Height, 0, 1);
        }

        D3D11Queue.DeviceContext.RSSetState(rasterizerStateManager.GetRasterizerState(new(this.cullMode, this.wireframe, this.clipRect is not null)));
        D3D11Queue.DeviceContext.OMSetBlendState(BlendEnabled ? this.blendState : null, blendFactor: new Color4(this.BlendConstant.ToVector4()));

        depthStencilManager.PreDraw(this.D3D11Queue.DeviceContext);
    }

    private void SetRenderTarget(ITexture<Color> renderTarget)
    {
        if (renderTarget is not D3D11Texture<Color> d3dTexture)
            throw new ArgumentException(null, nameof(renderTarget));

        currentRenderTarget = d3dTexture;
        D3D11Queue.DeviceContext.OMSetRenderTargets(d3dTexture.RenderTargetView, depthStencilManager.DepthStencilView);
    }

    public void BeginFrame()
    {
        SetViewport(new(0, 0, 0, 0));
    }

    public void SetViewport(Rectangle viewport)
    {
        D3D11Queue.DeviceContext.RSSetViewport(viewport.X, viewport.Y, viewport.Width, viewport.Height, 0, 1);
    }

    public override void Dispose()
    {
        this.D3D11Queue.DeviceContext.Dispose();
        this.depthStencilManager.Dispose();
        base.Dispose();
    }

    public void Clip(Rectangle? rectangle)
    {
        clipRect = rectangle;

        if (rectangle is not null) 
        {
            Rectangle rect = rectangle.Value;
            this.D3D11Queue.DeviceContext.RSSetScissorRect((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
        }
        else
        {
            this.D3D11Queue.DeviceContext.RSSetScissorRects(0, Array.Empty<Rectangle>());
        }
    }

    public void PushState()
    {
        // throw new NotImplementedException();
    }

    public void PopState()
    {
        // throw new NotImplementedException();
    }

    public void ResetState()
    {
    }

    public void Flush()
    {
        this.D3D11Queue.Flush();
    }

    public void SetInstanceBuffer<T>(IBuffer<T> instanceBuffer, int offset = 0) where T : unmanaged
    {
        throw new NotImplementedException();
    }

    public void DrawPrimitivesInstanced(PrimitiveKind kind, int primitiveCount, int instanceCount)
    {
        throw new NotImplementedException();
    }

    public void DrawPrimitivesIndexedInstanced(PrimitiveKind kind, int primitiveCount, int instanceCount)
    {
        throw new NotImplementedException();
    }

    public void SetVertexShader(IShader shader)
    {
        if (shader is null)
        {
            this.vertexShader = null;
            return;
        }


        this.GetType().GetMethod(nameof(SetVertexShader), BindingFlags.Instance | BindingFlags.NonPublic, new[] { shader.GetType() }).MakeGenericMethod(new[] { shader.GetType() }).Invoke(this, new[] { shader });
    }

    private void SetVertexShader<T>(IShader shader) where T : struct, IShader
    {
        var shaderObject = Resources.ShaderManager.Shaders.OfType<D3D11VertexShader<T>>().SingleOrDefault(s => s.ShaderType == shader.GetType());

        if (shaderObject is null)
        {
            shaderObject = new D3D11VertexShader<T>(this.Resources);
            Resources.ShaderManager.Shaders.Add(shaderObject);
        }

        shaderObject.Update(shader);
        shaderObject.Apply(this.D3D11Queue.DeviceContext);
        this.vertexShader = shader;
        this.vsOutputSignature = shaderObject.Compilation.OutputSignature;

        // recompile dependent shader stages
        SetGeometryShader(this.geometryShader);
        SetFragmentShader(this.fragmentShader);
    }

    public void SetGeometryShader(IShader shader)
    {
        if (shader is null)
        {
            this.geometryShader = null;
            return;
        }    

        this.GetType().GetMethod(nameof(SetGeometryShader), BindingFlags.Instance | BindingFlags.NonPublic, new[] { shader.GetType() }).MakeGenericMethod(new[] { shader.GetType() }).Invoke(this, new[] { shader });
    }

    private void SetGeometryShader<T>(IShader shader) where T : struct, IShader
    {
        ShaderSignature shaderSignature = vsOutputSignature;

        this.geometryShader = shader;

        // if theres no signature to compile against, wait until we have one
        // SetVertexShader() will call this method with the input signatures
        if (shaderSignature is null)
            return;

        var shaderObject = Resources.ShaderManager.Shaders.OfType<D3D11GeometryShader<T>>().SingleOrDefault(s => s.ShaderType == shader.GetType() && s.InputSignature == shaderSignature);

        if (shaderObject is null)
        {
            shaderObject = new D3D11GeometryShader<T>(this.Resources, shaderSignature);
            Resources.ShaderManager.Shaders.Add(shaderObject);
        }

        shaderObject.Update(shader);
        shaderObject.Apply(this.D3D11Queue.DeviceContext);

        gsOutputSignature = shaderObject.Compilation.OutputSignature;

        // recompile dependent shader stages
        SetFragmentShader(this.fragmentShader);
    }

    public void SetFragmentShader(IShader shader)
    {
        if (shader is null)
        {
            this.fragmentShader = null;
            return;
        }

        this.GetType().GetMethod(nameof(SetFragmentShader), BindingFlags.Instance | BindingFlags.NonPublic, new[] { shader.GetType() }).MakeGenericMethod(new[] { shader.GetType() }).Invoke(this, new[] { shader });
    }

    private void SetFragmentShader<T>(IShader shader) where T : struct, IShader
    {
        ShaderSignature shaderSignature = gsOutputSignature ?? vsOutputSignature;

        fragmentShader = shader;

        // if theres no signature to compile against, wait until we have one
        // SetGeometryShader() and SetVertexShader() will call this method with the input signatures
        if (shaderSignature is null)
            return;

        var shaderObject = Resources.ShaderManager.Shaders.OfType<D3D11FragmentShader<T>>().SingleOrDefault(s => s.ShaderType == shader.GetType() && s.InputSignature == shaderSignature);

        if (shaderObject is null)
        {
            shaderObject = new D3D11FragmentShader<T>(this.Resources, vsOutputSignature);
            Resources.ShaderManager.Shaders.Add(shaderObject);
        }

        shaderObject.Update(shader);
        shaderObject.Apply(this.D3D11Queue.DeviceContext);
    }

    public void ClearRenderTarget(Color color)
    {
        D3D11Queue.DeviceContext.ClearRenderTargetView(this.currentRenderTarget.RenderTargetView, new(color.ToVector4()));
    }

    public void ClearDepthTarget(float depth)
    {
        D3D11Queue.DeviceContext.ClearDepthStencilView(this.depthStencilManager.DepthStencilView, DepthStencilClearFlags.Depth, depth, 0);
    }

    public void ClearStencilTarget(byte stencil)
    {
        // DeviceContext.ClearDepthStencilView(this.depthStencilView, DepthStencilClearFlags.Depth, 0f, stencil);
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

    public void Submit(IGraphicsQueue deferredQueue)
    {
        throw new NotImplementedException();
    }

    public void SetVertexBuffer<T>(IBuffer<T> vertexBuffer) where T : unmanaged
    {
        if (vertexBuffer is not D3D11Buffer<T> d3dBuffer)
            throw new ArgumentException(null, nameof(vertexBuffer));

        D3D11Queue.DeviceContext.IASetVertexBuffer(0, d3dBuffer.GetInternalbuffer(BufferUsage.VertexBuffer), d3dBuffer.Stride);
    }

    public void SetInstanceBuffer<T>(IBuffer<T> instanceBuffer) where T : unmanaged
    {
        throw new NotImplementedException();
    }

    public void SetIndexBuffer(IBuffer<uint> indexBuffer)
    {
        if (indexBuffer is not D3D11Buffer<uint> d3dBuffer)
            throw new();

        D3D11Queue.DeviceContext.IASetIndexBuffer(d3dBuffer.GetInternalbuffer(BufferUsage.IndexBuffer), Vortice.DXGI.Format.R32_UInt, 0);
    }

    public void DrawPrimitives(PrimitiveKind kind, int vertexCount, int vertexOffset = 0)
    {
        PreDraw(kind);

        D3D11Queue.DeviceContext.Draw(vertexCount, vertexOffset);
    }

    public void DrawPrimitives(PrimitiveKind kind, int vertexCount, int instanceCount, int vertexOffset = 0, int instanceOffset = 0)
    {
        throw new NotImplementedException();
    }

    public void DrawPrimitives(PrimitiveKind kind, IBuffer<DrawCommand> commands, int commandOffset = 0)
    {
        throw new NotImplementedException();
    }

    public void DrawIndexedPrimitives(PrimitiveKind kind, int indexCount, int indexOffset = 0, int vertexOffset = 0)
    {
        PreDraw(kind);

        D3D11Queue.DeviceContext.DrawIndexed(indexCount, indexOffset, vertexOffset);
    }

    public void DrawIndexedPrimitives(PrimitiveKind kind, int indexCount, int instanceCount, int indexOffset = 0, int instanceOffset = 0)
    {
        throw new NotImplementedException();
    }

    public void DrawIndexedPrimitives(PrimitiveKind kind, IBuffer<IndexedDrawCommand> commands, int commandOffset = 0)
    {
        throw new NotImplementedException();
    }

    public void BlendState(BlendMode sourceBlend, BlendMode destinationBlend, BlendOperation operation = BlendOperation.Add)
    {
        BlendState(sourceBlend, destinationBlend, sourceBlend, destinationBlend, operation, operation);
    }

    public void BlendState(BlendMode sourceBlend, BlendMode destinationBlend, BlendMode sourceBlendAlpha, BlendMode destinationBlendAlpha, BlendOperation operation = BlendOperation.Add, BlendOperation operationAlpha = BlendOperation.Add)
    {
        var info = new BlendStateManager.BlendStateInfo(sourceBlend, destinationBlend, sourceBlendAlpha, destinationBlendAlpha, operation, operationAlpha);
        blendState = blendStateManager.GetBlendState(info);
    }
}