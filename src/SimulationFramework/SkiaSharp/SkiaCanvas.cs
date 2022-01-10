using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace SimulationFramework.SkiaSharp;

internal sealed class SkiaCanvas : ICanvas
{
    private Stack<SkiaCanvasState> stateStack = new();

    private readonly SkiaGraphicsProvider provider;
    private readonly ISurface surface;
    private readonly SKCanvas canvas;
    private readonly bool owner;
    private SKPaint paint = new();
    private DrawMode currentMode;
    private float currentStrokeWidth;
    private Rectangle currentClipRect;
    private TextStyles currentTextStyle;
    private SKFont currentFont;
    // usually used for gradients
    private SKShader currentShader;

    public Matrix3x2 Transform { get => canvas.TotalMatrix.AsMatrix3x2(); set => canvas.SetMatrix(value.AsSKMatrix()); }
    public int Width => surface?.Width ?? Simulation.Current.TargetWidth;
    public int Height => surface?.Height ?? Simulation.Current.TargetWidth;

    public SkiaCanvas(SkiaGraphicsProvider provider, ISurface surface, SKCanvas canvas, bool owner)
    {
        this.provider = provider;
        this.surface = surface;
        this.canvas = canvas;
        this.owner = owner;
        
        canvas.Save();

        this.SetFont("arial", TextStyles.Default, 20);
    }

    internal SKCanvas GetSKCanvas()
    {
        return canvas;
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

    public CanvasSession Push()
    {
        this.stateStack.Push(new SkiaCanvasState(this));
        return new CanvasSession(this);
    }

    public void Pop()
    {
        var state = this.stateStack.Pop();
        state.Restore();
    }

    public ISurface GetSurface()
    {
        return surface;
    }

    public void ResetState()
    {
        paint?.Dispose();
        paint = new SKPaint();
        SetDrawMode(DrawMode.Fill);
        currentStrokeWidth = 1;
        SetClipRect(0, 0, 0, 0);
    }

    public void SetStrokeWidth(float strokeWidth)
    {
        paint.StrokeWidth = strokeWidth;
        this.currentStrokeWidth = strokeWidth;
    }

    public void SetDrawMode(DrawMode mode)
    {
        paint.Style = (SKPaintStyle)mode;
        this.currentMode = mode;
    }

    public bool SetFont(string fontName, TextStyles styles, float size)
    {

        currentTextStyle = styles;

        currentFont = provider.GetFont(fontName, styles, (int)size);

        paint.TextSize = (int)size;
        paint.Typeface = currentFont.Typeface;

        return true;
    }

    public Vector2 MeasureText(string text)
    {
        SKRect bounds = default;
        paint.MeasureText(text, ref bounds);
        return (bounds.Width, bounds.Height);
    }

    public void Translate(Vector2 translation) => canvas.Translate(translation.X, translation.Y);
    public void Translate(float x, float y) => canvas.Translate(x, y);
    
    public void Rotate(float angle) => canvas.RotateRadians(angle);
    public void Rotate(float angle, float centerX, float centerY) => canvas.RotateRadians(angle, centerX, centerY);
    public void Rotate(float angle, Vector2 center) => canvas.RotateRadians(angle, center.X, center.Y);

    public void Scale(float scale) => Scale(scale, scale);
    public void Scale(Vector2 scale) => Scale(scale.X, scale.Y);
    public void Scale(float scaleX, float scaleY) => Scale(scaleX, scaleY, 0, 0);
    public void Scale(Vector2 scale, Vector2 center) => Scale(scale.X, scale.Y, center.X, center.Y);
    public void Scale(float scaleX, float scaleY, float centerX, float centerY)
    {
        canvas.Scale(scaleX, scaleX, centerX, centerY);
    }

    public void SetClipRect(float x, float y, float width, float height, Alignment alignment = Alignment.TopLeft) => SetClipRect((x, y), (width, height), alignment);
    public void SetClipRect(Vector2 position, Vector2 size, Alignment alignment = Alignment.TopLeft) => SetClipRect(new Rectangle(position, size, alignment));
    public void SetClipRect(Rectangle rect)
    {
        canvas.Restore();
        
        if (rect.Width > 0 && rect.Height > 0)
        {
            canvas.ClipRect(rect.AsSKRect(), SKClipOperation.Intersect);
        }

        canvas.Save();
    }

    public void DrawLine(Vector2 p1, Vector2 p2, Color color) => DrawLine(p1.X, p1.Y, p2.X, p2.Y, color);
    public void DrawLine(float x1, float y1, float x2, float y2, Color color)
    {
        this.paint.Color = color.AsSKColor();
        canvas.DrawLine(x1, y1, x2, y2, this.paint);
    }

    public void DrawRect(float x, float y, float width, float height, Color color, Alignment alignment = Alignment.TopLeft) => DrawRect(new(x, y), new(width, height), color, alignment);
    public void DrawRect(Vector2 position, Vector2 size, Color color, Alignment alignment = Alignment.TopLeft) => DrawRect(new Rectangle(position, size, alignment), color);
    public void DrawRect(Rectangle rect, Color color)
    {
        this.paint.Color = color.AsSKColor();
        canvas.DrawRect(rect.AsSKRect(), this.paint);
    }

    public void DrawEllipse(float x, float y, float radiusX, float radiusY, Color color, Alignment alignment = Alignment.Center) => DrawEllipse(new(x, y), new(radiusX, radiusY), color, alignment);
    public void DrawEllipse(Vector2 position, Vector2 radii, Color color, Alignment alignment = Alignment.Center) => DrawEllipse(new Rectangle(position.X, position.Y, radii.X * 2, radii.Y * 2, alignment), color);
    public void DrawEllipse(Rectangle bounds, Color color)
    {
        this.paint.Color = color.AsSKColor();
        canvas.DrawOval(bounds.AsSKRect(), this.paint);
    }

    public void DrawEllipse(float x, float y, float radiusX, float radiusY, float begin, float end, Color color, Alignment alignment = Alignment.TopLeft) => DrawEllipse((x, y), (radiusX, radiusY), begin, end, color, alignment);
    public void DrawEllipse(Vector2 position, Vector2 radii, float begin, float end, Color color, Alignment alignment = Alignment.TopLeft) => DrawEllipse(new Rectangle(position.X, position.Y, radii.X * 2, radii.Y * 2, alignment), begin, end, color);
    public void DrawEllipse(Rectangle bounds, float begin, float end, Color color)
    {
        if (end == begin)
            return;

        if (end > begin)
        {
            var swap = begin;
            begin = end;
            end = swap;
        }

        if (end - begin >= MathF.Tau)
            DrawEllipse(bounds, color);

        this.paint.Color = color.AsSKColor();
        canvas.DrawArc(bounds.AsSKRect(), begin, end - begin, true, this.paint);
    }

    public void DrawSurface(ISurface surface, Alignment alignment = Alignment.TopLeft) => DrawSurface(surface, Vector2.Zero, new(surface?.Width ?? 0f, surface?.Height ?? 0f), alignment);
    public void DrawSurface(ISurface surface, float x, float y, Alignment alignment = Alignment.TopLeft) => DrawSurface(surface, x, y, surface?.Width ?? 0f, surface?.Height ?? 0f, Alignment.Center);
    public void DrawSurface(ISurface surface, float x, float y, float width, float height, Alignment alignment = Alignment.TopLeft) => DrawSurface(surface, position: new(x, y), size: new(width, height), alignment);
    public void DrawSurface(ISurface surface, Vector2 position, Vector2 size, Alignment alignment = Alignment.TopLeft) => DrawSurface(surface, new Rectangle(0, 0, surface?.Width ?? 0f, surface?.Height ?? 0f), new Rectangle(position.X, position.Y, size.X, size.Y, alignment));
    public void DrawSurface(ISurface surface, Rectangle source, Rectangle destination)
    {
        if (surface is null)
            throw new ArgumentNullException(nameof(surface));
        
        if (surface is SkiaSurface skiaSurface)
            canvas.DrawBitmap(skiaSurface.bitmap, source.AsSKRect(), destination.AsSKRect());
    }

    public void DrawPolygon(Vector2[] polygon, Color color) => DrawPolygon(polygon.AsSpan(), color);
    public void DrawPolygon(IEnumerable<Vector2> polygon, Color color) => DrawPolygon(polygon.ToArray().AsSpan(), color);
    public unsafe void DrawPolygon(Span<Vector2> polygon, Color color)
    {
        this.paint.Color = color.AsSKColor();
        fixed (Vector2* p = polygon)
            SkiaNativeApi.sk_canvas_draw_points(canvas.Handle, SKPointMode.Polygon, (IntPtr)polygon.Length, (SKPoint*)p, this.paint.Handle);
    }

    public void DrawRoundedRect(float x, float y, float width, float height, float radius, Color color, Alignment alignment = Alignment.TopLeft) => DrawRoundedRect((x, y), (width, height), radius, color, alignment);
    public void DrawRoundedRect(Vector2 position, Vector2 size, float radius, Color color, Alignment alignment = Alignment.TopLeft) => DrawRoundedRect(new Rectangle(position, size, alignment), radius, color);
    public void DrawRoundedRect(Rectangle rect, float radius, Color color)
    {
        this.paint.Color = color.AsSKColor();
        canvas.DrawRoundRect(rect.AsSKRect(), radius, radius, this.paint);
    }

    public void DrawText(string text, float x, float y, Color color, Alignment alignment) => DrawText(text, (x, y), color, alignment);
    public void DrawText(string text, Vector2 position, Color color, Alignment alignment)
    {
        this.paint.Color = color.AsSKColor();

        SKRect skbounds = default;
        paint.MeasureText(text, ref skbounds);
        var bounds = new Rectangle(position, (skbounds.Width, skbounds.Height), alignment);
        var pos = bounds.GetAlignedPoint(Alignment.TopLeft);
        
        canvas.DrawText(text, pos.X - skbounds.Left, pos.Y - skbounds.Top, this.currentFont, this.paint);
    }

    public void SetGradientLinear(float fromX, float fromY, float toX, float toY, params GradientStop[] gradient)
    {
        SetGradientLinear(fromX, fromY, toX, toY, gradient.AsSpan());
    }

    public void SetGradientLinear(float fromX, float fromY, float toX, float toY, IEnumerable<GradientStop> gradient, GradientTileMode tileMode = GradientTileMode.Clamp)
    {
        SetGradientLinear((fromX, fromY), (toX, toY), gradient, tileMode);
    }

    public void SetGradientLinear(float fromX, float fromY, float toX, float toY, Span<GradientStop> gradient, GradientTileMode tileMode = GradientTileMode.Clamp)
    {
        SetGradientLinear((fromX, fromY), (toX, toY), gradient, tileMode);
    }

    public void SetGradientLinear(Vector2 from, Vector2 to, params GradientStop[] gradient)
    {
        SetGradientLinear(from, to, gradient.AsSpan());
    }

    public void SetGradientLinear(Vector2 from, Vector2 to, IEnumerable<GradientStop> gradient, GradientTileMode tileMode = GradientTileMode.Clamp)
    {
        throw new NotImplementedException();
    }

    public void SetGradientLinear(Vector2 from, Vector2 to, Span<GradientStop> gradient, GradientTileMode tileMode = GradientTileMode.Clamp)
    {
        throw new NotImplementedException();
    }

    public void SetGradientLinear(Alignment from, Alignment to, params GradientStop[] gradient)
    {
        throw new NotImplementedException();
    }

    public void SetGradientLinear(Alignment from, Alignment to, IEnumerable<GradientStop> gradient, GradientTileMode tileMode = GradientTileMode.Clamp)
    {
        throw new NotImplementedException();
    }

    public void SetGradientLinear(Alignment from, Alignment to, Span<GradientStop> gradient, GradientTileMode tileMode = GradientTileMode.Clamp)
    {
        throw new NotImplementedException();
    }

    public void SetGradientRadial(float x, float y, params GradientStop[] gradient)
    {
        throw new NotImplementedException();
    }

    public void SetGradientRadial(float x, float y, IEnumerable<GradientStop> gradient, GradientTileMode tileMode = GradientTileMode.Clamp)
    {
        throw new NotImplementedException();
    }

    public void SetGradientRadial(float x, float y, Span<GradientStop> gradient, GradientTileMode tileMode = GradientTileMode.Clamp)
    {
        throw new NotImplementedException();
    }

    public void SetGradientRadial(Vector2 position, params GradientStop[] gradient)
    {
        throw new NotImplementedException();
    }

    public void SetGradientRadial(Vector2 position, IEnumerable<GradientStop> gradient, GradientTileMode tileMode = GradientTileMode.Clamp)
    {
        throw new NotImplementedException();
    }

    public void SetGradientRadial(Vector2 position, Span<GradientStop> gradient, GradientTileMode tileMode = GradientTileMode.Clamp)
    {
        throw new NotImplementedException();
    }

    public void SetGradientRadial(Alignment position, params GradientStop[] gradient)
    {
        throw new NotImplementedException();
    }

    public void SetGradientRadial(Alignment position, IEnumerable<GradientStop> gradient, GradientTileMode tileMode = GradientTileMode.Clamp)
    {
        throw new NotImplementedException();
    }

    public void SetGradientRadial(Alignment position, Span<GradientStop> gradient, GradientTileMode tileMode = GradientTileMode.Clamp)
    {
        throw new NotImplementedException();
    }

    public void SetGradientRadial(Alignment position, Vector2 offset, params GradientStop[] gradient)
    {
        throw new NotImplementedException();
    }

    public void SetGradientRadial(Alignment position, Vector2 offset, IEnumerable<GradientStop> gradient, GradientTileMode tileMode = GradientTileMode.Clamp)
    {
        throw new NotImplementedException();
    }

    public void SetGradientRadial(Alignment position, Vector2 offset, Span<GradientStop> gradient, GradientTileMode tileMode = GradientTileMode.Clamp)
    {
        throw new NotImplementedException();
    }

    private readonly struct SkiaCanvasState
    {
        private readonly SkiaCanvas canvas;
        private readonly DrawMode mode;
        private readonly SKPaint paint;
        private readonly float strokeWidth;
        private readonly Rectangle clipRect;
        private readonly Matrix3x2 transform;

        public SkiaCanvasState(SkiaCanvas canvas)
        {
            this.canvas = canvas;
            this.mode = canvas.currentMode;
            this.paint = canvas.paint;
            canvas.paint = this.paint.Clone();
            this.transform = canvas.Transform;
            this.clipRect = canvas.currentClipRect;
            this.strokeWidth = canvas.currentStrokeWidth;
        }

        public void Restore()
        {
            this.canvas.currentMode = mode;
            this.canvas.paint.Dispose();
            this.canvas.paint = this.paint;
            this.canvas.currentStrokeWidth = strokeWidth;
            this.canvas.currentClipRect = clipRect;
            this.canvas.Transform = transform;
        }
    }
}