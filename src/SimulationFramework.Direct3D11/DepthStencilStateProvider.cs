using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;
using static SimulationFramework.Shaders.Compiler.ControlFlow.DgmlBuilder;
using D3DStencilOperation = Vortice.Direct3D11.StencilOperation;

namespace SimulationFramework.Drawing.Direct3D11;
internal class DepthStencilStateProvider : D3D11Object
{
    private readonly Dictionary<DepthStencilInfo, ID3D11DepthStencilState> states = new();

    public DepthStencilStateProvider(DeviceResources deviceResources) : base(deviceResources)
    {
    }

    public ID3D11DepthStencilState GetDepthStencilState(DepthStencilInfo info)
    {
        if (!states.TryGetValue(info, out var result))
        {
            result = CreateDepthStencilState(info);
            states.Add(info, result);
        }

        return result;
    }

    private ID3D11DepthStencilState CreateDepthStencilState(DepthStencilInfo info)
    {
        DepthStencilOperationDescription operation = new()
        {
            StencilFunc = GetCompFunction(info.stencilComparison),
            StencilPassOp = GetD3DStencilOperation(info.stencilPass),
            StencilFailOp = GetD3DStencilOperation(info.stencilFail),
            StencilDepthFailOp = GetD3DStencilOperation(info.stencilDepthFail),
        };

        DepthStencilDescription desc = new()
        {
            DepthEnable = info.depthEnabled,
            DepthFunc = GetCompFunction(info.depthComparison),
            DepthWriteMask = info.depthReadOnly ? DepthWriteMask.Zero : DepthWriteMask.All,
            StencilEnable = info.stencilEnabled,
            StencilReadMask = info.stencilReadMask,
            StencilWriteMask = info.stencilWriteMask,
            BackFace = operation,
            FrontFace = operation,
        };

        var desc2 = DepthStencilDescription.Default;

        return Resources.Device.CreateDepthStencilState(desc);
    }

    private ComparisonFunction GetCompFunction(DepthStencilComparison comparison)
    {
        return comparison switch
        {
            DepthStencilComparison.Always => ComparisonFunction.Always,
            DepthStencilComparison.Never => ComparisonFunction.Never,
            DepthStencilComparison.Equal => ComparisonFunction.Equal,
            DepthStencilComparison.NotEqual => ComparisonFunction.NotEqual,
            DepthStencilComparison.LessThan => ComparisonFunction.Less,
            DepthStencilComparison.LessThanOrEqual => ComparisonFunction.LessEqual,
            DepthStencilComparison.GreaterThan => ComparisonFunction.Greater,
            DepthStencilComparison.GreaterThanOrEqual => ComparisonFunction.GreaterEqual,
            _ => throw new Exception()
        };
    }

    private D3DStencilOperation GetD3DStencilOperation(StencilOperation stencilOperation)
    {
        return stencilOperation switch
        {
            StencilOperation.None => D3DStencilOperation.Keep,
            StencilOperation.Zero => D3DStencilOperation.Zero,
            StencilOperation.Replace => D3DStencilOperation.Replace,
            StencilOperation.Invert => D3DStencilOperation.Invert,
            StencilOperation.Increment => D3DStencilOperation.IncrementSaturate,
            StencilOperation.Decrement => D3DStencilOperation.IncrementSaturate,
            StencilOperation.IncrementWrap => D3DStencilOperation.Increment,
            StencilOperation.DecrementWrap => D3DStencilOperation.Decrement,
            _ => throw new Exception()
        };
    }

    public override void Dispose()
    {
        foreach (var (_, state) in states)
        {
            state.Dispose();
        }

        base.Dispose();
    }

    public record struct DepthStencilInfo(
        bool depthEnabled,
        bool depthReadOnly,
        float depthBias,
        DepthStencilComparison depthComparison,
        bool stencilEnabled,
        DepthStencilComparison stencilComparison,
        StencilOperation stencilPass,
        StencilOperation stencilDepthFail,
        StencilOperation stencilFail,
        byte stencilReadMask,
        byte stencilWriteMask
        );
}
