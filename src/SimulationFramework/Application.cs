using SimulationFramework.Components;
using SimulationFramework.Messaging;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SimulationFramework;

/// <summary>
/// Provides mechanisms for acquiring components to the simulation.
/// </summary>
public static class Application
{
    private static ISimulationPlatform Platform => GetComponent<ISimulationPlatform>();
    private static IApplicationProvider Provider => GetComponent<IApplicationProvider>();

    /// <summary>
    /// Gets an <see cref="IDisplay"/> instance which represents the system's primary display.
    /// </summary>
    public static IDisplay PrimaryDisplay => Provider.PrimaryDisplay;

    /// <summary>
    /// Invoked when the simulation is trying to exit. The exit can be cancelled using <see cref="CancelExit"/>.
    /// </summary>
    public static event MessageListener<ExitMessage> Exiting
    {
        add => SimulationHost.Current!.Dispatcher.Subscribe(value);
        remove => SimulationHost.Current!.Dispatcher.Unsubscribe(value);
    }

    /// <summary>
    /// Gets the first component of the a specific type. If no components of the specified type are found, this method throws an exception.
    /// </summary>
    /// <typeparam name="TComponent">The type of component to get.</typeparam>
    /// <returns>A component of type <typeparamref name="TComponent"/>.</returns>
    public static TComponent GetComponent<TComponent>() where TComponent : class, ISimulationComponent
    {
        return GetComponentOrDefault<TComponent>() ?? throw Exceptions.ComponentNotFound(typeof(TComponent));
    }

    /// <summary>
    /// Gets the first component of the a specific type. If no components of the specified type are found, this method returns null.
    /// </summary>
    /// <typeparam name="TComponent">The type of component to get.</typeparam>
    /// <returns>A component of type <typeparamref name="TComponent"/>; or null if one wasn't found.</returns>
    public static TComponent? GetComponentOrDefault<TComponent>() where TComponent : class, ISimulationComponent
    {
        return SimulationHost.Current?.GetComponent<TComponent>();
    }

    /// <summary>
    /// Adds a new component to the simulation.
    /// </summary>
    /// <typeparam name="TComponent">The type of component to add. Must be a class and have a parameterless constructor.</typeparam>
    public static void RegisterComponent<TComponent>() where TComponent : class, ISimulationComponent, new()
    {
        RegisterComponent(new TComponent());
    }


    /// <summary>
    /// Adds a new component to the simulation.
    /// </summary>
    /// <typeparam name="TComponent">The type of component to add. Must be a class.</typeparam>
    /// <param name="component">The component to add.</param>
    public static void RegisterComponent<TComponent>(TComponent component) where TComponent : class, ISimulationComponent
    {
        SimulationHost.Current?.RegisterComponent(component);
    }

    /// <summary>
    /// Removes the component of the provided type from the simulation and replaces it with the one returned by <paramref name="interceptProvider"/>.
    /// </summary>
    /// <typeparam name="TComponent">The type of component to intercept.</typeparam>
    /// <param name="interceptProvider">A delegate which accepts the component being intercepted and returns the new component to be added in it's place.</param>
    public static void InterceptComponent<TComponent>(Func<TComponent, TComponent> interceptProvider) where TComponent : class, ISimulationComponent
    {
        var component = GetComponent<TComponent>();
        SimulationHost.Current?.RemoveComponent(component);
        var newComponent = interceptProvider(component);
        RegisterComponent(newComponent);
    }

    /// <summary>
    /// Determines if the true if the simulation has a component of the specified type.
    /// </summary>
    /// <typeparam name="TComponent">The type of component.</typeparam>
    /// <returns><see langword="true"/> if the simulation has a component of type <typeparamref name="TComponent"/>; otherwise <see langword="false"/>.</returns>
    public static bool HasComponent<TComponent>() where TComponent : class, ISimulationComponent
    {
        return GetComponent<TComponent>() is not null;
    }

    /// <summary>
    /// Gets all of the system's currently active displays.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{IDisplay}"/> containing the system's displays.</returns>
    public static IEnumerable<IDisplay> GetDisplays()
    {
        return Provider.GetDisplays();
    }

    /// <summary>
    /// Makes request to exit the simulation.
    /// <para>
    /// Calling this method invokes the <see cref="Exiting"/> event and dispatches an <see cref="ExitMessage"/>, during which <see cref="CancelExit"/> can be called to cancel the exit request. </para>
    /// </summary>
    /// <param name="cancellable">Whether the exit request can be cancelled using <see cref="CancelExit"/>.</param>
    public static void Exit(bool cancellable)
    {
        Provider.Exit(cancellable);
    }

    /// <summary>
    /// Cancels a pending request to exit the simulation. Must be called from a listener of either the <see cref="Exiting"/> event or an <see cref="ExitMessage"/>.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    public static void CancelExit()
    {
        Provider.CancelExit();
    }
}