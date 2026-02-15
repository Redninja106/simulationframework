using Silk.NET.Windowing;
using SimulationFramework.Messaging;
using SimulationFramework.Components;
using Silk.NET.Windowing.Glfw;
using Silk.NET.Input;
using Silk.NET.Input.Glfw;
using Silk.NET.OpenGL;
using SimulationFramework.Desktop.Audio;
using SimulationFramework.OpenGL;
using Silk.NET.GLFW;

namespace SimulationFramework.Desktop;

/// <summary>
/// Implements a simulation environment which runs the simulation in a window.
/// </summary>
public class DesktopPlatform : ISimulationPlatform
{
    public IWindow Window { get; }

    private readonly IInputContext inputContext;
    private GLFrame frame;

    public DesktopPlatform(WindowOptions? windowOptions = null)
    {
        windowOptions ??= WindowOptions.Default with
        {
            API = GraphicsAPI.Default with
            {
                Version = new APIVersion(4, 5),
#if DEBUG
                Flags = ContextFlags.Debug
#endif
            },
            Samples = 1,
            PreferredBitDepth = null,
            PreferredDepthBufferBits = 32,
            PreferredStencilBufferBits = 8,
        };

        GlfwWindowing.RegisterPlatform();
        GlfwInput.RegisterPlatform();

        GlfwWindowing.Use();

        Window = Silk.NET.Windowing.Window.Create(windowOptions.Value with { IsVisible = false, });
        Window.Initialize();

        inputContext = Window.CreateInput();
    }

    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
        inputContext.Dispose();
        Window.Dispose();
    }

    public virtual void Initialize(MessageDispatcher dispatcher)
    {
        dispatcher.Subscribe<ResizeMessage>(m =>
        {
            frame.Resize(m.Width, m.Height);
            //frameProvider?.Resize(m.Width, m.Height);
        });

        frame = new(Window.Size.X, Window.Size.Y);
        // frameProvider = new DesktopSkiaFrameProvider(Window.Size.X, Window.Size.Y);
        Application.RegisterComponent(new DesktopApplicationProvider());
        var graphics = CreateGraphicsProvider();

        if (graphics != null)
        {
            Application.RegisterComponent(graphics);
        }

        Application.RegisterComponent(CreateTimeProvider());
        Application.RegisterComponent(CreateSimulationController());
        Application.RegisterComponent(CreateWindowProvider());
        Application.RegisterComponent(new DesktopAudioProvider());

        RegisterInputProviders();

        Application.RegisterComponent(new DesktopImGuiComponent(Window, inputContext));
    }


    public static void Register()
    {
        SimulationHost.RegisterPlatform(() => new DesktopPlatform());
    }

    protected virtual IGraphicsProvider CreateGraphicsProvider()
    {
        return new GLGraphics(frame, "#version 450 core", name => Window.GLContext!.TryGetProcAddress(name, out nint addr) ? addr : 0);
    }

    protected virtual ITimeProvider CreateTimeProvider()
    {
        return new RealTimeProvider();
    }

    protected virtual ISimulationController CreateSimulationController()
    {
        return new DesktopSimulationController(this.Window);
    }

    protected virtual IWindowProvider CreateWindowProvider()
    {
        return new DesktopWindowProvider(this.Window);
    }

    protected virtual void RegisterInputProviders()
    {
        var mouse = inputContext.Mice.FirstOrDefault();

        if (mouse is not null)
        {
            unsafe
            {
                Application.RegisterComponent(new DesktopMouseProvider(mouse, (WindowHandle*)Window.Native.Glfw!.Value));
            }
        }

        var keyboard = inputContext.Keyboards.FirstOrDefault();

        if (keyboard is not null)
        {
            Application.RegisterComponent(new DesktopKeyboardProvider(keyboard));
        }

        Application.RegisterComponent(new DesktopGamepadProvider(inputContext));
    }
}