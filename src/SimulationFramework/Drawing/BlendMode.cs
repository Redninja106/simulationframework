using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;
public enum BlendMode
{
    Zero,
    One,

    SourceColor,
    InverseSourceColor,

    DestinationColor,
    InverseDestinationColor,

    SourceAlpha,
    InverseSourceAlpha,

    DestinationAlpha,
    InverseDestinationAlpha,

    ConstantColor,
    InverseConstantColor,
}
