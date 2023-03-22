using SimulationFramework;
using SimulationFramework.Shaders;
using System.Numerics;

namespace Grass;
struct GrassFragmentShader : IShader
{
    [Output(OutputSemantic.Color)]
    ColorF outputColor;

    [Uniform]
    public ColorF TopColor;

    [Uniform]
    public ColorF BottomColor;

    [Input]
    float scaledHeight;

    public void Main()
    {
        outputColor = ColorF.Lerp(BottomColor, TopColor, scaledHeight);
    }
}