using SimulationFramework.Drawing.Direct3D11.Buffers;
using SimulationFramework.Drawing.Direct3D11.Shaders;
using SimulationFramework.Drawing.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;
using Vortice.Direct3D11.Debug;

namespace SimulationFramework.Drawing.Direct3D11;

internal sealed class D3D11Renderer : IRenderer
{
    public ID3D11DeviceContext DeviceContext { get; private set; }
    private D3D11Texture currentRenderTarget;
    private DeviceResources resources;

    public D3D11Renderer(DeviceResources resources, ID3D11DeviceContext deviceContext)
    {
        this.resources = resources;
        this.DeviceContext = deviceContext;
    }

    private ID3D11RasterizerState rs;
    public void PreDraw(PrimitiveKind primitiveKind)
    {
        DeviceContext.IASetPrimitiveTopology(primitiveKind.AsPrimitiveTopology());

        var vp = DeviceContext.RSGetViewport();

        if (vp.Width == 0 || vp.Height == 0)
        {
            DeviceContext.RSSetViewport(0, 0, this.currentRenderTarget.Width, this.currentRenderTarget.Height, 0, 1);
        }

        if (rs == null)
        {
            rs = resources.Device.CreateRasterizerState(RasterizerDescription.CullNone);
        }

        DeviceContext.RSSetState(rs);
    }

    public void DrawIndexedPrimitives(PrimitiveKind kind, int count, int vertexOffset, int indexOffset)
    {
        PreDraw(kind);

        DeviceContext.DrawIndexed(kind.GetVertexCount(count), indexOffset, vertexOffset);
    }

    public void DrawPrimitives(PrimitiveKind kind, int count, int offset)
    {
        PreDraw(kind);

        DeviceContext.Draw(kind.GetVertexCount(count), offset);
    }

    public void UseShader(IShader shader)
    {
        shader.Apply(this);
    }

    public void SetRenderTarget(ITexture renderTarget)
    {
        if (renderTarget is not D3D11Texture d3dTexture)
            throw new ArgumentException(null, nameof(renderTarget));

        currentRenderTarget = d3dTexture;
        DeviceContext.OMSetRenderTargets(d3dTexture.RenderTargetView);
    }

    public void Clear(Color color)
    {
        DeviceContext.ClearRenderTargetView(currentRenderTarget.RenderTargetView, new(color.ToVector4()));
    }

    public void BeginFrame()
    {
        SetViewport(new(0, 0, 0, 0));
    }

    public void VertexBuffer<T>(IBuffer<T> vertexBuffer) where T : unmanaged
    {
        if (vertexBuffer is not D3D11Buffer<T> d3dBuffer)
            throw new ArgumentException(null, nameof(vertexBuffer));

        DeviceContext.IASetVertexBuffer(0, d3dBuffer.GetInternalbuffer(BufferUsage.VertexBuffer), d3dBuffer.Stride);
    }

    public void IndexBuffer(IBuffer<int> indexBuffer)
    {
        throw new NotImplementedException();
    }

    public void SetViewport(Rectangle viewport)
    {
        DeviceContext.RSSetViewport(viewport.X, viewport.Y, viewport.Width, viewport.Height);
    }
}