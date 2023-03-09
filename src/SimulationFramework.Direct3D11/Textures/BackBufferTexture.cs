using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace SimulationFramework.Drawing.Direct3D11.Textures;
internal class BackBufferTexture : D3D11Texture<Color>
{
    private ID3D11Texture2D backBuffer;

    public override int Width => backBuffer.Description.Width;
    public override int Height => backBuffer.Description.Height;

    public BackBufferTexture(DeviceResources resources, ID3D11Texture2D backBuffer) 
        : base(resources, backBuffer.Description.Width, backBuffer.Description.Height, Span<Color>.Empty, ResourceOptions.None)
    {
        this.backBuffer = backBuffer;
    }

    protected internal override ID3D11Texture2D CreateInternalTexture(TextureUsage usage)
    {
        if (usage is TextureUsage.RenderTarget)
            return backBuffer;

        return base.CreateInternalTexture(usage);
    }

    public void FreeBackBufferReferences()
    {
        renderTargetView.DisposeValue();
        depthStencilView.DisposeValue();
        renderTargetView.DisposeValue();

        backBuffer.Dispose();
    }

    public void RestoreBackBuffer(ID3D11Texture2D backBuffer)
    {
        this.backBuffer = backBuffer;
    }

    public override void Dispose()
    {
        backBuffer?.Dispose();
        base.Dispose();
    }
}
