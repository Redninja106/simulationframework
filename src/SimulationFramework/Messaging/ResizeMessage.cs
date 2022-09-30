using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Messaging;

/// <summary>
/// Sent when the simulation's window is resized.
/// </summary>
public sealed class ResizeMessage : Message
{
    /// <summary>
    /// The simulation's new width.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// The simulation's new height.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Creates a new instance of the <see cref="ResizeMessage"/> class.
    /// </summary>
    /// <param name="width">The simulation's new width.</param>
    /// <param name="height">The simulation's new height.</param>
    public ResizeMessage(int width, int height)
    {
        Width = width;
        Height = height;
    }
}
