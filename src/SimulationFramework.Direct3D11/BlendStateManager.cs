using Vortice.Direct3D11;
using D3D11BlendOperation = Vortice.Direct3D11.BlendOperation;

namespace SimulationFramework.Drawing.Direct3D11;

internal class BlendStateManager : D3D11Object
{
    private Dictionary<BlendStateInfo, ID3D11BlendState> blendStates = new();

    public BlendStateManager(DeviceResources deviceResources) : base(deviceResources)
    {
    }

    public ID3D11BlendState GetBlendState(BlendStateInfo info)
    {
        if (blendStates.TryGetValue(info, out var result))
        {
            return result;
        }

        result = CreateBlendState(info);
        blendStates.Add(info, result);
        return result;
    }

    private ID3D11BlendState CreateBlendState(BlendStateInfo info)
    {
        var desc = new BlendDescription();
        desc.AlphaToCoverageEnable = false;
        desc.IndependentBlendEnable = false;
        desc.RenderTarget[0].IsBlendEnabled = true;
        desc.RenderTarget[0].SourceBlend = GetD3D11Blend(info.SourceBlend);
        desc.RenderTarget[0].DestinationBlend = GetD3D11Blend(info.DestinationBlend);
        desc.RenderTarget[0].BlendOperation = GetD3D11BlendOperation(info.BlendOperation);
        desc.RenderTarget[0].SourceBlendAlpha = GetD3D11Blend(info.SourceBlendAlpha);
        desc.RenderTarget[0].DestinationBlendAlpha = GetD3D11Blend(info.DestinationBlendAlpha);
        desc.RenderTarget[0].BlendOperationAlpha = GetD3D11BlendOperation(info.BlendOperationAlpha);
        desc.RenderTarget[0].RenderTargetWriteMask = ColorWriteEnable.All;

        return Resources.Device.CreateBlendState(desc);

        Blend GetD3D11Blend(BlendMode blendMode) => blendMode switch
        {
            BlendMode.Zero => Blend.Zero,
            BlendMode.One => Blend.One,
            BlendMode.SourceColor => Blend.SourceColor,
            BlendMode.InverseSourceColor => Blend.InverseSourceColor,
            BlendMode.DestinationColor => Blend.DestinationColor,
            BlendMode.InverseDestinationColor => Blend.InverseDestinationColor,
            BlendMode.SourceAlpha => Blend.SourceAlpha,
            BlendMode.InverseSourceAlpha => Blend.InverseSourceAlpha,
            BlendMode.DestinationAlpha => Blend.DestinationAlpha,
            BlendMode.InverseDestinationAlpha => Blend.InverseDestinationAlpha,
            BlendMode.ConstantColor => Blend.BlendFactor,
            BlendMode.InverseConstantColor => Blend.InverseBlendFactor,
        };

        D3D11BlendOperation GetD3D11BlendOperation(BlendOperation operation) => operation switch
        {
            BlendOperation.Add => D3D11BlendOperation.Add,
        };
    }

    public record struct BlendStateInfo(
        BlendMode SourceBlend, BlendMode DestinationBlend,
        BlendMode SourceBlendAlpha, BlendMode DestinationBlendAlpha,
        BlendOperation BlendOperation, BlendOperation BlendOperationAlpha);
}