using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Canvas;

/// <summary>
/// A utility which allows <see langword="using"/> statements to be used with <see cref="ICanvas.PushState"/> to automatically pop from the stack of the <see cref="ICanvas"/>.
/// </summary>
public readonly struct CanvasSession : IDisposable
{
    private readonly ICanvas canvas;

    /// <summary>
    /// Creates a new CanvasSession. This should only ever be called from <see cref="ICanvas"/> implementations, as it does not call <see cref="ICanvas.PushState"/>, only <see cref="ICanvas.PopState"/> (in the event that it is disposed).
    /// </summary>
    /// <param name="canvas"></param>
    public CanvasSession(ICanvas canvas)
    {
        this.canvas = canvas;
    }

    /// <summary>
    /// Calls <see cref="ICanvas.PopState"/> on this session's canvas.
    /// </summary>
    public void Dispose()
    {
        canvas?.PopState();
    }
}