using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;

namespace SimulationFramework;

/// <summary>
/// Provides a window with performance information. (default keybind Alt+f4
/// </summary>
public sealed class PerformanceViewer : DebugWindow
{
    private Stopwatch stopwatch = new Stopwatch();

    private PerfTreeNode perfTreeRoot;
    private PerfTreeNode currentTreeNode;

    internal PerformanceViewer() : base("Performance", IMGUI.WindowFlags.MenuBar)
    {
    }

    /// <inheritdoc/>
    protected override void OnLayout()
    {
    }

    public void BeginGroup(string name)
    {
        if (!stopwatch.IsRunning)
            stopwatch.Start();

        if (perfTreeRoot == null)
        {
            perfTreeRoot = new(name, GetCurrentTimestamp());
            currentTreeNode = perfTreeRoot;
        }
        else
        {
            currentTreeNode.AddChild(name, GetCurrentTimestamp());
        }
    }

    public void EndGroup()
    {
        if (currentTreeNode == perfTreeRoot)
        {
            stopwatch.Reset();
            perfTreeRoot = null;
        }
        else
        {
            currentTreeNode = currentTreeNode.Parent;
        }
    }

    public void MakeCheckpoint(string name)
    {
        currentTreeNode.AddChild(name, GetCurrentTimestamp());
    }

    private double GetCurrentTimestamp()
    {
        return stopwatch.ElapsedTicks / (double)Stopwatch.Frequency;
    }

    private class PerfTreeNode
    {
        public double Timestamp { get; set; }
        public string Name { get; set; }
        public List<PerfTreeNode> Children { get; set; } = new();
        public PerfTreeNode Parent;

        public PerfTreeNode(string name, double timestamp, PerfTreeNode parent = null)
        {
            this.Name = name;
            this.Timestamp = timestamp;
            this.Parent = parent;
            this.Children = null;
        }

        public void AddChild(string name, double timestamp)
        {
            if (this.Children == null)
                this.Children = new();

            this.Children.Add(new PerfTreeNode(name, timestamp));
        }

        public void OnLayout()
        {
            if ((Children?.Count ?? 0) > 0)
            {
                ImGui.TreeNode(this.Name);
                ImGui.SameLine();
                ImGui.Separator();
                ImGui.SameLine();
                ImGui.Text(this.Timestamp.ToString());
            }
            else
            {
                ImGui.Text(this.Name);
                ImGui.SameLine();
                ImGui.Separator();
                ImGui.SameLine();
                ImGui.Text(this.Timestamp.ToString());
            }
        }
    }
}