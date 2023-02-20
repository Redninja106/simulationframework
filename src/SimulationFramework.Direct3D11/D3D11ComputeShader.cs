﻿using SimulationFramework.Shaders;
using SimulationFramework.Shaders.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace SimulationFramework.Drawing.Direct3D11;
internal class D3D11ComputeShader<TShader> : D3D11Shader<TShader> where TShader : struct, IShader
{
    private ID3D11ComputeShader shader;
    private ID3D11UnorderedAccessView[] uavs;

    public D3D11ComputeShader(DeviceResources deviceResources, ShaderSignature inputSignature) : base(deviceResources, inputSignature)
    {
    }

    protected override ShaderKind Kind => ShaderKind.Compute;
    protected override string Profile => "cs_5_0";

    protected override void CreateShader(Span<byte> bytecode)
    {
        shader = Resources.Device.CreateComputeShader(bytecode);
    }

    public override void ApplyShader(ID3D11DeviceContext context)
    {
        context.CSSetShader(this.shader);

        for (int i = 0; i < uavs.Length; i++)
        {
            var uav = uavs[i];
            context.CSSetUnorderedAccessView(i, uav);
        }
    }

    public override void ApplyConstantBuffer(ID3D11DeviceContext context, ID3D11Buffer constantBuffer)
    {
        context.CSSetConstantBuffer(0, constantBuffer);
    }

    public override void Update(IShader shader)
    {
        base.Update(shader);
        UpdateUAVs(shader);
    }

    void UpdateUAVs(IShader shader)
    {
        uavs ??= new ID3D11UnorderedAccessView[Compilation.IntrinsicUniforms.Count()];

        for (int i = 0; i < uavs.Length; i++)
        {
            var uniform = Compilation.IntrinsicUniforms.ElementAt(i);
            
            var value = uniform.BackingField.GetValue(shader);

            if (value is null)
                continue;

            if (value is IUnorderedAccessViewProvider uavProvider)
            {
                uavs[i] = uavProvider.GetUnorderedAccessView();
            }
            else
            {
                throw new Exception();
            }
        }
    }
}