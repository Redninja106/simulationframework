using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.SkiaSharp
{
    internal class SkiaFrame : ITexture
    {
        public SkiaFrame(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public int Width { get; }
        public int Height { get; }

        public Span<Color> Pixels { get => throw new NotSupportedException(); }

        public void ApplyChanges()
        {
            throw new NotSupportedException();
        }

        public void Dispose()
        {
        }

        public ref Color GetPixel(int x, int y)
        {
            throw new NotSupportedException("This operation is not support on the main frame texture!");
        }

        public ICanvas OpenCanvas()
        {
            throw new NotSupportedException("This operation is not support on the main frame texture!");
        }
    }
}