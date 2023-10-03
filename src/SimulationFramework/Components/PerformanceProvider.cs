using SimulationFramework.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Components;
internal class PerformanceProvider : ISimulationComponent
{
    private static Queue<float> framerates = new();

    private int framerateAverageCount = Performance.DefaultFramerateAverageCount;

    public int FramerateAverageCount {
        get => framerateAverageCount;
        set
        {
            if (value < 1)
                throw new ArgumentException(null, nameof(value));

            framerateAverageCount = value;
        }
    }

    public float Framerate { get; private set; }
    public float RawFramerate => 1f / Application.GetComponent<ITimeProvider>().GetRawDeltaTime();

    public void Initialize(MessageDispatcher dispatcher)
    {
        dispatcher.Subscribe<BeforeRenderMessage>(m => {
            framerates.Enqueue(RawFramerate);
            while (framerates.Count > FramerateAverageCount)
            {
                framerates.Dequeue();
            }
            Framerate = framerates.Average();
        });
    }

    public void Dispose()
    {
    }
}
