using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Vortice.D3DCompiler;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.Direct3D11.Shader;
using Vortice.DXGI;

namespace SimulationFramework.Drawing.Direct3D11;

internal class D3D11Shader : IShader
{
    public ID3D11DeviceChild InternalShader { get; }
    public ShaderKind Kind { get; }
    // null except when Kind == ShaderKind.Vertex
    public ID3D11InputLayout InputLayout { get; }
    private DeviceResources resources;
    public ID3D11Buffer varConstBuffer;
    private Dictionary<string, (int position, int size)> varLookup;
    private byte[] varConstBufferValues;

    public D3D11Shader(DeviceResources resources, ShaderKind kind, string source)
    {
        Kind = kind;
        this.resources = resources;
        this.InternalShader = CreateShader(kind, source, out var bytecode, out varConstBuffer);

        if (kind == ShaderKind.Vertex)
        {
            InputLayout = ReflectInputLayout(bytecode);
        }
    }

    private ID3D11InputLayout ReflectInputLayout(byte[] bytecode)
    {
        using var reflection = Compiler.Reflect<ID3D11ShaderReflection>(bytecode);

        var inputElements = new InputElementDescription[reflection.InputParameters.Count()];
        for (int i = 0; i < inputElements.Length; i++)
        {
            inputElements[i] = new(
                reflection.InputParameters[i].SemanticName,
                reflection.InputParameters[i].SemanticIndex,
                GetFormat(reflection.InputParameters[i]),
                InputElementDescription.AppendAligned,
                0);
        }
        return resources.Device.CreateInputLayout(inputElements, bytecode);

        static Format GetFormat(ShaderParameterDescription desc)
        {
            // https://gist.github.com/mobius/b678970c61a93c81fffef1936734909f
            if ((int)desc.UsageMask == 1)
            {
                if (desc.ComponentType == RegisterComponentType.UInt32)  return Format.R32_UInt;
                if (desc.ComponentType == RegisterComponentType.SInt32)  return Format.R32_SInt;
                if (desc.ComponentType == RegisterComponentType.Float32) return Format.R32_Float;
            }
            if ((int)desc.UsageMask <= 3)
            {
                if (desc.ComponentType == RegisterComponentType.UInt32)  return Format.R32G32_UInt;
                if (desc.ComponentType == RegisterComponentType.SInt32)  return Format.R32G32_SInt;
                if (desc.ComponentType == RegisterComponentType.Float32) return Format.R32G32_Float;
            }
            if ((int)desc.UsageMask <= 7)
            {
                if (desc.ComponentType == RegisterComponentType.UInt32)  return Format.R32G32B32_UInt;
                if (desc.ComponentType == RegisterComponentType.SInt32)  return Format.R32G32B32_SInt;
                if (desc.ComponentType == RegisterComponentType.Float32) return Format.R32G32B32_Float;
            }
            if ((int)desc.UsageMask <= 15)
            {
                if (desc.ComponentType == RegisterComponentType.UInt32) return Format.R32G32B32A32_UInt;
                if (desc.ComponentType == RegisterComponentType.SInt32) return Format.R32G32B32A32_SInt;
                if (desc.ComponentType == RegisterComponentType.Float32)return Format.R32G32B32A32_Float;
            }
            return Format.Unknown;
        }
    }

    private ID3D11DeviceChild CreateShader(ShaderKind kind, string source, out byte[] bytecode, out ID3D11Buffer constBuffer)
    {
        switch (kind)
        {
            case ShaderKind.Vertex:
                return resources.Device.CreateVertexShader(bytecode = CompileShader(source, "vs_5_0", out constBuffer));
            case ShaderKind.Fragment:
                return resources.Device.CreatePixelShader(bytecode = CompileShader(source, "ps_5_0", out constBuffer));
            case ShaderKind.Compute:
            default:
                throw new ArgumentException("Invalid shader kind", nameof(kind));
        }

    }

    private byte[] CompileShader(string source, string version, out ID3D11Buffer constBuffer)
    {
        using var blob = Compiler.Compile(source, "main", null, version);
        var bytes = blob.AsBytes();

        Compiler.Reflect(bytes, out ID3D11ShaderReflection reflection);

        int varBufferLength = 0;

        var cbuffer = reflection.ConstantBuffers[0];

        for (int i = 0; i < cbuffer.Description.VariableCount; i++)
        {
            var variable = cbuffer.GetVariableByIndex(i);
            varBufferLength += variable.Description.Size;
            varLookup.Add(variable.Description.Name, (variable.Description.StartOffset, variable.Description.Size));
        }

        this.varConstBufferValues = new byte[varBufferLength];

        BufferDescription cbufferDesc = new()
        {
            SizeInBytes = varBufferLength,
            StructureByteStride = 1,
            BindFlags = BindFlags.ConstantBuffer | BindFlags.ShaderResource,
        };

        constBuffer = resources.Device.CreateBuffer(cbufferDesc);

        return bytes;
    }

    public void Dispose()
    {
        InternalShader.Dispose();
        InputLayout?.Dispose();
    }

    public void SetBuffer<T>(string name, IBuffer<T> buffer) where T : unmanaged
    {
        throw new NotImplementedException();
    }

    public void SetTexture(string name, ITexture texture, TileMode tileMode)
    {
        throw new NotImplementedException();
    }

    public unsafe void SetVariable<T>(string name, T value) where T : unmanaged
    {
        if (!varLookup.ContainsKey(name))
            throw new ArgumentException(null, nameof(name));

        var element = varLookup[name];

        if (element.size != sizeof(T))
            throw new ArgumentException(null, nameof(T));

        Unsafe.Copy(ref this.varConstBufferValues[element.position], &value);

        resources.ImmediateRenderer.DeviceContext.UpdateSubresource(this.varConstBufferValues, this.varConstBuffer);
    }
}
