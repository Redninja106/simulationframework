﻿using SimulationFramework.Drawing;
using SkiaSharp;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace SimulationFramework.SkiaSharp;

internal sealed class SkiaCanvasState : CanvasState, IDisposable
{
    private readonly SKCanvas canvas;
    private SKPaint paint;

    public bool IsActive => canvas is not null;
    public SKPaint Paint => paint;

    private SKShader? fillTextureShader;
    private SKShader? gradientShader;

    private SkiaFont? currentFont;
    private SKFont? skfont;

    public SkiaCanvasState(SKCanvas canvas, SkiaCanvasState? other = null)
    {
        this.canvas = canvas;
        this.Initialize(other);
    }

    [MemberNotNull(nameof(paint), nameof(skfont))]
    protected override void Initialize(CanvasState? other)
    {
        if (other is null)
        {
            this.paint = new SKPaint();
            this.skfont = new SKFont();
        }
        else
        {
            var o = ((SkiaCanvasState)other);
            this.paint = o.paint.Clone();
            this.skfont = new(o.skfont.Typeface, o.skfont.Size);
            if (other.Gradient != null)
            {
                this.gradientShader = GradientShaderCache.GetShader(other.Gradient);
            }
        }

        base.Initialize(other);
    }

    public void Apply()
    {
        Reapply();
    }

    public void Dispose()
    {
        fillTextureShader?.Dispose();
        skfont.Dispose();
        // paint.Dispose();
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

    protected override void UpdateGradient(Gradient? fillGradient)
    {
        if (fillGradient is not null)
        {
            this.gradientShader = GradientShaderCache.GetShader(fillGradient);
        }

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
            case DrawMode.Shader:
                {
                    var (effect, uniforms) = RuntimeEffectCache.GetValue(Shader);
                    using var skshader = effect.ToShader(uniforms);
                    Paint.Shader = skshader;
                    Paint.Style = SKPaintStyle.Fill;
                }
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

    protected override void UpdateFillTexture(ITexture? texture, Matrix3x2 transform, TileMode tileModeX, TileMode tileModeY)
    {
        if (texture is SkiaTexture skTexture)
        {
            bool changed = false;
            changed |= this.FillTexture != texture;
            changed |= this.FillTextureTransform != transform;
            changed |= this.FillTextureTileModeX != tileModeX;
            changed |= this.FillTextureTileModeY != tileModeY;

            if (changed)
            {
                this.fillTextureShader?.Dispose();
                this.fillTextureShader = SKShader.CreateImage(skTexture.GetImage(), tileModeX.AsSKShaderTileMode(), tileModeY.AsSKShaderTileMode(), transform.AsSKMatrix());
            }
        }

        base.UpdateFillTexture(texture, transform, tileModeX, tileModeY);
    }

    protected override void UpdateFontStyle(FontStyle style)
    {
        skfont.Typeface = currentFont?.GetTypeface(style);
        base.UpdateFontStyle(style);
    }

    protected override void UpdateFontSize(float fontSize)
    {
        this.skfont.Size = fontSize;
        base.UpdateFontSize(fontSize);
    }

    protected override void UpdateFont(IFont? font)
    {
        if (font != null)
        {
            currentFont = (SkiaFont)font;
            skfont.Typeface = currentFont.GetTypeface(FontStyle);
        }
        base.UpdateFont(font);
    }

    protected override void UpdateAntialias(bool antialias)
    {
        paint.IsAntialias = antialias;
        base.UpdateAntialias(antialias);
    }
}