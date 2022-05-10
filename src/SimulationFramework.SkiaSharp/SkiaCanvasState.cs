using SimulationFramework.Gradients;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.SkiaSharp;

internal sealed class SkiaCanvasState : IDisposable
{
    private SKCanvas canvas;
    private SKPaint paint;

    public bool IsActive => canvas is not null;
    public SKPaint Paint => paint;

    // properties on the paint object
    public DrawMode DrawMode { get; private set; }
    public float StrokeWidth { get; private set; }
    public Color Color { get; private set; }
    public FillTexture FillTexture { get; private set; }
    public Gradient Gradient { get; private set; }

    // properties on the canvas object
    public Matrix3x2 Transform { get; private set; }
    public Rectangle? ClipRect { get; private set; }

    private SKShader fillTextureShader;
    private SKShader gradientShader;

    private SkiaCanvasState()
    {
    }

    public void Activate(SKCanvas canvas)
    {
        this.canvas = canvas;

        UpdateTransform(Transform);
        UpdateClipRect(ClipRect);
    }

    public void Deactivate()
    {
        canvas = null;
    }

    public SkiaCanvasState Clone()
    {
        var clone = CreateDefault();

        clone.UpdateDrawMode(DrawMode);
        clone.UpdateStrokeWidth(StrokeWidth);
        clone.UpdateColor(Color);
        clone.UpdateFillTexture(FillTexture);
        clone.UpdateGradient(Gradient);
        clone.UpdateTransform(Transform);
        clone.UpdateClipRect(ClipRect);

        return clone;
    }

    public static SkiaCanvasState CreateDefault()
    {
        var state = new SkiaCanvasState();
        
        state.paint = new SKPaint();

        state.UpdateDrawMode(DrawMode.Fill);
        state.UpdateColor(Color.White);
        state.UpdateStrokeWidth(1.0f);
        state.UpdateFillTexture(new FillTexture(null));
        state.UpdateGradient(null);
        state.UpdateTransform(Matrix3x2.Identity);

        return state;
    }

    public void Dispose()
    {
        paint.Dispose();
    }

    public void UpdateDrawMode(DrawMode mode)
    {
        switch (mode)
        {
            case DrawMode.Fill:
                paint.Style = SKPaintStyle.Fill;
                break;
            case DrawMode.Border:
                paint.Style = SKPaintStyle.Stroke;
                break;
            case DrawMode.Gradient:
                paint.Style = SKPaintStyle.Fill;
                paint.Shader = gradientShader;
                break;
            case DrawMode.Textured:
                paint.Style = SKPaintStyle.Fill;
                paint.Shader = fillTextureShader;
                break;
            default:
                throw new ArgumentException("Unknown DrawMode value!", nameof(mode));
        }

        this.DrawMode = mode;
    }

    public void UpdateColor(Color color)
    {
        this.Color = color;
        paint.Color = color.AsSKColor();
    }

    public void UpdateStrokeWidth(float strokeWidth)
    {
        this.StrokeWidth = strokeWidth;
        this.paint.StrokeWidth = strokeWidth;
    }

    internal void UpdateFillTexture(FillTexture texture)
    {
        this.FillTexture = texture;

        this.fillTextureShader?.Dispose();

        if (texture.Texture is not null)
        {
            this.fillTextureShader = SKShader.CreateBitmap(
                SkiaInterop.GetBitmap(texture.Texture),
                texture.TileModeX.AsSKShaderTileMode(),
                texture.TileModeY.AsSKShaderTileMode(),
                texture.Transform.AsSKMatrix()
                );
        }

        if (this.DrawMode == DrawMode.Textured)
        {
            paint.Shader = fillTextureShader;
        }
    }

    internal void UpdateGradient(Gradient gradient)
    {
        this.Gradient = gradient;

        this.gradientShader?.Dispose();

        if (gradient is not null)
        {
            this.gradientShader = GradientShaderCache.GetShader(gradient);
        }
        
        if (this.DrawMode == DrawMode.Gradient)
        {
            paint.Shader = gradientShader;
        }
    }

    public void UpdateTransform(Matrix3x2 transform)
    {
        this.Transform = transform;

        if (this.IsActive)
        {
            this.canvas.SetMatrix(transform.AsSKMatrix());
        }
    }

    public void UpdateClipRect(Rectangle? clipRect)
    {
        this.ClipRect = clipRect;

        if (this.IsActive)
        {
            //throw new InvalidOperationException("Clipping is not supported on skia!");
        }
    }
}