using ImGuiNET;
using SimulationFramework;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Input;
using System.Diagnostics;
using System.Numerics;
using System.Reflection;

Start<Program>(new DesktopPlatform());

partial class Program : Simulation
{
    public override unsafe void OnInitialize()
    {
    }
    
    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.FromHSV(0, 0, .15f));
    }
}