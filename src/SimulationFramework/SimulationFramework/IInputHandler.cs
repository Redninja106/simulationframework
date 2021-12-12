using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

public interface IInputHandler
{
    Vector2 GetMousePosition(Key key);
    bool IsKeyPressed(Key key);
    bool WasKeyPressed(Key key);
}