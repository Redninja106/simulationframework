using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;

/// <summary>
/// Provides access to state-specific data of an <see cref="ICanvas"/>.
/// </summary>
public abstract class CanvasState
{
    /// <summary>
    /// The color of points and lines when <see cref="DrawMode"/> is <see cref="DrawMode.Stroke"/>.
    /// </summary>
    public Color StrokeColor { get; private set; }

    /// <summary>
    /// The color of filled regions when <see cref="DrawMode"/> is <see cref="DrawMode.Fill"/>.
    /// </summary>
    public Color FillColor { get; private set; }

    /// <summary>
    /// The texture used to fill when <see cref="DrawMode"/> is <see cref="DrawMode.Textured"/>, or <see langword="null"/> if none is set.
    /// </summary>
    public ITexture? FillTexture { get; private set; }

    /// <summary>
    /// The transform applied to the fill texture. This property's default value is <see cref="Matrix3x2.Identity"/>.
    /// </summary>
    public Matrix3x2 FillTextureTransform { get; private set; }

    /// <summary>
    /// The tiling mode of the fill texture on the X axis. This property's default value is <see cref="TileMode.Clamp"/>.
    /// </summary>
    public TileMode FillTextureTileModeX { get; private set; }

    /// <summary>
    /// The tiling mode of the fill texture on the Y axis. This property's default value is <see cref="TileMode.Clamp"/>.
    /// </summary>
    public TileMode FillTextureTileModeY { get; private set; }

    /// <summary>
    /// The gradient used to fill regions when <see cref="DrawMode"/> is <see cref="DrawMode.Gradient"/>, or <see langword="null"/> if none is set.
    /// </summary>
    public Gradient? Gradient { get; private set; }

    /// <summary>
    /// The canvas's current transformation matrix.
    /// </summary>
    public Matrix3x2 Transform { get; private set; }

    /// <summary>
    /// The clip region of the canvas, or <see langword="null"/> if clipping is disabled.
    /// </summary>
    public Rectangle? ClipRegion { get; private set; }

    /// <summary>
    /// The stroke width of the canvas. This value only has an effect on drawing when drawing lines or when <see cref="DrawMode"/> is <see cref="DrawMode.Stroke"/>.
    /// </summary>
    public float StrokeWidth { get; private set; }

    /// <summary>
    /// Gets or sets the drawing mode of the canvas.
    /// <para>
    /// The state for each value of <see cref="Drawing.DrawMode"/> is stored independently.
    /// Setting this property will use the previous state for that <see cref="DrawMode"/> value.
    /// </para>
    /// </summary>
    public DrawMode DrawMode { get; private set; }

    /// <summary>
    /// The font size when drawing text.
    /// </summary>
    public float FontSize { get; private set; }

    /// <summary>
    /// The font style whe drawing text.
    /// </summary>
    public FontStyle FontStyle { get; private set; }

    /// <summary>
    /// The name of the current font.
    /// </summary>
    public string? FontName { get; private set; }

    /// <summary>
    /// Initializes this <see cref="CanvasState"/> instance, optionally based off another instance.
    /// </summary>
    /// <param name="other">The <see cref="CanvasState"/> instance which this state should be initialized to match, or null if this state should be initialized to its default values.</param>
    protected virtual void Initialize(CanvasState? other)
    {
        if (other is null)
        {
            UpdateClipRegion(null);

            UpdateTransform(Matrix3x2.Identity);

            UpdateStrokeColor(Color.Black);
            UpdateStrokeWidth(1f);

            UpdateFillColor(Color.White);
            UpdateGradient(null);
            UpdateFillTexture(null, Matrix3x2.Identity, TileMode.Clamp, TileMode.Clamp);

            UpdateDrawMode(DrawMode.Fill);

            UpdateFont("Verdana");
            UpdateFontStyle(16, FontStyle.Normal);
        }
        else
        {
            UpdateValues(other);
        }
    }

    // updates this to match another state
    private void UpdateValues(CanvasState other)
    {
        UpdateClipRegion(other.ClipRegion);

        UpdateTransform(other.Transform);

        UpdateFillColor(other.FillColor);
        UpdateGradient(other.Gradient);
        UpdateFillTexture(other.FillTexture, other.FillTextureTransform, other.FillTextureTileModeX, other.FillTextureTileModeY);

        UpdateStrokeColor(other.StrokeColor);
        UpdateStrokeWidth(other.StrokeWidth);

        UpdateDrawMode(other.DrawMode);

        UpdateFont(other.FontName);
        UpdateFontStyle(other.FontSize, other.FontStyle);
    }

    internal protected void Reapply()
    {
        UpdateValues(this);
    }

    internal protected virtual void UpdateFillTexture(ITexture? texture, Matrix3x2 transform, TileMode tileModeX, TileMode tileModeY)
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

    internal protected virtual void UpdateGradient(Gradient? gradient)
    {
        Gradient = gradient;
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

    internal protected virtual void UpdateFontStyle(float size, FontStyle style)
    {
        FontSize = size;
        FontStyle = style;
    }

    internal protected virtual void UpdateFont(string? name)
    {
        FontName = name;
    }
}
