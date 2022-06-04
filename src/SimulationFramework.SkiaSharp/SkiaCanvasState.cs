using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Canvas;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.SkiaSharp;

internal sealed class SkiaCanvasState : CanvasState, IDisposable
{
    private SKCanvas canvas;
    private SKPaint paint;

    public bool IsActive => canvas is not null;
    public SKPaint Paint => paint;

    private SKShader fillTextureShader;
    private SKShader gradientShader;

    private SKTypeface typeface;

    public SkiaCanvasState(SKCanvas canvas, SkiaCanvasState other = null)
    {
        this.canvas = canvas;
        this.Initialize(other);
    }

    protected override void Initialize(CanvasState other)
    {
        if (other is null)
        {
            this.paint = new SKPaint();
            this.paint.IsAntialias = true;
        }
        else if (other != this)
        {
            this.paint = ((SkiaCanvasState)other).paint.Clone();
            this.fillTextureShader = GradientShaderCache.GetShader(other.Gradient);
            this.gradientShader = GradientShaderCache.GetShader(other.Gradient);
            this.typeface = FontTypefaceCache.GetTypeface(other.FontName, other.FontStyle);
        }

        base.Initialize(other);
    }

    public void Apply()
    {
        Reapply();
    }

    public void Dispose()
    {
        paint.Dispose();
    }

    protected override void UpdateTransform(Matrix3x2 transform)
    {
        canvas.SetMatrix(transform.AsSKMatrix());
        base.UpdateTransform(transform);
    }

    protected override void UpdateFillColor(Color fillColor)
    {
        paint.Color = fillColor.AsSKColor();
        base.UpdateFillColor(fillColor);
    }

    protected override void UpdateGradient(Gradient fillGradient)
    {
        this.gradientShader = GradientShaderCache.GetShader(fillGradient);
        base.UpdateGradient(fillGradient);
    }

    protected override void UpdateStrokeColor(Color strokeColor)
    {
        paint.Color = strokeColor.AsSKColor();
        base.UpdateStrokeColor(strokeColor);
    }

    protected override void UpdateDrawMode(DrawMode drawMode)
    {
        switch (drawMode)
        {
            case DrawMode.Fill:
                paint.Color = FillColor.AsSKColor();
                paint.Style = SKPaintStyle.Fill;
                paint.Shader = null;
                break;
            case DrawMode.Stroke:
                paint.Color = StrokeColor.AsSKColor();
                paint.Style = SKPaintStyle.Stroke;
                paint.Shader = null;
                break;
            case DrawMode.Gradient:
                paint.Shader = gradientShader;
                paint.Style = SKPaintStyle.Fill;
                break;
            case DrawMode.Textured:
                paint.Shader = fillTextureShader;
                paint.Style = SKPaintStyle.Fill;
                break;
            default:
                break;
        }

        base.UpdateDrawMode(drawMode);
    }

    protected override void UpdateStrokeWidth(float strokeWidth)
    {
        paint.StrokeWidth = strokeWidth;
        base.UpdateStrokeWidth(strokeWidth);
    }

    protected override void UpdateFillTexture(ITexture texture, Matrix3x2 transform, TileMode tileModeX, TileMode tileModeY)
    {
        if (texture is not null)
        {
            bool changed = false;
            changed |= this.FillTexture != texture;
            changed |= this.FillTextureTransform != transform;
            changed |= this.FillTextureTileModeX != tileModeX;
            changed |= this.FillTextureTileModeY != tileModeY;

            if (changed)
            {
                this.fillTextureShader?.Dispose();
                this.fillTextureShader = SKShader.CreateBitmap(SkiaInterop.GetBitmap(texture), tileModeX.AsSKShaderTileMode(), tileModeY.AsSKShaderTileMode(), transform.AsSKMatrix());
            }
        }

        base.UpdateFillTexture(texture, transform, tileModeX, tileModeY);
    }

    protected override void UpdateFontStyle(float size, FontStyle style)
    {
        this.paint.TextSize = size;
        UpdateTypeface(this.FontName, style);
        base.UpdateFontStyle(size, style);
    }

    protected override void UpdateFont(string name)
    {
        UpdateTypeface(name, this.FontStyle);
        base.UpdateFont(name);
    }

    private void UpdateTypeface(string name, FontStyle style)
    {
        if (this.FontName != name || this.FontStyle != style)
        {
            paint.Typeface = this.typeface = FontTypefaceCache.GetTypeface(name, style);
        }
    }
}