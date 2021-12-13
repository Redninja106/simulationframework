using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace SimulationFramework.SkiaSharp;

internal sealed class SkiaCanvas : ICanvas
{
    private Stack<CanvasState> canvasStates = new();

    internal readonly SKCanvas canvas;
    internal readonly bool owner;
    internal SKPaint paint = new();

    public Matrix3x2 Transform { get => canvas.TotalMatrix.AsMatrix3x2(); set => canvas.SetMatrix(value.AsSKMatrix()); }

    public SkiaCanvas(SKCanvas canvas, bool owner)
    {
        this.canvas = canvas;
        this.owner = owner;
    }

    public void Clear(Color color)
    {
        this.canvas.Clear(color.AsSKColor());
    }

    public void Dispose()
    {
        if (owner)
            canvas.Dispose();
    }

    public void Flush()
    {
        canvas.Flush();
    }

    public void Mode(DrawMode mode)
    {
    }

    public CanvasSession Push()
    {
        this.canvasStates.Push(new CanvasState(paint, Transform));
        this.paint = this.paint.Clone();

        return new CanvasSession(this);
    }

    public void Pop()
    {
        var state = this.canvasStates.Pop();
        this.paint.Dispose();
        this.paint = state.paint;
        this.Transform = state.transform;
    }

    public void Translate(float x, float y)
    {
        canvas.Translate(x, y);
    }

    public void Rotate(float angle)
    {
        canvas.RotateRadians(angle);
    }

    public void Scale(float scaleX, float scaleY)
    {
        canvas.Scale(scaleX, scaleY);
    }

    public void DrawLine(Vector2 p1, Vector2 p2, Color color)
    {
        DrawLine(p1.X, p1.Y, p2.X, p2.Y, color);
    }

    public void DrawLine(float x1, float y1, float x2, float y2, Color color)
    {
        this.paint.Color = color.AsSKColor();
        canvas.DrawLine(x1, y1, x2, y2, this.paint);
    }

    public void DrawRect(float x, float y, float width, float height, Color color, Alignment alignment = Alignment.TopLeft)
    {
        DrawRect(new(x, y), new(width, height), color, alignment);
    }

    public void DrawRect(Vector2 position, Vector2 size, Color color, Alignment alignment = Alignment.TopLeft)
    {
        DrawRect(new Rectangle(position, size), color, alignment);
    }

    public void DrawRect(Rectangle rect, Color color, Alignment alignment = Alignment.TopLeft)
    {
        this.paint.Color = color.AsSKColor();
        canvas.DrawRect(rect.AsSKRect(), this.paint);
    }

    public void DrawEllipse(float x, float y, float radiusX, float radiusY, Color color, Alignment alignment = Alignment.Center)
    {
        DrawEllipse(new(x, y), new(radiusX, radiusY), color, alignment);
    }

    public void DrawEllipse(Vector2 position, Vector2 radii, Color color, Alignment alignment = Alignment.Center)
    {
        DrawEllipse(new Rectangle(position.X, position.Y, radii.X * 2, radii.Y * 2, alignment), color);
    }

    public void DrawEllipse(Rectangle bounds, Color color)
    {
        this.paint.Color = color.AsSKColor();
        canvas.DrawOval(bounds.AsSKRect(), this.paint);
    }

    public void DrawSurface(ISurface surface, Alignment alignment = Alignment.TopLeft)
    {
        DrawSurface(surface, Vector2.Zero, new(surface?.Width ?? 0f, surface?.Height ?? 0f), alignment);
    }

    public void DrawSurface(ISurface surface, float x, float y, Alignment alignment = Alignment.TopLeft)
    {
        DrawSurface(surface, x, y, surface.Width, surface.Height, Alignment.Center);
    }

    public void DrawSurface(ISurface surface, float x, float y, float width, float height, Alignment alignment = Alignment.TopLeft)
    {
        DrawSurface(surface, position: new(x, y), size: new(width, height), alignment);
    }

    public void DrawSurface(ISurface surface, Vector2 position, Vector2 size, Alignment alignment = Alignment.TopLeft)
    {
        if (surface is null)
            throw new ArgumentNullException(nameof(surface));

        DrawSurface(surface, new Rectangle(0, 0, surface.Width, surface.Height), new Rectangle(position.X, position.Y, size.X, size.Y, alignment));
    }

    public void DrawSurface(ISurface surface, Rectangle source, Rectangle destination)
    {
        if (surface is SkiaSurface skiaSurface)
            canvas.DrawBitmap(skiaSurface.bitmap, source.AsSKRect(), destination.AsSKRect());
    }

    public struct CanvasState
    {
        public SKPaint paint;
        public Matrix3x2 transform;

        public CanvasState(SKPaint paint, Matrix3x2 transform)
        {
            this.paint = paint;
            this.transform = transform;
        }
    }
}