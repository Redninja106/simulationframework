using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace SimulationFramework.Drawing.Direct3D11.Shaders;
internal sealed class D3D11FragmentShader : D3D11ShaderBase<ID3D11PixelShader>
{
    public override ShaderKind Kind => ShaderKind.Fragment;

    public D3D11FragmentShader(DeviceResources deviceResources, string source) : base(deviceResources, source, "ps_5_0")
    {
    }

    private protected override ID3D11PixelShader CreateShader(byte[] bytecode)
    {
        return Resources.Device.CreatePixelShader(bytecode);
    }

    private protected override void Apply(ID3D11DeviceContext context, ID3D11PixelShader shader)
    {
        context.PSSetShader(shader);
    }

    private protected override void Apply(ID3D11DeviceContext context, int slot, ID3D11Buffer constantBuffer)
    {
        context.PSSetConstantBuffer(slot, constantBuffer);
    }

    private protected override void Apply(ID3D11DeviceContext context, int slot, ID3D11ShaderResourceView textureView, ID3D11SamplerState sampler)
    {
        context.PSSetSampler(slot, sampler);
        context.PSSetShaderResource(slot, textureView);
    }
}
