using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Canvas;
using SimulationFramework.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Base class for simulations.
/// </summary>
public sealed class Application : IDisposable
{
    public static Application Current { get; private set; }

    public MessageDispatcher Dispatcher { get; private set; }

    private readonly List<IAppComponent> components = new();
    
    private readonly IAppPlatform platform;

    private bool initialized = false;

    /// <summary>
    /// Creates a new instance of the <see cref="Application"/> class.
    /// </summary>
    public Application(IAppPlatform platform)
    {
        this.platform = platform ?? throw new ArgumentNullException(nameof(platform));
        components.Add(this.platform);

        Dispatcher = new();

        Dispatcher.Subscribe<InitializeMessage>(m =>
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Initialize(this);
            }
            
            this.initialized = true;
        }, MessagePriority.High);
    }

    public T GetComponent<T>() where T : IAppComponent
    {
        var c = components.Where(c => c is T);

        return (T)c.SingleOrDefault();
    }

    public void AddComponent<T>(T component) where T : IAppComponent
    {
        if (components.Any(c => c is T))
            throw new Exception("A component of type " + typeof(T).Name + " already exists");

        components.Add(component);
        
        if (initialized)
            component.Initialize(this);
    }
    
    public void Dispose()
    {
        foreach (var component in components)
        {
            try
            {
                component.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.GetType().Name} while disposing component {component.GetType().Name}: {e.Message}");
            }
        }
        
        GC.SuppressFinalize(this);
        
        if (Current == this)
            Current = null;
    }

    //public void Start()
    //{
    //    var appConfig = new AppConfig(this);

    //    appConfig.Width

    //    this.OnInitialize(appConfig);

    //    appConfig.Apply();

    //    this.Initialized?.Invoke();

    //    (int, int) prevSize = environment.GetOutputSize();

    //    while (!environment.ShouldExit() && !this.exitRequested)
    //    {
    //        InputContext.NewFrame();

    //        environment.ProcessEvents();

    //        if (prevSize != environment.GetOutputSize())
    //        {
    //            this.Resized?.Invoke(environment.GetOutputSize().Item1, environment.GetOutputSize().Item2);
    //            prevSize = environment.GetOutputSize();
    //        }

    //        this.BeforeRender?.Invoke();

    //        using var canvas = Graphics.GetFrameCanvas();

    //        canvas.ResetState();

    //        Render?.Invoke();

    //        using (canvas.PushState())
    //        {
    //            this.OnRender(canvas);
    //        }

    //        canvas.Flush();

    //        this.AfterRender?.Invoke();

    //        environment.EndFrame();
    //    }

    //    Console.WriteLine("OnUninitialize called");

    //    this.OnUnitialize();

    //    this.Uninitialized?.Invoke();
    //}

    /// <summary>
    /// Notifies the simulation to exit. If this is called during rendering, the simulation exits once the frame has finished.
    /// </summary>
    public void Exit()
    {
    }

    /// <summary>
    /// Runs the app using the provided platform.
    /// </summary>
    public void Start()
    {
        Current = this;
        
        var controller = platform.CreateController();
        AddComponent(controller);

        AddComponent(new InputContext());

        controller.Start(this.Dispatcher);

        Current = null;
    }
}