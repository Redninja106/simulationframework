﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;

public interface IGeometry
{
    PrimitiveKind PrimitiveKind { get; }

    void Update(VertexData data);
}