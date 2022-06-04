using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Canvas;

/// <summary>
/// Options for drawing text.
/// </summary>
[Flags]
public enum FontStyle
{
    /// <summary>
    /// Normal text.
    /// </summary>
    Normal = 0,
    ///
    Bold = 1 << 0,
    /// 
    Italic = 1 << 1,
    ///
    Underline = 1 << 2,
    ///
    Strikethrough = 1 << 3,
}