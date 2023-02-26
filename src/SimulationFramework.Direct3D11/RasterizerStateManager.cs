using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;
using D3D11CullMode = Vortice.Direct3D11.CullMode;

namespace SimulationFramework.Drawing.Direct3D11;
internal class RasterizerStateManager : D3D11Object
{
    private readonly Dictionary<RasterizerStateInfo, ID3D11RasterizerState> rasterizerStates = new();

    public RasterizerStateManager(DeviceResources deviceResources) : base(deviceResources)
    {
    }

    public ID3D11RasterizerState GetRasterizerState(RasterizerStateInfo info)
    {
        if (rasterizerStates.TryGetValue(info, out var result))
        {
            return result;
        }

        result = CreateRasterizerState(info);
        rasterizerStates.Add(info, result);
        return result;
    }

    private ID3D11RasterizerState CreateRasterizerState(RasterizerStateInfo info)
    {
        var desc = new RasterizerDescription();
        desc.CullMode = GetD3D11CullMode(info.CullMode);
        desc.FillMode = info.Wireframe ? FillMode.Wireframe : FillMode.Solid;
        desc.ScissorEnable = info.ClipEnable;

        return Resources.Device.CreateRasterizerState(desc);

        static D3D11CullMode GetD3D11CullMode(CullMode cullMode) => cullMode switch
        {
            CullMode.None => D3D11CullMode.None,
            CullMode.Back => D3D11CullMode.Back,
            CullMode.Front => D3D11CullMode.Front,
        };
    }

    public record struct RasterizerStateInfo(CullMode CullMode, bool Wireframe, bool ClipEnable);
}
