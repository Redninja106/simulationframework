*This page documents the what, why, and how of SimulationFramework.*

# Project Goals

The goal of SimulationFramework is to provide a ___simple___, __robust__ and __extensible__ platform for 
small desktop animations/games. 

# Platform Structure

The vast majority of SimulationFramework's API rests within the [`ISimulationEnviroment`]() interface.
`ISimulationEnviroment` provides access to various _services_. A service is a interface inheriting from [`IService`]()
which provides some form of functionality to the application. One example of this would be the [ITimeService]() interface

# A Basic Simulation

To write a simulation, inherit from [`Simulation`](). [`Simulation`]() provides callbacks to the application through 
abstract methods.

```cs
class BasicSimulation : Simulation
{
    public override void OnInitialize(AppConfig config) 
    {
    }

    public override void OnRender(ICanvas canvas)
    {
        // draw a square
        canvas.DrawRect(100, 100, 100, 100, Color.Red);
    }

    public override void OnUninitialize() 
    {
    }
}
```

And a `Main` method which passes an instance of that class to [`Simulation.Run(Simulation simulation)`]():

```cs
static void Main() 
{
    using BasicSimulation simulation = new BasicSimulation();
    Simulation.Run(simulation);
}
```

# Features

## Dear ImGui

SimulationFramework provides a API wrapper for [Dear ImGui]() (internally using [ImGui.NET]()) which can be 
accessed via the ImGui service ([`IImGuiService`]()).

## Simulation Pane

A simulation can be rendered to a smaller subset of the window, allowing the window to be resized freely 
without affecting the simulation. To enable the simulation pane, call [`AppConfig.EnableSimulationPane()`]() 
from the [`Simulation.OnInitialize(AppConfig)`]() method.

## Extensions

SimulationFramework provides an API for feature extensions. Extensions usually provide it's features through 
a service, which can be retrieved using [`ISimulationEnviroment.GetService<T>()`]().

# Building an Extension

An extension for SimulationFramework, at the very minimum: 
- Exposes at least one service, which is an interface which inherits from [`IService`]();
- Contains at least one concrete implementation of the serivce;
- And exposes an [`IExtensionProvider<T>`]() for the service interface, which is marked with an
[`ExtensionProviderAttribute`]().

A naive example of such would be similar to:

```cs
// for the sake of the example, this extension merely provides a single method which adds two numbers.

// IAdder.cs
...
public interface IAdder
{
    int Add(int a, int b)
}

// Adder.cs
...
internal class Adder : IAdder
{
    int Add(int a, int b)
    {
        return a + b;
    }
}
 
// AdderProvider.cs
...
public class AdderFactory : IExtensionProvider<IAdder>
{
    public IAdder Get()
    {
        return new Adder();
    }
}