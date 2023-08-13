namespace SimulationFramework.Messaging;

/// <summary>
/// Sent when the application is requested to exit.
/// </summary>
public sealed class ExitMessage : Message
{
    /// <summary>
    /// <see langword="true"/> if this exit message can be cancelled. To cancel an exit message, use <see cref="Application.CancelExit"/>.
    /// </summary>
    public bool IsCancellable { get; }

    /// <summary>
    /// Creates a new instance of the <see cref="ExitMessage"/> class.
    /// </summary>
    public ExitMessage(bool isCancellable)
    {
        IsCancellable = isCancellable;
    }
}
