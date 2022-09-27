using SimulationFramework.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace SimulationFramework.Drawing.Direct3D11;
internal class D3D11FragmentShader<T> : D3D11Shader<T> where T : struct, IShader
{
    private ID3D11PixelShader shader;

    protected override ShaderKind Kind => ShaderKind.Fragment;
    protected override string Profile => "ps_5_0";

    public D3D11FragmentShader(DeviceResources deviceResources) : base(deviceResources)
    {
    }

    protected override void CreateShader(Span<byte> bytecode)
    {
        shader = Resources.Device.CreatePixelShader(bytecode);
    }

    public override void Apply(ID3D11DeviceContext context)
    {
        context.PSSetShader(shader);
        base.Apply(context);
    }

    public override void ApplyConstantBuffer(ID3D11DeviceContext context, ID3D11Buffer constantBuffer)
    {
        context.PSSetConstantBuffer(0, constantBuffer);
        base.ApplyConstantBuffer(context, constantBuffer);
    }

    public override void Dispose()
    {
        shader.Dispose();
        base.Dispose();
    }
}
