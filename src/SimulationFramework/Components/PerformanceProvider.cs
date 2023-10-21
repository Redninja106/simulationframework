using SimulationFramework.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimulationFramework.Components;
internal class PerformanceProvider : ISimulationComponent
{
    private static Queue<float> framerates = new();

    private float framerateAverageDuration = Performance.DefaultFramerateAverageDuration;

    public float FramerateAverageDuration 
    {
        get
        {
            return framerateAverageDuration;
        }
        set
        {
            if (value < 0)
                throw new ArgumentException(null, nameof(value));

            framerateAverageDuration = value;
        }
    }

    public float Framerate { get; private set; }
    public float RawFramerate => 1f / Application.GetComponent<ITimeProvider>().GetRawDeltaTime();

    public void Initialize(MessageDispatcher dispatcher)
    {
        dispatcher.Subscribe<BeforeRenderMessage>(m => 
        {
            if (RawFramerate != float.PositiveInfinity)
            {
                framerates.Enqueue(RawFramerate);
            }

            while (framerates.Count > MathF.Max(1, RawFramerate * FramerateAverageDuration))
            {
                framerates.Dequeue();
            }

            if (framerates.Any())
            {
                Framerate = framerates.Average();
            }
        });
    }

    public void Dispose()
    {
    }
}
