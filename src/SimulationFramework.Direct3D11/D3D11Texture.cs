using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D;
using Vortice.Direct3D11;

namespace SimulationFramework.Drawing.Direct3D11;

internal class D3D11Texture : D3D11Object, ITexture
{
    private ImplicitResource<ID3D11RenderTargetView> renderTargetView;
    private ImplicitResource<ID3D11ShaderResourceView> shaderResourceView;
    
    public ID3D11Texture2D Texture { get; private set; }
    public ID3D11RenderTargetView RenderTargetView => renderTargetView.Value;
    public ID3D11ShaderResourceView ShaderResourceView => shaderResourceView.Value;

    public D3D11Texture(DeviceResources resources, ID3D11Texture2D tex) : base(resources)
    {
        this.Texture = tex;
        Width = tex.Description.Width;
        Height = tex.Description.Height;

        renderTargetView = new(resources, CreateRenderTargetView);
        shaderResourceView = new(resources, CreateShaderResourceView);
    }

    public unsafe D3D11Texture(DeviceResources resources, int width, int height, Span<Color> data, ResourceOptions flags) : base(resources)
    {
        Width = width;
        Height = height;

        var desc = new Texture2DDescription()
        {
            Width = width,
            Height = height,
        };

        fixed (void* dataPtr = data)
        {
            Texture = resources.Device.CreateTexture2D(desc, new[] { new SubresourceData(dataPtr, width * 4) });
        }
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
    
    private ID3D11RenderTargetView CreateRenderTargetView()
    {
        var desc = new RenderTargetViewDescription()
        {
            Format = Texture.Description.Format,
            ViewDimension = RenderTargetViewDimension.Texture2D,
            Texture2D = new Texture2DRenderTargetView()
            {
                MipSlice = 0,
            }
        };

        return this.Resources.Device.CreateRenderTargetView(Texture, desc);
    }

    private ID3D11ShaderResourceView CreateShaderResourceView()
    {
        var desc = new ShaderResourceViewDescription()
        {
            Format = Texture.Description.Format,
            ViewDimension = ShaderResourceViewDimension.Texture2D,
            Texture2D = new Texture2DShaderResourceView()
            {
                MipLevels = 1,
                MostDetailedMip = 0,
            }
        };

        return this.Resources.Device.CreateShaderResourceView(Texture, desc);
    }

    public ID3D11Texture2D GetInternalTexture()
    {
        return this.Texture;    
    }
}
