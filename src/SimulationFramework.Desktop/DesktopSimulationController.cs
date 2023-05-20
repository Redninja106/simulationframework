using Silk.NET.Maths;
using Silk.NET.Windowing;
using SimulationFramework.Components;
using SimulationFramework.Drawing;
using SimulationFramework.Messaging;

namespace SimulationFramework.Desktop;

internal class DesktopSimulationController : ISimulationController
{
    private readonly IWindow window;
    
    private bool isRunning;

    // where the window was located before we went into fullscreen
    private Vector2D<int>? lastWindowPosition;
    
    public DesktopSimulationController(IWindow window)
    {
        this.window = window;
    }

    public void Dispose()
    {
    }

    public void Initialize(MessageDispatcher dispatcher)
    {
        window.Resize += size => dispatcher.ImmediateDispatch(new ResizeMessage(size.X, size.Y));

        window.Closing += () =>
        {
            dispatcher.ImmediateDispatch<ExitMessage>(new());
        };

        dispatcher.Subscribe<ExitMessage>(m => 
        {
            isRunning = false;
        });
    }
    
    public void Start(Action runFrame)
    {
        var dispatcher = SimulationHost.Current.Dispatcher;

        window.IsVisible = true;
        isRunning = true;

        while (isRunning)
        {
            dispatcher.ImmediateDispatch<BeforeEventsMessage>(new());
            window.DoEvents();
            runFrame();
            window.GLContext.SwapBuffers();
        }
    }    
}