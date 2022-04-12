using SimulationFramework.Drawing.Direct3D11.Buffers;
using SimulationFramework.Drawing.Direct3D11.Shaders;
using System.Runtime.CompilerServices;
using Vortice.D3DCompiler;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;

namespace SimulationFramework.Drawing.Direct3D11;

public class D3D11Graphics : IGraphicsProvider
{
    private DeviceResources resources;

    public D3D11Graphics(IntPtr hwnd)
    {
        resources = new DeviceResources(hwnd);
    }

    public void Apply(Simulation simulation)
    {
        simulation.AfterRender += Simulation_AfterRender;
    }

    private void Simulation_AfterRender()
    {
        resources.SwapChain.Present(1);
    }

    public IBuffer<T> CreateBuffer<T>(int size, ResourceOptions flags) where T : unmanaged
    {
        return new D3D11Buffer<T>(this.resources, size, flags);
    }

    public IShader CreateShader(ShaderKind kind, string source)
    {
        switch (kind)
        {
            case ShaderKind.Vertex:
                return new D3D11VertexShader(resources, source);
            case ShaderKind.Fragment:
                return new D3D11FragmentShader(resources, source);
            case ShaderKind.Compute:
            default:
                throw new NotImplementedException();
        }
    }

    public ITexture CreateTexture(int width, int height, Span<Color> data, ResourceOptions flags)
    {
        return new D3D11Texture(this.resources, width, height, data, flags);
    }

    public void Dispose()
    {
    }

    public ITexture GetFrameTexture()
    {
        return new D3D11Texture(resources, resources.SwapChain.GetBuffer<ID3D11Texture2D>(0));
    }

    public ITexture LoadTexture(Span<byte> encodedData, ResourceOptions flags)
    {
        throw new NotImplementedException();
    }

    public void SetResourceLifetime(int lifetimeInFrames)
    {
        throw new NotImplementedException();
    }

    public IRenderer GetRenderer()
    {
        return resources.ImmediateRenderer;
    }
}
