using SimulationFramework.Drawing.Direct3D11.Buffers;
using SimulationFramework.Drawing.Direct3D11.ShaderGen;
using SimulationFramework.Shaders;
using SimulationFramework.Shaders.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Vortice.D3DCompiler;
using Vortice.Direct3D11;

namespace SimulationFramework.Drawing.Direct3D11;
internal abstract class D3D11Shader<T> : D3D11Object where T : struct, IShader
{
    private static ShaderCompiler compiler = new();

    public ShaderCompilation Compilation { get; }
    public ShaderSignature InputSignature { get; }
    public Type ShaderType { get; }

    protected abstract void CreateShader(Span<byte> bytecode);
    protected abstract ShaderKind Kind { get; }
    protected abstract string Profile { get; }

    private byte[] cbufferData;
    private ID3D11Buffer cbuffer;
    private ID3D11ShaderResourceView[] resourceViews;
    private ID3D11SamplerState[] samplerStates;

    protected D3D11Shader(DeviceResources deviceResources, ShaderSignature inputSignature) : base(deviceResources)
    {
        this.ShaderType = typeof(T);
        this.InputSignature = inputSignature;

        Compilation = compiler.Compile(ShaderType, inputSignature, Kind);

        var generator = new HLSLCodeGenerator(Compilation);

        var sourceWriter = new StringWriter();
        generator.Emit(sourceWriter);
        var source = sourceWriter.GetStringBuilder().ToString();
        Console.WriteLine(source);
        Console.WriteLine(new string('=', 100));

        try
        {
            using var blob = Compiler.Compile(source, nameof(IShader.Main), ShaderType.Name, this.Profile, ShaderFlags.PackMatrixRowMajor | ShaderFlags.SkipOptimization);
            var bytecode = blob.AsSpan();
            CreateShader(bytecode);
        }
        catch (Exception ex)
        {
            throw new Exception($"Internal Compiler Error: {ex.GetType()}", ex);
        }

    }

    public override void Dispose()
    {
        cbuffer?.Dispose();
        base.Dispose();
    }

    public virtual void Update(IShader shader)
    {
        if (shader is not T unboxedShader)
        {
            throw new Exception();
        }

        if (cbufferData is null)
        {
            int size = 0;
            foreach (var uniform in Compilation.Uniforms)
            {
                size += Marshal.SizeOf(uniform.BackingField.FieldType);
            }

            cbufferData = new byte[size];
        }

        int offset = 0;

        foreach (var uniform in Compilation.Uniforms)
        {
            var fieldSize = Marshal.SizeOf(uniform.VariableType);

            ref byte fieldReference = ref Unsafe.As<T, byte>(ref Unsafe.AddByteOffset(ref unboxedShader, offset));

            Unsafe.CopyBlock(ref cbufferData[offset], ref fieldReference, (uint)fieldSize);

            offset += fieldSize;
        }
        
        if (cbufferData.Length > 0)
        {
            if (cbuffer is null)
            {
                cbuffer = Resources.Device.CreateBuffer(BindFlags.ConstantBuffer, cbufferData.AsSpan(), sizeInBytes: CeilTo16(cbufferData.Length));
            }
            else
            {
                Resources.Device.ImmediateContext.UpdateSubresource(cbufferData, cbuffer);
            }
        }

        resourceViews ??= new ID3D11ShaderResourceView[16];
        samplerStates ??= new ID3D11SamplerState[16];

        Array.Clear(resourceViews);
        Array.Clear(samplerStates);

        int samplerStatesIndex = 0, resourceViewsIndex = 0;

        foreach (var uniform in Compilation.IntrinsicUniforms)
        {
            var value = uniform.BackingField.GetValue(shader);

            if (value is IShaderResourceViewProvider srvProvider)
            {
                resourceViews[resourceViewsIndex++] = srvProvider.GetShaderResourceView();
            }
            else if (value is TextureSampler sampler)
            {
                samplerStates[samplerStatesIndex++] = Resources.SamplerManager.GetSampler(sampler);
            }
            else if (value is not null)
            {
                throw new Exception();
            }

        }
    }

    static int CeilTo16(int value)
    {
        int remainder = value % 16;
        return remainder is 0 ? value : value + (16 - remainder);
    }

    public void Apply(ID3D11DeviceContext context)
    {
        ApplyShader(context);

        if (cbuffer is not null)
        {
            ApplyConstantBuffer(context, this.cbuffer);
        }

        for (int i = 0; i < resourceViews.Length; i++)
        {
            var srv = resourceViews[i];
            ApplyShaderResourceView(context, srv, i);
        }

        for (int i = 0; i < samplerStates.Length; i++)
        {
            var sampler = samplerStates[i];
            ApplySamplerState(context, sampler, i);
        }
    }

    public abstract void ApplyShader(ID3D11DeviceContext context);
    public abstract void ApplyConstantBuffer(ID3D11DeviceContext context, ID3D11Buffer constantBuffer);
    public abstract void ApplyShaderResourceView(ID3D11DeviceContext context, ID3D11ShaderResourceView shaderResourceView, int slot);
    public abstract void ApplySamplerState(ID3D11DeviceContext context, ID3D11SamplerState samplerState, int slot);
}