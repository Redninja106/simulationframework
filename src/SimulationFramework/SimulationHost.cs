using SimulationFramework.Components;
using SimulationFramework.Drawing;
using SimulationFramework.Input;
using SimulationFramework.Messaging;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace SimulationFramework;

/// <summary>
/// Runs a simulation and manages its components.
/// </summary>
public class SimulationHost
{
    /// <summary>
    /// The currently running simulation host.
    /// </summary>
    public static SimulationHost? Current { get; private set; }
    private static readonly List<Func<ISimulationPlatform?>> platformFactories = new();

    private Simulation? simulation;
    private bool initialized = false;
    private readonly List<ISimulationComponent> components = new();

    /// <summary>
    /// The simulation's dispatcher.
    /// </summary>
    public MessageDispatcher Dispatcher { get; set; }

    /// <summary>
    /// <see langword="true"/> if the simulation currently rendering, preventing some operations like window resizing; otherwise <see langword="false"/>.
    /// </summary>
    public bool IsRendering { get; private set; }

    /// <summary>
    /// Creates a new <see cref="SimulationHost"/> instance.
    /// </summary>
    public SimulationHost()
    {
        Dispatcher = new();
    }

    static SimulationHost()
    {
        // we try to register some known platforms so that the user doesn't have to call Register()

        (string assembly, string name)[] knownPlatforms = new[]
        {
            ("SimulationFramework.Desktop", "DesktopPlatform"),
        };


        foreach (var (platformAssembly, platformName) in knownPlatforms)
        {
            try
            {
                // try to load and call the Register() method

                Assembly assembly = Assembly.Load(platformAssembly);
                Type? type = assembly.GetType($"{platformAssembly}.{platformName}");
                MethodInfo? register = type?.GetMethod("Register", BindingFlags.Static | BindingFlags.Public);
                register?.Invoke(null, Array.Empty<Type>());
            }
            catch
            {
                // do nothing
            }
        }
    }

    /// <summary>
    /// Returns <see cref="Current"/> if it is not null; throw an exception otherwise.
    /// </summary>
    public static SimulationHost GetCurrent()
    {
        return Current ?? throw Exceptions.NoActiveHost();
    }

    /// <summary>
    /// </summary>
    /// <param name="factory"></param>
    public static void RegisterPlatform(Func<ISimulationPlatform?> factory)
    {
        platformFactories.Add(factory);
    }

    private static bool TryCreatePlatform([NotNullWhen(true)] out ISimulationPlatform? platform)
    {
        platform = null;

        // we take the first one that doesn't throw or return null.
        foreach (var factory in platformFactories)
        {
            try
            {
                platform = factory();
            }
            catch 
            {
                continue;
            }

            if (platform is not null)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Initializes this <see cref="SimulationHost"/> using the provided platform.
    /// </summary>
    /// <param name="platform"></param>
    public void Initialize(ISimulationPlatform? platform)
    {
        if (initialized)
        {
            throw Exceptions.HostAlreadyInitialized();
        }

        if (Current is not null)
        {
            throw Exceptions.SimulationRunning();
        }

        if (platform is null && !TryCreatePlatform(out platform))
        {
            throw Exceptions.NoPlatform();
        }

        initialized = true;
        Current = this;

        RegisterComponent(new PerformanceProvider());
        RegisterComponent(platform);

        if (simulation is not null && Application.HasComponent<IMouseProvider>())
        {
            Mouse.ButtonDown += simulation.OnButtonPressed;
            Mouse.ButtonUp += simulation.OnButtonReleased;
        }

        if (simulation is not null && Application.HasComponent<IKeyboardProvider>())
        {
            Keyboard.KeyPressed += simulation.OnKeyPressed;
            Keyboard.KeyReleased += simulation.OnKeyReleased;
            Keyboard.KeyTyped += simulation.OnKeyTyped;
        }
    }

    /// <summary>
    /// Starts a new instance of <typeparamref name="TSimulation"/> and makes this host current.
    /// <para>
    /// If this host is not initialized, then this method initializes it.
    /// </para>
    /// <para>
    /// This method throws if a <see cref="SimulationHost"/> is already running (<see cref="Current"/> is non-null).
    /// </para>
    /// </summary>
    /// <typeparam name="TSimulation">The type of simulation to start. Must have a parameterless constructor.</typeparam>
    public void Start<TSimulation>() where TSimulation : Simulation, new()
    {
        if (!initialized)
            Initialize(null);

        Start(new TSimulation());
    }

    /// <summary>
    /// Starts a simulation and makes this host current.
    /// <para>
    /// If this host is not initialized, then this method initializes it.
    /// </para>
    /// <para>
    /// This method throws if a <see cref="SimulationHost"/> is already running (<see cref="Current"/> is non-null).
    /// </para>
    /// </summary>
    /// <param name="simulation">The simulation to start. If this value is <see langword="null"/>, the host starts without a simulation.</param>
    public void Start(Simulation? simulation)
    {
        if (!initialized)
            Initialize(null);
        
        this.simulation = simulation;
        simulation?.OnInitialize();

        var appController = Application.GetComponent<ISimulationController>();
        
        Dispatcher.ImmediateDispatch<InitializeMessage>(new());
        appController.Start(RunFrame);
        Dispatcher.ImmediateDispatch<UninitializeMessage>(new());
    }

    private void RunFrame()
    {
        Dispatcher.ImmediateDispatch<BeforeRenderMessage>(new());
        IsRendering = true;

        if (Application.HasComponent<IGraphicsProvider>())
        {
            var outputCanvas = Graphics.GetOutputCanvas();
            outputCanvas.ResetState();

            var interceptor = GetComponent<FixedResolutionInterceptor>();
            var canvas = interceptor?.FrameBuffer?.GetCanvas() ?? outputCanvas;
            canvas.ResetState();
            interceptor?.BeforeRender();
            Dispatcher.ImmediateDispatch<RenderMessage>(new(canvas));
            simulation?.OnRender(canvas);
            canvas.Flush();
            interceptor?.AfterRender();
        }
        else 
        {
            simulation?.OnRender(null!);
        }
        IsRendering = false;

        Dispatcher.ImmediateDispatch<AfterRenderMessage>(new());
    }

    /// <summary>
    /// Register a component with the simulation.
    /// </summary>
    /// <param name="component">The component to register.</param>
    public void RegisterComponent(ISimulationComponent component)
    {
        component.Initialize(this.Dispatcher);
        components.Add(component);
    }

    /// <summary>
    /// Gets a component of the provided type.
    /// </summary>
    /// <typeparam name="T">The type of component to find.</typeparam>
    /// <returns>The component, of type <typeparamref name="T"/>, or null if it was not found.</returns>
    public T? GetComponent<T>() where T : class, ISimulationComponent
    {
        return components.OfType<T>().FirstOrDefault();
    }

    /// <summary>
    /// Removes a component from the simulation.
    /// </summary>
    /// <param name="component">The component to remove.</param>
    public void RemoveComponent(ISimulationComponent component)
    {
        components.Remove(component);
    }
}