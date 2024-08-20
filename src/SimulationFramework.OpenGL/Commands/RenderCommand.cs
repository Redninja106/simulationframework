using SimulationFramework.Drawing;
using SimulationFramework.OpenGL.Geometry;

namespace SimulationFramework.OpenGL.Commands;

abstract class RenderCommand
{
    public GeometryEffect Effect { get; }
    private CanvasState state;

    public ref readonly CanvasState State => ref state;

    protected RenderCommand(GeometryEffect effect, CanvasState state)
    {
        Effect = effect;
        this.state = state;
    }

    public abstract void Submit();
}
