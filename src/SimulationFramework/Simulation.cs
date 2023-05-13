using SimulationFramework.Components;
using SimulationFramework.Drawing;
using SimulationFramework.Input;
using SimulationFramework.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// The base class for all simulations. Inherit this class or use <see cref="Create(Action{SimulationFramework.AppConfig}, Action{ICanvas})"/> to create a simulation.
/// </summary>
public abstract class Simulation
{
    /// <summary>
    /// Called when the simulation should initialize.
    /// </summary>
    public abstract void OnInitialize();

    /// <summary>
    /// Called when the simulation should render.
    /// </summary>
    /// <param name="canvas"></param>
    public abstract void OnRender(ICanvas canvas);

    /// <summary>
    /// Called when the simulation should uninitialize.
    /// </summary>
    public virtual void OnUninitialize() { }

    /// <summary>
    /// Called when the simulation's video output is resized.
    /// </summary>
    /// <param name="width">The new width of the simulation's video output.</param>
    /// <param name="height">The new height of the simulation's video output.</param>
    public virtual void OnResize(int width, int height) { }

    /// <summary>
    /// Called when a key is pressed on the keyboard.
    /// </summary>
    /// <param name="key">The key that was pressed.</param>
    public virtual void OnKeyPressed(Key key) { }

    /// <summary>
    /// Called when a key is released on the keyboard.
    /// </summary>
    /// <param name="key">The key that was released.</param>
    public virtual void OnKeyReleased(Key key) { }

    /// <summary>
    /// Called when a button is pressed on the mouse.
    /// </summary>
    /// <param name="button">The button that was pressed.</param>
    public virtual void OnButtonPressed(MouseButton button) { }

    /// <summary>
    /// Called when a button is released on the mouse.
    /// </summary>
    /// <param name="button">The button that was released.</param>
    public virtual void OnButtonReleased(MouseButton button) { }

    public void Run()
    {
        Run(null);
    }

    public void Run(ISimulationPlatform? platform)
    {
        SimulationHost host = new();
        host.Initialize(platform);
        host.Start(this);
    }

    public static void Start<T>() where T : Simulation, new()
    {
        Start<T>(null);
    }

    public static void Start<T>(ISimulationPlatform? platform) where T : Simulation, new()
    {
        SimulationHost host = new();
        host.Initialize(platform);
        host.Start(new T());
    }

    public static Simulation Create(Action? initialize, Action<ICanvas>? render)
    {
        return new ActionSimulation(initialize, render);
    }

    public static void CreateAndRun(Action? initialize, Action<ICanvas>? render)
    {
        CreateAndRun(initialize, render, null);
    }

    public static void CreateAndRun(Action? initialize, Action<ICanvas>? render, ISimulationPlatform? platform)
    {
        Create(initialize, render).Run(platform);
    }

    private class ActionSimulation : Simulation
    {
        public readonly Action? initialize;
        public readonly Action<ICanvas>? render;

        public ActionSimulation(Action? initialize, Action<ICanvas>? render)
        {
            this.initialize = initialize;
            this.render = render;
        }

        public override void OnInitialize()
        {
            this.initialize?.Invoke();
        }

        public override void OnRender(ICanvas canvas)
        {
            this.render?.Invoke(canvas);
        }
    }
}