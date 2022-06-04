<h1 align="center">
<img src="https://raw.githubusercontent.com/Redninja106/simulationframework/master/assets/logo-128x128.png"/>
<br>SimulationFramework</br>
</h1>

SimulationFramework is a cross-platform library for creative coding, game development, and graphical apps built on .NET 6. Designed with simplicity and ease of use in mind, it cuts down on development time for quick and easy rapid prototyping. 

⚠️ Currently, SimulationFramework is still in the early stages of development. ⚠️

## Features

- **Simple and Usable**: SimulationFramework is designed to be developer-friendly. It is well documented and provides flexible, intuitive, and easy-to-use APIs. 

- **2D Drawing**: A powerful and performant 2D drawing API, with backends in [SkiaSharp](https://github.com/mono/SkiaSharp) and soon Direct2D.

- **Portable**: Working entirely through interfaces, SimulationFramework can be made to run anywhere .NET can.

- **Input**: A simple input API that doesn't get in the way.

- **[Dear ImGui](https://github.com/ocornut/imgui) Support**: Dear ImGui is completely built-in with zero setup.

- **Abstract**: all features are provided through interfaces to make SimulationFramework completely dependency free and by extension completely cross-platform.

## Getting Started

To start, create a new Console App using .NET 6. Add the [SimulationFramework Nuget Package](https://www.nuget.org/packages/SimulationFramework/) using Visual Studio or the following command:
```
dotnet add package SimulationFramework
dotnet add package SimulationFramework.Desktop
```

Next, create a class which inherits from `Simulation`. `Simulation` has virtual methods that can be overridden to add functionality to your simulation.
```cs
using SimualtionFramework;

class MySimulation : Simulation
{
    public override OnInitalize(AppConfig config)
    {
        
    }

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
public override void OnRender(ICanvas canvas)
{
    // don't forget to clear the screen each frame!
    canvas.Clear(Color.CornflowerBlue); 
    // draw a 50 pixel wide red square at the mouse position
    canvas.DrawRect(Mouse.Position, (50, 50), Color.Red, Alignment.Center); 
}
```
To see more, go to [the wiki](https://github.com/Redninja106/simulationframework/wiki).
