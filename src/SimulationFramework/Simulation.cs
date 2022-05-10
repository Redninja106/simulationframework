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

        var supportedComponents = this.environment.CreateSupportedComponents().ToArray();

        // check for common issues and emit warnings
        if (supportedComponents.Count() < 0)
            Console.WriteLine("The provided environment has no components! (Most APIs won't work!"); 
        if (!supportedComponents.Any(c => c is ITimeProvider))
            Console.WriteLine("No provided environment has no time component! (The time API won't work!)");
        if (!supportedComponents.Any(c => c is IGraphicsProvider))
            Console.WriteLine("No provided environment has no graphics component! (The graphics API won't work!)");

        // apply all the components to this simulation
        foreach (var component in supportedComponents)
        {
            Console.WriteLine($"Applied Component of type '{component.GetType().Name}'");
            component.Apply(this);
            this.components.Add(component);
        }
    }

    // starts a simulation. This is only to be called from `Run()`.
    private void Start()
    {
        Console.WriteLine("OnInitialize called!");

        var appConfig = new AppConfig(this);
        this.OnInitialize(appConfig);

        this.Initialized?.Invoke();

        Console.WriteLine("Beginning Render loop");

        (int, int) prevSize = environment.GetOutputSize();

        while (!environment.ShouldExit() && !this.exitRequested)
        {
            InputContext.NewFrame();

            environment.ProcessEvents();

            if (prevSize != environment.GetOutputSize())
            {
                this.Resized?.Invoke(environment.GetOutputSize().Item1, environment.GetOutputSize().Item2);
                prevSize = environment.GetOutputSize();
            }

            this.BeforeRender?.Invoke();

            using var canvas = Graphics.GetFrameCanvas();

            canvas.ResetState();

            Render?.Invoke();

            using (canvas.Push())
            {
                this.OnRender(canvas);
            }

            canvas.Flush();

            this.AfterRender?.Invoke();

            environment.EndFrame();
        }

        Console.WriteLine("OnUninitialize called");

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
    /// Notifies the simulation to exit. If this is called during rendering, the simulation exits once the frame has finished.
    /// </summary>
    public void Exit()
    {
        this.exitRequested = true;
    }
}