using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;

namespace SimulationFramework;

/// <summary>
/// Defines a the api for an ImGui Backend.
/// </summary>
public interface IImGuiBackend : IDisposable
{
    /// <summary>
    /// Called at the start of a new frame.
    /// </summary>
    void NewFrame();
    /// <summary>
    /// Called when the backend should render the GUI.
    /// </summary>
    void Render();
    /// <summary>
    /// Gets the texture ID that should be provided to ImGui for <see cref="ISurface"/>.
    /// </summary>
    /// <returns>The ImGui texture ID of the surface.</returns>
    IntPtr GetTextureID(ISurface surface);
}