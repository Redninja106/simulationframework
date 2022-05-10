using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

public struct FillTexture
{
    public ITexture Texture { get; set; }
    public Matrix3x2 Transform { get; set; }
    public TileMode TileModeX { get; set; }
    public TileMode TileModeY { get; set; }
    
    public FillTexture() : this(null) { }

    public FillTexture(ITexture texture) : this(texture, Matrix3x2.Identity) { }

    public FillTexture(ITexture texture, TileMode tileMode) : this(texture, Matrix3x2.Identity, tileMode) { }
    
    public FillTexture(ITexture texture, Matrix3x2 transform) : this(texture, transform, TileMode.Clamp) { }

    public FillTexture(ITexture texture, Matrix3x2 transform, TileMode tileMode) : this(texture, transform, tileMode, tileMode) { }

    public FillTexture(ITexture texture, Matrix3x2 transform, TileMode tileModeX, TileMode tileModeY)
    {
        Texture = texture;
        Transform = transform;
        TileModeX = tileModeX;
        TileModeY = tileModeY;
    }
}