using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace SimulationFramework.SkiaSharp;

public interface ISkiaFrameProvider
{
    void SetContext(GRContext context);
    SKCanvas GetCurrentFrame();
}