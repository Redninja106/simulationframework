using SimulationFramework.Drawing.WebGPU.Renderers;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using WebGPU;

namespace SimulationFramework.Drawing.WebGPU;
internal class WebGPUCanvas : ICanvas
{
    public ITexture Target => target;
    public CanvasState State => GetCurrentState();

    private GraphicsResources resources;
    private ITextureViewProvider target;

    private CommandEncoder commandEncoder;
    private RenderPassEncoder renderPassEncoder;
    private readonly Stack<WebGPUCanvasState> states = new();
    private WebGPUCanvasState? defaultState;

    private TriangleRenderer triangleRenderer;

    private Renderer? currentRenderer;

    public WebGPUCanvas(GraphicsResources resources, ITextureViewProvider target)
    {
        this.resources = resources;
        this.target = target;
        commandEncoder = resources.Device.CreateCommandEncoder(default);
    }

    private WebGPUCanvasState GetCurrentState()
    {
        defaultState ??= new();
        return states.TryPeek(out var state) ? state : defaultState;
    }

    public void Clear(Color color)
    {
        RenderPassColorAttachment colorAttachment = new()
        {
            View = target.GetView(),
            ClearValue = new()
            {
                R = color.R * (1f / 255f),
                G = color.G * (1f / 255f),
                B = color.B * (1f / 255f),
                A = color.A * (1f / 255f),
            },
            LoadOp = LoadOp.Clear,
            StoreOp = StoreOp.Store,
        };

        RenderPassEncoder renderPassEncoder = commandEncoder.BeginRenderPass(new()
        {
            ColorAttachments = [colorAttachment],
        });

        renderPassEncoder.End();
    }

    public void Dispose()
    {
        commandEncoder.Dispose();
    }

    public void DrawArc(Rectangle bounds, float begin, float end, bool includeCenter)
    {
        throw new NotImplementedException();
    }

    public void DrawLine(Vector2 p1, Vector2 p2)
    {
        throw new NotImplementedException();
    }

    public void DrawPolygon(ReadOnlySpan<Vector2> polygon, bool close = true)
    {
        throw new NotImplementedException();
    }

    public void DrawRect(Rectangle rect)
    {
        Span<Vector2> vertices = [
            new(rect.X, rect.Y),
            new(rect.X + rect.Width, rect.Y),
            new(rect.X, rect.Y + rect.Height),
            new(rect.X + rect.Width, rect.Y + rect.Height),
            ];

        Span<ushort> indices = [0, 1, 2, 1, 3, 2];
        var e = resources.Device.CreateRenderBundleEncoder(new()
        {

        });
        e.
        MakeRendererCurrent(triangleRenderer);
        triangleRenderer.RenderTriangles(vertices, indices);
    }

    public void DrawRoundedRect(Rectangle rect, float radius)
    {
        throw new NotImplementedException();
    }

    public void DrawText(ReadOnlySpan<char> text, Vector2 position, Alignment alignment = Alignment.TopLeft, TextBounds origin = TextBounds.BestFit)
    {
        throw new NotImplementedException();
    }

    public void DrawTexture(ITexture texture, Rectangle source, Rectangle destination)
    {
        throw new NotImplementedException();
    }

    public void Flush()
    {
        using var commandBuffer = commandEncoder.Finish(default);
        resources.Queue.Submit([commandBuffer]);
        commandEncoder.Dispose();
        commandEncoder = resources.Device.CreateCommandEncoder(default);
    }

    public Vector2 MeasureText(ReadOnlySpan<char> text, float maxLength, out int charsMeasured, TextBounds bounds = TextBounds.BestFit)
    {
        throw new NotImplementedException();
    }

    public void PopState()
    {
        if (states.Count <= 0)
            throw new InvalidOperationException("No state to pop!");

        states.Pop();
    }

    public void PushState()
    {
        var state = new WebGPUCanvasState(GetCurrentState());
        states.Push(state);
    }

    public void ResetState()
    {
        GetCurrentState().Reset();
    }

    private void MakeRendererCurrent(Renderer renderer)
    {
        currentRenderer?.FinishPass();
        currentRenderer = renderer;
        currentRenderer.StartPass(this.commandEncoder);
    }
}