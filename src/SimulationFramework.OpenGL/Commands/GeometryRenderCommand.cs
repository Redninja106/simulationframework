using SimulationFramework.Drawing;
using SimulationFramework.OpenGL.Geometry;
using System;
using System.Numerics;

namespace SimulationFramework.OpenGL.Commands;

class GeometryRenderCommand : RenderCommand
{
    public IGeometry Geometry { get; init; }
    Matrix4x4[] instanceTransforms;

    public GeometryRenderCommand(IGeometry geometry, GeometryEffect effect, CanvasState state) : base(effect, state)
    {
        Geometry = geometry;
    }

    public override void Submit()
    {
        throw new NotImplementedException();
    }
}
