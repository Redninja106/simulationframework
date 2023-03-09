using SimulationFramework.Shaders;
using SimulationFramework.Shaders.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace SimulationFramework.Drawing.Direct3D11;
internal class D3D11ComputeShader : D3D11Shader, IBindableResource
{
    private ID3D11ComputeShader shader;
    private ID3D11UnorderedAccessView[] uavs;
    private IResourceProvider<ID3D11UnorderedAccessView>[] uavProviders;

    public D3D11ComputeShader(DeviceResources deviceResources, Type shaderType) : base(deviceResources, shaderType, null)
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
        uavProviders ??= new IResourceProvider<ID3D11UnorderedAccessView>[Compilation.IntrinsicUniforms.Count()];

        for (int i = 0; i < uavs.Length; i++)
        {
            var uniform = Compilation.IntrinsicUniforms.ElementAt(i);
            
            var value = uniform.BackingField.GetValue(shader);

            if (value is null)
                continue;

            if (value is IResourceProvider<ID3D11UnorderedAccessView> uavProvider)
            {
                uavProviders[i] = uavProvider;
                uavProvider.NotifyBound((GraphicsQueueBase)Graphics.ImmediateQueue, BindingUsage.UnorderedAccess, true);
                uavProvider.GetResource(out uavs[i]);
            }
            else
            {
                throw new Exception();
            }
        }
    }

    public override void ApplySamplerState(ID3D11DeviceContext context, ID3D11SamplerState samplerState, int slot)
    {
        context.CSSetSampler(slot, samplerState);
    }

    public override void ApplyShaderResourceView(ID3D11DeviceContext context, ID3D11ShaderResourceView shaderResourceView, int slot)
    {
        context.CSSetShaderResource(slot, shaderResourceView);
    }

    public void NotifyBound(GraphicsQueueBase queue, BindingUsage usage, bool mayWrite)
    {
    }

    public void NotifyUnbound(GraphicsQueueBase queue)
    {
        for (int i = 0; i < uavProviders.Length; i++)
        {
            uavProviders[i]?.NotifyUnbound(queue);
        }
    }
}
