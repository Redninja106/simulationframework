<h1 align="center">
<img src="https://raw.githubusercontent.com/Redninja106/simulationframework/master/assets/logo-128x128.png"/>
<br>SimulationFramework</br>
</h1>

<div align="center">

![asteroids](https://user-images.githubusercontent.com/45476006/187408130-aaf81b10-f940-4eea-88da-e610c8db31af.gif) ![physics](https://user-images.githubusercontent.com/45476006/187408134-6199d6f9-32cc-434f-8331-7000373f9bad.gif)

</div>

SimulationFramework is a cross-platform library for creative coding, game development, and graphical apps built on .NET 6. Designed with simplicity and ease of use in mind, it cuts down on development time for quick and easy prototyping. 

[Join the discord server!](https://discord.gg/V4X2vTvV2G)

### SimulationFramework is:

- **Simple**: SimulationFramework is designed to be developer-friendly. It is well documented and provides flexible, intuitive, and easy-to-use APIs. 

- **Portable**: The core package is dependency free, meaning SimulationFramework can run anywhere .NET can.

- **Documented**: [The wiki](https://github.com/Redninja106/simulationframework/wiki) contains everything you need to know about SimulationFramework, it's features, and how to use them.


### Features:
- **Windowing**: automatically creates a configurable window

- **2D Drawing**: A powerful 2D drawing API, backed by [SkiaSharp](https://github.com/mono/SkiaSharp).

- **Input**: Mouse, keyboard and controller support.

- **Built-in [Dear ImGui](https://github.com/ocornut/imgui) Support**: Dear ImGui is completely built-in with zero setup.

There are more on the way! See [Planned Features](https://github.com/Redninja106/simulationframework#planned-features).

> Note: Right now SimulationFramework is still in the very early stages of development and is changing with every new version.

## Getting Started

> SimulationFramework requires .NET 6 and Visual Studio 2022.

Create a new Console App using .NET 6. Add the [SimulationFramework Nuget Package](https://www.nuget.org/packages/SimulationFramework/) (and it's desktop environment) using Visual Studio or the .NET CLI via the following commands:

```
dotnet add package SimulationFramework
dotnet add package SimulationFramework.Desktop
```

Next, create two methods `OnInitialize` and `OnRender` and pass them to `Simulation.Create`. To start the simulation, call `Simulation.Run()`:
```cs
// Program.cs
using SimulationFramework;
using SimulationFramework.Drawing;

Simulation mySimulation = Simulation.Create(OnInitialize, OnRender);
mySimulation.Run();

void OnInitialize(AppConfig config)
{
    // called when the simulation starts
}

void OnRender(ICanvas canvas)
{
    // called every frame
}
```

Running the program will result in a blank window:

<div align="center">
    
![blank-simulation](https://user-images.githubusercontent.com/45476006/187409330-160e4f8f-db41-4cb3-91f2-8957ef2b4c66.png)
    
</div>

Next, to start drawing. The `ICanvas` provided in `OnRender()` contains a variety methods for drawing.

```cs
// Program.cs
using SimulationFramework;
using SimulationFramework.Drawing;

Simulation mySimulation = Simulation.Create(OnInitialize, OnRender);
mySimulation.Run();

void OnInitialize(AppConfig config)
{
}

void OnRender(ICanvas canvas)
{
    // don't forget to clear the screen each frame!
    canvas.Clear(Color.CornflowerBlue); 
    
    // draw a 50 pixel wide red square centered at the mouse position
    canvas.Fill(Color.Red);
    canvas.DrawRect(Mouse.Position, new Vector2(50, 50), Alignment.Center); 
}
```
Here is what that should look like: 
<div align="center">
    
![readme-example](https://user-images.githubusercontent.com/45476006/187409007-ec8abaea-3c59-456e-9106-d1c1860b0b45.gif)
  
</div>

To see more, [go to the wiki](https://github.com/Redninja106/simulationframework/wiki) or [join the discord server](https://discord.gg/V4X2vTvV2G).

## Planned Features
- **C# shaders**: .NET CIL to HLSL/GLSL compilation to write any kind of shader in plain C# (or any other .NET language!).
- **Dependency Free**: SimulationFramework won't depend on any other nuget packages or have any native dependencies (except imgui, which will be optional).
- **3D Drawing**: A performant and cross-platform 3D graphics API. (see the '3d' branch)
- **WebAssembly and Mobile Support**: Any simulations you write will run on a web browser or mobile device, no code changes needed.
