using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;
public abstract class SoundPlayback : IDisposable
{
    public abstract bool IsPaused { get; }
    public abstract bool IsStopped { get; }

    public bool IsPlaying => !IsPaused && !IsStopped;

    public abstract void Pause();
    public abstract void Resume();
    public abstract void Stop();

    public SoundPlayback()
    {

    }

    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    ~SoundPlayback()
    {
        Dispose();
    }
}