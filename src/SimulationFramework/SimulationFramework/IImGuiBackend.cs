using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;

namespace SimulationFramework;

public interface IImGuiBackend : IDisposable
{
    void NewFrame();
    void Render();
    IntPtr GetTextureID(ISurface surface);
}