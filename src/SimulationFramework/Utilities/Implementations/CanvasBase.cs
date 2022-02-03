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

    public int Width => this.GetTarget().Width;
    public int Height => this.GetTarget().Height;
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
    protected abstract void DrawEllipseCore(Rectangle bounds, float begin, float end, bool includeCenter, Color color);
    protected abstract void DrawPolygonCore(Span<Vector2> polygon, Color color);
    protected abstract void DrawRectCore(Rectangle bounds, float radius, Color color);
    protected abstract void DrawSurfaceCore(ITexture texture, Rectangle source, Rectangle destination);
    protected abstract void DrawTextCore(string text, Vector2 position, Color color, Alignment alignment);
    protected abstract bool UpdateClipRectCore(Rectangle rect);
    protected abstract bool UpdateDrawModeCore(DrawMode mode);
    protected abstract bool UpdateFontCore(string fontName, TextStyles styles, float size);
    protected abstract bool UpdateGradientLinearCore(Vector2 from, Vector2 to, Span<Color> gradient, TileMode tileMode = TileMode.Clamp);
    protected abstract bool UpdateGradientRadialCore(Vector2 position, float radius, Span<Color> gradient, TileMode tileMode = TileMode.Clamp);
    protected abstract bool UpdateStrokeWidthCore(float strokeWidth);
    protected abstract bool UpdateTransformCore(Matrix3x2 transform);
    protected abstract bool UpdateFillTextureCore(ITexture texture, Matrix3x2 transform, TileMode tileMode);
    protected abstract Vector2 MeasureTextCore(string text);
    protected abstract void FlushCore();
    protected abstract ITexture GetSurfaceCore();

    public CanvasBase()
    {
        // this.ResetState();
        this.ResetState();
    }

    public void Clear(Color color)
    {
        ClearCore(color);
    }
    
    public void DrawEllipse(float x, float y, float radiusX, float radiusY, Color color, Alignment alignment = Alignment.Center) => DrawEllipse((x, y), (radiusX, radiusY), color, alignment);
    public void DrawEllipse(Vector2 position, Vector2 radii, Color color, Alignment alignment = Alignment.Center) => DrawEllipse(new Rectangle(position, radii * 2, alignment), color);
    public void DrawEllipse(Rectangle bounds, Color color) => DrawEllipse(bounds, 0, Simulation.ConvertToCurrentAngleMode(MathF.PI * 2, AngleMode.Radians), false, color);
    public void DrawEllipse(float x, float y, float radiusX, float radiusY, float begin, float end, bool includeCenter, Color color, Alignment alignment = Alignment.Center) => DrawEllipse((x, y), (radiusX, radiusY), begin, end, includeCenter, color, alignment);
    public void DrawEllipse(Vector2 position, Vector2 radii, float begin, float end, bool includeCenter, Color color, Alignment alignment = Alignment.Center) => DrawEllipse(new Rectangle(position, radii * 2, alignment), begin, end, includeCenter, color);
    public void DrawEllipse(Rectangle bounds, float begin, float end, bool includeCenter, Color color)
    {
        RefreshRelativeGradient(bounds);

        if (CurrentState.DrawMode == DrawMode.Gradient && CurrentState.gradientTileMode == TileMode.Stop)
        {
            using (this.Push())
            {
                this.SetDrawMode(DrawMode.Fill);
                this.DrawEllipseCore(bounds, begin, end, includeCenter, color);
            }
        }

        DrawEllipseCore(bounds, begin, end, includeCenter, color);
    }

    public void DrawLine(float x1, float y1, float x2, float y2, Color color) => DrawLine((x1, y1), (x2, y2), color);
    public void DrawLine(Vector2 p1, Vector2 p2, Color color)
    {
        DrawLineCore(p1, p2, color);
    }

    public ITexture GetTarget()
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


        if (CurrentState.DrawMode == DrawMode.Gradient && CurrentState.gradientTileMode == TileMode.Stop)
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

        RefreshRelativeGradient(rect);

        if (CurrentState.DrawMode == DrawMode.Gradient && CurrentState.gradientTileMode == TileMode.Stop)
        {
            using (this.Push())
            {
                this.SetDrawMode(DrawMode.Fill);
                this.DrawRectCore(rect, radius, color);
            }
        }

        DrawRectCore(rect, radius, color);
    }

    public void DrawTexture(ITexture texture, Alignment alignment = Alignment.TopLeft) => DrawSurface(texture, (0,0), (texture?.Width ?? 0, texture?.Height ?? 0), alignment);
    public void DrawSurface(ITexture texture, float x, float y, Alignment alignment = Alignment.TopLeft) => DrawSurface(texture, (x, y), (texture?.Width ?? 0, texture?.Height ?? 0), alignment);
    public void DrawSurface(ITexture texture, float x, float y, float width, float height, Alignment alignment = Alignment.TopLeft) => DrawSurface(texture, (x, y), (width, height), alignment);
    public void DrawSurface(ITexture texture, Vector2 position, Vector2 size, Alignment alignment = Alignment.TopLeft) => DrawSurface(texture, new(0, 0, texture?.Width ?? 0, texture?.Height ?? 0), new(position, size, alignment));
    public void DrawSurface(ITexture texture, Rectangle source, Rectangle destination)
    {
        if (texture is null)
            throw new ArgumentNullException(nameof(texture));

        DrawSurfaceCore(texture, source, destination);
    }

    public void DrawText(string text, float x, float y, Color color, Alignment alignment = Alignment.TopLeft) => DrawText(text, (x, y), color, alignment);
    public void DrawText(string text, Vector2 position, Color color, Alignment alignment = Alignment.TopLeft)
    {
        RefreshRelativeGradient(new Rectangle(position, MeasureText(text), alignment));

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
        ApplyState(CurrentState);
    }

    private void ApplyState(CanvasState state)
    {
        UpdateTransformCore(state.Transform);
        UpdateStrokeWidthCore(state.strokeWidth);
        UpdateClipRectCore(state.clipRect);
        UpdateDrawModeCore(state.DrawMode);
        UpdateFontCore(state.fontName, state.styles, state.size);
        UpdateFillTextureCore(state.texture, state.textureTransform, state.textureTileMode);
        if (!state.isGradientRelative)
        {
            if (state.isGradientRadial)
            {
                UpdateGradientRadialCore(state.gradientFrom, state.gradientTo.X, state.Colors, state.gradientTileMode);
            }
            else
            {
                UpdateGradientLinearCore(state.gradientFrom, state.gradientTo, state.Colors, state.gradientTileMode);
            }
        }
    }

    public CanvasSession Push()
    {
        stateStack.Push(CurrentState.Clone());
        return new CanvasSession(this);
    }

    public void ResetState()
    {
        this.stateStack.Clear();
        this.stateStack.Push(new CanvasState(this));
    }

    public void Rotate(float angle) => Rotate(angle, 0, 0);
    public void Rotate(float angle, float centerX, float centerY) => Rotate(angle, (centerX, centerY));
    public void Rotate(float angle, Vector2 center)
    {
        this.Transform = Matrix3x2.CreateRotation(angle, center) * this.Transform;
    }

    public void Scale(float scale) => Scale(scale, scale);
    public void Scale(float scaleX, float scaleY) => Scale((scaleX, scaleY));
    public void Scale(Vector2 scale)
    {
        this.Transform = Matrix3x2.CreateScale(scale) * this.Transform;
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
    
    public static Span<Color> ColorEnumerableToSpan(IEnumerable<Color> enumerable, Span<Color> buffer)
    {
        var enumerator = enumerable.GetEnumerator();
        for (int i = 0; i < enumerable.Count(); i++)
        {
            enumerator.MoveNext();
            buffer[i] = enumerator.Current;
        }

        return buffer;
    }

    public void SetGradientLinear(float fromX, float fromY, float toX, float toY, params Color[] gradient) => SetGradientLinear((fromX, fromY), (toX, toY), gradient);
    public void SetGradientLinear(float fromX, float fromY, float toX, float toY, IEnumerable<Color> gradient, TileMode tileMode = TileMode.Clamp) => SetGradientLinear((fromX, fromY), (toX, toY), gradient, tileMode);
    public void SetGradientLinear(float fromX, float fromY, float toX, float toY, Span<Color> gradient, TileMode tileMode = TileMode.Clamp) => SetGradientLinear((fromX, fromY), (toX, toY), gradient, tileMode);
    
    public void SetGradientLinear(Vector2 from, Vector2 to, params Color[] gradient) => SetGradientLinear(from, to, gradient.AsSpan());
    public void SetGradientLinear(Vector2 from, Vector2 to, IEnumerable<Color> gradient, TileMode tileMode = TileMode.Clamp) => SetGradientLinear(from, to, ColorEnumerableToSpan(gradient, stackalloc Color[gradient.Count()]));
    public void SetGradientLinear(Vector2 from, Vector2 to, Span<Color> gradient, TileMode tileMode = TileMode.Clamp)
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

    public void SetGradientLinear(Alignment from, Alignment to, params Color[] gradient) => SetGradientLinear(from, to, gradient.AsSpan());
    public void SetGradientLinear(Alignment from, Alignment to, IEnumerable<Color> gradient, TileMode tileMode = TileMode.Clamp) => SetGradientLinear(from, to, ColorEnumerableToSpan(gradient, stackalloc Color[gradient.Count()]));
    public void SetGradientLinear(Alignment from, Alignment to, Span<Color> gradient, TileMode tileMode = TileMode.Clamp)
    {
        CurrentState.isGradientRadial = false;
        CurrentState.isGradientRelative = true;
        CurrentState.relativeGradientFrom = from;
        CurrentState.relativeGradientTo = to;
        CurrentState.gradientTileMode = tileMode;
        CurrentState.UpdateGradient(gradient);
    }

    public void SetGradientRadial(float x, float y, float radius, params Color[] gradient) => SetGradientRadial(x, y, radius, gradient.AsSpan());
    public void SetGradientRadial(float x, float y, float radius, IEnumerable<Color> gradient, TileMode tileMode = TileMode.Clamp) => SetGradientRadial((x,y), radius, gradient, tileMode);
    public void SetGradientRadial(float x, float y, float radius, Span<Color> gradient, TileMode tileMode = TileMode.Clamp) => SetGradientRadial((x,y), radius, gradient, tileMode);
    public void SetGradientRadial(Vector2 position, float radius, params Color[] gradient) => SetGradientRadial(position, radius, gradient.AsSpan());
    public void SetGradientRadial(Vector2 position, float radius, IEnumerable<Color> gradient, TileMode tileMode = TileMode.Clamp) => SetGradientRadial((position.X, position.Y), radius, ColorEnumerableToSpan(gradient, stackalloc Color[gradient.Count()]), tileMode);
    public void SetGradientRadial(Vector2 position, float radius, Span<Color> gradient, TileMode tileMode = TileMode.Clamp)
    {
        if (UpdateGradientRadialCore(position, radius, gradient, tileMode))
        {
            CurrentState.isGradientRadial = true;
            CurrentState.isGradientRelative = false;
            CurrentState.gradientFrom = position;
            CurrentState.gradientTo.X = radius;
            CurrentState.gradientTileMode = tileMode;
            CurrentState.UpdateGradient(gradient);
        }
    }

    public void SetGradientRadial(Alignment position, float radius, params Color[] gradient) => SetGradientRadial(position, (0, 0), radius, gradient);
    public void SetGradientRadial(Alignment position, float radius, IEnumerable<Color> gradient, TileMode tileMode = TileMode.Clamp) => SetGradientRadial(position, radius, ColorEnumerableToSpan(gradient, stackalloc Color[gradient.Count()]), tileMode);
    public void SetGradientRadial(Alignment position, float radius, Span<Color> gradient, TileMode tileMode = TileMode.Clamp) => SetGradientRadial(position, (0, 0), radius, gradient, tileMode);
    public void SetGradientRadial(Alignment position, Vector2 offset, float radius, params Color[] gradient) => SetGradientRadial(position, offset, radius, gradient.AsSpan());
    public void SetGradientRadial(Alignment position, Vector2 offset, float radius,  IEnumerable<Color> gradient, TileMode tileMode = TileMode.Clamp) => SetGradientRadial(position, offset, radius, ColorEnumerableToSpan(gradient, stackalloc Color[gradient.Count()]), tileMode);
    public void SetGradientRadial(Alignment position, Vector2 offset, float radius,  Span<Color> gradient, TileMode tileMode = TileMode.Clamp)
    {
        CurrentState.isGradientRadial = true;
        CurrentState.isGradientRelative = true;
        CurrentState.relativeGradientFrom = position;
        CurrentState.gradientFrom = offset;
        CurrentState.gradientTo.X = radius;
        CurrentState.gradientTileMode = tileMode;
        CurrentState.UpdateGradient(gradient);
    }

    private unsafe void RefreshRelativeGradient(Rectangle bounds)
    {
        if (CurrentState.isGradientRelative)
        {
            if (CurrentState.isGradientRadial)
            {
                UpdateGradientRadialCore(
                    bounds.GetAlignedPoint(CurrentState.relativeGradientFrom) + CurrentState.gradientFrom,
                    CurrentState.gradientTo.X,
                    CurrentState.Colors,
                    CurrentState.gradientTileMode
                    );
            }
            else
            {
                UpdateGradientLinearCore(
                    bounds.GetAlignedPoint(CurrentState.relativeGradientFrom),
                    bounds.GetAlignedPoint(CurrentState.relativeGradientTo),
                    CurrentState.Colors,
                    CurrentState.gradientTileMode
                    );
            }
        }
    }

    public void Translate(float x, float y) => Translate((x, y));
    public void Translate(Vector2 translation)
    {
        this.Transform = Matrix3x2.CreateTranslation(translation) * this.Transform;
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

    public void SetFillTexture(ITexture texture, TileMode tileMode = TileMode.Clamp)
    {
        SetFillTexture(texture, Matrix3x2.Identity, tileMode);
    }

    public void SetFillTexture(ITexture texture, Matrix3x2 transform, TileMode tileMode = TileMode.Clamp)
    {
        if (UpdateFillTextureCore(texture, transform, tileMode))
        {
            CurrentState.texture = texture;
            CurrentState.textureTransform = transform;
            CurrentState.textureTileMode = tileMode;
        }
    }

    public void TransformBy(Matrix3x2 transformation)
    {
        this.Transform = transformation * this.Transform;
    }
}