using SimulationFramework.Drawing;
using SimulationFramework.OpenGL.Geometry;
using SimulationFramework.OpenGL.Geometry.Streams;
using System;
using System.Numerics;

namespace SimulationFramework.OpenGL.Commands;

class GeometryRenderCommand : RenderCommand
{
    public GLGeometry Geometry { get; init; }
    Matrix4x4[] instanceTransforms;

    public GeometryRenderCommand(GLGeometry geometry, GeometryEffect effect, CanvasState state) : base(effect, state)
    {
        Geometry = geometry;
    }

    public override void Submit()
    {
        Geometry.Draw(in this.State);
    }
}
