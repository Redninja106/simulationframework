using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace SimulationFramework.Drawing.Direct3D11;

internal class D3D11Texture : ITexture
{
    ID3D11Texture2D tex;
    ID3D11RenderTargetView rtv;
    DeviceResources resources;
    public D3D11Texture(DeviceResources resources, ID3D11Texture2D tex)
    {
        this.tex = tex;
        this.resources = resources;
    }

    public int Width { get; }
    public int Height { get; }
    public Span<Color> Pixels { get => throw new NotImplementedException(); }

    public void ApplyChanges()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public ref Color GetPixel(int x, int y)
    {
        throw new NotImplementedException();
    }

    public ICanvas OpenCanvas()
    {
        return null;
    }

    internal ID3D11RenderTargetView GetRenderTargetView()
    {
        if (rtv is null)
        {
            var desc = new RenderTargetViewDescription()
            {
                Format = tex.Description.Format,
                ViewDimension = RenderTargetViewDimension.Texture2D,
                Texture2D = new Texture2DRenderTargetView()
                {
                    MipSlice = 0,
                }
            };

            rtv = resources.Device.CreateRenderTargetView(tex, desc);
        }

        return rtv;
    }
}
