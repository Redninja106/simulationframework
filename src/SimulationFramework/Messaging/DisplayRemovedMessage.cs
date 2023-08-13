namespace SimulationFramework.Messaging;

/// <summary>
/// Dispatched when a display is removed by the system.
/// </summary>
public class DisplayRemovedMessage : DisplayMessage
{
    /// <summary>
    /// Creates a instance of the <see cref="DisplayRemovedMessage"/> class.
    /// </summary>
    /// <param name="display">The display that was removed.</param>
    public DisplayRemovedMessage(IDisplay display) : base(display)
    {
    }
}
