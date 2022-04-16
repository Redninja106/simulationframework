using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.D3DCompiler;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.Direct3D11.Shader;
using Vortice.DXGI;

namespace SimulationFramework.Drawing.Direct3D11.Shaders;

internal class D3D11VertexShader : D3D11ShaderBase<ID3D11VertexShader>
{
    public override ShaderKind Kind => ShaderKind.Vertex;
    
    private ID3D11InputLayout inputLayout;

    public D3D11VertexShader(DeviceResources deviceResources, string source) : base(deviceResources, source, "vs_5_0")
    {
    }

    private protected override ID3D11VertexShader CreateShader(byte[] bytecode)
    {
        this.inputLayout = ReflectInputLayout(bytecode);
        return Resources.Device.CreateVertexShader(bytecode);
    }

    private ID3D11InputLayout ReflectInputLayout(byte[] bytecode)
    {
        using var reflection = Compiler.Reflect<ID3D11ShaderReflection>(bytecode);

        var inputElements = new InputElementDescription[reflection.InputParameters.Length];
        for (int i = 0; i < inputElements.Length; i++)
        {
            inputElements[i] = new(
                reflection.InputParameters[i].SemanticName,
                reflection.InputParameters[i].SemanticIndex,
                GetFormat(reflection.InputParameters[i]),
                InputElementDescription.AppendAligned,
                0);
        }

        return Resources.Device.CreateInputLayout(inputElements, bytecode);

        static Format GetFormat(ShaderParameterDescription desc)
        {
            // https://gist.github.com/mobius/b678970c61a93c81fffef1936734909f
            if ((int)desc.UsageMask == 1)
            {
                if (desc.ComponentType == RegisterComponentType.UInt32) return Format.R32_UInt;
                if (desc.ComponentType == RegisterComponentType.SInt32) return Format.R32_SInt;
                if (desc.ComponentType == RegisterComponentType.Float32) return Format.R32_Float;
            }
            if ((int)desc.UsageMask <= 3)
            {
                if (desc.ComponentType == RegisterComponentType.UInt32) return Format.R32G32_UInt;
                if (desc.ComponentType == RegisterComponentType.SInt32) return Format.R32G32_SInt;
                if (desc.ComponentType == RegisterComponentType.Float32) return Format.R32G32_Float;
            }
            if ((int)desc.UsageMask <= 7)
            {
                if (desc.ComponentType == RegisterComponentType.UInt32) return Format.R32G32B32_UInt;
                if (desc.ComponentType == RegisterComponentType.SInt32) return Format.R32G32B32_SInt;
                if (desc.ComponentType == RegisterComponentType.Float32) return Format.R32G32B32_Float;
            }
            if ((int)desc.UsageMask <= 15)
            {
                if (desc.ComponentType == RegisterComponentType.UInt32) return Format.R32G32B32A32_UInt;
                if (desc.ComponentType == RegisterComponentType.SInt32) return Format.R32G32B32A32_SInt;
                if (desc.ComponentType == RegisterComponentType.Float32) return Format.R32G32B32A32_Float;
            }
            return Format.Unknown;
        }
    }
    
    public override void Dispose()
    {
        inputLayout?.Dispose();
        base.Dispose();
    }

    private protected override void Apply(ID3D11DeviceContext context, ID3D11VertexShader shader)
    {
        context.VSSetShader(shader);
        context.IASetInputLayout(this.inputLayout);
    }

    private protected override void Apply(ID3D11DeviceContext context, int slot, ID3D11Buffer constantBuffer)
    {
        context.VSSetConstantBuffer(slot, constantBuffer);
    }

    private protected override void Apply(ID3D11DeviceContext context, int slot, ID3D11ShaderResourceView textureView, ID3D11SamplerState sampler)
    {
        context.VSSetShaderResource(slot, textureView);
        context.VSSetSampler(slot, sampler);
    }
}
