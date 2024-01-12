using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WebGPU;

namespace SimulationFramework.Drawing.WebGPU.Renderers;
internal class ColoredRectangleRenderer : RectangleRenderer
{
    BufferWriter<Color> colorWriter;

    public ColoredRectangleRenderer(GraphicsResources resources, WebGPUCanvas canvas) : base(resources, canvas)
    {
        colorWriter = new(resources, BufferUsage.Vertex, capacity);
    }

    public override void Submit(RenderPassEncoder encoder)
    {
        var colorBuffer = colorWriter.GetBuffer();
        encoder.SetVertexBuffer(1, colorBuffer, 0, colorBuffer.Size);

        base.Submit(encoder);
    }

    public void RenderRectangle(Rectangle rectangle, Color color)
    {
        this.RenderRectangle(rectangle);
        colorWriter.Write([color]);
    }

    public override void OnFlush()
    {
        colorWriter.Upload();
        colorWriter.Reset();

        base.OnFlush();
    }

    protected override RenderPipelineDescriptor GetRenderPipelineDescriptor()
    {
        var descriptor = base.GetRenderPipelineDescriptor();
        descriptor.Vertex.Buffers = [.. descriptor.Vertex.Buffers,
            new VertexBufferLayout()
            {
                ArrayStride = (ulong)Unsafe.SizeOf<Color>(),
                Attributes = [new VertexAttribute(VertexFormat.Unorm8x4, 0, 1)],
                StepMode = VertexStepMode.Instance,
            }];
        return descriptor;
    }

}
