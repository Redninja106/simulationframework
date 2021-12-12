using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Implements core application logic
/// </summary>
public sealed class SimulationEngine : IDisposable
{
    public event Action<ICanvas> Drawing;
    public event Action Starting;
    public event Action Ending;

    private readonly ISimulationEnvironment environment;
    // private readonly IGraphicsFactory graphicsFactory;
    private readonly IGraphicsProvider graphicsContext;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="environment"></param>
    public SimulationEngine(ISimulationEnvironment environment)
    {
        this.environment = environment;
        this.graphicsContext = environment.CreateDefaultGraphicsFactory().CreateGraphics();
    }

    public IGraphicsProvider GetGraphicsContext()
    {
        return this.graphicsContext;
    }

    /// <summary>
    /// Runs the simulation on the current thread.
    /// </summary>
    public void Start()
    {
        Starting?.Invoke();

        while (!environment.ShouldExit())
        {
            environment.ProcessEvents();

            using var canvas = graphicsContext.GetFrameCanvas();

            using (canvas.Push())
            {
                Drawing?.Invoke(canvas);
            }

            canvas.Flush();

            environment.EndFrame();
        }

        Ending?.Invoke();
    }

    public void Dispose()
    {
        environment.Dispose();
    }
}