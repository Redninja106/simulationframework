using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.DXGI;

namespace SimulationFramework.Drawing.Direct3D11;
internal class SwapChainProvider : D3D11Object
{
    public IDXGISwapChain1 SwapChain { get; private set; }

    public SwapChainProvider(DeviceResources deviceResources, nint hwnd) : base(deviceResources)
    {
        Win32.GetWindowRect(hwnd, out var windowBounds);

        if (hwnd is not 0)
        {

            var desc = new SwapChainDescription1()
            {
                Width = windowBounds.Width,
                Height = windowBounds.Height,
                AlphaMode = AlphaMode.Ignore,
                SampleDescription = new SampleDescription(1, 0),
                BufferCount = 2,
                BufferUsage = Usage.Backbuffer | Usage.RenderTargetOutput,
                Flags = SwapChainFlags.None,
                Format = Format.B8G8R8A8_UNorm,
                Scaling = Scaling.None,
                Stereo = false,
                SwapEffect = SwapEffect.FlipSequential,
            };

            using var dxgiDevice = deviceResources.Device.QueryInterface<IDXGIDevice>();

            using var adapter = dxgiDevice.GetAdapter();

            using var factory = adapter.GetParent<IDXGIFactory4>();

            SwapChain = factory.CreateSwapChainForHwnd(deviceResources.Device, hwnd, desc);
        }
    }

    public void Resize(int width, int height)
    {
        if (SwapChain.Description1.Width == width && SwapChain.Description1.Height == height)
            return;

        SwapChain.ResizeBuffers(0, width, height);
    }

    public override void Dispose()
    {
        SwapChain.Dispose();
        base.Dispose();
    }
}
