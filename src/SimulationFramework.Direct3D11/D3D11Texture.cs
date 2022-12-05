using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;

namespace SimulationFramework.Drawing.Direct3D11;

internal unsafe class D3D11Texture<T> : D3D11Object, ITexture<T> where T : unmanaged
{
    private LazyResource<ID3D11RenderTargetView> renderTargetView;
    private LazyResource<ID3D11ShaderResourceView> shaderResourceView;

    public ID3D11RenderTargetView RenderTargetView => renderTargetView.GetValue();
    public ID3D11ShaderResourceView ShaderResourceView => shaderResourceView.GetValue();

    private T[] cpuData;
    private ID3D11Texture2D[] internalTextures;

    public D3D11Texture(DeviceResources resources, ID3D11Texture2D tex) : base(resources)
    {
        if (typeof(T) != typeof(Color) && typeof(T) != typeof(float))
        {
            throw new NotSupportedException("Texture<Color> or Texture<float> only.");
        }

        this.internalTextures = new ID3D11Texture2D[2];
        this.internalTextures[0] = tex;
        Width = tex.Description.Width;
        Height = tex.Description.Height;

        renderTargetView = new(resources, CreateRenderTargetView);
        shaderResourceView = new(resources, CreateShaderResourceView);
    }

    public unsafe D3D11Texture(DeviceResources resources, int width, int height, Span<T> data, ResourceOptions flags) : base(resources)
    {
        Width = width;
        Height = height;

        Format format; 

        if (typeof(T) == typeof(float))
        {
            format = Format.R32_Float;
        }
        else if (typeof(T) == typeof(Color))
        {
            format = Format.B8G8R8A8_UNorm;
        }
        else
        {
            throw new Exception();
        }

        var desc = new Texture2DDescription()
        {
            Width = width,
            Height = height,
            SampleDescription = new(1, 0),
            ArraySize = 1,
            BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
            Usage = ResourceUsage.Default,
            CpuAccessFlags = CpuAccessFlags.Read | CpuAccessFlags.Write,
            Format = format,
        };

        this.internalTextures = new ID3D11Texture2D[2];
        internalTextures[0] = resources.Device.CreateTexture2D(desc);

        renderTargetView = new(resources, CreateRenderTargetView);
        shaderResourceView = new(resources, CreateShaderResourceView);

        if (!data.IsEmpty)
        {
            data.CopyTo(Pixels);
            ApplyChanges();
        }
    }

    public int Width { get; }
    public int Height { get; }
    public Span<T> Pixels => GetPixels();

    public void ApplyChanges()
    {
        if (cpuData == null)
            return;

        Resources.ImmediateRenderer.DeviceContext.UpdateSubresource(cpuData.AsSpan(), this.internalTextures[0], rowPitch: Width * sizeof(Color));
        if (this.internalTextures[1] is not null)
        {
            Resources.ImmediateRenderer.DeviceContext.UpdateSubresource(cpuData.AsSpan(), this.internalTextures[1], rowPitch: Width * sizeof(Color));
        }
    }

    private Span<T> GetPixels()
    {
        if (cpuData is null)
        {
            cpuData = new T[Width * Height];
        }

        return cpuData.AsSpan();
    }

    public override void Dispose()
    {
        this.shaderResourceView?.Dispose();
        this.renderTargetView?.Dispose();
        this.internalTextures[0].Dispose();
        this.internalTextures[1].Dispose();
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
        var texture = GetInternalTexture(TextureUsage.RenderTarget);

        var desc = new RenderTargetViewDescription()
        {
            Format = texture.Description.Format,
            ViewDimension = RenderTargetViewDimension.Texture2D,
            Texture2D = new Texture2DRenderTargetView()
            {
                MipSlice = 0,
            }
        };

        return this.Resources.Device.CreateRenderTargetView(texture, desc);
    }

    private ID3D11ShaderResourceView CreateShaderResourceView()
    {
        var texture = GetInternalTexture(TextureUsage.RenderTarget);

        var desc = new ShaderResourceViewDescription()
        {
            Format = texture.Description.Format,
            ViewDimension = ShaderResourceViewDimension.Texture2D,
            Texture2D = new Texture2DShaderResourceView()
            {
                MipLevels = 1,
                MostDetailedMip = 0,
            }
        };

        return this.Resources.Device.CreateShaderResourceView(texture, desc);
    }

    public ID3D11Texture2D GetInternalTexture(TextureUsage usage)
    {
        switch (usage)
        {
            case TextureUsage.RenderTarget:
                return internalTextures[0];
            case TextureUsage.DepthStencil:
                if (typeof(T) != typeof(float))
                    throw new Exception();

                if (internalTextures[1] is null)
                {
                    Texture2DDescription desc = new()
                    {
                        ArraySize = 1,
                        BindFlags = BindFlags.DepthStencil,
                        Height = Height,
                        Width = Width,
                        SampleDescription = new(1, 0),
                        CpuAccessFlags = CpuAccessFlags.None,
                        Format = Format.D32_Float,
                        OptionFlags = ResourceOptionFlags.None,
                        MipLevels = 1,
                        Usage = ResourceUsage.Default
                    };

                    unsafe
                    {
                        fixed (void* data = cpuData) 
                        {
                            internalTextures[1] = Resources.Device.CreateTexture2D(desc);
                        } 
                    }
                }

                return internalTextures[1];
            default:
                throw new Exception();
        }
    }

    public void Clear(T value)
    {
        cpuData.AsSpan().Fill(value);
        this.ApplyChanges();
    }
}
