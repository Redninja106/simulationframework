﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders;

public abstract class ComputeShader : Shader
{
    public abstract void RunThread(int i, int j, int k);
}
