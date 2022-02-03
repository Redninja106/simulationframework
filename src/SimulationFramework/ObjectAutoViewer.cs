using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SimulationFramework.IMGUI;

namespace SimulationFramework;

/// <summary>
/// An IViewable which generates a layout for an object using reflection.
/// </summary>
public sealed class ObjectAutoViewer : IViewable
{
    /// <summary>
    /// The target object.
    /// </summary>
    public object Object { get; private set; }

    /// <summary>
    /// Creates a new ObjectAutoViewable targeting the provided object.
    /// </summary>
    /// <param name="obj">The target object.</param>
    public ObjectAutoViewer(object obj)
    {
        Object = obj;
    }

    /// <inheritdoc/>
    public void Layout()
    {
        var type = Object.GetType();
        var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var prop in props)
        {
            ImGui.Text(prop.Name + " " + prop.Name + ": " + prop.GetValue(Object).ToString());
        }
    }
}