using SimulationFramework.Shaders;
using SimulationFramework.Shaders.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace SimulationFramework.Drawing.Direct3D11;
internal class VertexShader : D3D11Shader
{
    protected override ShaderKind Kind => ShaderKind.Vertex;
    protected override string Profile => "vs_5_0";

    private ID3D11VertexShader vertexShader;
    private ID3D11InputLayout inputLayout;
    
    internal byte[] bytecode;

    public VertexShader(DeviceResources deviceResources, Type shaderType) : base(deviceResources, shaderType, null)
    {
    }

    protected override void CreateShader(Span<byte> bytecode)
    {
        vertexShader = Resources.Device.CreateVertexShader(bytecode);

        this.bytecode = bytecode.ToArray();
    }


    public override void ApplyConstantBuffer(ID3D11DeviceContext context, ID3D11Buffer constantBuffer)
    {
        context.VSSetConstantBuffer(0, constantBuffer);
    }

    public override void ApplyShader(ID3D11DeviceContext context)
    {
        if (inputLayout is not null)
            context.IASetInputLayout(this.inputLayout);

        context.VSSetShader(vertexShader);
    }

    public override void Dispose()
    {
        vertexShader.Dispose();
        inputLayout?.Dispose();
        base.Dispose();
    }

    public override void ApplySamplerState(ID3D11DeviceContext context, ID3D11SamplerState samplerState, int slot)
    {
        context.VSSetSampler(slot, samplerState);
    }

    public override void ApplyShaderResourceView(ID3D11DeviceContext context, ID3D11ShaderResourceView shaderResourceView, int slot)
    {
        context.VSSetShaderResource(slot, shaderResourceView);
    }
}
