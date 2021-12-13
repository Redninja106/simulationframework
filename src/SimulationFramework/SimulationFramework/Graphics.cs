using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

public static class Graphics
{
    private static IGraphicsProvider Provider => Simulation.Current.GetComponent<IGraphicsProvider>();

    public static ICanvas GetFrameCanvas()
    {
        return Provider.GetFrameCanvas();
    }

    public static ISurface CreateSurface(string file)
    {
        // TODO: perhaps use NativeMemory.Alloc() here to save on big array?
        
        var fileData = File.ReadAllBytes(file);

        return Provider.CreateSurface(fileData.AsSpan());
    }

    public static ISurface CreateSurface(int width, int height)
    {
        return Provider.CreateSurface(width, height, null);
    }

    public static ISurface CreateSurface(int width, int height, Span<Color> colors)
    {
        return Provider.CreateSurface(width, height, colors);
    }

    public static ISurface CreateSurface(int width, int height, Color[] colors)
    {
        return Provider.CreateSurface(width, height, colors.AsSpan());
    }
}