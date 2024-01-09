using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebGPU;

namespace SimulationFramework.Drawing.WebGPU.Renderers;
internal abstract class Renderer
{
    public abstract bool RequiresFlush();
    public abstract void OnFlush();

    public abstract void StartPass(RenderPassEncoder encoder);
    public abstract void FinishPass(RenderPassEncoder encoder);
}
