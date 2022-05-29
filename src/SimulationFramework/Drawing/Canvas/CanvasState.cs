using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Canvas;

/// <summary>
/// Provides access to state-specific data of an <see cref="ICanvas"/>.
/// </summary>
public abstract class CanvasState
{
    public Color StrokeColor { get; private set; }
    public Color FillColor { get; private set; }

    public ITexture FillTexture { get; private set; }
    public Matrix3x2 FillTextureTransform { get; private set; }
    public TileMode FillTextureTileModeX { get; private set; }
    public TileMode FillTextureTileModeY { get; private set; }

    public Gradient FillGradient { get; private set; }
    public Gradient StrokeGradient { get; private set; }

    /// <summary>
    /// Gets or sets the canvas's current transformation matrix.
    /// </summary>
    public Matrix3x2 Transform { get; private set; }

    /// <summary>
    /// Gets or sets the clip region of the canvas. Set this to <see langword="null"/> to disable clipping.
    /// </summary>
    public Rectangle? ClipRegion { get; private set; }

    /// <summary>
    /// Gets or sets the stroke width of the canvas. This value only has an effect on drawing when drawing lines or when <see cref="DrawMode"/> is <see cref="DrawMode.Border"/>.
    /// </summary>
    public float StrokeWidth { get; private set; }

    /// <summary>
    /// Gets or sets the drawing mode of the canvas.
    /// <para>
    /// Each kind of <see cref="Canvas.DrawMode"/> has it's own state in the canvas, 
    /// so setting this property will use whatever state was last set for that value.
    /// </para>
    /// </summary>
    public DrawMode DrawMode { get; private set; }

    protected virtual void Initialize(CanvasState other)
    {
        if (other is null)
        {
            UpdateClipRegion(null);

            UpdateTransform(Matrix3x2.Identity);

            UpdateStrokeColor(Color.Black);
            UpdateStrokeWidth(1f);
            UpdateStrokeGradient(null);

            UpdateFillColor(Color.White);
            UpdateFillGradient(null);
            UpdateFillTexture(null, Matrix3x2.Identity, TileMode.Clamp, TileMode.Clamp);

            UpdateDrawMode(DrawMode.Fill);
        }
        else
        {
            UpdateClipRegion(other.ClipRegion);

            UpdateTransform(other.Transform);

            UpdateFillColor(other.FillColor);
            UpdateFillGradient(other.FillGradient);
            UpdateFillTexture(other.FillTexture, other.FillTextureTransform, other.FillTextureTileModeX, other.FillTextureTileModeY);

            UpdateStrokeColor(other.StrokeColor);
            UpdateStrokeWidth(other.StrokeWidth);
            UpdateStrokeGradient(other.StrokeGradient);

            UpdateDrawMode(other.DrawMode);
        }
    }

    internal protected virtual void UpdateFillTexture(ITexture texture, Matrix3x2 transform, TileMode tileModeX, TileMode tileModeY)
    {
        FillTexture = texture;
        FillTextureTransform = transform;
        FillTextureTileModeX = tileModeX;
        FillTextureTileModeY = tileModeY;
    }

    internal protected virtual void UpdateStrokeColor(Color strokeColor)
    {
        StrokeColor = strokeColor;
    }

    internal protected virtual void UpdateFillColor(Color fillColor)
    {
        FillColor = fillColor;
    }

    internal protected virtual void UpdateFillGradient(Gradient fillGradient)
    {
        FillGradient = fillGradient;
    }

    internal protected virtual void UpdateStrokeGradient(Gradient strokeGradient)
    {
        StrokeGradient = strokeGradient;
    }

    internal protected virtual void UpdateTransform(Matrix3x2 transform)
    {
        Transform = transform;
    }

    internal protected virtual void UpdateClipRegion(Rectangle? clipRegion)
    {
        ClipRegion = clipRegion;
    }

    internal protected virtual void UpdateStrokeWidth(float strokeWidth)
    {
        StrokeWidth = strokeWidth;
    }

    internal protected virtual void UpdateDrawMode(DrawMode drawMode)
    {
        DrawMode = drawMode;
    }
}
