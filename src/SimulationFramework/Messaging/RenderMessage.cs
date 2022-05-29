using SimulationFramework.Drawing.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Messaging;

public sealed class RenderMessage : Message
{
    public ICanvas Canvas { get; private set; }
 
    public RenderMessage(ICanvas canvas)
    {
        Canvas = canvas;
    }
}
