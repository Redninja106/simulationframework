﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders;
public static class TetxureExtensions
{
    [ShaderIntrinsic]
    public static ColorF Sample(this ITexture texture, Vector2 uv)
    {
        throw new NotImplementedException();
    }
}