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
internal abstract class D3D11Shader : D3D11Object
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

    protected D3D11Shader(DeviceResources deviceResources, Type shaderType, ShaderSignature inputSignature) : base(deviceResources)
    {
        this.ShaderType = shaderType;
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
            using var blob = Compiler.Compile(source, nameof(IShader.Main), ShaderType.Name, this.Profile, ShaderFlags.PackMatrixColumnMajor);
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
        CopyUniforms(shader);
        
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

            if (value is IResourceProvider<ID3D11ShaderResourceView> srvProvider)
            {
                srvProvider.NotifyBound((GraphicsQueueBase)Graphics.ImmediateQueue, BindingUsage.ShaderResource, false);
                srvProvider.GetResource(out resourceViews[resourceViewsIndex++]);
            }
            else if (value is TextureSampler sampler)
            {
                samplerStates[samplerStatesIndex++] = Resources.SamplerProvider.GetSampler(sampler);
            }
            else if (value is not null)
            {
                throw new Exception();
            }
        }
    }

    private void CopyUniforms(IShader shader)
    {
        if (cbufferData is null)
        {
            int size = 0;
            foreach (var uniform in Compilation.Uniforms)
            {
                var uniformSize = Marshal.SizeOf(uniform.BackingField.FieldType);

                if (uniformSize + (size % 16) >= 16)
                {
                    size = CeilTo16(size);
                }

                size += uniformSize;
            }

            cbufferData = new byte[size];
        }

        // https://learn.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-packing-rules

        int offset = 0;

        MethodInfo copyValueGeneric = this.GetType().GetMethod(nameof(CopyUniform), BindingFlags.Instance | BindingFlags.Public);
        
        foreach (var uniform in Compilation.Uniforms)
        {
            var copyValue = copyValueGeneric.MakeGenericMethod(uniform.VariableType);
            var size = Marshal.SizeOf(uniform.VariableType);
            
            if (size + (offset % 16) >= 16)
            {
                offset = CeilTo16(offset);
            }

            offset += (int)copyValue.Invoke(this, new[] { offset, uniform.BackingField.GetValue(shader) });
        }
    }
    
    public int CopyUniform<TVariable>(int offset, TVariable value) where TVariable : unmanaged
    {
        if (value is Matrix4x4 matrix)
        {
            matrix = Matrix4x4.Transpose(matrix);

            if (matrix is TVariable variable)
            {
                value = variable;
            }
        }

        var bytes = MemoryMarshal.AsBytes(new Span<TVariable>(ref value));
        bytes.CopyTo(cbufferData.AsSpan(offset));
        return Unsafe.SizeOf<TVariable>();
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