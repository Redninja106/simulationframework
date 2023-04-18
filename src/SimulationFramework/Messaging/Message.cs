using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Messaging;

/// <summary>
/// The base class for all messages.
/// </summary>
public abstract class Message
{
    public float DispatchTime { get; internal set; }
}