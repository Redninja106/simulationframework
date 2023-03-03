using SimulationFramework.Shaders;
using SimulationFramework.Shaders.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace SimulationFramework.Drawing.Direct3D11;
internal class D3D11GeometryShader : D3D11Shader
{
    protected override ShaderKind Kind => ShaderKind.Geometry;
    protected override string Profile => "gs_5_0";

    private ID3D11GeometryShader shader;

    public D3D11GeometryShader(DeviceResources deviceResources, Type shaderType, ShaderSignature inputSignature) : base(deviceResources, shaderType, inputSignature)
    {
    }

    protected override void CreateShader(Span<byte> bytecode)
    {
        shader = Resources.Device.CreateGeometryShader(bytecode);
    }

    public override void ApplyShader(ID3D11DeviceContext context)
    {
        context.GSSetShader(shader);
    }

    public override void ApplyConstantBuffer(ID3D11DeviceContext context, ID3D11Buffer constantBuffer)
    {
        context.GSSetConstantBuffer(0, constantBuffer);
    }

    public override void Dispose()
    {
        shader.Dispose();
        base.Dispose();
    }

    public override void ApplySamplerState(ID3D11DeviceContext context, ID3D11SamplerState samplerState, int slot)
    {
        context.GSSetSampler(slot, samplerState);
    }

    public override void ApplyShaderResourceView(ID3D11DeviceContext context, ID3D11ShaderResourceView shaderResourceView, int slot)
    {
        context.GSSetShaderResource(slot, shaderResourceView);
    }
}
