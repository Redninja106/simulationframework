using SimulationFramework.Drawing;
using System.Numerics;

namespace SimulationFramework.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public interface IWindowProvider : ISimulationComponent
{
    string Title { get; set; }
    IDisplay Display { get; }
    int Width { get; set; }
    int Height { get; set; }
    Vector2 Position { get; }

    bool IsUserResizable { get; set; }
    bool ShowSystemMenu { get; set; }

    bool IsMinimized { get; }
    bool IsMaximized { get; }

    ITexture GetBackBuffer();

    bool TryMove(int x, int y);
    bool TryResize(int width, int height);
}