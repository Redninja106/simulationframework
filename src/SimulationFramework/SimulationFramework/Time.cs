using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

public static class Time
{
    internal static ITimeProvider Provider => Simulation.Current.GetComponent<ITimeProvider>();
    public static float DeltaTime => Provider.GetDeltaTime();
    public static float TotalTime => Provider.GetTotalTime();

    /// <summary>
    /// <see langword="true"/> if duration of the previous frame exceeded <see cref="MaxDeltaTime"/>.
    /// </summary>
    public static bool IsRunningSlowly => Provider.IsRunningSlowly();
    public static float MaxDeltaTime { get => Provider.MaxDeltaTime; set => Provider.MaxDeltaTime = value; }
    public static float TimeScale { get => Provider.TimeScale; set => Provider.TimeScale = value; }

}