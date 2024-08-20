using SimulationFramework.Drawing;

namespace SimulationFramework.OpenGL.Geometry.Writers;

class GeometryWriterCollection
{
    private readonly FillGeometryWriter fillGeometryWriter;
    private readonly HairlineGeometryWriter hairlineGeometryWriter;
    private readonly PathGeometryWriter pathGeometryWriter;

    public GeometryWriterCollection()
    {
        fillGeometryWriter = new FillGeometryWriter();
        hairlineGeometryWriter = new HairlineGeometryWriter();
        pathGeometryWriter = new PathGeometryWriter();
    }

    public GeometryWriter GetWriter(ref readonly CanvasState state, bool lineOverride = false)
    {
        if (state.Fill && !lineOverride)
        {
            return fillGeometryWriter;
        }

        if (state.StrokeWidth == 0)
        {
            return hairlineGeometryWriter;
        }
        else
        {
            pathGeometryWriter.StrokeWidth = state.StrokeWidth;
            return pathGeometryWriter;
        }
    }
}
