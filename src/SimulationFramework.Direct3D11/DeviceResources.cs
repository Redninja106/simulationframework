using SharpGen.Runtime;
using SimulationFramework.Drawing.Direct3D11.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.Direct3D11.Debug;
using Vortice.DXGI;

namespace SimulationFramework.Drawing.Direct3D11;

internal sealed class DeviceResources : IDisposable
{
    public ID3D11Device5 Device => DeviceProvider.Device;
    public ShaderProvider ShaderProvider { get; private set; }
    public SamplerProvider SamplerProvider { get; private set; }
    public RasterizerStateProvider RasterizerStateProvider { get; private set; }
    public BlendStateProvider BlendStateProvider { get; private set; }
    public InputLayoutProvider InputLayoutProvider {  get; private set;  }
    public DepthStencilStateProvider DepthStencilStateProvider { get; private set; }
    public SwapChainProvider SwapChainProvider { get; private set; }
    public DeviceProvider DeviceProvider { get; private set; }

    public DeviceResources(nint hwnd)
    {
        DeviceProvider = new(this);
        SwapChainProvider = new(this, hwnd);

        ShaderProvider = new(this);
        SamplerProvider = new(this);
        RasterizerStateProvider = new(this);
        BlendStateProvider = new(this);
        InputLayoutProvider = new(this);
        DepthStencilStateProvider = new(this);
    }


    public void Dispose()
    {
        ShaderProvider.Dispose();
        SamplerProvider.Dispose();
        RasterizerStateProvider.Dispose();
        BlendStateProvider.Dispose();
        InputLayoutProvider.Dispose();
        DepthStencilStateProvider.Dispose();
        ShaderProvider.Dispose();
        SwapChainProvider.Dispose();
        DeviceProvider.Dispose();
    }
}