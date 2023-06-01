using SimulationFramework.Components;
using System.Collections.Generic;

namespace SimulationFramework;

/// <summary>
/// Provides mechanisms for acquiring components to the simulation.
/// </summary>
public static class Application
{
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
        return GetComponent<ISimulationPlatform>().GetDisplays();
    }
}