using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;

/// <summary/>
public interface IGradientVisitor
{
    /// <summary/>
    void VisitRadial(Gradient gradient, Vector2 position, float radius);
    /// <summary/>
    void VisitLinear(Gradient gradient, Vector2 from, Vector2 to);
}
