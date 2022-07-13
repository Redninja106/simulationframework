using SimulationFramework;
using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BasicInput;

internal class BasicInputSimulation : Simulation
{
    // this example shows how to take user input from the mouse, keyboard, and gamepad.
    // it draws a box that can be moved using WASD or the left stick of the gamepad.
    // a line is drawn from that box to the mouse position. 

    private Vector2 boxPosition;
    private Vector2 boxSize;

    public override void OnInitialize(AppConfig config)
    {
        config.Title = "Basic Input Example";
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.Gray);

        canvas.Fill(Color.White);
        canvas.DrawRect(boxPosition, boxSize);
    }
}
