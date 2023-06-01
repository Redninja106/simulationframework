using SkiaSharp;

namespace SimulationFramework.SkiaSharp;

public interface ISkiaFrameProvider
{
    void SetContext(GRContext context);
    SKCanvas GetCurrentFrame();
    void GetCurrentFrameSize(out int width, out int height);
}