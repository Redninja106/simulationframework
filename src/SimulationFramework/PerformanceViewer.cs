using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationFramework.IMGUI;

namespace SimulationFramework;

/// <summary>
/// Provides a window with performance information. (default keybind F4)
/// </summary>
public sealed class PerformanceViewer : DebugWindow
{
    /// <summary>
    /// The interval at which the Performance viewer's FPS counter is updated.
    /// </summary>
    public static float FPSUpdateInterval { get => fpsUpdateInterval; set => fpsUpdateInterval = value; }

    private static float fpsUpdateInterval = .5f;

    private static Stopwatch stopwatch = new Stopwatch();

    private static Node lastFrameRoot = null;
    private static Node rootTreeNode = null;
    private static Node currentNode = null;

    private static Queue<float> pastFramerates = new();
    private static float averageFramerate;

    internal PerformanceViewer() : base("Performance", IMGUI.WindowFlags.MenuBar)
    {
    }

    /// <inheritdoc/>
    protected override (Key, Key) GetDefaultKeybind()
    {
        return (Key.F4, Key.Unknown);
    }

    /// <inheritdoc/>
    protected override void OnLayout()
    {
        pastFramerates.Enqueue(Performance.Framerate);
        
        if (pastFramerates.Sum(x=>1.0/x) >= fpsUpdateInterval)
        {
            averageFramerate = pastFramerates.Average();
            pastFramerates.Clear();
        }

        if (ImGui.BeginMenuBar())
        {
            ImGui.Text($"{averageFramerate:f2} FPS");
            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.Text($"{(1000f/averageFramerate):f2}ms per frame");
                ImGui.Text("Use scroll wheel to edit:");
                
                ImGui.Text("FPS Update Interval");
                ImGui.SameLine();

                var input = Simulation.Current.GetComponent<IInputProvider>();
                
                input.OverrideCaptureState(false);
                fpsUpdateInterval = MathF.Max(fpsUpdateInterval + 0.05f * Mouse.ScrollWheelDelta, 0);
                input.RestoreCaptureState();
                
                ImGui.SliderFloat("", ref fpsUpdateInterval, 0, 2.5f, format: "%.2f");
                
                ImGui.Text($"Exact Values: {Performance.Framerate}FPS, {(1000f / Performance.Framerate):f2}ms per frame");
                ImGui.EndTooltip();
            }
            ImGui.EndMenuBar();
        }

        lastFrameRoot.Layout();
    }

    /// <summary>
    /// Begins tracking a group of tasks. Task groups can be nested inside other groups. 
    /// </summary>
    /// <param name="name">The name of the task group.</param>
    public static void BeginTaskGroup(string name)
    {
        if (rootTreeNode == null)
        {
            rootTreeNode = new Node(name);
            currentNode = rootTreeNode;
            stopwatch.Start();
        }
        else
        {
            currentNode = currentNode.AddChild(name);
        }
    }

    /// <summary>
    /// Ends tracking the current task group.
    /// </summary>
    public static void EndTaskGroup()
    {
        currentNode.SetEndTime(GetCurrentTime());
        currentNode = currentNode.Parent;

        if (currentNode == null)
        {
            stopwatch.Reset();
            lastFrameRoot = rootTreeNode;
            rootTreeNode = null;
        }
    }

    /// <summary>
    /// Adds a completed task to the current group.
    /// </summary>
    /// <param name="name">The name of the task.</param>
    public static void MarkTaskCompleted(string name)
    {
        BeginTaskGroup(name);
        EndTaskGroup();
    }

    private static double GetCurrentTime()
    {
        return stopwatch.ElapsedTicks / (double)Stopwatch.Frequency;
    }

    private class Node
    {
        public string Name { get; }
        public double EndTime { get; set; }
        public Node Parent { get; }
        public List<Node> Children { get; } = new();

        public Node(string name, Node parent = null)
        {
            this.Name = name;
            this.Parent = parent;
        }

        public Node AddChild(string name)
        {
            var n = new Node(name, this);
            Children.Add(n);
            return n;
        }

        public void SetEndTime(double time)
        {
            this.EndTime = time;
        }

        public double GetStartTime()
        {
            if ((Parent?.Children?.IndexOf(this) ?? 0) > 0)
            {
                return Parent.Children[Parent.Children.IndexOf(this) - 1].EndTime;
            }
            else
            {
                return Parent?.GetStartTime() ?? 0;
            }
        }

        public void Layout()
        {
            bool isNodeOpened = false;

            if (Children.Any()) 
            { 
                isNodeOpened = ImGui.TreeNode(this.Name);
            }
            else
            {
                ImGui.Text(this.Name);
            }

            ImGui.SameLine();
            ImGui.Text($"\t{(EndTime - GetStartTime()) * 1000:f3}ms elapsed ({GetStartTime() * 1000:f1}ms-{EndTime * 1000:f3}ms)");

            if (isNodeOpened)
            {
                Children.ForEach(n => n.Layout());
                ImGui.TreePop();
            }
        }
    }
}