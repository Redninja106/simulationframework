namespace SimulationFramework.Messaging;

/// <summary>
/// Base class for display-related messages.
/// </summary>
public abstract class DisplayMessage : Message
{
    /// <summary>
    /// The display which the message pertains to.
    /// </summary>
    public IDisplay Display { get; }

    /// <summary>
    /// Creates a new instance of the <see cref="DisplayMessage"/> class. 
    /// </summary>
    /// <param name="display">The display which the message pertains to.</param>
    protected DisplayMessage(IDisplay display)
    {
        Display = display;
    }
}
