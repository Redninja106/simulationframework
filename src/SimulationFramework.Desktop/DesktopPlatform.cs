using Silk.NET.Windowing;
using SimulationFramework.SkiaSharp;
using SimulationFramework.Messaging;
using SimulationFramework.Components;
using Silk.NET.Windowing.Glfw;
using Silk.NET.Input;
using Silk.NET.Input.Glfw;

namespace SimulationFramework.Desktop;

/// <summary>
/// Implements a simulation environment which runs the simulation in a window.
/// </summary>
public sealed class DesktopPlatform : ISimulationPlatform
{
    public IWindow Window { get; }

    private IInputContext inputContext;

    private DesktopSkiaFrameProvider frameProvider;

    public DesktopPlatform()
    {
        GlfwWindowing.RegisterPlatform();
        GlfwInput.RegisterPlatform();

        GlfwWindowing.Use();

        Window = Silk.NET.Windowing.Window.Create(WindowOptions.Default with { IsVisible = false });
        Window.Initialize();

        inputContext = Window.CreateInput();
    }

    public void Dispose()
    {
    }

    public void Initialize(MessageDispatcher dispatcher)
    {
        dispatcher.Subscribe<ResizeMessage>(m =>
        {
            frameProvider?.Resize(m.Width, m.Height);
        });

        frameProvider = new DesktopSkiaFrameProvider(Window.Size.X, Window.Size.Y);
        Application.RegisterComponent(new SkiaGraphicsProvider(frameProvider, name => Window.GLContext.TryGetProcAddress(name, out nint addr) ? addr : 0));
        Application.RegisterComponent(new RealtimeProvider());
        Application.RegisterComponent(new DesktopAppController(this.Window));
        Application.RegisterComponent(new DesktopWindowProvider(this.Window));
        Application.RegisterComponent(new DesktopImGuiComponent(this.Window, inputContext));

        var mouse = inputContext.Mice.FirstOrDefault();
        if (mouse is not null)
        {
            Application.RegisterComponent(new DesktopMouseProvider(mouse));
        }

        var keyboard = inputContext.Keyboards.FirstOrDefault();
        if (keyboard is not null)
        {
            Application.RegisterComponent(new DesktopKeyboardProvider(keyboard));
        }

        var gamepad = inputContext.Gamepads.FirstOrDefault();
        if (gamepad is not null)
        {
            Application.RegisterComponent(new DesktopGamepadProvider(gamepad));
        }


    }

    public IEnumerable<IDisplay> GetDisplays()
    {
        return DesktopDisplay.GetDisplayList();
    }

    public static void Register()
    {
        SimulationHost.RegisterPlatform(() => new DesktopPlatform());
    }
}