using Silk.NET.GLFW;
using SimulationFramework.Messaging;
using Monitor = Silk.NET.GLFW.Monitor;

namespace SimulationFramework.Desktop;

internal unsafe class DesktopDisplay : IDisplay
{
    internal readonly Monitor* monitor;

    public bool IsPrimary { get; }
    public Rectangle Bounds { get; }
    public string Name { get; }
    public float Scaling { get; }
    public float RefreshRate { get; }

    private static GlfwCallbacks.MonitorCallback? monitorCallback;
    private static List<DesktopDisplay>? displays;

    private DesktopDisplay(Monitor* monitor)
    {
        this.monitor = monitor;

        var glfw = Glfw.GetApi();
        
        this.IsPrimary = monitor == glfw.GetPrimaryMonitor();

        Name = glfw.GetMonitorName(monitor);
        glfw.GetMonitorContentScale(monitor, out var xScale, out var yScale);

        glfw.GetMonitorPos(monitor, out int x, out int y);
        
        VideoMode* videoMode = glfw.GetVideoMode(monitor);
        RefreshRate = videoMode->RefreshRate;
        int width = videoMode->Width;
        int height = videoMode->Height;

        Bounds = new Rectangle(x, y, width, height);
    }

    public static List<DesktopDisplay> GetDisplayList()
    {
        if (displays is null)
        {
            displays = new();

            var glfw = Glfw.GetApi();
            monitorCallback = MonitorCallback;
            glfw.SetMonitorCallback(monitorCallback);

            Monitor** monitors = glfw.GetMonitors(out int count);

            for (int i = 0; i < count; i++)
            {
                Monitor* monitor = monitors[i];
                displays.Add(new DesktopDisplay(monitor));
            }
        }

        return displays;
    }
    static void MonitorCallback(Monitor* monitor, ConnectedState state)
    {
        var glfw = Glfw.GetApi();

        DesktopDisplay display;
        switch (state)
        {
            case ConnectedState.Connected:
                display = new(monitor);
                displays!.Add(display);
                SimulationHost.Current!.Dispatcher.ImmediateDispatch(new DisplayAddedMessage(display));
                break;
            case ConnectedState.Disconnected:
                display = displays!.Single(d => d.monitor == monitor);
                displays!.Remove(display);
                SimulationHost.Current!.Dispatcher.ImmediateDispatch(new DisplayRemovedMessage(display));
                break;
            default:
                throw new();
        }
    }

    public override string ToString()
    {
        return Name;
    }
}