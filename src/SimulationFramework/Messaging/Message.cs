namespace SimulationFramework.Messaging;

/// <summary>
/// The base class for all messages.
/// </summary>
public abstract class Message
{
    /// <summary>
    /// The time at which the message was dispatched.
    /// </summary>
    public float DispatchTime { get; internal set; }
}