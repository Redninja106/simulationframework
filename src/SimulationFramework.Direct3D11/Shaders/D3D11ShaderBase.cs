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

namespace SimulationFramework.Drawing.Direct3D11.Shaders;

// Contains common functionality for shader types.
internal abstract class D3D11ShaderBase<T> : D3D11Object, IShader where T : ID3D11DeviceChild
{
    public abstract ShaderKind Kind { get; }

    private T internalShader;
    private byte[] bytecode;

    private ID3D11SamplerState[] samplers;
    private ID3D11ShaderResourceView[] textureViews;
    private ITexture[] textures;

    private Dictionary<string, int> textureLookup = new();

    // constant buffer for file-level shader variables
    private ID3D11Buffer variableConstBuffer;

    // dictionary which maps variable names to their constant buffer location
    private Dictionary<string, (int position, int size)> varLookup = new();

    // actual variabel constant buffer data
    private byte[] variableConstBufferData;

    private protected abstract T CreateShader(byte[] bytecode);

    private protected abstract void Apply(ID3D11DeviceContext context, T shader);
    private protected abstract void Apply(ID3D11DeviceContext context, int slot, ID3D11Buffer constantBuffer);
    private protected abstract void Apply(ID3D11DeviceContext context, int slot, ID3D11ShaderResourceView textureView, ID3D11SamplerState sampler);

    public D3D11ShaderBase(DeviceResources deviceResources, string source, string shaderModel) : base(deviceResources)
    {
        this.bytecode = CompileShader(source, shaderModel, out variableConstBuffer);
        this.internalShader = CreateShader(bytecode);
    }

    private byte[] CompileShader(string source, string shaderModel, out ID3D11Buffer constBuffer)
    {
        using var blob = Compiler.Compile(source, "main", null, shaderModel);
        var bytes = blob.AsBytes();

        Compiler.Reflect(bytes, out ID3D11ShaderReflection reflection);

        int varBufferLength = 0;

        if (reflection.ConstantBuffers.Any())
        {
            var cbuffer = reflection.ConstantBuffers[0];

            for (int i = 0; i < cbuffer.Description.VariableCount; i++)
            {
                var variable = cbuffer.GetVariableByIndex(i);
                varBufferLength += variable.Description.Size;
                varLookup.Add(variable.Description.Name, (variable.Description.StartOffset, variable.Description.Size));
            }

            this.variableConstBufferData = new byte[varBufferLength];

            BufferDescription cbufferDesc = new()
            {
                SizeInBytes = PadTo16(varBufferLength),
                BindFlags = BindFlags.ConstantBuffer,
            };

            constBuffer = Resources.Device.CreateBuffer(cbufferDesc);
        }
        else
        {
            constBuffer = null;
        }

        for (int i = 0; i < reflection.Resources.Length; i++)
        {
            ref var resource = ref reflection.Resources[i];
            
            if (resource.Type != ShaderInputType.Texture)
                continue;

            if (resource.Dimension != ShaderResourceViewDimension.Texture2D)
                continue;

            var sampler = reflection.GetResourceBindingDescByName(resource.Name + "_sampler");
            
            textureLookup.Add(resource.Name, resource.BindPoint);
        }

        this.textures = new ITexture[textureLookup.Count];
        this.samplers = new ID3D11SamplerState[textureLookup.Count];
        this.textureViews = new ID3D11ShaderResourceView[textureLookup.Count];

        return bytes;
    }

    // pad a value to the next multiple of 16
    private int PadTo16(int value)
    {
        return (value + 15) & ~15;
    }

    public override void Dispose()
    {
        internalShader.Dispose();
        base.Dispose();
    }

    public void SetBuffer<T>(string name, IBuffer<T> buffer) where T : unmanaged
    {

    }

    public void SetTexture(string name, ITexture texture, TileMode tileMode)
    {
        if (!textureLookup.TryGetValue(name, out int slot))
            throw new ArgumentException($"Texture {name} not found in shader");

        if (texture is not D3D11Texture)
            throw new ArgumentException($"Texture {name} is not a D3D11 Texture!");

        this.textures[slot] = texture;
        this.samplers[slot] = UpdateSampler(this.samplers[slot], tileMode);
        this.textureViews[slot] = (texture as D3D11Texture).ShaderResourceView;
    }

    private ID3D11SamplerState UpdateSampler(ID3D11SamplerState samplerState, TileMode tileMode)
    {
        if (samplerState.Description.AddressU == TileModeToAddressMode(tileMode))
        {
            // the existing state is good!
            return samplerState;
        }

        samplerState?.Dispose();

        var desc = new SamplerDescription()
        {
            AddressU = TileModeToAddressMode(tileMode),
            AddressV = TileModeToAddressMode(tileMode),
            AddressW = TileModeToAddressMode(tileMode),
            Filter = Filter.MinMagMipLinear,
        };

        return Resources.Device.CreateSamplerState(desc);
    }

    private TextureAddressMode TileModeToAddressMode(TileMode tileMode)
    {
        switch (tileMode)
        {
            case TileMode.Clamp:
                return TextureAddressMode.Clamp;
            case TileMode.Mirror:
                return TextureAddressMode.Mirror;
            case TileMode.Repeat:
                return TextureAddressMode.Wrap;
            case TileMode.Stop:
                return TextureAddressMode.Border;
            default:
                throw new ArgumentException($"Unknown tile mode {tileMode}");
        }
    }

    public unsafe void SetVariable<T>(string name, T value) where T : unmanaged
    {
        if (!varLookup.ContainsKey(name))
            throw new ArgumentException(null, nameof(name));

        var element = varLookup[name];

        if (element.size != sizeof(T))
            throw new ArgumentException(null, nameof(T));

        fixed (byte* ptr = &variableConstBufferData[element.position])
        {
            *(T*)ptr = value;
        }

        Resources.ImmediateRenderer.DeviceContext.UpdateSubresource(this.variableConstBufferData, this.variableConstBuffer);
    }

    public void Apply(IRenderer renderer)
    {
        if (renderer is not D3D11Renderer d3dRenderer)
            throw new Exception();
        this.Apply(d3dRenderer.DeviceContext, this.internalShader);
        this.Apply(d3dRenderer.DeviceContext, 0, this.variableConstBuffer);
        for (int i = 0; i < textures.Length; i++)
        {
            if (textures[i] is not null)
                this.Apply(d3dRenderer.DeviceContext, i, (this.textures[i] as D3D11Texture).ShaderResourceView, this.samplers[i]);
        }
    }

    public void GetVariable<T>(string name, out T value) where T : unmanaged
    {
        throw new NotImplementedException();
    }

    public T1 GetVariable<T1>(string name) where T1 : unmanaged
    {
        throw new NotImplementedException();
    }

    public ITexture GetTexture(string name)
    {
        throw new NotImplementedException();
    }
}
