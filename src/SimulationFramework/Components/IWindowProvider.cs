using SimulationFramework.Drawing;

namespace SimulationFramework.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public interface IWindowProvider : ISimulationComponent
{
    string Title { get; set; }
    IDisplay Display { get; }
    int Width { get; set; }
    int Height { get; set; }
    
    bool IsUserResizable { get; set; }
    bool ShowSystemMenu { get; set; }

    bool IsMinimized { get; }
    bool IsMaximized { get; }

    ITexture GetBackBuffer();

    void GetPosition(out int x, out int y);
    bool TrySetPosition(int x, int y);
    bool TryResize(int width, int height);
}