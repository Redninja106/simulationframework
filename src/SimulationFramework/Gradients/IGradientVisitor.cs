using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Gradients;

public interface IGradientVisitor
{
    void VisitRadial(Gradient gradient, Vector2 position, float radius);
    void VisitLinear(Gradient gradient, Vector2 from, Vector2 to);
}
