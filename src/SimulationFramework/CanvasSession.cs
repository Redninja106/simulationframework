using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

public struct CanvasSession : IDisposable
{
    private readonly ICanvas canvas;

    public CanvasSession(ICanvas canvas)
    {
        this.canvas = canvas;
    }

    public void Dispose()
    {
        canvas.Pop();
    }
}