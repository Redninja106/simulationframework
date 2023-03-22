using SimulationFramework.Shaders;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Implementations.Canvas;
public class RendererCanvas : ICanvas
{
    public ITexture<Color> Target => renderer.RenderTarget;
    public CanvasState State { get; }

    private readonly IRenderingContext renderer;
    private Matrix4x4 viewportTransform;

    public RendererCanvas()
    {
        renderer = Graphics.CreateRenderingContext();
        renderer.RenderTarget = Graphics.DefaultRenderTarget;

        float aspectRatio = Target.Width / (float)Target.Height;
        viewportTransform = Matrix4x4.CreateScale(1f/aspectRatio, 1, 1);
    }

    public void Clear(Color color)
    {
        renderer.ClearRenderTarget(color);
    }

    public void Dispose()
    {
        renderer.Dispose();
    }

    public void DrawArc(Rectangle bounds, float begin, float end, bool includeCenter)
    {
        throw new NotImplementedException();
    }

    public void DrawLine(Vector2 p1, Vector2 p2)
    {
        throw new NotImplementedException();
    }

    public void DrawPolygon(Span<Vector2> polygon)
    {
        throw new NotImplementedException();
    }

    public void DrawRoundedRect(Rectangle rect, float radius)
    {
        throw new NotImplementedException();
    }

    public void DrawText(string text, Vector2 position, Alignment alignment = Alignment.TopLeft)
    {
        throw new NotImplementedException();
    }

    public void DrawTexture(ITexture<Color> texture, Rectangle source, Rectangle destination)
    {
        throw new NotImplementedException();
    }

    public void Flush()
    {
        renderer.Queue.Flush();
    }

    public Vector2 MeasureText(string text, float maxLength, out int charsMeasured)
    {
        throw new NotImplementedException();
    }

    public void PopState()
    {
        throw new NotImplementedException();
    }

    public void PushState()
    {
        throw new NotImplementedException();
    }

    public void ResetState()
    {
        throw new NotImplementedException();
    }

    public void DrawRect(Rectangle rect)
    {
    }
}

class RendererCanvasState : CanvasState
{
}

class RendererBatch
{
    readonly IRenderingContext renderer;

    public Matrix4x4 ViewportTransform;
    public List<Matrix4x4> matrices;
    public List<QuadData> quads;

    public IBuffer<Matrix4x4> matrixBuffer;
    public IBuffer<QuadData> quadBuffer;

    public int currentTransformMatrix;

    public RendererBatch(IRenderingContext renderer)
    {
        this.renderer = renderer;
    }

    public void PushQuad(Rectangle quad)
    {
        quads.Add(new QuadData
        {
            bounds = quad,
            matrix = currentTransformMatrix
        });
    }

    public void UpdateTransformMatrix(Matrix4x4 matrix)
    {
        currentTransformMatrix++;
        matrices.Add(matrix);
    }

    public void Complete()
    {
        CreateOrResizeQuadBuffer();
        CreateOrResizeMatrixBuffer();

        renderer.RenderTarget = Graphics.DefaultRenderTarget;
        renderer.SetViewport(new(0, 0, renderer.RenderTarget.Width, renderer.RenderTarget.Height));

        QuadGenVertexShader quadGenShader = new() { ViewportTransform = this.ViewportTransform };
        FillFragmentShader fillShader = new() { FillColor = ColorF.Red };

        renderer.SetVertexShader(quadGenShader);
        renderer.SetFragmentShader(fillShader);
        renderer.DrawPrimitives(PrimitiveKind.TriangleStrip, 4);
    }

    private void CreateOrResizeQuadBuffer()
    {
        var matricesData = CollectionsMarshal.AsSpan(matrices);

        if (matrixBuffer is null || matrixBuffer.Length < matrices.Count)
        {
            matrixBuffer?.Dispose();
            matrixBuffer = Graphics.CreateBuffer(matricesData);
            return;
        }

        matrixBuffer.Update(matricesData);
    }

    private void CreateOrResizeMatrixBuffer()
    {
        var quadsData = CollectionsMarshal.AsSpan(quads);

        if (quadBuffer is null || quadBuffer.Length < quads.Count)
        {
            quadBuffer?.Dispose();
            quadBuffer = Graphics.CreateBuffer(quadsData);
            return;
        }

        quadBuffer.Update(quadsData);
    }
}

struct QuadGenVertexShader : IShader
{
    const int VERTS_PER_QUAD = 4;

    [Uniform]
    public IBuffer<QuadData> quadDataBuffer;

    [Uniform]
    public IBuffer<Matrix4x4> matrixBuffer;

    [Input(InputSemantic.VertexIndex)]
    private uint vertexIndex;

    [Output(OutputSemantic.Position)]
    Vector4 position;

    [Uniform]
    public Matrix4x4 ViewportTransform;

    public void Main()
    {
        uint quadIndex = vertexIndex / VERTS_PER_QUAD;
        uint quadVertIndex = vertexIndex % VERTS_PER_QUAD;

        var quadData = quadDataBuffer[quadIndex];

        float x = quadVertIndex / 2;
        float y = quadVertIndex % 2;

        position = new(x * quadData.bounds.Width + quadData.bounds.X, y * quadData.bounds.Height + quadData.bounds.Y, 0, 1);

        position = ShaderIntrinsics.Mul(position, ViewportTransform);
        position = ShaderIntrinsics.Mul(position, matrixBuffer[quadData.matrix]);
    }
}

struct QuadData
{
    public Rectangle bounds;
    public int matrix;
}

struct FillFragmentShader : IShader
{
    [Uniform]
    public ColorF FillColor;

    [Output(OutputSemantic.Color)]
    public ColorF outputColor;

    public void Main()
    {
        outputColor = FillColor;
    }
}