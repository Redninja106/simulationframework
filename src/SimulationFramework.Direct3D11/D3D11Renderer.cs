using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Direct3D11.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

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
            DeviceContext.RSSetViewport(0, 0, 1920, 1080);
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

    public void SetIndexBuffer(IBuffer<int> buffer)
    {
        throw new NotImplementedException();
    }

    public void SetShader(IShader shader)
    {
        if (shader is not D3D11Shader d3dShader)
            throw new ArgumentException(null, nameof(shader));

        switch (shader.Kind)
        {
            case ShaderKind.Vertex:
                DeviceContext.VSSetShader(d3dShader.InternalShader as ID3D11VertexShader, null, 0);
                DeviceContext.IASetInputLayout(d3dShader.InputLayout);
                DeviceContext.VSSetConstantBuffer(0, d3dShader.varConstBuffer);
                break;
            case ShaderKind.Fragment:
                DeviceContext.PSSetShader(d3dShader.InternalShader as ID3D11PixelShader, null, 0);
                DeviceContext.PSSetConstantBuffer(0, d3dShader.varConstBuffer);
                break;
            case ShaderKind.Compute:
            default:
                throw new NotImplementedException();
        }
    }

    public void SetVertexBuffer<T>(IBuffer<T> buffer) where T : unmanaged
    {
        if (buffer is not D3D11Buffer<T> d3dBuffer)
            throw new ArgumentException(null, nameof(buffer));

        DeviceContext.IASetVertexBuffer(0, d3dBuffer.GetInternalbuffer(BufferUsage.VertexBuffer), d3dBuffer.Stride);
    }

    public void SetRenderTarget(ITexture renderTarget)
    {
        if (renderTarget is not D3D11Texture d3dTexture)
            throw new ArgumentException(null, nameof(renderTarget));

        currentRenderTarget = d3dTexture;
        DeviceContext.OMSetRenderTargets(d3dTexture.GetRenderTargetView());
    }

    public void Clear(Color color)
    {
        DeviceContext.ClearRenderTargetView(currentRenderTarget.GetRenderTargetView(), new(color.ToVector4()));
    }
}