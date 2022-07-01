using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using SimulationFramework.Drawing;
using SkiaSharp;

namespace SimulationFramework.SkiaSharp;

///// <summary>
///// Allows drawing to an <see cref="SKCanvas"/> via the <see cref="ICanvas"/> interface.
///// </summary>
//internal sealed class SkiaCanvas : ICanvas
//{
//    /// <inheritdoc/>
//    public ITexture Target { get; }

//    /// <inheritdoc/>
//    public DrawMode DrawMode { get => currentState.DrawMode; set => currentState.UpdateDrawMode(value); }

//    /// <inheritdoc/>
//    public float StrokeWidth { get => currentState.StrokeWidth; set => currentState.UpdateStrokeWidth(value); }

//    /// <inheritdoc/>
//    public Color Color { get => currentState.FillColor; set => currentState.UpdateColor(value); }

//    /// <inheritdoc/>
//    public Matrix3x2 Transform { get => currentState.Transform; set => currentState.UpdateTransform(value); }

//    /// <inheritdoc/>
//    public FillTexture FillTexture { get => currentState.FillTexture; set => currentState.UpdateFillTexture(value); }

//    /// <inheritdoc/>
//    public Gradient Gradient { get => currentState.Gradient; set => currentState.UpdateGradient(value); }

//    /// <inheritdoc/>
//    public Rectangle? ClipRegion { get => currentState.ClipRect; set => currentState.UpdateClipRect(value); }

//    // the canvas maintains a stack of state objects that can be controlled by the user
//    private readonly Stack<SkiaCanvasState> stateStack = new();
//    private SkiaCanvasState currentState = SkiaCanvasState.CreateDefault();

//    // the graphics provider which this canvas belongs to
//    private readonly SkiaGraphicsProvider provider;

//    // internal skcanvas
//    private readonly SKCanvas canvas;

//    // do we own our skcanvas?
//    private readonly bool owner; 

//    public SkiaCanvas(SkiaGraphicsProvider provider, ITexture texture, SKCanvas canvas, bool owner)
//    {
//        this.Target = texture;
//        this.provider = provider;
//        this.canvas = canvas;
//        this.owner = owner;
//    }

//    /// <summary>
//    /// Returns this <see cref="SkiaCanvas"/>'s internal <see cref="SKCanvas"/>.
//    /// </summary>
//    internal SKCanvas GetSKCanvas()
//    {
//        return this.canvas;
//    }

//    public void Clear(Color color)
//    {
//        canvas.Clear(color.AsSKColor());
//    }

//    public void Dispose()
//    {
//        if (owner)
//        {
//            canvas.Dispose();
//        }

//        currentState.Dispose();

//        while (stateStack.Any())
//        {
//            stateStack.Pop().Dispose();
//        }
//    }

//    public void DrawArc(Rectangle bounds, float begin, float end, bool includeCenter)
//    {
//        if (bounds.Width <= 0 || bounds.Height <= 0)
//            return;

//        if (end < begin)
//        {
//            (begin, end) = (end, begin);
//        }

//        if (end - begin >= MathF.Tau)
//        {
//            canvas.DrawOval(bounds.AsSKRect(), currentState.Paint);
//        }
//        else 
//        {
//            canvas.DrawArc(bounds.AsSKRect(), begin, end - begin, includeCenter, currentState.Paint);
//        }
//    }

//    public void DrawLine(Vector2 p1, Vector2 p2)
//    {
//        if (p1 == p2)
//            return;

//        canvas.DrawLine(p1.AsSKPoint(), p2.AsSKPoint(), currentState.Paint);
//    }

//    public void DrawPolygon(Span<Vector2> polygon)
//    {
//        using SKPath path = new();

//        bool shouldClose = polygon[0] != polygon[^1];

//        unsafe
//        {
//            fixed (Vector2* polygonPtr = polygon)
//            {
//                SkiaNativeApi.sk_path_add_poly(path.Handle, polygonPtr, polygon.Length, shouldClose);
//            }
//        }

//        canvas.DrawPath(path, currentState.Paint);
//    }

//    public void DrawRoundedRect(Rectangle rect, float radius)
//    {
//        if (radius <= 0)
//        {
//            canvas.DrawRect(rect.AsSKRect(), currentState.Paint);
//        }
//        else
//        {
//            canvas.DrawRoundRect(rect.AsSKRect(), radius, radius, currentState.Paint);
//        }
//    }

//    public void DrawText(string text, Vector2 position, Alignment alignment = Alignment.TopLeft)
//    {
//        throw new NotImplementedException();
//    }

//    public void DrawTexture(ITexture texture, Rectangle source, Rectangle destination)
//    {
//        if (texture is not SkiaTexture skiaTexture)
//            throw new ArgumentException("texture must be a texture created by the skiasharp renderer!", nameof(texture));

//        canvas.DrawBitmap(skiaTexture.GetBitmap(), source.AsSKRect(), destination.AsSKRect());
//    }

//    public Vector2 MeasureText(string text, float maxLength, out int charsMeasured)
//    {
//        Vector2 result;
//        var paint = currentState.Paint;

//        if (maxLength > 0)
//        {
//            charsMeasured = (int)paint.BreakText(text, maxLength, out result.X);
//            result.Y = paint.TextSize;
//        }
//        else
//        {
//            charsMeasured = text.Length;
//            result = new(paint.MeasureText(text), paint.TextSize);
//        }

//        return result;
//    }

//    public CanvasSession PushState()
//    {
//        currentState.Deactivate();

//        stateStack.Push(currentState);
//        currentState = currentState.Clone();

//        currentState.Activate(GetSKCanvas());

//        return new CanvasSession(this);
//    }

//    public void PopState()
//    {
//        if (!stateStack.Any())
//            throw new InvalidOperationException();

//        currentState.Deactivate();

//        currentState.Dispose();
//        currentState = stateStack.Pop();

//        currentState.Activate(GetSKCanvas());
//    }

//    public void ResetState()
//    {
//        currentState.Deactivate();

//        currentState.Dispose();
//        currentState = SkiaCanvasState.CreateDefault();

//        currentState.Activate(GetSKCanvas());
//    }

//    public bool SetFont(string fontName, TextStyles styles, float size)
//    {
//        throw new NotImplementedException();
//    }

//    public void Flush()
//    {
//        canvas.Flush();
//    }

//    public void SetFillTexture(ITexture texture, Matrix3x2 transform, TileMode tileModeX, TileMode tileModeY)
//    {
//        throw new NotImplementedException();
//    }
//}

internal sealed class SkiaCanvas : ICanvas
{
    public ITexture Target { get; }

    public CanvasState State => currentState;

    ITexture ICanvas.Target { get; }

    //the canvas maintains a stack of state objects that can be controlled by the user
    private readonly Stack<SkiaCanvasState> stateStack = new();
    private SkiaCanvasState currentState;

    // the graphics provider which this canvas belongs to
    private readonly SkiaGraphicsProvider provider;
    // internal skcanvas
    private readonly SKCanvas canvas;
    // do we own this skcanvas object?
    private readonly bool owner;

    public SkiaCanvas(SkiaGraphicsProvider provider, ITexture texture, SKCanvas canvas, bool owner)
    {
        this.Target = texture;
        this.provider = provider;
        this.canvas = canvas;
        this.owner = owner;

        ResetState();
    }

    public SKCanvas GetSKCanvas() => canvas;

    public void Clear(Color color) => canvas.Clear(color.AsSKColor());
    public void Flush() => canvas.Flush();
    public void DrawLine(Vector2 p1, Vector2 p2) => canvas.DrawLine(p1.AsSKPoint(), p2.AsSKPoint(), currentState.Paint);
    public void DrawRoundedRect(Rectangle rect, float radius)
    {
        if (rect.Width <= 0 || rect.Height <= 0)
            return;

        if (radius <= 0)
        {
            canvas.DrawRect(rect.AsSKRect(), currentState.Paint);
        }
        else
        {
            canvas.DrawRoundRect(rect.AsSKRect(), radius, radius, currentState.Paint);
        }
    }

    public void DrawArc(Rectangle bounds, float begin, float end, bool includeCenter)
    {
        if (bounds.Width <= 0 || bounds.Height <= 0)
            return;

        if (end < begin)
        {
            (begin, end) = (end, begin);
        }

        if (end - begin >= MathF.Tau)
        {
            canvas.DrawOval(bounds.AsSKRect(), currentState.Paint);
        }
        else
        {
            canvas.DrawArc(bounds.AsSKRect(), begin, end - begin, includeCenter, currentState.Paint);
        }
    }

    public void DrawTexture(ITexture texture, Rectangle source, Rectangle destination)
    {
        if (texture is not SkiaTexture skTexture)
            throw new ArgumentException("texture must be a texture created by the skiasharp renderer!", nameof(texture));

        canvas.DrawBitmap(skTexture.GetBitmap(), source.AsSKRect(), destination.AsSKRect());
    }

    public unsafe void DrawPolygon(Span<Vector2> polygon)
    {
        using var path = new SKPath();

        bool shouldClose = true;
        shouldClose &= polygon[0] != polygon[polygon.Length - 1];
        shouldClose &= (this.State.DrawMode == DrawMode.Fill || this.State.DrawMode == DrawMode.Gradient);

        fixed (Vector2* polygonPtr = polygon)
            SkiaNativeApi.sk_path_add_poly(path.Handle, polygonPtr, polygon.Length, shouldClose);

        canvas.DrawPath(path, currentState.Paint);
    }

    public void DrawText(string text, Vector2 position, Alignment alignment = Alignment.TopLeft)
    {
        throw new NotImplementedException();
    }

    public Vector2 MeasureText(string text, float maxLength, out int charsMeasured)
    {
        throw new NotImplementedException();
    }

    public bool SetFont(string fontName, TextStyles styles, float size)
    {
        throw new NotImplementedException();
    }

    public CanvasSession PushState()
    {
        stateStack.Push(currentState);
        currentState = new SkiaCanvasState(this.GetSKCanvas(), currentState);
        
        return new(this);
    }

    public void PopState()
    {
        currentState.Dispose();

        if (stateStack.Count == 0)
            throw new InvalidOperationException();

        currentState = stateStack.Pop();
    }

    public void ResetState()
    {
        currentState?.Dispose();
        currentState = new SkiaCanvasState(this.GetSKCanvas(), null);
    }

    public void Dispose()
    {
        if (owner)
            canvas.Dispose();

        currentState.Dispose();

        while (stateStack.Count > 0)
        {
            stateStack.Pop().Dispose();
        }
    }
}