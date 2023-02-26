﻿using SharpGen.Runtime;
using SimulationFramework.Drawing.Direct3D11.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.Direct3D11.Debug;
using Vortice.DXGI;

namespace SimulationFramework.Drawing.Direct3D11;

internal sealed class DeviceResources : IDisposable
{
    public ID3D11Device Device { get; private set; }
    public IDXGISwapChain1 SwapChain { get; private set; }
    public ID3D11Debug Debug { get; private set; }
    public ShaderManager ShaderManager { get; private set; }
    public SamplerManager SamplerManager { get; private set; }

    public DeviceResources(IntPtr hwnd)
    {
        var hr = D3D11.D3D11CreateDevice(null, DriverType.Hardware, DeviceCreationFlags.Debug, new[] { FeatureLevel.Level_11_0 }, out var device);
        this.Device = device;
        this.Debug = device.QueryInterface<ID3D11Debug>();
        
        if (hr.Failure)
            throw new Exception();

        Win32.GetWindowRect(hwnd, out var windowBounds);
        
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

        using var dxgiDevice = Device.QueryInterface<IDXGIDevice>();

        using var adapter = dxgiDevice.GetAdapter();

        using var factory = adapter.GetParent<IDXGIFactory4>();

        SwapChain = factory.CreateSwapChainForHwnd(Device, hwnd, desc);

        ShaderManager = new(this);
        SamplerManager = new(this);
    }

    public void Resize(int width, int height)
    {
        if (SwapChain.Description1.Width == width && SwapChain.Description1.Height == height)
            return;

        SwapChain.ResizeBuffers(0, width, height);
    }

    public void Dispose()
    {
        ShaderManager.Dispose();
        SwapChain.Dispose();
        //Debug.ReportLiveDeviceObjects(ReportLiveDeviceObjectFlags.Detail);
        Debug.Dispose();
        Device.Dispose();
    }
}