using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;

/// <summary>
/// Options for drawing text.
/// </summary>
[Flags]
public enum TextStyles
{
    /// <summary>
    /// Normal text.
    /// </summary>
    Default = 0,
    ///
    Bold = 1 << 0,
    /// 
    Italic = 1 << 1,
    ///
    Underline = 1 << 2,
    ///
    Strikethrough = 1 << 3,
}