using SimulationFramework.Shaders;
using SimulationFramework.Shaders.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace SimulationFramework.Drawing.Direct3D11;
internal class D3D11GeometryShader<T> : D3D11Shader<T> where T : struct, IShader
{
    protected override ShaderKind Kind => ShaderKind.Geometry;
    protected override string Profile => "gs_5_0";

    private ID3D11GeometryShader shader;

    public D3D11GeometryShader(DeviceResources deviceResources, ShaderSignature inputSignature) : base(deviceResources, inputSignature)
    {
    }

    protected override void CreateShader(Span<byte> bytecode)
    {
        shader = Resources.Device.CreateGeometryShader(bytecode);
    }

    public override void Apply(ID3D11DeviceContext context)
    {
        context.GSSetShader(shader);
        base.Apply(context);
    }

    public override void ApplyConstantBuffer(ID3D11DeviceContext context, ID3D11Buffer constantBuffer)
    {
        context.GSSetConstantBuffer(0, constantBuffer);
        base.ApplyConstantBuffer(context, constantBuffer);
    }

    public override void Dispose()
    {
        shader.Dispose();
        base.Dispose();
    }
}
