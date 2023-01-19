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
        using var blob = Compiler.Compile(source, nameof(IShader.Main), ShaderType.Name, this.Profile, ShaderFlags.PackMatrixRowMajor);
        var bytecode = blob.AsSpan();
        CreateShader(bytecode);
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
            var fieldOffset = Marshal.OffsetOf<T>(uniform.BackingField.Name);
            var fieldSize = Marshal.SizeOf(uniform.VariableType);

            ref byte fieldReference = ref Unsafe.As<T, byte>(ref Unsafe.AddByteOffset(ref unboxedShader, fieldOffset));

            Unsafe.CopyBlock(ref cbufferData[offset], ref fieldReference, (uint)fieldSize);

            offset += fieldSize;
        }
        
        if (cbufferData.Length > 0)
        {
            if (cbuffer is null)
            {
                cbuffer = Resources.Device.CreateBuffer(BindFlags.ConstantBuffer, cbufferData.AsSpan());
            }
            else
            {
                Resources.Device.ImmediateContext.UpdateSubresource(cbufferData, cbuffer);
            }
        }
    }

    public virtual void Apply(ID3D11DeviceContext context)
    {
        if (cbuffer is not null)
        {
            ApplyConstantBuffer(context, this.cbuffer);
        }
    }

    public virtual void ApplyConstantBuffer(ID3D11DeviceContext context, ID3D11Buffer constantBuffer)
    {

    }
}