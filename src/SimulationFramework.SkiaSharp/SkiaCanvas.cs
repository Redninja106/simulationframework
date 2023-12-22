using System;
using System.Collections.Generic;
using System.Numerics;
using SimulationFramework.Drawing;
using SkiaSharp;

namespace SimulationFramework.SkiaSharp;

internal sealed class SkiaCanvas : SkiaGraphicsObject, ICanvas
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

    private SkiaTexture SkiaTextureTarget => Target as SkiaTexture;

    public SkiaCanvas(SkiaGraphicsProvider provider, ITexture target, SKCanvas canvas, bool owner)
    {
        this.Target = target;
        this.provider = provider;
        this.canvas = canvas;
        this.owner = owner;
        ResetState();
    }

    public SKCanvas GetSKCanvas() => canvas;

    public void Clear(Color color)
    {
        canvas.Clear(color.AsSKColor());
        SkiaTextureTarget?.InvalidatePixels();
    }
    public void Flush() => canvas.Flush();
    public void DrawLine(Vector2 p1, Vector2 p2)
    {
        canvas.DrawLine(p1.AsSKPoint(), p2.AsSKPoint(), currentState.Paint);
        SkiaTextureTarget?.InvalidatePixels();
    }
    public void DrawRoundedRect(Rectangle rect, float radius)
    {
        if (rect.Width <= 0 || rect.Height <= 0)
            return;
        
        canvas.DrawRoundRect(rect.AsSKRect(), radius, radius, currentState.Paint);
        SkiaTextureTarget?.InvalidatePixels();
    }

    public void DrawRect(Rectangle rectangle)
    {
        canvas.DrawRect(rectangle.AsSKRect(), currentState.Paint);
        SkiaTextureTarget?.InvalidatePixels();
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
        
        SkiaTextureTarget?.InvalidatePixels();
    }

    public void DrawTexture(ITexture texture, Rectangle source, Rectangle destination)
    {
        if (texture is not SkiaTexture skTexture)
            throw new ArgumentException("texture must be a texture created by the skiasharp renderer!", nameof(texture));

        canvas.DrawImage(skTexture.GetImage(), source.AsSKRect(), destination.AsSKRect());
        SkiaTextureTarget?.InvalidatePixels();
    }

    public unsafe void DrawPolygon(ReadOnlySpan<Vector2> polygon, bool close)
    {
        bool alreadyClosed = Polygon.IsClosed(polygon);
        bool isFill = (this.State.DrawMode == DrawMode.Fill || this.State.DrawMode == DrawMode.Gradient);
        bool shouldClose = !alreadyClosed && (close || isFill);

        using var path = new SKPath();
        path.FillType = SKPathFillType.EvenOdd;
        fixed (Vector2* polygonPtr = polygon)
            SkiaNativeApi.sk_path_add_poly(path.Handle, polygonPtr, polygon.Length, shouldClose);

        canvas.DrawPath(path, currentState.Paint);
        SkiaTextureTarget?.InvalidatePixels();
    }

    public void DrawText(ReadOnlySpan<char> text, Vector2 position, Alignment alignment = Alignment.TopLeft, TextBounds bounds = TextBounds.BestFit)
    {
        var fontMetrics = currentState.Paint.FontMetrics;

        currentState.Paint.SubpixelText = true;
        currentState.Paint.TextAlign = SKTextAlign.Left;
        var size = MeasureTextInternal(text, 0, out _, bounds, out Vector2 skiaOffset);

        Rectangle textRect = new(position, size, alignment);
        
        var drawPos = skiaOffset + textRect.GetAlignedPoint(Alignment.BottomLeft);
        using var blob = SKTextBlob.Create(text, currentState.Paint.GetFont());
        canvas.DrawText(blob, drawPos.X, drawPos.Y, currentState.Paint);

        if (currentState.FontStyle.HasFlag(FontStyle.Underline))
        {
            using var underline = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = fontMetrics.UnderlineThickness ?? 0,
                Color = currentState.FillColor.AsSKColor()
            };

            float y = skiaOffset.Y + textRect.Y + textRect.Height + (fontMetrics.UnderlinePosition ?? 0);

            canvas.DrawLine(textRect.X, y, textRect.X + textRect.Width, y, underline);
        }
        
        if (currentState.FontStyle.HasFlag(FontStyle.Strikethrough))
        {
            using var strikethrough = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = fontMetrics.StrikeoutThickness ?? 0,
                Color = currentState.FillColor.AsSKColor()
            };

            float y = skiaOffset.Y + textRect.Y + textRect.Height + (fontMetrics.StrikeoutPosition ?? 0);

            canvas.DrawLine(textRect.X, y, textRect.X + textRect.Width, y, strikethrough);
        }
        SkiaTextureTarget?.InvalidatePixels();
    }

    public Vector2 MeasureText(ReadOnlySpan<char> text, float maxWidth, out int charsMeasured, TextBounds origin)
    {
        return MeasureTextInternal(text, maxWidth, out charsMeasured, origin, out _);
    }

    private Vector2 MeasureTextInternal(ReadOnlySpan<char> text, float maxWidth, out int charsMeasured, TextBounds bounds, out Vector2 skiaOffset)
    {
        // ok wow, this is bad:
        // SKPaint.MeasureText seems to return a zero-size rectangle for really small text sizes,
        // even when the transform matrix may still scale it back up. 
        // As a workaround, we measure with the TextSize set to 100, then manually rescale the bounds from there.
        const float measureTextWorkaroundSize = 100;

        // save old text size
        var prevTextSize = currentState.Paint.TextSize;
        currentState.Paint.TextSize = measureTextWorkaroundSize;

        // if we have a max width, make sure we don't exceed it
        charsMeasured = text.Length;
        if (maxWidth > 0)
        {
            charsMeasured = (int)currentState.Paint.BreakText(text, maxWidth);
        }

        // actually measure the text
        SKRect skiabounds = default;
        currentState.Paint.MeasureText(text, ref skiabounds);

        // restore old text size
        currentState.Paint.TextSize = prevTextSize;

        // rescale rectangle
        skiabounds.Left *= (1f / measureTextWorkaroundSize) * prevTextSize;
        skiabounds.Right *= (1f / measureTextWorkaroundSize) * prevTextSize;
        skiabounds.Top *= (1f / measureTextWorkaroundSize) * prevTextSize;
        skiabounds.Bottom *= (1f / measureTextWorkaroundSize) * prevTextSize;

        var fontMetrics = currentState.Paint.FontMetrics;
        Vector2 offset = Vector2.Zero;
        float width = skiabounds.Width;
        float height = skiabounds.Height;
        switch (bounds)
        {
            case TextBounds.BestFit:
                offset.X -= skiabounds.Left;
                offset.Y -= skiabounds.Bottom;

                break;
            case TextBounds.Largest:
                offset.X -= skiabounds.Left;
                offset.Y -= fontMetrics.Descent;
                height = fontMetrics.CapHeight + fontMetrics.Descent;
                break;
            case TextBounds.Smallest:
                offset.X -= skiabounds.Left;
                height = fontMetrics.XHeight;
                break;
            default:
                throw new ArgumentException(null, nameof(bounds));
        }

        // if using best fit, include underline
        if (bounds is TextBounds.BestFit && this.currentState.FontStyle.HasFlag(FontStyle.Underline))
        {
            float underlinePos = offset.Y + height + (fontMetrics.UnderlinePosition ?? 0);
            float underlineThickness = (fontMetrics.UnderlineThickness ?? 0);
            float underlineBottom = underlinePos + underlineThickness / 2f;
            
            // how much larger does the text's bounds get?
            float growth = underlineBottom - height;
            if (growth > 0)
            {
                height += growth;
                offset.Y -= growth;
            }
        }

        skiaOffset = offset;
        return new(width, height);
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

    public override void Dispose()
    {
        if (IsDisposed)
            return;

        if (owner)
            canvas.Dispose();

        currentState.Dispose();

        while (stateStack.Count > 0)
        {
            stateStack.Pop().Dispose();
        }
    }
}