using System.Runtime.CompilerServices;
using SimulationFramework;
using SimulationFramework.Desktop;
using SimulationFramework.IMGUI;

using var sim = new DrawingShapesSimulation();
sim.RunWindowed("Shapes!", 1920, 1080);

class DrawingShapesSimulation : Simulation
{
    ITexture texture;
    string text = "Hello world!";
    public override void OnInitialize(AppConfig config)
    {
        texture = Graphics.LoadTexture("logo-512x512.png");
        config.SetAngleMode(AngleMode.Radians);
    }

    float radius = 0;
    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.GreenYellow);

        canvas.DrawTexture(texture, Alignment.TopLeft);
       
        canvas.Translate(canvas.Width / 2, canvas.Height / 2);

        canvas.DrawRect((10, 10), (100, 100), Color.OrangeRed);

        canvas.DrawEllipse((90, 10), (50, 60), Color.Purple);

        canvas.SetDrawMode(DrawMode.Border);
        canvas.SetStrokeWidth(10);
        canvas.DrawRect(-50, -50, 110, 120, Color.HotPink);

        canvas.SetDrawMode(DrawMode.Fill);
        canvas.SetFont("verdana", TextStyles.Underline, 40);
        canvas.DrawText(text, (0, -canvas.Height / 4), Color.Indigo, Alignment.Center);

        canvas.SetDrawMode(DrawMode.Border);
        canvas.SetStrokeWidth(2);

        canvas.SetDrawMode(DrawMode.Fill);
        canvas.DrawRect((0, 100), (50, 50), Color.Gray, Alignment.BottomRight);
        canvas.DrawRect((0, 100), (50, 50), Color.Black, Alignment.BottomLeft);
        canvas.DrawRect((0, 100), (50, 50), Color.White, Alignment.TopRight);
        canvas.DrawRect((0, 100), (50, 50), Color.DarkGray, Alignment.TopLeft);

        canvas.DrawRoundedRect((0, -canvas.Height / 5), (90, 90), 25, Color.Bisque);

        canvas.DrawLine(100, 100, -110, -100, Color.White);

        canvas.DrawArc(300, 0, 100, 100, 0, radius, true, Color.LightGray);
        canvas.DrawArc(300, 0, 90, 90, 0, radius, true, Color.SteelBlue);
        canvas.SetDrawMode(DrawMode.Border);
        canvas.DrawArc(300, 0, 70, 70, 0, radius, false, Color.LightGray);
        canvas.SetDrawMode(DrawMode.Fill);
        canvas.DrawArc(300, 0, 50, 50, 0, radius, true, Color.LightGray);

        radius = MathF.Sin(Time.TotalTime) * MathF.Tau;

        canvas.SetFillTexture(texture, Matrix3x2.CreateTranslation(x,y), TileMode.Stop);
        canvas.SetDrawMode(DrawMode.Textured);

        (x, y) = ImGui.DragFloat("pos", (x, y));
        canvas.DrawRect(x,y , 300, 300, Color.OldLace, Alignment.Center);

        canvas.SetDrawMode(DrawMode.Gradient);
        canvas.SetGradientRadial(Alignment.Center, 50f, Color.Orange, Color.Green);
        canvas.DrawEllipse(0, 0, 50, 50, Color.Black, Alignment.Center);

        canvas.SetDrawMode(DrawMode.Fill);
        canvas.DrawEllipse(Mouse.Position - (canvas.Width / 2f, canvas.Height / 2f), (100, 100), Keyboard.IsKeyDown(Key.G) ? Color.Purple : Color.Peru);
    }
    float x, y;

    public override void OnUnitialize()
    {
    }
}