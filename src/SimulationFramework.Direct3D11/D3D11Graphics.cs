﻿using SimulationFramework.Serialization.PNG;
using SimulationFramework.Drawing.Direct3D11.Buffers;
using SimulationFramework.Messaging;
using System.Numerics;
using System.Runtime.CompilerServices;
using Vortice.D3DCompiler;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;
using SimulationFramework.Drawing.RenderPipeline;
using SimulationFramework.Shaders;
using SimulationFramework.Shaders.Compiler;

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
        resources.SwapChain.Present(0);
    }

    public IBuffer<T> CreateBuffer<T>(int size, ResourceOptions flags) where T : unmanaged
    {
        return new D3D11Buffer<T>(this.resources, size, flags);
    }

    public ITexture CreateTexture(int width, int height, Span<Color> data, ResourceOptions flags)
    {
        return new D3D11Texture(this.resources, width, height, data, flags);
    }

    public void Dispose()
    {
        frameTexture.Dispose();
        resources.Dispose();
    }

    public ITexture GetFrameTexture()
    {
        return frameTexture;
    }

    public ITexture LoadTexture(Span<byte> encodedData, ResourceOptions flags)
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

    public IRenderer GetRenderer()
    {
        return resources.ImmediateRenderer;
    }

    public void Initialize(Application application)
    {
        application.Dispatcher.Subscribe<RenderMessage>(AfterRender, ListenerPriority.Low);
        application.Dispatcher.Subscribe<RenderMessage>(BeforeRender, ListenerPriority.High);
        application.Dispatcher.Subscribe<ResizeMessage>(Resize, ListenerPriority.High);
    }

    private void BeforeRender(RenderMessage message)
    {
        resources.ImmediateRenderer.BeginFrame();
    }

    private void Resize(ResizeMessage message)
    {
        if (message.Width == 0 || message.Height == 0)
            return;

        this.resources.ImmediateRenderer.DeviceContext.ClearState();
        frameTexture.Dispose();
        resources.Resize(message.Width, message.Height);
        frameTexture = new D3D11Texture(resources, resources.SwapChain.GetBuffer<ID3D11Texture2D>(0));
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
