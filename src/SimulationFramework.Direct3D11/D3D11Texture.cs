using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D;
using Vortice.Direct3D11;

namespace SimulationFramework.Drawing.Direct3D11;

internal unsafe class D3D11Texture : D3D11Object, ITexture
{
    private LazyResource<ID3D11RenderTargetView> renderTargetView;
    private LazyResource<ID3D11ShaderResourceView> shaderResourceView;
    
    public ID3D11Texture2D Texture { get; private set; }
    public ID3D11RenderTargetView RenderTargetView => renderTargetView.GetValue();
    public ID3D11ShaderResourceView ShaderResourceView => shaderResourceView.GetValue();

    private Color[] cpuData;

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
            SampleDescription = new(1, 0),
            ArraySize = 1,
            BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
            Usage = ResourceUsage.Default,
            CpuAccessFlags = CpuAccessFlags.Read | CpuAccessFlags.Write,
            Format = Vortice.DXGI.Format.B8G8R8A8_UNorm,
        };
        
        fixed (void* dataPtr = data)
        {
            SubresourceData[] subresourceData = null;
            
            if (!data.IsEmpty)
            {
                cpuData = new Color[Width * height];
                data.CopyTo(cpuData.AsSpan());

                subresourceData = new[] { new SubresourceData(dataPtr, width * Unsafe.SizeOf<Color>()) };
            }
            
            Texture = resources.Device.CreateTexture2D(desc, subresourceData);
        }

        renderTargetView = new(resources, CreateRenderTargetView);
        shaderResourceView = new(resources, CreateShaderResourceView);
    }

    public int Width { get; }
    public int Height { get; }
    public Span<Color> Pixels => GetPixels();

    public void ApplyChanges()
    {
        if (cpuData == null)
            return;

        Resources.ImmediateRenderer.DeviceContext.UpdateSubresource(cpuData.AsSpan(), this.Texture, rowPitch: Width * sizeof(Color));
    }

    private Span<Color> GetPixels()
    {
        if (cpuData is null)
        {
            cpuData = new Color[Width * Height];
        }

        return cpuData.AsSpan();
    }

    public override void Dispose()
    {
        this.shaderResourceView?.Dispose();
        this.renderTargetView?.Dispose();
        this.Texture.Dispose();
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
