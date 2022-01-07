# SimulationFramework
SimulationFramework is a framework for graphical desktop apps using .NET. It is designed with simplicity and ease of use in mind in order to cut down on development time for rapid prototyping. Making heavy use of interfaces, all major features are abstracted to make SimulationFramework completely dependency free and by extension completely cross-platform.

SimulationFramework’s features include a flexible 2d drawing api, built-in Dear ImGui support (currently using ImGui.NET, custom bindings planned), and an easy-to-use input api.

Currently, the only available graphics backend is SkiaSharp, though there are plans for Direct2D. For windowing, SimulationFramework uses Slik.NET by default, although any windowing backend can be plugged in. In the future, the plan for SimulationFramework is to reduce dependencies as much as possible and implement more feature backends.

# Getting Started

To start, create a class which inherits from `Simulation`. `Simulation` has virtual methods that can be overridden to add functionality to your simulation.
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

To open the simulation, call `Simulation.RunWindowed()`:

```cs
public static void Main()
{
    using MySimulation sim = new MySimulation();
    Simulation.RunWindowed(sim, "Hello, World!", 1280, 720);
}
```

Next, to start drawing. The ICanvas provided in `OnRender` contains a variety methods for drawing.

```cs
public override void OnRender(ICanvas canvas)
{
    canvas.Clear(Color.CornflowerBlue); // dont forget to clear the screen each frame!
    canvas.DrawRect(100, 100, 50, 50, Color.Red, Alignment.Center); // draw a 50 pixel wide red square centered around (100, 100)
}
```
To see more, go to the wiki.
