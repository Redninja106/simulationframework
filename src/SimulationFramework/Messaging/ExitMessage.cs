namespace SimulationFramework.Messaging;

/// <summary>
/// Sent when the application is requested to exit.
/// </summary>
public sealed class ExitMessage : Message
{
    public bool IsCancellable { get; }

    public ExitMessage(bool isCancellable)
    {
        IsCancellable = isCancellable;
    }
}
