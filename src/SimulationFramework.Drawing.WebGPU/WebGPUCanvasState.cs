using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.WebGPU;
internal class WebGPUCanvasState : CanvasState
{
    public WebGPUCanvasState(WebGPUCanvasState? parentState = null)
    {
        Initialize(parentState);
    }

    public void Reset()
    {
        Initialize(null);
    }
}
