using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SimulationFramework.Drawing.Direct3D11.Textures;
using Vortice.Direct3D11;
using Vortice.DXGI;

namespace SimulationFramework.Drawing.Direct3D11;
internal class DepthStencilManager : D3D11Object
{
    private ID3D11DepthStencilState depthStencilState;

    public D3D11Texture<float> DepthTexture { get; private set; }
    public ID3D11DepthStencilView DepthStencilView { get; private set; }

    public DepthStencilManager(DeviceResources deviceResources) : base(deviceResources)
    {
        depthStencilState = deviceResources.Device.CreateDepthStencilState(DepthStencilDescription.Default);
    }

    public void SetDepthTexture(D3D11Texture<float> texture)
    {
        this.DepthTexture = texture;
        DepthStencilView = CreateDepthStencilView();
    }

    public void PreDraw(ID3D11DeviceContext deviceContext)
    {
        deviceContext.OMSetDepthStencilState(depthStencilState);
    }

    private ID3D11DepthStencilView CreateDepthStencilView()
    {
        var desc = new DepthStencilViewDescription()
        {
            Format = Format.Unknown,
            Flags = DepthStencilViewFlags.None,
            ViewDimension = DepthStencilViewDimension.Texture2D,
            Texture2D = new Texture2DDepthStencilView()
            {
                MipSlice = 0
            }
        };

        return this.Resources.Device.CreateDepthStencilView(DepthTexture.GetInternalTexture(TextureUsage.DepthStencil), desc);
    }

    public override void Dispose()
    {
        depthStencilState?.Dispose();
        DepthTexture?.Dispose();
        DepthStencilView?.Dispose();
        base.Dispose();
    }
}
