using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Specifies how a shape should be aligned to it's position.
/// </summary>
public enum Alignment
{
    /// <summary>
    /// The upper-left corner of the shape.
    /// </summary>
    TopLeft,
    /// <summary>
    /// The center of the top edge of the shape.
    /// </summary>
    TopCenter,
    /// <summary>
    /// The upper-right corner of the shape.
    /// </summary>
    TopRight,
    /// <summary>
    /// The center of the left edge of the shape.
    /// </summary>
    CenterLeft,
    /// <summary>
    /// The middle of the shape.
    /// </summary>
    Center,
    /// <summary>
    /// The center of the right edge of the shape.
    /// </summary>
    CenterRight,
    /// <summary>
    /// The lower-left corner of the shape.
    /// </summary>
    BottomLeft,
    /// <summary>
    /// The center of the bottom edge of the shape.
    /// </summary>
    BottomCenter,
    /// <summary>
    /// The lower-right corner of the shape.
    /// </summary>
    BottomRight,
}