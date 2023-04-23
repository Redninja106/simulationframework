using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Messaging;
public class DisplayRemovedMessage : DisplayMessage
{
    public DisplayRemovedMessage(IDisplay display) : base(display)
    {
    }
}
