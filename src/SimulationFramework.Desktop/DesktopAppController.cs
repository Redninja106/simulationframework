using Silk.NET.Maths;
using Silk.NET.Windowing;
using SimulationFramework.Drawing;
using SimulationFramework.Messaging;

namespace SimulationFramework.Desktop;

internal class DesktopAppController : IApplicationController
{
    private readonly IWindow window;
    
    private bool isRunning;

    // where the window was located before we went into fullscreen
    private Vector2D<int>? lastWindowPosition;
    
    public DesktopAppController(IWindow window)
    {
        this.window = window;
    }

    public bool ApplyConfig(AppConfig config)
    {
        try
        {
            window.IsVisible = true;
            window.Title = config.Title;
            if (config.Fullscreen)
            {
                window.Size = window.Monitor.Bounds.Size;
                lastWindowPosition = window.Position;
                window.Position = new(0, 0);
                window.WindowBorder = WindowBorder.Hidden;
            }
            else
            {
                window.Size = new(config.Width, config.Height);

                // set old position if we're coming out of fullscreen
                if (lastWindowPosition is not null)
                {
                    window.Position = lastWindowPosition.Value;
                    lastWindowPosition = null;
                }
                
                if (config.TitlebarHidden)
                {
                    window.WindowBorder = WindowBorder.Hidden;
                }
                else
                {
                    if (config.Resizable)
                    {
                        window.WindowBorder = WindowBorder.Resizable;
                    }
                    else
                    {
                        window.WindowBorder = WindowBorder.Fixed;
                    }
                }
            }
        }
        catch
        {
            return false;
        }
        
        return true;
    }

    public void InitializeConfig(AppConfig config)
    {
        config.Width = window.Size.X;
        config.Height = window.Size.Y;
        config.Title = window.Title;
        config.Fullscreen = window.WindowBorder == WindowBorder.Fixed && window.Size == window.Monitor.Bounds.Size && window.Position == Vector2D<int>.Zero;
        config.TitlebarHidden = window.WindowBorder == WindowBorder.Hidden && !config.Fullscreen;
        config.Resizable = window.WindowBorder == WindowBorder.Resizable && !config.Fullscreen;
    }

    public void Dispose()
    {
    }

    public void Initialize(Application application)
    {
        window.Resize += size => application.Dispatcher.ImmediateDispatch(new ResizeMessage(size.X, size.Y));

        window.Closing += () =>
        {
            application.Dispatcher.ImmediateDispatch<ExitMessage>(new());
        };

        application.Dispatcher.Subscribe<ExitMessage>(m => 
        {
            isRunning = false;
        });
    }
    
    public void Start(MessageDispatcher dispatcher)
    {
        isRunning = true;

        dispatcher.ImmediateDispatch(new InitializeMessage());
    
        while (isRunning)
        {
            window.DoEvents();
            dispatcher.ImmediateDispatch(new FrameBeginMessage());
            dispatcher.Flush();

            dispatcher.ImmediateDispatch(new RenderMessage(Graphics.GetOutputCanvas()));

            window.GLContext.SwapBuffers();

            dispatcher.ImmediateDispatch(new FrameEndMessage());
            dispatcher.Flush();
        }

        dispatcher.ImmediateDispatch(new UninitializeMessage());
    }    
}