using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Messaging;
public class DisplayMessage : Message
{
    public IDisplay Display { get; }

    public DisplayMessage(IDisplay display)
    {
        Display = display;
    }
}
