using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Gradients;

internal record LinearGradient(Vector2 From, Vector2 To, GradientStop[] Stops, Matrix3x2 Transform, TileMode TileMode) : Gradient(Stops, Transform, TileMode)
{
    public override void Accept(IGradientVisitor inspector)
    {
        inspector.VisitLinear(this, From, To);
    }
}
