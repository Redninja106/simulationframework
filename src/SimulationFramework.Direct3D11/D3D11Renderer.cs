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

    public ID3D11DeviceContext DeviceContext { get; private set; }
    public ITexture<float> DepthTarget { get; set; }
    public ITexture<byte> StencilTarget { get; set; }

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
            return false;
        }
        set
        {

        }
    }

    public float DepthBias { get; set; }

    private D3D11Texture<Color> currentRenderTarget;
    private Dictionary<CullMode, ID3D11RasterizerState> rasterizerStates = new();

    private ShaderSignature vsOutputSignature;
    private ShaderSignature gsOutputSignature;

    private IShader vertexShader;
    private IShader geometryShader;
    private IShader fragmentShader;

    private CullMode cullMode;

    public D3D11Renderer(DeviceResources resources, ID3D11DeviceContext deviceContext) : base(resources)
    {
        this.DeviceContext = deviceContext;
    }

    public void PreDraw(PrimitiveKind primitiveKind)
    {
        DeviceContext.IASetPrimitiveTopology(primitiveKind.AsPrimitiveTopology());

        var vp = DeviceContext.RSGetViewport();

        if (vp.Width == 0 || vp.Height == 0)
        {
            DeviceContext.RSSetViewport(0, 0, this.currentRenderTarget.Width, this.currentRenderTarget.Height, 0, 1);
        }

        if (rasterizerStates.Count is 0)
        {
            rasterizerStates[CullMode.None] = Resources.Device.CreateRasterizerState(RasterizerDescription.CullNone);
            rasterizerStates[CullMode.Front] = Resources.Device.CreateRasterizerState(RasterizerDescription.CullFront);
            rasterizerStates[CullMode.Back] = Resources.Device.CreateRasterizerState(RasterizerDescription.CullBack);
        }

        DeviceContext.RSSetState(rasterizerStates[this.CullMode]);
    }

    public void DrawPrimitivesIndexed(PrimitiveKind kind, int count, int vertexOffset, int indexOffset)
    {
        PreDraw(kind);

        DeviceContext.DrawIndexed(kind.GetVertexCount(count), indexOffset, vertexOffset);
    }

    public void DrawPrimitives(PrimitiveKind kind, int count, int offset)
    {
        PreDraw(kind);

        DeviceContext.Draw(kind.GetVertexCount(count), offset);
    }

    private void SetRenderTarget(ITexture<Color> renderTarget)
    {
        if (renderTarget is not D3D11Texture<Color> d3dTexture)
            throw new ArgumentException(null, nameof(renderTarget));

        currentRenderTarget = d3dTexture;
        DeviceContext.OMSetRenderTargets(d3dTexture.RenderTargetView);
    }

    public void BeginFrame()
    {
        SetViewport(new(0, 0, 0, 0));
    }

    public void SetVertexBuffer<T>(IBuffer<T> vertexBuffer) where T : unmanaged
    {
        if (vertexBuffer is not D3D11Buffer<T> d3dBuffer)
            throw new ArgumentException(null, nameof(vertexBuffer));

        DeviceContext.IASetVertexBuffer(0, d3dBuffer.GetInternalbuffer(BufferUsage.VertexBuffer), d3dBuffer.Stride);
    }

    public void SetViewport(Rectangle viewport)
    {
        DeviceContext.RSSetViewport(viewport.X, viewport.Y, viewport.Width, viewport.Height, 0, 1);
    }

    public override void Dispose()
    {
        this.currentRenderTarget?.Dispose();
        foreach (var (key, value) in rasterizerStates)
        {
            value.Dispose();
        }
        this.DeviceContext.Dispose();
        base.Dispose();
    }

    public void SetIndexBuffer(IBuffer<uint> indexBuffer)
    {
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

    public void SetInstanceBuffer<T>(IBuffer<T> instanceBuffer) where T : unmanaged
    {
        throw new NotImplementedException();
    }

    public void DrawPrimitivesInstanced(PrimitiveKind kind, int count, int instanceCount, int vertexOffset = 0, int instanceOffset = 0)
    {
        throw new NotImplementedException();
    }

    public void DrawPrimitivesIndexedInstanced(PrimitiveKind kind, int count, int instanceCount, int vertexOffset = 0, int indexOffset = 0, int instanceOffset = 0)
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
        shaderObject.Apply(this.DeviceContext);
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
        shaderObject.Apply(this.DeviceContext);

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
        shaderObject.Apply(this.DeviceContext);
    }

    public void ClearRenderTarget(Color color)
    {
        DeviceContext.ClearRenderTargetView(this.currentRenderTarget.RenderTargetView, new(color.ToVector4()));
    }

    public void ClearDepthTarget(float depth)
    {
        // DeviceContext.ClearDepthStencilView(this.depthStencilView, DepthStencilClearFlags.Depth, depth, 0);
        throw new NotImplementedException();
    }

    public void ClearStencilTarget(byte stencil)
    {
        // DeviceContext.ClearDepthStencilView(this.depthStencilView, DepthStencilClearFlags.Depth, 0f, stencil);
        throw new NotImplementedException();
    }
}