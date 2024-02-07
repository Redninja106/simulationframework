using SimulationFramework.Components;
using SimulationFramework.Drawing;
using SimulationFramework.Input;
using SimulationFramework.Messaging;
using System;
using System.Runtime.CompilerServices;

namespace SimulationFramework;

/// <summary>e
/// The base class for all simulations. Inherit this class or use <see cref="Create(Action, Action{ICanvas})"/> to create a simulation.
/// </summary>
public abstract class Simulation
{
    /// <summary>
    /// Called when the simulation should initialize.
    /// </summary>
    public abstract void OnInitialize();

    /// <summary>
    /// Called when the simulation should render.
    /// </summary>
    /// <param name="canvas"></param>
    public abstract void OnRender(ICanvas canvas);

    /// <summary>
    /// Called when the simulation should uninitialize.
    /// </summary>
    public virtual void OnUninitialize() { }

    /// <summary>
    /// Called when the simulation's video output is resized.
    /// </summary>
    /// <param name="width">The new width of the simulation's video output.</param>
    /// <param name="height">The new height of the simulation's video output.</param>
    public virtual void OnResize(int width, int height) { }

    /// <summary>
    /// Called when a key is pressed on the keyboard.
    /// </summary>
    /// <param name="key">The key that was pressed.</param>
    public virtual void OnKeyPressed(Key key) { }

    /// <summary>
    /// Called when a key is released on the keyboard.
    /// </summary>
    /// <param name="key">The key that was released.</param>
    public virtual void OnKeyReleased(Key key) { }

    /// <summary>
    /// Called when a key is typed. This is usually used for text input.
    /// </summary>
    /// <param name="character">The character that was typed.</param>
    public virtual void OnKeyTyped(char character) { }

    /// <summary>
    /// Called when a button is pressed on the mouse.
    /// </summary>
    /// <param name="button">The button that was pressed.</param>
    public virtual void OnButtonPressed(MouseButton button) { }

    /// <summary>
    /// Called when a button is released on the mouse.
    /// </summary>
    /// <param name="button">The button that was released.</param>
    public virtual void OnButtonReleased(MouseButton button) { }

    /// <summary>
    /// Runs the simulation, automatically determining a platform.
    /// </summary>
    public void Run()
    {
        Run(null);
    }

    /// <summary>
    /// Runs the simulation using the provided platform.
    /// </summary>
    /// <param name="platform">The platform to run the simulation on. If this value is <see langword="null"/>, the platform is automatically determined.</param>
    public void Run(ISimulationPlatform? platform)
    {
        SimulationHost host = new();
        host.Initialize(platform);
        host.Start(this);
    }

    /// <summary>
    /// Runs a simulation of the given type, automatically determining a platform. This method initializes a <see cref="SimulationHost"/> before creating the simulation so that the constructor of <typeparamref name="TSimulation"/> can use SimulationFramework components (like <see cref="Time"/> and <see cref="Graphics"/>).
    /// </summary>
    /// <typeparam name="TSimulation">The type of simulation to run.</typeparam>
    public static void Start<TSimulation>() where TSimulation : Simulation, new()
    {
        Start<TSimulation>(null);
    }

    /// <summary>
    /// Runs a simulation of the given type using the provided platform. This method initializes a <see cref="SimulationHost"/> before creating the simulation so that the constructor of <typeparamref name="TSimulation"/> can use SimulationFramework components (like <see cref="Time"/> and <see cref="Graphics"/>).
    /// </summary>
    /// <typeparam name="TSimulation">The type of simulation to run.</typeparam>
    /// <param name="platform">The platform to run the simulation on. If this value is <see langword="null"/>, the platform is automatically determined.</param>
    public static void Start<TSimulation>(ISimulationPlatform? platform) where TSimulation : Simulation, new()
    {
        SimulationHost host = new();
        host.Initialize(platform);
        host.Start(new TSimulation());
    }

    /// <summary>
    /// Creates a <see cref="Simulation"/> from callbacks.
    /// </summary>
    /// <param name="initialize">Called once when the simulation should initialize.</param>
    /// <param name="render">Called every frame when the simulation should render.</param>
    /// <returns>A new <see cref="Simulation"/> which calls the given delegates.</returns>
    public static Simulation Create(Action? initialize, Action<ICanvas>? render)
    {
        return new ActionSimulation(initialize, render);
    }

    /// <summary>
    /// Creates a simulaton from callbacks, and then runs it, automatically determining the platform.
    /// </summary>
    /// <param name="initialize">Called once when the simulation should initialize.</param>
    /// <param name="render">Called every frame when the simulation should render.</param>
    public static void CreateAndRun(Action? initialize, Action<ICanvas>? render)
    {
        CreateAndRun(initialize, render, null);
    }

    /// <summary>
    /// Creates a simulaton from callbacks, and then runs it using the provided platform
    /// </summary>
    /// <param name="initialize">Called once when the simulation should initialize.</param>
    /// <param name="render">Called every frame when the simulation should render.</param>
    /// <param name="platform">The platform to run the simulation on. If this value is <see langword="null"/>, the platform is automatically determined.</param>
    public static void CreateAndRun(Action? initialize, Action<ICanvas>? render, ISimulationPlatform? platform)
    {
        Create(initialize, render).Run(platform);
    }

    /// <summary>
    /// Configures the simulation to render to an fixed-size off-screen texture that will automatically be rendered to fit the window.
    /// <para>
    /// This method can be called any time to change the size of a fixed-size simulation, but the changed won't occur until the next frame.
    /// </para>
    /// </summary>
    /// <param name="width">The width of the off-screen texture.</param>
    /// <param name="height">The height of the off-screen texture.</param>
    /// <param name="backgroundColor">The color to fill the window area not covered by the texture.</param>
    /// <param name="transparent">Whether the framebuffer should be rendered to the window with transparency.</param>
    /// <param name="subpixelInput">Whether resolution dependent input values (such as <see cref="Mouse.Position"/>) should report values more precise than one pixel (when possible).</param>
    /// <param name="stretchToFit">Whether the off-screen texture should be stretched to fit the window.</param>
    public static void SetFixedResolution(int width, int height, Color backgroundColor, bool transparent = false, bool subpixelInput = false, bool stretchToFit = false)
    {
        if (SimulationHost.Current!.IsRendering)
        {
            SimulationHost.GetCurrent().Dispatcher.NotifyAfter<AfterRenderMessage>(m =>
            {
                SetFixedResolutionCore(width, height, backgroundColor, transparent, subpixelInput, stretchToFit);
            });
        }
        else
        {
            SetFixedResolutionCore(width, height, backgroundColor, transparent, subpixelInput, stretchToFit);
        }

        static void SetFixedResolutionCore(int width, int height, Color backgroundColor, bool transparent, bool subpixelInput, bool stretchToFit)
        {
            var interceptor = Application.GetComponentOrDefault<FixedResolutionInterceptor>();

            if (interceptor is not null && (width is 0 || height is 0))
            {
                SimulationHost.Current?.RemoveComponent(interceptor);
                return;
            }

            if (interceptor is null)
            {
                Application.RegisterComponent(new FixedResolutionInterceptor(width, height, backgroundColor, transparent, subpixelInput, stretchToFit));
                return;
            }

            interceptor.Resize(width, height);
            interceptor.BackgroundColor = backgroundColor;
            interceptor.Transparent = transparent;
            interceptor.SubpixelInput = subpixelInput;
            interceptor.StretchToFit = stretchToFit;
        }
    }

    private class ActionSimulation : Simulation
    {
        public readonly Action? initialize;
        public readonly Action<ICanvas>? render;

        public ActionSimulation(Action? initialize, Action<ICanvas>? render)
        {
            this.initialize = initialize;
            this.render = render;
        }

        public override void OnInitialize()
        {
            this.initialize?.Invoke();
        }

        public override void OnRender(ICanvas canvas)
        {
            this.render?.Invoke(canvas);
        }
    }
}