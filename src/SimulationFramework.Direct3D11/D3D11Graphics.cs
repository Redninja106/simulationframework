using SimulationFramework.Serialization.PNG;
using SimulationFramework.Drawing.Direct3D11.Buffers;
using SimulationFramework.Messaging;
using System.Numerics;
using System.Runtime.CompilerServices;
using Vortice.D3DCompiler;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;
using SimulationFramework.Shaders;
using SimulationFramework.Shaders.Compiler;

namespace SimulationFramework.Drawing.Direct3D11;

public class D3D11Graphics : IGraphicsProvider
{
    private DeviceResources resources;
    private D3D11Texture<Color> defaultRenderTarget;
    private D3D11Texture<float> defaultDepthTarget;
    private NullCanvas frameCanvas;
    private D3D11ImmediateQueue immediateQueue;

    public IGraphicsQueue ImmediateQueue => immediateQueue;

    public D3D11Graphics(IntPtr hwnd)
    {
        resources = new DeviceResources(hwnd);
        defaultRenderTarget = new D3D11Texture<Color>(resources, resources.SwapChain.GetBuffer<ID3D11Texture2D>(0));

        var swapchainDesc = resources.SwapChain.Description1;
        defaultDepthTarget = new D3D11Texture<float>(resources, swapchainDesc.Width, swapchainDesc.Height, Span<float>.Empty, ResourceOptions.None);
        frameCanvas = new NullCanvas(defaultRenderTarget);

        immediateQueue = new D3D11ImmediateQueue(resources);
    }

    private void AfterRender(RenderMessage message)
    {
        resources.SwapChain.Present(0);
    }

    public IBuffer<T> CreateBuffer<T>(int size, ResourceOptions flags) where T : unmanaged
    {
        return new D3D11Buffer<T>(this.resources, size, flags);
    }

    public ITexture<T> CreateTexture<T>(int width, int height, Span<T> data, ResourceOptions flags) where T : unmanaged
    {
        return new D3D11Texture<T>(this.resources, width, height, data, flags);
    }

    public void Dispose()
    {
        defaultRenderTarget.Dispose();
        resources.Dispose();
    }

    public ITexture<Color> GetDefaultRenderTarget()
    {
        return defaultRenderTarget;
    }
    public ITexture<float> GetDefaultDepthTarget()
    {
        return defaultDepthTarget;
    }

    public ITexture<Color> LoadTexture(Span<byte> encodedData, ResourceOptions flags)
    {
        using var stream = new MemoryStream(encodedData.Length);
        stream.Write(encodedData);
        stream.Position = 0;

        var decoder = new PNGDecoder(stream);

        //var result = Graphics.Create

        return null;
    }

    public void SetResourceLifetime(int lifetimeInFrames)
    {
        throw new NotImplementedException();
    }

    public void Initialize(Application application)
    {
        application.Dispatcher.Subscribe<RenderMessage>(AfterRender, ListenerPriority.After);
        application.Dispatcher.Subscribe<RenderMessage>(BeforeRender, ListenerPriority.High);
        application.Dispatcher.Subscribe<ResizeMessage>(Resize, ListenerPriority.High);
    }

    private void BeforeRender(RenderMessage message)
    {
    }

    private void Resize(ResizeMessage message)
    {
        if (message.Width == 0 || message.Height == 0)
            return;

        this.resources.Device.ImmediateContext.ClearState();
        defaultRenderTarget.Dispose();
        resources.Resize(message.Width, message.Height);
        defaultRenderTarget = new D3D11Texture<Color>(resources, resources.SwapChain.GetBuffer<ID3D11Texture2D>(0));
    }

    public ICanvas GetFrameCanvas()
    {
        return this.frameCanvas;
    }

    public void InvalidateShader(Type shaderType)
    {
        var shaderObj = FindShader(shaderType);

        if (shaderObj is not null)
        {
            resources.Shaders.Remove(shaderObj);
            shaderObj.Dispose();
        }
    }

    private D3D11Object FindShader(Type shaderType)
    {
        return resources.Shaders.SingleOrDefault(obj => 
            obj.GetType().GenericTypeArguments.FirstOrDefault() == shaderType
            );
    }

    public IRenderer CreateRenderer(IGraphicsQueue queue)
    {
        queue ??= this.immediateQueue;

        if (queue is not D3D11QueueBase d3dQueue)
        {
            throw new Exception();
        }

        return new D3D11Renderer(resources, d3dQueue);
    }

    record NullCanvas(ITexture<Color> Target) : ICanvas
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

        public void DrawTexture(ITexture<Color> texture, Rectangle source, Rectangle destination)
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

        public void ResetState()
        {
        }

        void ICanvas.PushState()
        {
        }

        class NullCanvasState : CanvasState
        {
        }
    }

}
