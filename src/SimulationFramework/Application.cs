using SimulationFramework.Drawing;
using SimulationFramework.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Hosts all of the components in a simulation.
/// </summary>
public sealed class Application : IDisposable
{
    /// <summary>
    /// The currently running application, or null if there is none.
    /// </summary>
    [AllowNull]
    public static Application Current { get; private set; }

    /// <summary>
    /// The application's dispatcher.
    /// </summary>
    public MessageDispatcher Dispatcher { get; private set; }

    private readonly List<IAppComponent> components = new();
    private readonly IAppPlatform platform;
    private bool initialized;

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
        }, MessagePriority.Internal);
    }

    /// <summary>
    /// Finds the component of the provided type.
    /// </summary>
    /// <typeparam name="T">The type of component to find.</typeparam>
    /// <returns>The component if one of the provided type was found, else null.</returns>
    public T? GetComponent<T>() where T : IAppComponent
    {
        return components.OfType<T>().FirstOrDefault();
    }

    /// <summary>
    /// Adds the provided component to the application.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    public void AddComponent<T>(T component) where T : IAppComponent
    {
        if (components.Any(c => c is T))
            throw new Exception("A component of type " + typeof(T).Name + " already exists");

        components.Add(component);
        
        if (initialized)
            component.Initialize(this);
    }
    
    /// <inheritdoc/>
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