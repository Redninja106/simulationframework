using SimulationFramework;
using SimulationFramework.Drawing;
using System.Numerics;

Start<Program>();

partial class Program : Simulation
{
    const float CellSize = 120;
    const float Padding = 20;
    const float LabelHeight = 30;

    static readonly BlendMode[] Modes =
    [
        BlendMode.Alpha,
        BlendMode.Overwrite,
        BlendMode.Add,
        BlendMode.Subtract,
        BlendMode.Min,
        BlendMode.Max,
        BlendMode.Multiply,
    ];

    public override void OnInitialize()
    {
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.FromHSV(0, 0, 0.15f));

        float startX = Padding;
        float startY = Padding;

        for (int i = 0; i < Modes.Length; i++)
        {
            BlendMode mode = Modes[i];

            float x = startX + i * (CellSize + Padding);
            float y = startY;

            canvas.PushState();

            canvas.Fill(Color.White);
            canvas.DrawAlignedText(mode.ToString(), 18, x + CellSize / 2, y, Alignment.TopCenter);

            y += LabelHeight;

            canvas.Fill(Color.FromHSV(0, 0, 0.25f));
            canvas.BlendMode(BlendMode.Alpha);
            canvas.DrawRect(x, y, CellSize, CellSize);

            canvas.Fill(new ColorF(0.2f, 0.4f, 0.8f, 1f));
            canvas.DrawCircle(x + CellSize * 0.35f, y + CellSize * 0.45f, CellSize * 0.25f);

            canvas.Fill(new ColorF(0.1f, 0.7f, 0.3f, 1f));
            canvas.DrawCircle(x + CellSize * 0.65f, y + CellSize * 0.45f, CellSize * 0.25f);

            canvas.BlendMode(mode);
            canvas.Fill(new ColorF(0.9f, 0.2f, 0.1f, 0.7f));
            canvas.DrawCircle(x + CellSize * 0.5f, y + CellSize * 0.6f, CellSize * 0.3f);

            canvas.PopState();
        }
    }
}
