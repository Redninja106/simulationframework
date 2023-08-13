using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Messaging;

/// <summary>
/// Represents a message listener.
/// </summary>
public delegate void MessageListener<in T>(T message) where T : Message;