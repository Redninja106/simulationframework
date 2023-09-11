using SimulationFramework.Drawing;
using System;

namespace SimulationFramework.SkiaSharp
{
    internal class SkiaFrame : SkiaGraphicsObject, ITexture
    {
        public SkiaFrame(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public int Width { get; }
        public int Height { get; }
        public TextureOptions Options => TextureOptions.None;

        public Span<Color> Pixels { get => throw new NotSupportedException(); }

        public void ApplyChanges()
        {
            throw new NotSupportedException();
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public ref Color GetPixel(int x, int y)
        {
            throw new NotSupportedException("This operation is not support on the main frame texture!");
        }

        public ICanvas GetCanvas()
        {
            return Graphics.GetOutputCanvas();
        }
    }
}