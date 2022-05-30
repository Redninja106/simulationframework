using SimulationFramework;
using SimulationFramework.Drawing.Canvas;
using System;
using System.Numerics;

namespace DrawingShapes;

class DrawingShapesSimulation : Simulation
{
    public override void OnInitialize(AppConfig config)
    {
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.Gray);

        canvas.Fill(Color.Red);
        canvas.DrawRect(100, 100, 100, 100);

        canvas.Fill(Gradient.CreateLinear(300, 100, 400, 200, Color.Blue, Color.Purple, Color.Red));
        canvas.DrawRect(300, 100, 100, 100);

        canvas.Stroke(Color.Yellow);
        canvas.DrawRect(500, 100, 100, 100);
        
        canvas.Stroke(Color.Green);
        
        canvas.StrokeWidth(10);
        canvas.DrawLine(500, 500, 1000, 1000);

        //canvas.StrokeWidth(0);
        canvas.DrawLine(100, 100, 500, 500);

        canvas.DrawPolygon(new[]
        {
            new Vector2(500, 10),
            new Vector2(10, 10),
            new Vector2(10, 500),
            Mouse.Position,
            new Vector2(500, 10),
        });

    }
}
