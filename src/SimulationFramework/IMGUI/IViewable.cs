using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.IMGUI;

/// <summary>
/// Provides support for custom ImGui views for objects. 
/// </summary>
public interface IViewable
{
    /// <summary>
    /// Called when the object should use the <see cref="ImGui"/> layout to create it's custom view.
    /// </summary>
    void Layout();
}