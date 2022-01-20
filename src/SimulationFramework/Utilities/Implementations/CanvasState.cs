using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Utilities.Implementations;

public unsafe sealed class CanvasState : IDisposable
{
    private readonly CanvasBase canvas;

    public Matrix3x2 Transform { get; set; } = Matrix3x2.Identity;
    public DrawMode DrawMode { get; set; }
    public float strokeWidth = 2;
    public Rectangle clipRect;

    public string fontName = "arial";
    public TextStyles styles = TextStyles.Default;
    public float size = 12;

    public Span<GradientStop> GradientStops => gradientStops.AsSpan();

    private GradientStop[] gradientStops;
    public Vector2 gradientFrom;
    public Vector2 gradientTo;
    public Alignment relativeGradientFrom;
    public Alignment relativeGradientTo;
    public TileMode gradientTileMode;
    public bool isGradientRelative;
    public bool isGradientRadial;
    public ITexture texture;
    public TileMode textureTileMode;
    public Matrix3x2 textureTransform;

    public CanvasState(CanvasBase canvas)
    {
        this.canvas = canvas;
    }

    public void UpdateGradient(Span<GradientStop> gradient)
    {
        this.gradientStops = new GradientStop[gradient.Length];
        gradient.CopyTo(this.gradientStops);
    }

    public CanvasState Clone()
    {
        return new CanvasState(this.canvas)
        {
            Transform = this.Transform,
            DrawMode = this.DrawMode,
            strokeWidth = this.strokeWidth,
            clipRect = this.clipRect,

            fontName = this.fontName,
            styles = this.styles,
            size = this.size,

            gradientStops = this.gradientStops?.Clone() as GradientStop[] ?? Array.Empty<GradientStop>(),
            gradientTileMode = this.gradientTileMode,

            gradientFrom = this.gradientFrom,
            gradientTo = this.gradientTo,

            relativeGradientFrom = this.relativeGradientFrom,
            relativeGradientTo = this.relativeGradientTo,

            isGradientRadial = this.isGradientRadial,
            isGradientRelative = this.isGradientRelative,
        };
    }

    public void Dispose()
    {
    }
}
