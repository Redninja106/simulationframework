using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using SimulationFramework.Drawing;

namespace SimulationFramework.IMGUI;

/// <summary>
/// Defines a the api for an ImGui Backend.
/// </summary>
public interface IImGuiNETBackend : IDisposable
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
    /// Gets the texture ID that should be provided to ImGui for <see cref="ITexture"/>.
    /// </summary>
    /// <returns>The ImGui texture ID of the texture.</returns>
    IntPtr GetTextureID(ITexture texture);
}