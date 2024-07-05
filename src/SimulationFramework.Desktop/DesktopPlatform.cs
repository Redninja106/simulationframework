﻿using Silk.NET.Windowing;
using SimulationFramework.Messaging;
using SimulationFramework.Components;
using Silk.NET.Windowing.Glfw;
using Silk.NET.Input;
using Silk.NET.Input.Glfw;
using Silk.NET.OpenGL;
using SimulationFramework.Desktop.Audio;
using SimulationFramework.OpenGL;

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
                Flags = ContextFlags.Debug
            },
            Samples = 8,
        };

        GlfwWindowing.RegisterPlatform();
        GlfwInput.RegisterPlatform();

        GlfwWindowing.Use();

        Window = Silk.NET.Windowing.Window.Create(windowOptions.Value with { IsVisible = false });
        Window.Initialize();

        inputContext = Window.CreateInput();
    }

    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
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
        if (graphics != null)
        {
            Application.RegisterComponent(CreateImGuiProvider());
        }
    }


    public static void Register()
    {
        SimulationHost.RegisterPlatform(() => new DesktopPlatform());
    }

    protected virtual IGraphicsProvider CreateGraphicsProvider()
    {
        return new GLGraphicsProvider(frame, name => Window.GLContext!.TryGetProcAddress(name, out nint addr) ? addr : 0);
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

    protected virtual ISimulationComponent CreateImGuiProvider()
    {
        return new DesktopImGuiComponent(this.Window, this.inputContext);
    }

    protected virtual void RegisterInputProviders()
    {
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

        Application.RegisterComponent(new DesktopGamepadProvider(inputContext));
    }
}