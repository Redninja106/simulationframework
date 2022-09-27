using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;

public interface ITexture : ITexture<Color>
{
    public Color Sample(Vector2 uv) => Sample(uv.X, uv.Y);

    public Color Sample(float u, float v)
    {
        return GetPixel((int)u, (int)v);
    }
}