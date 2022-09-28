using System;

namespace SimulationFramework.Drawing;

/// <summary>
/// A utility which allows <see langword="using"/> statements to be used with <see cref="ICanvas.Push"/> to automatically pop from the stack of the <see cref="ICanvas"/>.
/// </summary>
public struct CanvasSession : IDisposable
{
    private readonly ICanvas canvas;

    /// <summary>
    /// Creates a new CanvasSession. This should only ever be called from <see cref="ICanvas"/> implementations, as it does not call <see cref="ICanvas.Push"/>, only <see cref="ICanvas.Pop"/> (in the event that it is disposed).
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