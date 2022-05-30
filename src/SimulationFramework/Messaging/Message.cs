using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Messaging;
public abstract class Message
{
    /// <summary>
    /// The value of Time.TotalTime when the event was dispatched.
    /// </summary>
    public float DispatchTime { get; internal set; }
}
