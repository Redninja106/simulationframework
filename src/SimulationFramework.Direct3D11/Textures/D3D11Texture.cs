using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;

namespace SimulationFramework.Drawing.Direct3D11.Textures;

internal unsafe class D3D11Texture<T> : D3D11Object, 
    ITexture<T>, 
    IShaderResourceViewProvider,
    IResourceProvider<ID3D11RenderTargetView>,
    IResourceProvider<ID3D11DepthStencilView>,
    IResourceProvider<ID3D11ShaderResourceView>
    where T : unmanaged
{
    internal LazyResource<ID3D11RenderTargetView> renderTargetView;
    internal LazyResource<ID3D11DepthStencilView> depthStencilView;
    internal LazyResource<ID3D11ShaderResourceView> shaderResourceView;

    public ID3D11RenderTargetView RenderTargetView => renderTargetView.GetValue();
    public ID3D11ShaderResourceView ShaderResourceView => shaderResourceView.GetValue();

    private T[] cpuData;
    private ID3D11Texture2D?[] internalTextures;
    private Format format;

    public virtual int Width { get; private set; }
    public virtual int Height { get; private set; }
    public Span<T> Pixels => GetPixels();

    public D3D11Texture(DeviceResources resources, ImageResult image, ResourceOptions options) : this(resources, image.Width, image.Height, Span<T>.Empty, options)
    {
        Debug.Assert(image.Comp is ColorComponents.RedGreenBlueAlpha);
        var imageData = MemoryMarshal.Cast<byte, T>(image.Data);
        imageData.CopyTo(GetPixels());
        ApplyChanges();
    }

    public unsafe D3D11Texture(DeviceResources resources, int width, int height, Span<T> data, ResourceOptions flags) : base(resources)
    {
        Debug.Assert(width > 0);
        Debug.Assert(height > 0);

        Width = width;
        Height = height;

        if (typeof(T) == typeof(float))
        {
            format = Format.R32_Float;
        }
        else if (typeof(T) == typeof(Color))
        {
            format = Format.R8G8B8A8_UNorm;
        }
        else
        {
            throw new Exception();
        }

        this.internalTextures = new ID3D11Texture2D[Enum.GetValues<TextureUsage>().Length];

        renderTargetView = new(resources, CreateRenderTargetView);
        shaderResourceView = new(resources, CreateShaderResourceView);
        depthStencilView = new(resources, CreateDepthStencilView);

        if (!data.IsEmpty)
        {
            data.CopyTo(Pixels);
            ApplyChanges();
        }
    }

    public void ApplyChanges()
    {
        if (cpuData == null)
            return;

        if (internalTextures.All(t => t is null))
        {
            GetInternalTexture(TextureUsage.ShaderResource);
        }

        var bytes = MemoryMarshal.AsBytes(cpuData.AsSpan());

        foreach (var texture in internalTextures)
        {
            if (texture is null)
                continue;
            
            Resources.Device.ImmediateContext.UpdateSubresource(cpuData.AsSpan(), texture, rowPitch: Width * sizeof(T));
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
        this.shaderResourceView?.Dispose();

        foreach (var texture in internalTextures)
        {
            texture?.Dispose();
        }
    }

    public ref Color GetPixel(int x, int y)
    {
        throw new NotImplementedException();
    }

    private ID3D11DepthStencilView CreateDepthStencilView()
    {
        var texture = GetInternalTexture(TextureUsage.DepthStencil);

        var desc = new DepthStencilViewDescription()
        {
            Format = texture.Description.Format,
            ViewDimension = DepthStencilViewDimension.Texture2D,
            Texture2D = new Texture2DDepthStencilView()
            {
                MipSlice = 0,
            }
        };

        return Resources.Device.CreateDepthStencilView(texture, desc);
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

        return Resources.Device.CreateRenderTargetView(texture, desc);
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

        return Resources.Device.CreateShaderResourceView(texture, desc);
    }

    public ID3D11Texture2D GetInternalTexture(TextureUsage usage)
    {
        if (internalTextures[(int)usage]?.NativePointer == 0)
            internalTextures[(int)usage] = null;

        return internalTextures[(int)usage] ??= CreateInternalTexture(usage);
    }

    protected virtual internal ID3D11Texture2D CreateInternalTexture(TextureUsage usage)
    {
        switch (usage)
        {
            case TextureUsage.ShaderResource:
                var srdesc = new Texture2DDescription()
                {
                    Width = Width,
                    Height = Height,
                    SampleDescription = new(1, 0),
                    ArraySize = 1,
                    BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                    Usage = ResourceUsage.Default,
                    CpuAccessFlags = CpuAccessFlags.Read | CpuAccessFlags.Write,
                    Format = format,
                };

                return Resources.Device.CreateTexture2D(srdesc);
            case TextureUsage.RenderTarget:
                return GetInternalTexture(TextureUsage.ShaderResource);
            case TextureUsage.DepthStencil:
                if (typeof(T) != typeof(float))
                    throw new Exception();

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

                return Resources.Device.CreateTexture2D(desc);
            default:
                throw new NotImplementedException();
        }
    }

    public void Clear(T value)
    {
        cpuData.AsSpan().Fill(value);
        ApplyChanges();
    }

    public ID3D11ShaderResourceView GetShaderResourceView()
    {
        return ShaderResourceView;
    }

    public void NotifyBound(GraphicsQueueBase queue, BindingUsage usage, bool mayWrite)
    {
    }

    public void GetResource(out ID3D11DepthStencilView resource)
    {
        if (typeof(T) != typeof(float))
            throw new Exception();

        resource = this.depthStencilView.GetValue();
    }

    public void GetResource(out ID3D11RenderTargetView resource)
    {
        if (typeof(T) != typeof(Color))
            throw new Exception();

        resource = this.renderTargetView.GetValue();
    }

    public void ResizeInternal(int width, int height)
    {
        this.Width = width;
        this.Height = height;

        for (int i = 0; i < internalTextures.Length; i++)
        {
            internalTextures[i]?.Dispose();
            internalTextures[i] = null;
        }

        renderTargetView.DisposeValue();
        depthStencilView.DisposeValue();
        shaderResourceView.DisposeValue();
    }

    public void GetResource(out ID3D11ShaderResourceView resource)
    {
        resource = this.GetShaderResourceView();
    }

    public void NotifyUnbound(GraphicsQueueBase queue)
    {
        throw new NotImplementedException();
    }
}
