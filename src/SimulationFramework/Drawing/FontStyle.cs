using System;

namespace SimulationFramework.Drawing;

/// <summary>
/// Options for drawing text. Multiple values can be combined using the "<c>|</c>" (bitwise or) operator.
/// </summary>
[Flags]
public enum FontStyle
{
    /// <summary>
    /// Normal text.
    /// </summary>
    Normal = 0,
    /// <summary>
    /// Bold text.
    /// </summary>
    Bold = 1 << 0,
    /// <summary>
    /// Italicized text.
    /// </summary>
    Italic = 1 << 1,
    /// <summary>
    /// Underlined text.
    /// </summary>
    Underline = 1 << 2,
    /// <summary>
    /// Crossed-out text.
    /// </summary>
    Strikethrough = 1 << 3,
}