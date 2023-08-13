namespace SimulationFramework.Messaging;

/// <summary>
/// Specifies how a message listener should be prioritized by the dispatcher. Higher priority listeners are notified before lower ones.
/// </summary>
public enum ListenerPriority
{
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
    Internal
}
