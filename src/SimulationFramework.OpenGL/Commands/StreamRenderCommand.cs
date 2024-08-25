using SimulationFramework.Drawing;
using SimulationFramework.OpenGL.Geometry;
using SimulationFramework.OpenGL.Geometry.Streams;
using System.Collections.Generic;

namespace SimulationFramework.OpenGL.Commands;

class StreamRenderCommand : RenderCommand
{
    public GeometryBuffer buffer;
    public GeometryStream stream;
    private List<DrawInfo> draws = [];

    public StreamRenderCommand(GeometryEffect effect, CanvasState state) : base(effect, state)
    {
    }

    public override void Submit()
    {
        buffer.Upload();
        buffer.Bind();
        stream.BindVertexArray();

        for (int i = 0; i < draws.Count; i++)
        {
            glDrawArrays(draws[i].triangles ? GL_TRIANGLES : GL_LINES, draws[i].vertexOffset, draws[i].count);
        }
    }

    public void AddCommand(bool triangles, int offset, int count)
    {
        if (draws.Count > 0)
        {
            DrawInfo lastDraw = draws[^1];
            if (lastDraw.vertexOffset + lastDraw.count == offset && lastDraw.triangles == triangles)
            {
                lastDraw.count += count;
                draws[^1] = lastDraw;
                return;
            }
        }

        draws.Add(new()
        {
            triangles = triangles,
            vertexOffset = offset,
            count = count
        });
    }

    private struct DrawInfo
    {
        public bool triangles;
        public int vertexOffset;
        public int count;
    }
}
