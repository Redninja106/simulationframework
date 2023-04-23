using SimulationFramework.Components;
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
public static class Application
{
    public static T GetComponent<T>() where T : class, ISimulationComponent
    {
        return GetComponentOrDefault<T>() ?? throw Exceptions.ComponentNotFound(typeof(T));
    }

    public static T? GetComponentOrDefault<T>() where T : class, ISimulationComponent
    {
        return SimulationHost.Current?.GetComponent<T>();
    }

    public static void RegisterComponent<T>() where T : class, ISimulationComponent, new()
    {
        RegisterComponent(new T());
    }

    public static void RegisterComponent<T>(T component) where T : class, ISimulationComponent
    {
        SimulationHost.Current?.RegisterComponent(component);
    }

    public static IEnumerable<IDisplay> GetDisplays()
    {
        return GetComponent<ISimulationPlatform>().GetDisplays();
    }
}