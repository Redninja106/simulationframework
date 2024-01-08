using Silk.NET.GLFW;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using SimulationFramework.Components;
using SimulationFramework.Messaging;

namespace SimulationFramework.Desktop;

internal class DesktopSimulationController : ISimulationController
{
    private readonly IWindow window;
    
    private bool isRunning;

    private readonly Glfw glfw = Glfw.GetApi();

    public DesktopSimulationController(IWindow window)
    {
        this.window = window;
    }

    public void Dispose()
    {
    }

    public void Initialize(MessageDispatcher dispatcher)
    {
        window.Closing += () =>
        {
            Application.Exit(true);
        };
    }
    
    public unsafe void Start(Action runFrame)
    {
        var dispatcher = SimulationHost.Current!.Dispatcher;

        window.IsVisible = true;
        isRunning = true;
        var lastHeight = Window.Height;

        glfw.SetWindowRefreshCallback((WindowHandle*)window.Native!.Glfw!.Value, (window) =>
        {
            // runFrame();
        });

        window.Resize += size =>
        {
            dispatcher.ImmediateDispatch(new ResizeMessage(size.X, size.Y));
        };

        while (isRunning)
        {
            dispatcher.ImmediateDispatch<BeforeEventsMessage>(new());
            window.DoEvents();
            dispatcher.ImmediateDispatch<AfterEventsMessage>(new());
            runFrame();
        }
    }

    internal void NotifySuccessfulExit()
    {
        isRunning = false;
    }
}