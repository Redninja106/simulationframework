namespace SimulationFramework.Messaging;

/// <summary>
/// Dispatched when a system adds a new display.
/// </summary>
public class DisplayAddedMessage : DisplayMessage
{
    /// <summary>
    /// Creates a new instance of the <see cref="DisplayAddedMessage"/> class.
    /// </summary>
    /// <param name="display">The display that was added.</param>
    public DisplayAddedMessage(IDisplay display) : base(display)
    {
    }
}
