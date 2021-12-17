using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

public interface IInputProvider : ISimulationComponent
{
    Vector2 GetMousePosition();
    Vector2 GetMouseDelta();
    int GetScrollWheelDelta();
    bool IsMouseButtonDown(MouseButton key);
    bool IsKeyDown(Key key);
}