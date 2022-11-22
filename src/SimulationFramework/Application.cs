using SimulationFramework.Drawing;
using SimulationFramework.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
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
    public static Application? Current { get; private set; }

    /// <summary>
    /// The application's dispatcher.
    /// </summary>
    public MessageDispatcher Dispatcher { get; private set; }

    private readonly List<IApplicationComponent> components = new();
    private readonly IApplicationPlatform platform;
    private bool initialized;

    /// <summary>
    /// Creates a new instance of the <see cref="Application"/> class.
    /// </summary>
    public Application(IApplicationPlatform platform)
    {
        ArgumentNullException.ThrowIfNull(platform);
        
        this.platform = platform;
        components.Add(platform);

        Dispatcher = new();

        Dispatcher.Subscribe<InitializeMessage>(m =>
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Initialize(this);
            }

            this.initialized = true;
        }, ListenerPriority.Internal);
    }

    /// <summary>
    /// Finds the component of the provided type.
    /// </summary>
    /// <typeparam name="T">The type of component to find.</typeparam>
    /// <returns>The component if one of the provided type was found, else null.</returns>
    public T? GetComponent<T>() where T : IApplicationComponent
    {
        return components.OfType<T>().FirstOrDefault();
    }

    /// <summary>
    /// Adds the provided component to the application.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    public void AddComponent<T>(T component) where T : IApplicationComponent
    {
        AddComponent(component as IApplicationComponent);
    }

    /// <summary>
    /// Adds the provided component to the application.
    /// </summary>
    /// <param name="component"></param>
    public void AddComponent(IApplicationComponent component)
    {
        var componentType = component.GetType();

        if (components.Any(c => c.GetType() == componentType))
            throw Exceptions.DuplicateComponent(componentType);

        Debug.Message($"Added component \"{componentType}\".");

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
    /// Notifies the simulation to exit after the current frame.
    /// </summary>
    public void Exit()
    {
        Dispatcher.NotifyAfter<Messaging.FrameEndMessage>(msg => Dispatcher.QueueDispatch(new ExitMessage()));
    }

    /// <summary>
    /// Runs the app using the provided platform.
    /// </summary>
    public void Start()
    {
        Current = this;

        AddComponent(platform.CreateController());
        AddComponent(platform.CreateGraphicsProvider());
        AddComponent(platform.CreateTimeProvider());

        foreach (var additionalComponent in platform.CreateAdditionalComponents())
        {
            AddComponent(additionalComponent);
        }

        AddComponent(new InputContext());

        var controller = GetComponent<IApplicationController>()!;
        controller.Start(this.Dispatcher);

        Current = null;
    }

    /// <summary>
    /// Redirects all internal console messages to the provided text writer.
    /// </summary>
    /// <param name="writer">The writer to redirect to, or null to redirect to the console.</param>
    public static void RedirectConsoleOutput(TextWriter? writer)
    {
        if (writer == Console.Out)
        {
            writer = null;
        }

        Debug.Redirect(writer);
    }
}