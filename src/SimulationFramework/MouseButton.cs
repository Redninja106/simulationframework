using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Represents buttons on a mouse.
/// </summary>
public enum MouseButton
{
    /// <summary>
    /// The left mouse button.
    /// </summary>
    Left,
    /// <summary>
    /// The right mouse button.
    /// </summary>
    Right,
    /// <summary>
    /// The right mouse button.
    /// </summary>
    Middle,

    /// <summary>
    /// The first extra mouse button (usually the forward side button).
    /// </summary>
    X1,
    /// <summary>
    /// The second extra mouse button (usually the backward side button).
    /// </summary>
    X2,
}