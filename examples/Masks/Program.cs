using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Input;

Start<Program>();

partial class Program : Simulation
{
    IMask mask;

    public override void OnInitialize()
    {
        mask = Graphics.CreateMask(Window.Width, Window.Height);
        mask.Clear(false);
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.Black);

        if (Keyboard.IsKeyPressed(Key.C))
        {
            mask.Clear(false);
        }

        if (Mouse.IsButtonDown(MouseButton.Left))
        {
            canvas.Mask(mask);
            canvas.WriteMask(mask, true);
            for (int i = 0; i < 20; i++)
            {
                canvas.DrawCircle(Mouse.Position - (Mouse.DeltaPosition * (i / 20f)), 10, Alignment.Center);
            }
        }

        canvas.Fill(Color.Green);
        canvas.Mask(mask);
        canvas.WriteMask(null);
        canvas.DrawRect(0, 0, canvas.Width, canvas.Height);
    }
}