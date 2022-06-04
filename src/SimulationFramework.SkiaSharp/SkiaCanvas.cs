using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using SimulationFramework.Drawing.Canvas;
using SkiaSharp;

namespace SimulationFramework.SkiaSharp;

internal sealed class SkiaCanvas : ICanvas
{
    public ITexture Target { get; }

    public CanvasState State => currentState;
    
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
        path.FillType = SKPathFillType.EvenOdd;
        fixed (Vector2* polygonPtr = polygon)
            SkiaNativeApi.sk_path_add_poly(path.Handle, polygonPtr, polygon.Length, shouldClose);

        canvas.DrawPath(path, currentState.Paint);
    }

    public void DrawText(string text, Vector2 position, Alignment alignment = Alignment.TopLeft)
    {
        SKRect skbounds = default;
        currentState.Paint.MeasureText(text, ref skbounds);
        Rectangle bounds = new(position, new(skbounds.Width, skbounds.Height), alignment);
        canvas.DrawText(text, bounds.X - skbounds.Left, bounds.Y - skbounds.Top, currentState.Paint);

        if (currentState.FontStyle.HasFlag(FontStyle.Underline))
        {
            var font = currentState.Paint.FontMetrics;

            using var underline = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = font.UnderlineThickness ?? 0,
                Color = currentState.FillColor.AsSKColor()
            };

            float y = bounds.Y + font.CapHeight + (font.UnderlinePosition ?? 0);

            canvas.DrawLine(bounds.X, y, bounds.X + bounds.Width, y, underline);
        }
        
        if (currentState.FontStyle.HasFlag(FontStyle.Strikethrough))
        {
            var font = currentState.Paint.FontMetrics;

            using var strikethrough = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = font.StrikeoutThickness ?? 0,
                Color = currentState.FillColor.AsSKColor()
            };

            float y = bounds.Y + font.CapHeight + (font.StrikeoutPosition ?? 0);

            canvas.DrawLine(bounds.X, y, bounds.X + bounds.Width, y, strikethrough);
        }
    }

    public Vector2 MeasureText(string text, float maxWidth, out int charsMeasured)
    {
        if (text == null)
        {
            charsMeasured = 0;
            return Vector2.Zero;
        }

        int length = text.Length;
        if (maxWidth > 0)
        {
            length = (int)currentState.Paint.BreakText(text, maxWidth);
        }
            
        SKRect bounds = default;
        currentState.Paint.MeasureText(text.Length == length ? text : text[0..length], ref bounds);
        charsMeasured = length;
        return new(bounds.Width, bounds.Height);
    }

    public void PushState()
    {
        stateStack.Push(currentState);
        currentState = new SkiaCanvasState(this.GetSKCanvas(), currentState);
    }

    public void PopState()
    {
        currentState.Dispose();

        if (stateStack.Count == 0)
            throw new InvalidOperationException();

        currentState = stateStack.Pop();
        currentState.Apply();
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