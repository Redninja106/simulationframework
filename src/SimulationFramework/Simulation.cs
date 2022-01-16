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

    public int TargetWidth => environment.GetOutputSize().Item1;
    public int TargetHeight => environment.GetOutputSize().Item2;

    private readonly List<ISimulationComponent> components = new();
    private ISimulationEnvironment environment;

    public event Action Initialized;
    public event Action BeforeRender;
    public event Action AfterRender;
    public event Action Uninitialized;

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
    public abstract void OnUnitialize();

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

        this.OnInitialize(new AppConfig(this));

        this.Initialized?.Invoke();

        DebugConsole.Log("Beginning Render loop");

        (int, int) prevSize = environment.GetOutputSize();

        while (!environment.ShouldExit())
        {
            environment.ProcessEvents();

            if (prevSize != environment.GetOutputSize())
            {
                this.Resized?.Invoke(this.TargetWidth, this.TargetHeight);
                prevSize = environment.GetOutputSize();
            }

            this.BeforeRender?.Invoke();

            using var canvas = Graphics.GetFrameCanvas();

            canvas.ResetState();

            using (canvas.Push())
            {
                this.OnRender(canvas);
            }

            canvas.Flush();

            DebugWindow.Layout();

            this.AfterRender?.Invoke();

            environment.EndFrame();
        }

        DebugConsole.Log("OnUninitialize called");

        this.OnUnitialize();

        this.Uninitialized?.Invoke();
    }

    /// <summary>
    /// Runs the provided simulation in the specified environment.
    /// </summary>
    /// <param name="simulation">The simulation to start.</param>
    public static void Run(Simulation simulation, ISimulationEnvironment environment)
    {
        if (environment is null)
            throw new Exception("Simulation must select an enviroment");

        simulation.SetEnvironment(environment);
        Current = simulation;
        simulation.Start();
    }

    public static void RunWindowed(Simulation simulation, string title, int width, int height, bool resizable = true)
    {
        Run(simulation, new WindowEnvironment(title, width, height, resizable));
    }
}