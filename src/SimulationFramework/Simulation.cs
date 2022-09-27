using SimulationFramework.Drawing;
using SimulationFramework.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

public abstract class Simulation
{
    public Application Application { get; private set; }

    /// <summary>
    /// Called when the simulation should initialize.
    /// </summary>
    /// <param name="config"></param>
    public abstract void OnInitialize(AppConfig config);

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
    /// Starts this simulation using the provided platform.
    /// </summary>
    /// <param name="platform"></param>
    public void Run(IAppPlatform platform)
    {
        Application = new Application(platform);

        Application.Dispatcher.Subscribe<InitializeMessage>(m => {
            var config = AppConfig.CreateDefault();
            config.Title = "Simulation";
            OnInitialize(config);
            config.Apply();
        });

        Application.Dispatcher.Subscribe<RenderMessage>(m =>
        {
            m.Canvas.ResetState();
            OnRender(m.Canvas);
            m.Canvas.Flush();
        });

        Application.Dispatcher.Subscribe<UninitializeMessage>(m =>
        {
            OnUninitialize();
        });

        Application.Dispatcher.Subscribe<ResizeMessage>(m =>
        {
            OnResize(m.Width, m.Height);
        });
        
        Application.Start();

        Application.Dispose();
    }

    /// <summary>
    /// Creates a simulation from the provided delegates.
    /// </summary>
    /// <param name="initialize">The delegate to call when simulation initializes.</param>
    /// <param name="render">The delegate to call when simulation renders.</param>
    /// <returns>A simulation which uses the provided delegates.</returns>
    public static Simulation Create(Action<AppConfig> initialize, Action<ICanvas> render)
    {
        return Create(initialize, render, null);
    }

    /// <summary>
    /// Creates a simulation from the provided delegates.
    /// </summary>
    /// <param name="initialize">The delegate to call when simulation initializes.</param>
    /// <param name="render">The delegate to call when simulation renders.</param>
    /// <param name="uninitialize">The delegate to call when simulation uninitializes.</param>
    /// <returns>A simulation which uses the provided delegates.</returns>
    public static Simulation Create(Action<AppConfig> initialize, Action<ICanvas> render, Action? uninitialize)
    {
        return new ActionSimulation(initialize, render, uninitialize);
    }

    private class ActionSimulation : Simulation
    {
        private readonly Action<AppConfig> initialize;
        private readonly Action<ICanvas> render;
        private readonly Action? uninitialize;

        public ActionSimulation(Action<AppConfig> initialize, Action<ICanvas> render, Action? uninitialize)
        {
            this.initialize= initialize;
            this.render = render;
            this.uninitialize = uninitialize;
        }

        public override void OnInitialize(AppConfig config)
        {
            this.initialize(config);
        }

        public override void OnRender(ICanvas canvas)
        {
            this.render(canvas);
        }

        public override void OnUninitialize()
        {
            if (this.uninitialize is not null)
            {
                this.uninitialize();
            }
        }
    }
}