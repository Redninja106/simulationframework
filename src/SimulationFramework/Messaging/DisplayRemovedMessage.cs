namespace SimulationFramework.Messaging;
public class DisplayRemovedMessage : DisplayMessage
{
    public DisplayRemovedMessage(IDisplay display) : base(display)
    {
    }
}
