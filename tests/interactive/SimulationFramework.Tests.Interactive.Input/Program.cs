using System;
using System.Numerics;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing;

namespace SimulationFramework.Tests.Interactive.Input;

class Program : Simulation
{
    //private int mouseScroll;

    static void Main()
    {
        var sim = new Program();
        sim.RunDesktop();
    }

    public override void OnInitialize(AppConfig config)
    {
        config.Title = "Input Tests";
        config.Width = 1920;
        config.Height = 1080;
    }

    public override void OnRender(ICanvas canvas)
    {
        //canvas.SetFont("arial", TextStyles.Default, 20);
        //Color background = (45, 45, 45);
        //canvas.Clear(background);

        //// draw mouse
        //Color shapeBorderCol = (100, 100, 100);

        //canvas.PushState();
        
        //var mouseBounds = new Rectangle(canvas.Width * (3 / 4f), 0, canvas.Width / 6f, canvas.Height / 2f, Alignment.TopCenter);
        //var mouseWheelBounds = new Rectangle(
        //    mouseBounds.GetAlignedPoint(Alignment.Center).X,
        //    MathUtilities.Lerp(mouseBounds.GetAlignedPoint(Alignment.Center).Y, mouseBounds.GetAlignedPoint(Alignment.TopCenter).Y, .6f),
        //    mouseBounds.Width / 10f,
        //    mouseBounds.Height / 5f,
        //    Alignment.TopCenter);

        //canvas.DrawMode = DrawMode.Fill;
        //canvas.DrawArc(mouseBounds, 180, 90, true, Mouse.IsButtonDown(MouseButton.Left) ? (20, 20, 20) : background);
        //canvas.DrawArc(mouseBounds, 270, 180, true, Mouse.IsButtonDown(MouseButton.Right) ? (20, 20, 20) : background);
        //canvas.DrawArc(mouseBounds.GetAlignedPoint(Alignment.CenterLeft), new Vector2(mouseBounds.Width / 16f, mouseBounds.Height / 8f), 90, 0, true, Mouse.IsButtonDown(MouseButton.X1) ? (20,20,20) : background);
        //canvas.DrawArc(mouseBounds.GetAlignedPoint(Alignment.CenterLeft), new Vector2(mouseBounds.Width / 16f, mouseBounds.Height / 8f), 180, 90, true, Mouse.IsButtonDown(MouseButton.X2) ? (20,20,20) : background);
        //canvas.DrawEllipse(mouseWheelBounds, Mouse.IsButtonDown(MouseButton.Middle) ? (20, 20, 20) : background);

        //mouseScroll += Mouse.ScrollWheelDelta;

        //var scrollTextPos = mouseBounds.GetAlignedPoint(Alignment.CenterRight);
        //scrollTextPos.X += canvas.Width / 18f;
        //canvas.DrawText("Scroll: " + mouseScroll, scrollTextPos, shapeBorderCol);

        //canvas.StrokeColor(shapeBorderCol);

        //canvas.DrawArc(mouseBounds.GetAlignedPoint(Alignment.CenterLeft), new Vector2(mouseBounds.Width / 16f, mouseBounds.Height / 8f), 80, -120, true, shapeBorderCol);
        //canvas.DrawEllipse(mouseBounds, shapeBorderCol);
        //canvas.DrawLine(mouseBounds.GetAlignedPoint(Alignment.CenterLeft) - new Vector2(mouseBounds.Width / 16f, 0), mouseBounds.GetAlignedPoint(Alignment.CenterRight),  shapeBorderCol);
        //canvas.DrawLine(mouseBounds.GetAlignedPoint(Alignment.TopCenter), mouseWheelBounds.GetAlignedPoint(Alignment.TopCenter),  shapeBorderCol);
        //canvas.DrawLine(mouseBounds.GetAlignedPoint(Alignment.Center), mouseWheelBounds.GetAlignedPoint(Alignment.BottomCenter),  shapeBorderCol);
        //canvas.DrawEllipse(mouseWheelBounds, shapeBorderCol);

        //canvas.PopState();

        //// draw controller

        //canvas.PushState();

        //canvas.SetDrawMode(DrawMode.Border);
        //canvas.DrawEllipse(0, 0, 100, 100, shapeBorderCol, Alignment.TopLeft);
        
        //canvas.SetDrawMode(DrawMode.Fill);

        //canvas.DrawEllipse(new Vector2(100, 100) + 100 * Gamepad.RightJoystick, new Vector2(10, 10), (20, 20, 20), Alignment.Center);
        //canvas.DrawEllipse(new Vector2(100, 100) + 100 * Gamepad.RightJoystick, new Vector2(10, 10), (20, 20, 20), Alignment.Center);

        //canvas.DrawRect(200, 0, 25, 200 * Gamepad.LeftTrigger, Color.Orange);

        //canvas.PopState();

        //// draw keyboard

        //// draw area borders
        //DrawRegion(canvas, (0, 0, canvas.Width / 2f, canvas.Height / 2f));
        //DrawRegion(canvas, (canvas.Width / 2f, 0, canvas.Width / 2f, canvas.Height / 2f));

        //// draw something at mouse pos
        //canvas.DrawEllipse(Mouse.Position, new Vector2(5, 5), (Mouse.IsButtonDown(MouseButton.Left) ? Color.Black : (20, 20, 20)) with { A = 100 });
    }

    public void DrawRegion(ICanvas canvas, Rectangle region)
    {
        //using (canvas.PushState())
        //{
        //    canvas.SetDrawMode(DrawMode.Border);
        //    canvas.SetStrokeWidth(4f);
        //    canvas.DrawRect(region, Color.BlueViolet);
        //}
    }
}