﻿<h1 align="center">
<img src="https://raw.githubusercontent.com/Redninja106/simulationframework/master/assets/logo-128x128.png"/>
<br>SimulationFramework</br>
</h1>

SimulationFramework is a framework for creative coding, game development, and graphical apps built on .NET 6. It is designed with simplicity and ease of use in mind in order to cut down on development time for rapid prototyping. Making heavy use of interfaces, all major features are abstracted to make SimulationFramework completely dependency free and by extension completely cross-platform.

⚠️ Currently, SimulationFramework is in a very early state of development. ⚠️

## Features

- **Simple and Usable**: SimulationFramework is designed to be developer-friendly. It is well documented and provides flexible, intuitive, and easy-to-use APIs. 

- **2D Drawing**: A powerful and performant 2D drawing API, with backends in [SkiaSharp](https://github.com/mono/SkiaSharp) and soon Direct2D.

- **3D Drawing**: SimulationFramework provides a high level 3D rendering system using models and materials, as well a low level direct3d style api.

- **Portable**: Working entirely through interfaces, SimulationFramework can be made to run anywhere .NET can.

- **Input**: A simple input API that doesn't get in the way.

- **[Dear ImGui](https://github.com/ocornut/imgui) Support**: Dear ImGui is completely built-in with zero setup.

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
    canvas.DrawRect(Mouse.Position, new Vector2(50, 50), Color.Red, Alignment.Center); 
}
```
To see more, go to [the wiki](https://github.com/Redninja106/simulationframework/wiki).
