using Silk.NET.GLFW;
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
    private Glfw glfw = Glfw.GetApi();

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
            dispatcher.ImmediateDispatch<ExitMessage>(new());
        };

        dispatcher.Subscribe<ExitMessage>(m => 
        {
            isRunning = false;
        });
    }
    
    public unsafe void Start(Action runFrame)
    {
        var dispatcher = SimulationHost.Current.Dispatcher;

        window.IsVisible = true;
        isRunning = true;
        var lastHeight = Window.Height;

        glfw.SetFramebufferSizeCallback((WindowHandle*)window.Native.Glfw.Value, (window, width, height) =>
        {
            // when resizing, the window contents seem to lag behind the rest of the window by ~1 on the y axis
            // calculate change in size and adjust so content stays in place
            var dh = height - lastHeight;
            Graphics.GetOutputCanvas().Translate(0, -dh);
            lastHeight = height;

            runFrame();
            this.window.GLContext.SwapBuffers();
        });

        window.Resize += size =>
        {
            dispatcher.ImmediateDispatch(new ResizeMessage(size.X, size.Y));
        };

        while (isRunning)
        {
            dispatcher.ImmediateDispatch<BeforeEventsMessage>(new());
            window.DoEvents();
            runFrame();
            window.GLContext.SwapBuffers();
        }
    }    
}