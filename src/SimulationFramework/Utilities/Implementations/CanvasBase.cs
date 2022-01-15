using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Utilities.Implementations;


#pragma warning disable CS1591 // Disable missing doc warning since most public members implicitly inherit the docs of ICanvas.

/// <summary>
/// Abstract base class for <see cref="ICanvas"/> implementations.
/// </summary>
public abstract class CanvasBase : ICanvas
{
    public const int MAX_STACK_ALLOCATION = 1024;

    public int Width => this.GetSurface()?.Width ?? Simulation.Current.TargetWidth;
    public int Height => this.GetSurface()?.Height ?? Simulation.Current.TargetHeight;
    public Matrix3x2 Transform
    {
        get
        {
            return CurrentState.Transform;
        }
        set
        {
            if (UpdateTransformCore(value))
            {
                CurrentState.Transform = value;
            }
        }
    }

    private readonly Stack<CanvasState> stateStack = new();
    public CanvasState CurrentState => stateStack.Peek();

    protected abstract void ClearCore(Color color);
    protected abstract void DrawLineCore(Vector2 p1, Vector2 p2, Color color);
    protected abstract void DrawEllipseCore(Rectangle bounds, float begin, float end, Color color);
    protected abstract void DrawPolygonCore(Span<Vector2> polygon, Color color);
    protected abstract void DrawRectCore(Rectangle bounds, float radius, Color color);
    protected abstract void DrawSurfaceCore(ISurface surface, Rectangle source, Rectangle destination);
    protected abstract void DrawTextCore(string text, Vector2 position, Color color, Alignment alignment);
    protected abstract bool UpdateClipRectCore(Rectangle rect);
    protected abstract bool UpdateDrawModeCore(DrawMode mode);
    protected abstract bool UpdateFontCore(string fontName, TextStyles styles, float size);
    protected abstract bool UpdateGradientLinearCore(Vector2 from, Vector2 to, Span<GradientStop> gradient, GradientTileMode tileMode = GradientTileMode.Clamp);
    protected abstract bool UpdateGradientRadialCore(Vector2 position, float radius, Span<GradientStop> gradient, GradientTileMode tileMode = GradientTileMode.Clamp);
    protected abstract bool UpdateStrokeWidthCore(float strokeWidth);
    protected abstract bool UpdateTransformCore(Matrix3x2 transform);
    protected abstract Vector2 MeasureTextCore(string text);
    protected abstract void FlushCore();
    protected abstract ISurface GetSurfaceCore();

    public CanvasBase()
    {
        // this.ResetState();
    }

    public void Clear(Color color)
    {
        ClearCore(color);
    }
    
    public void DrawEllipse(float x, float y, float radiusX, float radiusY, Color color, Alignment alignment = Alignment.Center) => DrawEllipse((x, y), (radiusX, radiusY), color, alignment);
    public void DrawEllipse(Vector2 position, Vector2 radii, Color color, Alignment alignment = Alignment.Center) => DrawEllipse(new Rectangle(position, radii * 2, alignment), color);
    public void DrawEllipse(Rectangle bounds, Color color) => DrawEllipse(bounds, 0, MathF.PI * 2, color);
    public void DrawEllipse(float x, float y, float radiusX, float radiusY, float begin, float end, Color color, Alignment alignment = Alignment.TopLeft) => DrawEllipse((x, y), (radiusX, radiusY), begin, end, color, alignment);
    public void DrawEllipse(Vector2 position, Vector2 radii, float begin, float end, Color color, Alignment alignment = Alignment.TopLeft) => DrawEllipse(new Rectangle(position, radii * 2, alignment), begin, end, color);
    public void DrawEllipse(Rectangle bounds, float begin, float end, Color color)
    {
        if (CurrentState.DrawMode == DrawMode.Gradient && CurrentState.gradientTileMode == GradientTileMode.Stop)
        {
            using (this.Push())
            {
                this.SetDrawMode(DrawMode.Fill);
                this.DrawEllipseCore(bounds, begin, end, color);
            }
        }

        DrawEllipseCore(bounds, begin, end, color);
    }

    public void DrawLine(float x1, float y1, float x2, float y2, Color color) => DrawLine(x1, y1, x2, y2, color);
    public void DrawLine(Vector2 p1, Vector2 p2, Color color)
    {
        DrawLineCore(p1, p2, color);
    }

    public ISurface GetSurface()
    {
        return this.GetSurfaceCore();
    }

    public unsafe void DrawPolygon(IEnumerable<Vector2> polygon, Color color)
    {
        bool extraElement = CurrentState.DrawMode == DrawMode.Fill || CurrentState.DrawMode == DrawMode.Gradient;

        int count = polygon.Count() + (extraElement ? 1 : 0);
        int byteLength = count * Unsafe.SizeOf<Vector2>();
        
        void DrawPolygonHeap()
        {
            Vector2* poly = (Vector2*)NativeMemory.Alloc((nuint)Unsafe.SizeOf<Vector2>(), (nuint)count);

            for (int i = 0; i < polygon.Count(); i++)
            {
                poly[i] = polygon.ElementAt(i);
            }

            if (extraElement)
                poly[count - 1] = poly[0];

            DrawPolygon(new Span<Vector2>(poly, count), color);

            NativeMemory.Free(poly);
        }

        void DrawPolygonStack()
        {
            Span<Vector2> poly = stackalloc Vector2[count];

            for (int i = 0; i < polygon.Count(); i++)
            {
                poly[i] = polygon.ElementAt(i);
            }
            
            if (extraElement)
                poly[count - 1] = poly[0];

            DrawPolygon(poly, color);
        }

        if (byteLength > MAX_STACK_ALLOCATION)
            DrawPolygonHeap();
        else
            DrawPolygonStack();
    }
    public void DrawPolygon(Vector2[] polygon, Color color) => DrawPolygon(polygon.AsSpan(), color);
    public void DrawPolygon(Span<Vector2> polygon, Color color)
    {
        if (polygon.Length <= 2)
            throw new ArgumentException("Polygon must have at least 3 vertices!");


        if (CurrentState.DrawMode == DrawMode.Gradient && CurrentState.gradientTileMode == GradientTileMode.Stop)
        {
            using (this.Push())
            {
                this.SetDrawMode(DrawMode.Fill);
                this.DrawPolygonCore(polygon, color);
            }
        }

        DrawPolygonCore(polygon, color);
    }


    public void DrawRect(float x, float y, float width, float height, Color color, Alignment alignment = Alignment.TopLeft) => DrawRect((x, y), (width, height), color, alignment);
    public void DrawRect(Vector2 position, Vector2 size, Color color, Alignment alignment = Alignment.TopLeft) => DrawRect(new(position, size, alignment), color);
    public void DrawRect(Rectangle rect, Color color)
    {
        DrawRoundedRect(rect, 0, color);
    }

    public void DrawRoundedRect(float x, float y, float width, float height, float radius, Color color, Alignment alignment = Alignment.TopLeft) => DrawRoundedRect((x, y), (width, height), radius, color, alignment);
    public void DrawRoundedRect(Vector2 position, Vector2 size, float radius, Color color, Alignment alignment = Alignment.TopLeft) => DrawRoundedRect(new(position, size, alignment), radius, color);
    public void DrawRoundedRect(Rectangle rect, float radius, Color color)
    {
        if (rect.Width <= 0 || rect.Height <= 0)
            return;

        if (radius < 0)
            throw new ArgumentException("Radius may not be negative!", nameof(radius));

        UpdateRelativeGradient(rect);

        if (CurrentState.DrawMode == DrawMode.Gradient && CurrentState.gradientTileMode == GradientTileMode.Stop)
        {
            using (this.Push())
            {
                this.SetDrawMode(DrawMode.Fill);
                this.DrawRectCore(rect, radius, color);
            }
        }

        DrawRectCore(rect, radius, color);
    }

    public void DrawSurface(ISurface surface, Alignment alignment = Alignment.TopLeft) => DrawSurface(surface, (0,0), (surface?.Width ?? 0, surface?.Height ?? 0), alignment);
    public void DrawSurface(ISurface surface, float x, float y, Alignment alignment = Alignment.TopLeft) => DrawSurface(surface, (x, y), (surface?.Width ?? 0, surface?.Height ?? 0), alignment);
    public void DrawSurface(ISurface surface, float x, float y, float width, float height, Alignment alignment = Alignment.TopLeft) => DrawSurface(surface, (x, y), (width, height), alignment);
    public void DrawSurface(ISurface surface, Vector2 position, Vector2 size, Alignment alignment = Alignment.TopLeft) => DrawSurface(surface, new(0, 0, surface?.Width ?? 0, surface?.Height ?? 0), new(position, size, alignment));
    public void DrawSurface(ISurface surface, Rectangle source, Rectangle destination)
    {
        if (surface is null)
            throw new ArgumentNullException(nameof(surface));

        DrawSurfaceCore(surface, source, destination);
    }

    public void DrawText(string text, float x, float y, Color color, Alignment alignment = Alignment.TopLeft) => DrawText(text, (x, y), color, alignment);
    public void DrawText(string text, Vector2 position, Color color, Alignment alignment = Alignment.TopLeft)
    {
        UpdateRelativeGradient(new Rectangle(position, MeasureText(text), alignment));

        DrawTextCore(text, position, color, alignment);
    }
    public Vector2 MeasureText(string text)
    {
        return MeasureTextCore(text);
    }

    public void Flush()
    {
        FlushCore();
    }

    public void Pop()
    {
        stateStack.Pop();
    }

    public CanvasSession Push()
    {
        stateStack.Push(CurrentState);
        CurrentState.Clone();
        return new CanvasSession(this);
    }

    public void ResetState()
    {
        this.stateStack.Clear();
        this.stateStack.Push(new CanvasState(this));
    }

    public void Rotate(float angle) => Rotate(angle, 0, 0);
    public void Rotate(float angle, float centerX, float centerY) => Rotate(angle, centerX, centerY);
    public void Rotate(float angle, Vector2 center)
    {
        this.Transform *= Matrix3x2.CreateRotation(angle, center);
    }

    public void Scale(float scale) => Scale(scale, scale);
    public void Scale(float scaleX, float scaleY) => Scale((scaleX, scaleY));
    public void Scale(float scaleX, float scaleY, float centerX, float centerY) => Scale((scaleX, scaleY), (centerX, centerY));
    public void Scale(Vector2 scale) => Scale(scale.X, scale.Y, 0, 0);
    public void Scale(Vector2 scale, Vector2 center)
    {
        this.Transform *= Matrix3x2.CreateScale(scale, center);
    }

    public void SetClipRect(float x, float y, float width, float height, Alignment alignment = Alignment.TopLeft) => SetClipRect((x, y), (width, height), alignment);
    public void SetClipRect(Vector2 position, Vector2 size, Alignment alignment = Alignment.TopLeft) => SetClipRect(position, size, alignment);
    public void SetClipRect(Rectangle rect)
    {
        if (UpdateClipRectCore(rect))
        {
            this.CurrentState.clipRect = rect;
        }
    }
    public void SetDrawMode(DrawMode mode)
    {
        if (UpdateDrawModeCore(mode))
        {
            this.CurrentState.DrawMode = mode;
        }
    }

    public bool SetFont(string fontName, TextStyles styles, float size)
    {
        if (UpdateFontCore(fontName, styles, size))
        {
            this.CurrentState.fontName = fontName;
            this.CurrentState.styles = styles;
            this.CurrentState.size = size;
            return true;
        }
        return false;
    }
    
    public static Span<GradientStop> GradientStopEnumerableToSpan(IEnumerable<GradientStop> enumerable, Span<GradientStop> buffer)
    {
        var enumerator = enumerable.GetEnumerator();
        for (int i = 0; i < enumerable.Count(); i++)
        {
            buffer[i] = enumerator.Current;
            enumerator.MoveNext();
        }

        return buffer;
    }

    public void CompleteGradientStop(Span<GradientStop> stops)
    {
        float Lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        float GetPosition(int index, Span<GradientStop> stops)
        {
            if (index < 0)
            {
                return 0f;
            }
            if (index >= stops.Length)
            {
                return 1f;
            }
            
            return stops[index].Position;
        }

        int autoStart = -1;
        int autoEnd = -1;

        for (int i = 0; i < stops.Length; i++)
        {
            // once we hit a negative, keep going until we find a valid value
            if (autoStart == -1)
            {
                if (stops[i].Position < 0)
                {
                    autoStart = i;
                }
            }
            else if (autoEnd == -1)
            {
                if (stops[i].Position >= 0)
                {
                    autoEnd = i;

                    int len = autoStart - autoEnd;
                    float t = 1f/len;
                    for (int j = autoStart; j < autoEnd; j++)
                    {
                        Lerp(GetPosition(autoStart - 1, stops), GetPosition(autoEnd, stops), t);
                        t += 1f/len;
                    }

                    autoStart = -1;
                    autoEnd = -1;
                }
            }
        }
    }

    public void SetGradientLinear(float fromX, float fromY, float toX, float toY, params GradientStop[] gradient) => SetGradientLinear((fromX, fromY), (toX, toY), gradient);
    public void SetGradientLinear(float fromX, float fromY, float toX, float toY, IEnumerable<GradientStop> gradient, GradientTileMode tileMode = GradientTileMode.Clamp) => SetGradientLinear((fromX, fromY), (toX, toY), gradient, tileMode);
    public void SetGradientLinear(float fromX, float fromY, float toX, float toY, Span<GradientStop> gradient, GradientTileMode tileMode = GradientTileMode.Clamp) => SetGradientLinear((fromX, fromY), (toX, toY), gradient, tileMode);
    
    public void SetGradientLinear(Vector2 from, Vector2 to, params GradientStop[] gradient) => SetGradientLinear(from, to, gradient.AsSpan());
    public void SetGradientLinear(Vector2 from, Vector2 to, IEnumerable<GradientStop> gradient, GradientTileMode tileMode = GradientTileMode.Clamp) => SetGradientLinear(from, to, GradientStopEnumerableToSpan(gradient, stackalloc GradientStop[gradient.Count()]));
    public void SetGradientLinear(Vector2 from, Vector2 to, Span<GradientStop> gradient, GradientTileMode tileMode = GradientTileMode.Clamp)
    {
        if (UpdateGradientLinearCore(from, to, gradient, tileMode))
        {
            CurrentState.isGradientRadial = true;
            CurrentState.isGradientRelative = false;
            CurrentState.gradientFrom = from;
            CurrentState.gradientTo = to;
            CurrentState.gradientTileMode = tileMode;
            CurrentState.UpdateGradient(gradient);
        }
    }

    public void SetGradientLinear(Alignment from, Alignment to, params GradientStop[] gradient) => SetGradientLinear(from, to, gradient.AsSpan());
    public void SetGradientLinear(Alignment from, Alignment to, IEnumerable<GradientStop> gradient, GradientTileMode tileMode = GradientTileMode.Clamp) => SetGradientLinear(from, to, GradientStopEnumerableToSpan(gradient, stackalloc GradientStop[gradient.Count()]));
    public void SetGradientLinear(Alignment from, Alignment to, Span<GradientStop> gradient, GradientTileMode tileMode = GradientTileMode.Clamp)
    {
        CurrentState.isGradientRadial = false;
        CurrentState.isGradientRelative = true;
        CurrentState.relativeGradientFrom = from;
        CurrentState.relativeGradientTo = to;
        CurrentState.gradientTileMode = tileMode;
        CurrentState.UpdateGradient(gradient);
    }

    public void SetGradientRadial(float x, float y, float radius, params GradientStop[] gradient) => SetGradientRadial(x, y, radius, gradient.AsSpan());
    public void SetGradientRadial(float x, float y, float radius, IEnumerable<GradientStop> gradient, GradientTileMode tileMode = GradientTileMode.Clamp) => SetGradientRadial((x,y), radius, gradient, tileMode);
    public void SetGradientRadial(float x, float y, float radius, Span<GradientStop> gradient, GradientTileMode tileMode = GradientTileMode.Clamp) => SetGradientRadial((x,y), radius, gradient, tileMode);
    public void SetGradientRadial(Vector2 position, float radius, params GradientStop[] gradient) => SetGradientRadial(position, radius, gradient.AsSpan());
    public void SetGradientRadial(Vector2 position, float radius, IEnumerable<GradientStop> gradient, GradientTileMode tileMode = GradientTileMode.Clamp) => SetGradientRadial((position.X, position.Y), radius, GradientStopEnumerableToSpan(gradient, stackalloc GradientStop[gradient.Count()]), tileMode);
    public void SetGradientRadial(Vector2 position, float radius, Span<GradientStop> gradient, GradientTileMode tileMode = GradientTileMode.Clamp)
    {
        if (UpdateGradientRadialCore(position, radius, gradient, tileMode))
        {
            CurrentState.isGradientRadial = true;
            CurrentState.isGradientRelative = false;
            CurrentState.gradientFrom = position;
            CurrentState.gradientTileMode = tileMode;
            CurrentState.UpdateGradient(gradient);
        }
    }

    public void SetGradientRadial(Alignment position, float radius, params GradientStop[] gradient) => SetGradientRadial(position, (0, 0), radius, gradient);
    public void SetGradientRadial(Alignment position, float radius, IEnumerable<GradientStop> gradient, GradientTileMode tileMode = GradientTileMode.Clamp) => SetGradientRadial(position, radius, GradientStopEnumerableToSpan(gradient, stackalloc GradientStop[gradient.Count()]), tileMode);
    public void SetGradientRadial(Alignment position, float radius, Span<GradientStop> gradient, GradientTileMode tileMode = GradientTileMode.Clamp) => SetGradientRadial(position, (0, 0), radius, gradient, tileMode);
    public void SetGradientRadial(Alignment position, Vector2 offset, float radius, params GradientStop[] gradient) => SetGradientRadial(position, offset, radius, gradient.AsSpan());
    public void SetGradientRadial(Alignment position, Vector2 offset, float radius,  IEnumerable<GradientStop> gradient, GradientTileMode tileMode = GradientTileMode.Clamp) => SetGradientRadial(position, offset, radius, GradientStopEnumerableToSpan(gradient, stackalloc GradientStop[gradient.Count()]), tileMode);
    public void SetGradientRadial(Alignment position, Vector2 offset, float radius,  Span<GradientStop> gradient, GradientTileMode tileMode = GradientTileMode.Clamp)
    {
        CurrentState.isGradientRadial = true;
        CurrentState.isGradientRelative = true;
        CurrentState.relativeGradientFrom = position;
        CurrentState.gradientFrom = offset;
        CurrentState.gradientTo.X = radius;
        CurrentState.gradientTileMode = tileMode;
        CurrentState.UpdateGradient(gradient);
    }

    private unsafe void UpdateRelativeGradient(Rectangle bounds)
    {
        if (CurrentState.isGradientRelative)
        {
            if (CurrentState.isGradientRadial)
            {
                UpdateGradientRadialCore(
                    bounds.GetAlignedPoint(CurrentState.relativeGradientFrom) + CurrentState.gradientFrom,
                    CurrentState.gradientTo.X,
                    CurrentState.GradientStops,
                    CurrentState.gradientTileMode
                    );
            }
            else
            {
                UpdateGradientLinearCore(
                    bounds.GetAlignedPoint(CurrentState.relativeGradientFrom),
                    bounds.GetAlignedPoint(CurrentState.relativeGradientTo),
                    CurrentState.GradientStops,
                    CurrentState.gradientTileMode
                    );
            }
        }
    }

    public void Translate(float x, float y) => Translate(x, y);
    public void Translate(Vector2 translation)
    {
        this.Transform *= Matrix3x2.CreateTranslation(translation);
    }

    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public void SetStrokeWidth(float strokeWidth)
    {
        if (UpdateStrokeWidthCore(strokeWidth))
        {
            this.CurrentState.strokeWidth = strokeWidth;
        }
    }

}