﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders;
public abstract class CanvasShader
{
    public Matrix3x2 TransformMatrix { get; set; }

    [ShaderIntrinsic]
    protected void Discard()
    {
        throw new Exception("Shader discarded");
    }

    public abstract ColorF GetPixelColor(Vector2 position);
}