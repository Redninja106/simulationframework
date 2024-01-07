using SimulationFramework.Drawing;
using System.Numerics;

namespace SimulationFramework.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public interface IWindowProvider : ISimulationComponent
{
    string Title { get; set; }
    IDisplay Display { get; }

    Vector2 Size { get; }
    Vector2 Position { get; }

    bool IsUserResizable { get; set; }
    bool ShowSystemMenu { get; set; }

    bool IsMinimized { get; }
    bool IsMaximized { get; }

    ITexture GetBackBuffer();

    void SetPosition(Vector2 position);
    void Resize(Vector2 size);

    void Maximize();
    void Minimize();
    void Restore();

    void SetIcon(ReadOnlySpan<Color> icon, int width, int height);
}