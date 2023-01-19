using SimulationFramework.Drawing.Direct3D11.Buffers;
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

    public D3D11QueueBase CurrentQueue { get; private set; }

    public IGraphicsQueue Queue { get => CurrentQueue; set => CurrentQueue = value as D3D11QueueBase; }

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

    private D3D11Texture<Color> currentRenderTarget;
    private D3D11Texture<float> currentDepthTarget;
    private Dictionary<(CullMode cullMode, bool wireframe), ID3D11RasterizerState> rasterizerStates = new();

    private ShaderSignature vsOutputSignature;
    private ShaderSignature gsOutputSignature;

    private IShader vertexShader;
    private IShader geometryShader;
    private IShader fragmentShader;

    private int vertexBufferOffset, indexBufferOffset, instanceBufferOffset;

    private CullMode cullMode;
    private bool wireframe;

    private readonly DepthStencilManager depthStencilManager;

    public D3D11Renderer(DeviceResources resources, D3D11QueueBase queue) : base(resources)
    {
        this.CurrentQueue = queue;
        depthStencilManager = new(resources);
    }

    public void PreDraw(PrimitiveKind primitiveKind)
    {
        CurrentQueue.DeviceContext.IASetPrimitiveTopology(primitiveKind.AsPrimitiveTopology());

        var vp = CurrentQueue.DeviceContext.RSGetViewport();

        if (vp.Width == 0 || vp.Height == 0)
        {
            CurrentQueue.DeviceContext.RSSetViewport(0, 0, this.currentRenderTarget.Width, this.currentRenderTarget.Height, 0, 1);
        }

        if (rasterizerStates.Count is 0)
        {
            rasterizerStates[(CullMode.None, true)] = Resources.Device.CreateRasterizerState(RasterizerDescription.CullNone with { FillMode = FillMode.Wireframe });
            rasterizerStates[(CullMode.None, false)] = Resources.Device.CreateRasterizerState(RasterizerDescription.CullNone with { FillMode = FillMode.Solid });
            rasterizerStates[(CullMode.Front, true)] = Resources.Device.CreateRasterizerState(RasterizerDescription.CullFront with { FillMode = FillMode.Wireframe });
            rasterizerStates[(CullMode.Front, false)] = Resources.Device.CreateRasterizerState(RasterizerDescription.CullFront with { FillMode = FillMode.Solid });
            rasterizerStates[(CullMode.Back, true)] = Resources.Device.CreateRasterizerState(RasterizerDescription.CullBack with { FillMode = FillMode.Wireframe });
            rasterizerStates[(CullMode.Back, false)] = Resources.Device.CreateRasterizerState(RasterizerDescription.CullBack with {  FillMode = FillMode.Solid });
        }

        CurrentQueue.DeviceContext.RSSetState(rasterizerStates[(this.CullMode, this.wireframe)]);

        depthStencilManager.PreDraw(this.CurrentQueue.DeviceContext);
    }

    public void DrawPrimitivesIndexed(PrimitiveKind kind, int count)
    {
        PreDraw(kind);

        CurrentQueue.DeviceContext.DrawIndexed(kind.GetVertexCount(count), this.indexBufferOffset, this.vertexBufferOffset);
    }

    public void DrawPrimitives(PrimitiveKind kind, int count)
    {
        PreDraw(kind);

        CurrentQueue.DeviceContext.Draw(kind.GetVertexCount(count), this.vertexBufferOffset);
    }

    private void SetRenderTarget(ITexture<Color> renderTarget)
    {
        if (renderTarget is not D3D11Texture<Color> d3dTexture)
            throw new ArgumentException(null, nameof(renderTarget));

        currentRenderTarget = d3dTexture;
        CurrentQueue.DeviceContext.OMSetRenderTargets(d3dTexture.RenderTargetView, depthStencilManager.DepthStencilView);
    }

    public void BeginFrame()
    {
        SetViewport(new(0, 0, 0, 0));
    }

    public void SetVertexBuffer<T>(IBuffer<T> vertexBuffer, int offset = 0) where T : unmanaged
    {
        if (vertexBuffer is not D3D11Buffer<T> d3dBuffer)
            throw new ArgumentException(null, nameof(vertexBuffer));

        CurrentQueue.DeviceContext.IASetVertexBuffer(0, d3dBuffer.GetInternalbuffer(BufferUsage.VertexBuffer), d3dBuffer.Stride);
    }

    public void SetViewport(Rectangle viewport)
    {
        CurrentQueue.DeviceContext.RSSetViewport(viewport.X, viewport.Y, viewport.Width, viewport.Height, 0, 1);
    }

    public override void Dispose()
    {
        foreach (var (key, value) in rasterizerStates)
        {
            value.Dispose();
        }
        this.CurrentQueue.DeviceContext.Dispose();
        this.depthStencilManager.Dispose();
        base.Dispose();
    }

    public void SetIndexBuffer(IBuffer<uint> indexBuffer, int offset)
    {
        if (indexBuffer is not D3D11Buffer<uint> d3dBuffer)
            throw new();

        this.indexBufferOffset = offset;

        CurrentQueue.DeviceContext.IASetIndexBuffer(d3dBuffer.GetInternalbuffer(BufferUsage.IndexBuffer), Vortice.DXGI.Format.R32_UInt, 0);
    }

    public void Clip(Rectangle? rectangle)
    {
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
        var shaderObject = Resources.Shaders.OfType<D3D11VertexShader<T>>().SingleOrDefault(s => s.ShaderType == shader.GetType());

        if (shaderObject is null)
        {
            shaderObject = new D3D11VertexShader<T>(this.Resources);
            Resources.Shaders.Add(shaderObject);
        }

        shaderObject.Update(shader);
        shaderObject.Apply(this.CurrentQueue.DeviceContext);
        this.vertexShader = shader;

        vsOutputSignature = shaderObject.Compilation.OutputSignature;

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

        var shaderObject = Resources.Shaders.OfType<D3D11GeometryShader<T>>().SingleOrDefault(s => s.ShaderType == shader.GetType() && s.InputSignature == shaderSignature);

        if (shaderObject is null)
        {
            shaderObject = new D3D11GeometryShader<T>(this.Resources, shaderSignature);
            Resources.Shaders.Add(shaderObject);
        }

        shaderObject.Update(shader);
        shaderObject.Apply(this.CurrentQueue.DeviceContext);

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

        var shaderObject = Resources.Shaders.OfType<D3D11FragmentShader<T>>().SingleOrDefault(s => s.ShaderType == shader.GetType() && s.InputSignature == shaderSignature);

        if (shaderObject is null)
        {
            shaderObject = new D3D11FragmentShader<T>(this.Resources, vsOutputSignature);
            Resources.Shaders.Add(shaderObject);
        }

        shaderObject.Update(shader);
        shaderObject.Apply(this.CurrentQueue.DeviceContext);
    }

    public void ClearRenderTarget(Color color)
    {
        CurrentQueue.DeviceContext.ClearRenderTargetView(this.currentRenderTarget.RenderTargetView, new(color.ToVector4()));
    }

    public void ClearDepthTarget(float depth)
    {
        CurrentQueue.DeviceContext.ClearDepthStencilView(this.depthStencilManager.DepthStencilView, DepthStencilClearFlags.Depth, depth, 0);
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
}