using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Messaging;

/// <summary>
/// Specifies how a message listener should be prioritized by the dispatcher. Higher priority listeners are notified before lower ones.
/// </summary>
public enum ListenerPriority
{
    /// <summary>
    /// The listener should be notified after all other listeners of another priority.
    /// </summary>
    After,
    /// <summary>
    /// The listener is low priority.
    /// </summary>
    Low,
    /// <summary>
    /// The listener is normal priority.
    /// </summary>
    Normal,
    /// <summary>
    /// The listener is high priority.
    /// </summary>
    High,
    /// <summary>
    /// The listener is internal. This is for internal use only.
    /// </summary>
    Internal,
    /// <summary>
    /// The listener should be notified before all other listeners of another priority.
    /// </summary>
    Before,
}
