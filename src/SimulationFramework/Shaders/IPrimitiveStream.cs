﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders;

public interface IPrimitiveStream<T>
    where T : unmanaged
{
    void EmitVertex(T vertex);
    void EndPrimitive();
}