using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Messaging;

/// <summary>
/// Sent when the simulation should render a frame.
/// </summary>
public sealed class RenderMessage : Message
{
    /// <summary>
    /// The canvas the simulation is rendering to.
    /// </summary>
    public ICanvas Canvas { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RenderMessage"/> class.
    /// </summary>
    /// <param name="canvas">The canvas the simulation is rendering to.</param>
    public RenderMessage(ICanvas canvas)
    {
        Canvas = canvas;
    }
}
