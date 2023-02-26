using SimulationFramework.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;

[ShaderIntrinsic]
public sealed record TextureSampler(
    FilterMode FilterMode, 
    TileMode TileModeX, 
    TileMode TileModeY
    )
{
    public static TextureSampler Linear { get; } = new(FilterMode.Linear, TileMode.None);
    public static TextureSampler Point { get; } = new(FilterMode.Point, TileMode.None);

    public TextureSampler(FilterMode filterMode, TileMode tileMode) : this(filterMode, tileMode, tileMode)
    {
    }
}
