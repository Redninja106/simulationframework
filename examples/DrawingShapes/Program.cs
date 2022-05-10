using System.Numerics;
using System.Runtime.CompilerServices;
using SimulationFramework;
using SimulationFramework.Desktop;
using SimulationFramework.Gradients;
using SimulationFramework.IMGUI;

using var sim = new DrawingShapesSimulation();
sim.RunWindowed("Shapes!", 1920, 1080, false);

class DrawingShapesSimulation : Simulation
{
    ITexture texture;
    string text = "Hello world!";
    public override void OnInitialize(AppConfig config)
    {
        texture = Graphics.LoadTexture("logo-512x512.png");
    }

    float radius = 0;
    float x, y;
    private Gradient gradient = Gradient.CreateRadial(Vector2.Zero, 50f, Color.Orange, Color.Green);
    
    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.LightGray);

        canvas.Translate(canvas.Width / 2, canvas.Height / 2);

        for (int i = 0; i < 100000; i++)
        {
            using(canvas.Push())
                canvas.DrawRect(0, 0, 100, 100, Alignment.Center);
        }

        //canvas.Translate(canvas.Width / 2, canvas.Height / 2);

        //canvas.Color = Color.OrangeRed;
        //canvas.DrawRect(new(10, 10), new(100, 100));

        //canvas.Color = Color.Purple;
        //canvas.DrawEllipse(new(90, 10), new(50, 60));

        //canvas.DrawMode = DrawMode.Border;
        //canvas.StrokeWidth = 10;
        //canvas.Color = Color.HotPink;
        //canvas.DrawRect(-50, -50, 110, 120);

        //canvas.DrawMode = DrawMode.Fill;
        //canvas.Color = Color.Indigo;
        ////canvas.SetFont("verdana", TextStyles.Underline, 40);
        ////canvas.DrawText(text, new(0, -canvas.Height / 4), Alignment.Center);

        //canvas.DrawMode = DrawMode.Border;
        //canvas.StrokeWidth = 2;

        //canvas.DrawMode = DrawMode.Fill;
        //canvas.Color = Color.Gray;
        //canvas.DrawRect(new(0, 100), new(50, 50), Alignment.BottomRight);
        //canvas.Color = Color.Black;
        //canvas.DrawRect(new(0, 100), new(50, 50), Alignment.BottomLeft);
        //canvas.Color = Color.White;
        //canvas.DrawRect(new(0, 100), new(50, 50), Alignment.TopRight);
        //canvas.Color = Color.DarkGray;
        //canvas.DrawRect(new(0, 100), new(50, 50), Alignment.TopLeft);

        //canvas.Color = Color.Bisque;
        //canvas.DrawRoundedRect(new(0, -canvas.Height / 5), new(90, 90), 25);

        //canvas.Color = Color.White;
        //canvas.DrawLine(100, 100, -110, -100);

        //canvas.Color = Color.LightGray;
        //canvas.DrawArc(300, 0, 100, 100, 0, radius, true);
        //canvas.Color = Color.SteelBlue;
        //canvas.DrawArc(300, 0, 90, 90, 0, radius, true);
        //canvas.Color = Color.LightGray;
        //canvas.DrawMode = DrawMode.Border;
        //canvas.DrawArc(300, 0, 70, 70, 0, radius, false);
        //canvas.DrawMode = DrawMode.Fill;
        //canvas.Color = Color.LightGray;
        //canvas.DrawArc(300, 0, 50, 50, 0, radius, true);

        //radius = MathF.Sin(Time.TotalTime) * MathF.Tau;

        ////canvas.FillTexture = new(texture, Matrix3x2.CreateTranslation(x, y), TileMode.None);
        ////canvas.DrawMode = DrawMode.Textured;
        ////canvas.Color = Color.OldLace;
        ////canvas.DrawRect(x, y, 300, 300, Alignment.Center);

        ////canvas.DrawMode = DrawMode.Gradient;
        //////canvas.SetGradientRadial(Alignment.Center, 50f, Color.Orange, Color.Green);
        ////canvas.Gradient = this.gradient;
        ////canvas.DrawEllipse(0, 0, 50, 50, Alignment.Center);

        //canvas.DrawMode = DrawMode.Fill;
        //canvas.Color = Keyboard.IsKeyDown(Key.G) ? Color.Purple : Color.Peru;
        //canvas.DrawEllipse(Mouse.Position - new Vector2(canvas.Width / 2f, canvas.Height / 2f), new Vector2(100, 100));
    }


    public override void OnUnitialize()
    {
    }
}