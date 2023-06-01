namespace SimulationFramework.Messaging;
public class DisplayMessage : Message
{
    public IDisplay Display { get; }

    public DisplayMessage(IDisplay display)
    {
        Display = display;
    }
}
