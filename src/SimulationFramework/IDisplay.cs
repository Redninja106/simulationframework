namespace SimulationFramework;
public interface IDisplay
{
    bool IsPrimary { get; }
    Rectangle Bounds { get; }
    string Name { get; }
    float Scaling { get; }
    float RefreshRate { get; }
}