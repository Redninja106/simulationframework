using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.IMGUI;

// imgui won't be comment as it will be replace by a custom gui solution
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

[Flags]
public enum SliderFlags
{
    None = 0,
    AlwaysClamp = 16,
    Logarithmic = 32,
    NoRoundToFormat = 64,
    NoInput = 128,
}
