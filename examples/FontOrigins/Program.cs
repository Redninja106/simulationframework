using SimulationFramework;
using SimulationFramework.Drawing;

Start<Program>();

partial class Program : Simulation
{
    string[] words = new[]
    {
        "Say",
        "Hello",
        "nears",
        "$#!&*"
    };

    public override void OnInitialize()
    {
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.Black);
        canvas.Translate(canvas.Width / 2f, canvas.Height / 2f);
        canvas.Scale(canvas.Width / 256f);

        canvas.DrawCircle(0, 0, .1f);

        float col1 = -96, col2 = -32, col3 = 32;

        for (int i = 0; i < words.Length; i++)
        {
            var word = words[i];

            var y = i * 20 - 40;

            canvas.FontStyle(FontStyle.Underline | FontStyle.Strikethrough);

            var alignment = Alignment.TopLeft;
            canvas.Stroke(Color.Green);
            canvas.DrawRect(new(col1, y), canvas.MeasureText(word, TextBounds.BestFit));
            canvas.Fill(Color.Green);
            canvas.DrawText(word, col1, y, alignment, TextBounds.BestFit);

            canvas.Stroke(Color.Blue);
            canvas.DrawRect(new(col2, y), canvas.MeasureText(word, TextBounds.Smallest));
            canvas.Fill(Color.Blue);
            canvas.DrawText(word, col2, y, alignment, TextBounds.Smallest);

            canvas.Stroke(Color.Yellow);
            canvas.DrawRect(new(col3, y), canvas.MeasureText(word, TextBounds.Largest));
            canvas.Fill(Color.Yellow);
            canvas.DrawText(word, col3, y, alignment, TextBounds.Largest);
        }
    }
}