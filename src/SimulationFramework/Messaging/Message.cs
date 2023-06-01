namespace SimulationFramework.Messaging;

/// <summary>
/// The base class for all messages.
/// </summary>
public abstract class Message
{
    public float DispatchTime { get; internal set; }
}