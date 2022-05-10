using System;
using System.Numerics;

namespace SimulationFramework.Gradients;

internal record RadialGradient(Vector2 Position, float Radius, GradientStop[] Stops, Matrix3x2 Transform, TileMode TileMode) : Gradient(Stops, Transform, TileMode)
{
    public override void Accept(IGradientVisitor inspector)
    {
        inspector.VisitRadial(this, Position, Radius);
    }
}