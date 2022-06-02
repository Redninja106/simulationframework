using SimulationFramework.Drawing.Canvas;
using SimulationFramework.Drawing.Direct3D11.Buffers;
using SimulationFramework.Drawing.Direct3D11.Shaders;
using SimulationFramework.Drawing.Pipelines;
using SimulationFramework.Messaging;
using System.Numerics;
using System.Runtime.CompilerServices;
using Vortice.D3DCompiler;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;

namespace SimulationFramework.Drawing.Direct3D11;

public class D3D11Graphics : IGraphicsProvider
{
    private DeviceResources resources;
    private D3D11Texture frameTexture;
    private NullCanvas frameCanvas;
    
    public D3D11Graphics(IntPtr hwnd)
    {
        resources = new DeviceResources(hwnd);
        frameTexture = new D3D11Texture(resources, resources.SwapChain.GetBuffer<ID3D11Texture2D>(0));
        frameCanvas = new NullCanvas(frameTexture);
    }

    private void AfterRender(RenderMessage message)
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
        return frameTexture;
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

    public void Initialize(Application application)
    {
        application.Dispatcher.Subscribe<RenderMessage>(AfterRender, MessagePriority.Low);
        application.Dispatcher.Subscribe<RenderMessage>(BeforeRender, MessagePriority.High);
        application.Dispatcher.Subscribe<ResizeMessage>(Resize, MessagePriority.High);
    }

    private void BeforeRender(RenderMessage message)
    {
        resources.ImmediateRenderer.BeginFrame();
    }

    private void Resize(ResizeMessage message)
    {
        this.resources.ImmediateRenderer.DeviceContext.ClearState();
        frameTexture.Dispose();
        resources.Resize(message.Width, message.Height);
        frameTexture = new D3D11Texture(resources, resources.SwapChain.GetBuffer<ID3D11Texture2D>(0));
    }

    public ICanvas GetFrameCanvas()
    {
        return this.frameCanvas;
    }

    record NullCanvas(ITexture Target) : ICanvas
    {
        public CanvasState State { get; } = new NullCanvasState();

        public void Clear(Color color)
        {
        }

        public void Dispose()
        {
        }

        public void DrawArc(Rectangle bounds, float begin, float end, bool includeCenter)
        {
        }

        public void DrawLine(Vector2 p1, Vector2 p2)
        {
        }

        public void DrawPolygon(Span<Vector2> polygon)
        {
        }

        public void DrawRoundedRect(Rectangle rect, float radius)
        {
        }

        public void DrawText(string text, Vector2 position, Alignment alignment = Alignment.TopLeft)
        {
        }

        public void DrawTexture(ITexture texture, Rectangle source, Rectangle destination)
        {
        }

        public void Flush()
        {
        }

        public Vector2 MeasureText(string text, float maxLength, out int charsMeasured)
        {
            charsMeasured = 0;
            return default;
        }

        public void PopState()
        {
        }

        public CanvasSession PushState()
        {
            return new CanvasSession(this);
        }

        public void ResetState()
        {
        }

        public bool SetFont(string fontName, TextStyles styles, float size) 
        {
            return true;
        }

        class NullCanvasState : CanvasState
        {
        }
    }

}
