using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Base class for simulations.
/// </summary>
public abstract class Simulation : IDisposable
{
    /// <summary>
    /// Gets the presently running simulation.
    /// </summary>
    public static Simulation Current { get; private set; }

    private readonly List<ISimulationComponent> components = new();
    private ISimulationEnvironment environment;
    private bool exitRequested;

    /// <summary>
    /// This simulation's current angle mode.
    /// </summary>
    public AngleMode AngleMode { get; internal set; }

    public event Action Initialized;
    public event Action BeforeRender;
    public event Action Render;
    public event Action AfterRender;
    public event Action Uninitialized;

    public InputContext InputContext { get; } = new();

    /// <summary>
    /// Raised when the window is resized.
    /// </summary>
    public event Action<int, int> Resized;

    /// <summary>
    /// Called when the simulation should initialize.
    /// </summary>
    /// <param name="config"></param>
    public abstract void OnInitialize(AppConfig config);
    
    /// <summary>
    /// Called when the simulation should render.
    /// </summary>
    /// <param name="canvas"></param>
    public abstract void OnRender(ICanvas canvas);
    
    /// <summary>
    /// Called when the simulation should uninitialize.
    /// </summary>
    public virtual void OnUnitialize() { }

    /// <summary>
    /// Called when the simulation's video output is resized.
    /// </summary>
    /// <param name="width">The new width of the simulation's video output.</param>
    /// <param name="height">The new height of the simulation's video output.</param>
    public virtual void OnResize(int width, int height) { }

    /// <summary>
    /// Creates a new instance of the <see cref="Simulation"/> class.
    /// </summary>
    public Simulation()
    {
    }

    public T GetComponent<T>() where T : ISimulationComponent
    {
        return (T)components.SingleOrDefault(c => c is T) ?? throw new Exception("A component of the specified type was not found!");
    }

    /// <summary>
    /// Destroys the simulation.
    /// </summary>
    public void Dispose()
    {
        try
        {
            environment.Dispose();
        }
        finally
        {
            GC.SuppressFinalize(this);
            if (Current == this)
                Current = null;
        }
    }
    
    /// <summary>
    /// Sets the simulation to use the provided enviroment. This may only be called once on each simulation.
    /// </summary>
    /// <param name="environment"></param>
    /// <remarks>
    /// To use a SimulationFramework-included environment, call the method for that environment on the <see cref="AppConfig"/> instance provided in <see cref="OnInitialize(AppConfig)"/>.
    /// </remarks>
    private void SetEnvironment(ISimulationEnvironment environment)
    {
        if (this.environment is not null)
            throw new InvalidOperationException("This simulation already has an environment!");

        this.environment = environment;

        var supportedComponents = this.environment.CreateSupportedComponents();

        // check for common issues and emit warnings
        if (supportedComponents.Count() < 0)
            DebugConsole.Warn("The provided environment has no components! (Most APIs won't work!"); 
        if (!supportedComponents.Any(c => c is ITimeProvider))
            DebugConsole.Warn("No provided environment has no time component! (The time API won't work!)");
        if (!supportedComponents.Any(c => c is IGraphicsProvider))
            DebugConsole.Warn("No provided environment has no graphics component! (The graphics API won't work!)");

        // apply all the components to this simulation
        foreach (var component in supportedComponents)
        {
            DebugConsole.Log($"Applied Component of type '{component.GetType().Name}'");
            component.Apply(this);
            this.components.Add(component);
        }
    }

    // starts a simulation. This is only to be called from `Run()`.
    private void Start()
    {
        DebugConsole.Log("OnInitialize called!");

        var appConfig = new AppConfig(this);
        appConfig.SetAngleMode(AngleMode.Degrees);
        this.OnInitialize(appConfig);

        this.Initialized?.Invoke();

        DebugConsole.Log("Beginning Render loop");

        (int, int) prevSize = environment.GetOutputSize();

        while (!environment.ShouldExit() && !this.exitRequested)
        {
            PerformanceViewer.BeginTaskGroup("frame");

            InputContext.NewFrame();

            environment.ProcessEvents();
            PerformanceViewer.MarkTaskCompleted("ProcessEvents()");

            if (prevSize != environment.GetOutputSize())
            {
                this.Resized?.Invoke(environment.GetOutputSize().Item1, environment.GetOutputSize().Item2);
                prevSize = environment.GetOutputSize();
                PerformanceViewer.MarkTaskCompleted("resizing");
            }

            this.BeforeRender?.Invoke();
            PerformanceViewer.MarkTaskCompleted("BeforeRender()");

            using var canvas = Graphics.GetFrameCanvas();

            canvas.ResetState();

            Render?.Invoke();

            using (canvas.Push())
            {
                this.OnRender(canvas);
                PerformanceViewer.MarkTaskCompleted("OnRender()");
            }

            canvas.Flush();
            PerformanceViewer.MarkTaskCompleted("Canvas.Flush()");

            DebugWindow.Layout();

            this.AfterRender?.Invoke();
            PerformanceViewer.MarkTaskCompleted("AfterRender()");

            environment.EndFrame();
            PerformanceViewer.MarkTaskCompleted("EndFrame()");

            PerformanceViewer.EndTaskGroup();
        }

        DebugConsole.Log("OnUninitialize called");

        this.OnUnitialize();

        this.Uninitialized?.Invoke();
    }

    /// <summary>
    /// Runs the simulation in the specified environment.
    /// </summary>
    /// <param name="environment">The environment in which to run the simulation</param>
    public void Run(ISimulationEnvironment environment)
    {
        if (environment is null)
            throw new Exception("Simulation must select an enviroment");

        this.SetEnvironment(environment);
        Current = this;
        this.Start();
    }

    /// <summary>
    /// Converts an angle from this simulation's current mode to the specified mode.
    /// </summary>
    /// <param name="angle">The which to convert from the current mode.</param>
    /// <param name="targetMode">The mode to convert the angle to.</param>
    /// <returns>An angle equivalent to <paramref name="angle"/>, in the units specified by <paramref name="targetMode"/>.</returns>
    public static float ConvertFromCurrentAngleMode(float angle, AngleMode targetMode)
    {
        if (targetMode == Current.AngleMode)
            return angle;

        float degrees = Current.AngleMode switch
        {
            AngleMode.Radians => angle * 180 / MathF.PI,
            AngleMode.Revolutions => angle * 360,
            AngleMode.Gradians => angle * (365 / 400),
            _ => angle
        };

        return targetMode switch
        {
            AngleMode.Radians => degrees / 180 * MathF.PI,
            AngleMode.Revolutions => degrees / 360f,
            AngleMode.Gradians => degrees * (400 * 365),
            _ => degrees,
        };
    }

    public static float ConvertToCurrentAngleMode(float angle, AngleMode sourceMode)
    {
        if (sourceMode == Current.AngleMode)
            return angle;

        float degrees = sourceMode switch
        {
            AngleMode.Radians => angle * 180 / MathF.PI,
            AngleMode.Revolutions => angle * 360,
            AngleMode.Gradians => angle * (365 / 400),
            _ => angle
        };

        return Current.AngleMode switch
        {
            AngleMode.Radians => degrees / 180 * MathF.PI,
            AngleMode.Revolutions => degrees / 360f,
            AngleMode.Gradians => degrees * (400 * 365),
            _ => degrees,
        };
    }

    /// <summary>
    /// Notifies the simulation to exit. If this is called during rendering, the simulation exits once the frame has finished.
    /// </summary>
    public void Exit()
    {
        this.exitRequested = true;
    }
}