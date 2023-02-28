using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;
using static System.Net.Mime.MediaTypeNames;

namespace SimulationFramework.Drawing.Direct3D11.Textures;
internal class D3D11BackBufferTexture<T> : D3D11Texture<T> where T : unmanaged
{
    private ID3D11Texture2D backBuffer;

    public D3D11BackBufferTexture(DeviceResources resources, ID3D11Texture2D backBuffer) 
        : base(resources, backBuffer.Description.Width, backBuffer.Description.Height, Span<T>.Empty, ResourceOptions.None)
    {
        if (typeof(T) != typeof(Color) && typeof(T) != typeof(float))
        {
            throw new NotSupportedException("Texture<Color> or Texture<float> only.");
        }

        this.backBuffer = backBuffer;
    }

    protected internal override ID3D11Texture2D CreateInternalTexture(TextureUsage usage)
    {
        if (usage is TextureUsage.RenderTarget)
            return backBuffer;

        return base.CreateInternalTexture(usage);
    }

    public override void Dispose()
    {
        backBuffer.Dispose();
        base.Dispose();
    }
}
