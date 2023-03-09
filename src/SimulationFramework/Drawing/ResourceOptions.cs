using System;

namespace SimulationFramework.Drawing;

/// <summary>
/// Provides of control over the functionality of a resource./>.
/// </summary>
[Flags]
public enum ResourceOptions
{
    /// <summary>
    /// Default resource behavior.
    /// </summary>
    None = 0,
    Immutable,
    FrequentUpdate,
}