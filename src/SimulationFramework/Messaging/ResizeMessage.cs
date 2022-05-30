using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Messaging;

public sealed class ResizeMessage : Message
{
    public int Width { get; private set; }
    public int Height { get; private set; }

    public ResizeMessage(int width, int height)
    {
        Width = width;
        Height = height;
    }
}
