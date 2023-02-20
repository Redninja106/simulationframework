﻿using SimulationFramework.Shaders;
using SimulationFramework.Shaders.Compiler;
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

    public D3D11FragmentShader(DeviceResources deviceResources, ShaderSignature signature) : base(deviceResources, signature)
    {
    }

    protected override void CreateShader(Span<byte> bytecode)
    {
        shader = Resources.Device.CreatePixelShader(bytecode);
    }

    public override void ApplyShader(ID3D11DeviceContext context)
    {
        context.PSSetShader(shader);
    }

    public override void ApplyConstantBuffer(ID3D11DeviceContext context, ID3D11Buffer constantBuffer)
    {
        context.PSSetConstantBuffer(0, constantBuffer);
    }

    public override void Dispose()
    {
        shader.Dispose();
        base.Dispose();
    }
}