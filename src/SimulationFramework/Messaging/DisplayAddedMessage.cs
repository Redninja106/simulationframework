namespace SimulationFramework.Messaging;
public class DisplayAddedMessage : DisplayMessage
{
    public DisplayAddedMessage(IDisplay display) : base(display)
    {
    }
}
