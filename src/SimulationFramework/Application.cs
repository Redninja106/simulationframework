using SimulationFramework.Components;
using System.Collections.Generic;

namespace SimulationFramework;

public static class Application
{
    public static TComponent GetComponent<TComponent>() where TComponent : class, ISimulationComponent
    {
        return GetComponentOrDefault<TComponent>() ?? throw Exceptions.ComponentNotFound(typeof(TComponent));
    }

    public static TComponent? GetComponentOrDefault<TComponent>() where TComponent : class, ISimulationComponent
    {
        return SimulationHost.Current?.GetComponent<TComponent>();
    }

    public static void RegisterComponent<TComponent>() where TComponent : class, ISimulationComponent, new()
    {
        RegisterComponent(new TComponent());
    }

    public static void RegisterComponent<TComponent>(TComponent component) where TComponent : class, ISimulationComponent
    {
        SimulationHost.Current?.RegisterComponent(component);
    }

    public static bool HasComponent<TComponent>() where TComponent : class, ISimulationComponent
    {
        return GetComponent<TComponent>() is not null;
    }

    public static IEnumerable<IDisplay> GetDisplays()
    {
        return GetComponent<ISimulationPlatform>().GetDisplays();
    }
}