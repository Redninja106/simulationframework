using ImGuiNET;
using SimulationFramework;
using SimulationFramework.SkiaSharp;
using SkiaSharp;
using System.Drawing;

namespace SkiaSimulation
{
    internal class SkiaSimulation : Simulation
    {
        public SkiaSimulation()
        {
        }

        public override void OnInitialize(AppConfig config)
        {
            config.UseSkia();
            config.WindowWidth = 1920;
            config.WindowHeight = 1080;
            config.Title = "A Simulation Using SkiaSharp!";
        }

        public override void OnRender(float dt, Graphics graphics)
        {
            var canvas = graphics.GetCanvas();
            canvas.Clear(new SKColor(60, 80, 130));
            canvas.DrawRect(100,100,100,100, graphics.GetSharedPaint(Color.CornflowerBlue));
        }

        private AppConfig config;

        public override void OnUpdate(float dt, Input input)
        {
            if (config == null)
                config = OpenConfig();

            if (ImGui.Begin("Simulation Config Editor"))
            {
                bool resizable = config.Resizable;
                ImGui.Checkbox("Resizeable", ref resizable);
                config.Resizable = resizable;

                if (ImGui.Button("Apply"))
                {
                    config.Apply();
                    config = OpenConfig();
                }
            }
            ImGui.End();
        }

        public override void OnUnitialize()
        {
        }
    }
}