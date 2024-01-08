using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebGPU;

namespace SimulationFramework.Drawing.WebGPU;
internal class SurfaceTextureViewProvider : ITextureViewProvider
{
    WebGPUResources resources;
    TextureView? view;
    Texture? texture;

    public int Width => (int)texture.Width;
    public int Height => (int)texture.Height;
    public TextureOptions Options => TextureOptions.None;
    public Span<Color> Pixels => throw new NotSupportedException("Cannot read/write the pixels of the frame buffer!");

    public SurfaceTextureViewProvider(WebGPUResources resources)
    {
        this.resources = resources;
    }

    public void NewFrame()
    {
        this.texture?.Dispose();
        view?.Dispose();

        resources.Surface.GetCurrentTexture(out SurfaceTexture texture);
        
        if (texture.Suboptimal || texture.Status == SurfaceGetCurrentTextureStatus.Outdated)
        {
            resources.Resize(Window.Width, Window.Height);
            resources.Surface.GetCurrentTexture(out texture);
        }

        if (texture.Status != SurfaceGetCurrentTextureStatus.Success)
        {
            throw new Exception($"Error getting frame texture! ({texture.Status})");
        }

        this.texture = texture.Texture;

        view = texture.Texture.CreateView(new()
        {
            BaseArrayLayer = 0,
            ArrayLayerCount = 1,
            BaseMipLevel = 0,
            MipLevelCount = 1,
            Aspect = TextureAspect.All,
            Dimension = TextureViewDimension._2D,
            Format = texture.Texture.Format,
        });
    }

    public TextureView GetView()
    {
        return view!;
    }

    public ICanvas GetCanvas()
    {
        Graphics.GetOutputCanvas();
        throw new NotSupportedException("Cannot create a new canvas on the frame buffer. Use Graphics.GetOutputCanvas() instead.");
    }

    public void ApplyChanges()
    {
        throw new NotSupportedException("Cannot read/write the pixels of the frame buffer!");
    }

    public void Encode(Stream destination, TextureEncoding encoding)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        this.texture?.Dispose();
        this.view?.Dispose();
    }
}
