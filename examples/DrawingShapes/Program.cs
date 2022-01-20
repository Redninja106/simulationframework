using System.Runtime.CompilerServices;
using SimulationFramework;
using SimulationFramework.Desktop;
using SimulationFramework.IMGUI;

using var sim = new DrawingShapesSimulation();
sim.RunWindowed("Shapes!", 1920, 1080);

class DrawingShapesSimulation : Simulation
{
    ISurface surface;

    public override void OnInitialize(AppConfig config)
    {
        surface = Graphics.LoadSurface("logo-512x512.png");
        config.SetAngleMode(AngleMode.Radians);
    }

    float radius = 0;
    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.GreenYellow);

        canvas.DrawSurface(surface, Alignment.TopLeft);
       
        canvas.Translate(canvas.Width / 2, canvas.Height / 2);

        canvas.DrawRect((10, 10), (100, 100), Color.OrangeRed);

        canvas.DrawEllipse((90, 10), (50, 60), Color.Purple);

        canvas.SetDrawMode(DrawMode.Border);
        canvas.SetStrokeWidth(10);
        canvas.DrawRect(-50, -50, 110, 120, Color.HotPink);

        canvas.SetDrawMode(DrawMode.Fill);
        canvas.SetFont("verdana", TextStyles.Underline, 40);
        canvas.DrawText("Hello, World!", (0, -TargetHeight / 4), Color.Indigo, Alignment.Center);

        canvas.SetDrawMode(DrawMode.Border);
        canvas.SetStrokeWidth(2);

        canvas.SetDrawMode(DrawMode.Fill);
        canvas.DrawRect((0, 100), (50, 50), Color.Gray, Alignment.BottomRight);
        canvas.DrawRect((0, 100), (50, 50), Color.Black, Alignment.BottomLeft);
        canvas.DrawRect((0, 100), (50, 50), Color.White, Alignment.TopRight);
        canvas.DrawRect((0, 100), (50, 50), Color.DarkGray, Alignment.TopLeft);

        canvas.DrawRoundedRect((0, -TargetHeight / 5), (90, 90), 25, Color.Bisque);

        canvas.DrawLine(100, 100, -110, -100, Color.White);

        canvas.DrawEllipse(300, 0, 100, 100, 0, radius, true, Color.LightGray);
        canvas.DrawEllipse(300, 0, 90, 90, 0, radius, true, Color.SteelBlue);
        canvas.SetDrawMode(DrawMode.Border);
        canvas.DrawEllipse(300, 0, 70, 70, 0, radius, false, Color.LightGray);
        canvas.SetDrawMode(DrawMode.Fill);
        canvas.DrawEllipse(300, 0, 50, 50, 0, radius, true, Color.LightGray);

        radius = MathF.Sin(Time.TotalTime) * MathF.Tau;

        canvas.SetFillTexture(surface, TileMode.Mirror);
        canvas.SetDrawMode(DrawMode.Textured);

        (x, y) = ImGui.DragFloat("pos", (x, y));
        canvas.DrawRect(x,y , 300, 300, Color.OldLace, Alignment.Center);
    }
    float x, y;

    public override void OnUnitialize()
    {
    }
}