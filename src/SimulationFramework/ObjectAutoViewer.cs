using System;
using System.Collections;
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
        LayoutRecurse(this.Object);
    }

    private void LayoutRecurse(object obj)
    {
        var type = obj.GetType();
        
        var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var prop in props)
        {
            if (prop.GetValue(obj) is IEnumerable enumerable)
            {
                if (ImGui.TreeNode(prop.Name))
                {
                    foreach (var item in enumerable)
                    {
                        if (ImGui.TreeNode(item.ToString()))
                        {
                            LayoutRecurse(item);
                            ImGui.TreePop();
                        }
                    }

                    ImGui.TreePop();
                }
            }
            else
            {
                ImGui.Text(prop.Name + ": " + prop.GetValue(obj).ToString());
            }
        }
    }
}