using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebGPU;

namespace SimulationFramework.Drawing.WebGPU.Renderers;
internal abstract class Renderer : IDisposable
{
    public GraphicsResources Resources { get; }
    public WebGPUCanvas Canvas { get; }

    protected RenderPipeline RenderPipeline { get; }

    public Renderer(GraphicsResources resources, WebGPUCanvas canvas)
    {
        Resources = resources;
        Canvas = canvas;

        Init(resources);

        RenderPipeline = resources.Device.CreateRenderPipeline(this.GetRenderPipelineDescriptor());
    }

    protected virtual void Init(GraphicsResources resources) { }

    public abstract void OnFlush();
    public abstract void Submit(RenderPassEncoder encoder);

    protected abstract RenderPipelineDescriptor GetRenderPipelineDescriptor();

    public void Dispose()
    {
        RenderPipeline.Dispose();
    }
}
