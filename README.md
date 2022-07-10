<h1 align="center">
<img src="https://raw.githubusercontent.com/Redninja106/simulationframework/master/assets/logo-128x128.png"/>
<br>SimulationFramework</br>
</h1>

SimulationFramework is a cross-platform library for creative coding, game development, and graphical apps built on .NET 6. Designed with simplicity and ease of use in mind, it cuts down on development time for quick and easy rapid prototyping. 

[Join the discord server!](https://discord.gg/V4X2vTvV2G)

### SimulationFramework is:

- **Simple**: SimulationFramework is designed to be developer-friendly. It is well documented and provides flexible, intuitive, and easy-to-use APIs. 

- **Portable**: All platform dependent components are abstracted into interfaces, meaning SimulationFramework can be made to run anywhere .NET can.

- **Documented**: [The wiki](https://github.com/Redninja106/simulationframework/wiki) contains everything you need to know about SimulationFramework, it's features, and how to use them.


### Features:

- **2D Drawing**: A powerful 2D drawing API, backed by [SkiaSharp](https://github.com/mono/SkiaSharp).

- **Input**: A simple input API that doesn't get in the way.

- **Built-in [Dear ImGui](https://github.com/ocornut/imgui) Support**: Dear ImGui is completely built-in with zero setup.

- **Many more planned**: See [Planned Features](https://github.com/Redninja106/simulationframework#planned-features).

> Note: Right now SimulationFramework is still in the very early stages of development and therefore is still changing with every new version.


## Getting Started

To start, create a new Console App using .NET 6. Add the [SimulationFramework Nuget Package](https://www.nuget.org/packages/SimulationFramework/) (and it's desktop environment) using Visual Studio or the .NET CLI via the following commands:
```
dotnet add package SimulationFramework
dotnet add package SimulationFramework.Desktop
```

Next, create a class which inherits from `Simulation`. The abstract methods `OnInitialize` and `OnRender` allow you to add functionality to your simulation
```cs
using SimualtionFramework;

class MySimulation : Simulation
{
    // the OnInitialize method is when the simulation is started
    public override OnInitalize(AppConfig config)
    {
        
    }

    // the OnRender method is called each frame
    public override OnRender(ICanvas canvas)
    {
        
    }
}
```

To start the simulation, call `Simulation.RunWindowed()`:

```cs
using SimulationFramework.Desktop;

public static void Main()
{
    MySimulation sim = new MySimulation();
    Simulation.RunDesktop();
}
```

Next, to start drawing. The `ICanvas` provided in `OnRender()` contains a variety methods for drawing.

```cs
using SimulationFramework.Canvas;
using System.Numerics;

// the OnRender method is called each frame
public override void OnRender(ICanvas canvas)
{
    // don't forget to clear the screen each frame!
    canvas.Clear(Color.CornflowerBlue); 
    
    // draw a 50 pixel wide red square at the mouse position
    canvas.Fill(Color.Red);
    canvas.DrawRect(Mouse.Position, new Vector2(50, 50)); 
}
```

To see more, [go to the wiki](https://github.com/Redninja106/simulationframework/wiki) or [join the discord server](https://discord.gg/V4X2vTvV2G).

## Planned features
- **Dependency Free**: SimulationFramework won't depend on any other nuget packages or have any native dependencies.
- **3D Drawing**: A performant and cross-platform 3D graphics API backed by a custom C#-inspired shader language. (see the branch '3d')
