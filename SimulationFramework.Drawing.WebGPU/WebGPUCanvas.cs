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
    private readonly Stack<WebGPUCanvasState> states = new();
    private WebGPUCanvasState? defaultState;

    private TriangleRenderer triangleRenderer;
    private ArcRenderer arcRenderer;
    private RectangleRenderer rectangleRenderer;
    private ColoredRectangleRenderer coloredRectangleRenderer;


    private Renderer? currentRenderer;
    private RenderPassEncoder? renderPassEncoder;

    private ShaderSnippet FrameConstShaderPart;

    internal struct FrameConstUniforms
    {
        public Matrix4x4 viewportMatrix;
    }

    internal FrameConstUniforms uniformData;
    internal Buffer uniformBuffer;

    public WebGPUCanvas(GraphicsResources resources, ITextureViewProvider target)
    {
        this.resources = resources;
        this.target = target;
        commandEncoder = resources.Device.CreateCommandEncoder(default);

        // triangleRenderer = new(resources, this);
        coloredRectangleRenderer = new(resources, this);
    }

    private WebGPUCanvasState GetCurrentState()
    {
        defaultState ??= new();
        return states.TryPeek(out var state) ? state : defaultState;
    }

    public void Clear(Color color)
    {
        MakeRendererCurrent(null);

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

        renderPassEncoder = commandEncoder.BeginRenderPass(new()
        {
            ColorAttachments = [colorAttachment],
        });
    }

    public void Dispose()
    {
        commandEncoder.Dispose();
    }

    public void DrawArc(Rectangle bounds, float begin, float end, bool includeCenter)
    {
        MakeRendererCurrent(arcRenderer);
        arcRenderer.RenderArc(bounds, begin, end, GetCurrentState().FillColor);
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
        var state = GetCurrentState();
        if (state.DrawMode == DrawMode.Fill)
        {
            MakeRendererCurrent(coloredRectangleRenderer);
            coloredRectangleRenderer.RenderRectangle(rect, state.FillColor);
        }
        else
        {
            MakeRendererCurrent(rectangleRenderer);
            rectangleRenderer.RenderRectangle(rect);
        }

        return;

        MakeRendererCurrent(rectangleRenderer);

        Span<Vector2> vertices = [
            new(rect.X, rect.Y),
            new(rect.X + rect.Width, rect.Y),
            new(rect.X, rect.Y + rect.Height),
            new(rect.X + rect.Width, rect.Y),
            new(rect.X + rect.Width, rect.Y + rect.Height),
            new(rect.X, rect.Y + rect.Height),
            ];

        Span<Color> colors = [
            State.FillColor,
            State.FillColor,
            State.FillColor,
            State.FillColor,
            State.FillColor,
            State.FillColor,
        ];

        MakeRendererCurrent(triangleRenderer);
        triangleRenderer.RenderTriangles(vertices, colors);
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
        MakeRendererCurrent(null);

        triangleRenderer.OnFlush();

        uniformData.viewportMatrix = Matrix4x4.CreateTranslation(-Target.Width / 2f, -Target.Height / 2f, 0) * Matrix4x4.CreateOrthographic(Target.Width, -Target.Height, 0, 1);
        resources.Queue.WriteBuffer(uniformBuffer, 0, [uniformData]);

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

    private void MakeRendererCurrent(Renderer? renderer)
    {
        if (currentRenderer != null)
        {
            renderPassEncoder ??= commandEncoder.BeginRenderPass(new RenderPassDescriptor()
            {
                ColorAttachments =
                [
                    new RenderPassColorAttachment(target.GetView(), null!, LoadOp.Load, StoreOp.Store, default)
                ]
            });

            if (currentRenderer != renderer)
            {
                currentRenderer.Submit(this.renderPassEncoder);
            }

            if (renderer == null)
            {
                renderPassEncoder?.End();
                renderPassEncoder?.Dispose();
                renderPassEncoder = null;
            }
        }

        currentRenderer = renderer;
    }
}