using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;

public interface IGraphicsQueue : IDisposable
{
    // event Action Completed;
    // void WaitForCompleted(IGraphicsQueue other);
    // bool IsRecording { get; }

    void Flush();

    /// <summary>
    /// Signals the provided <see cref="WaitHandle"/> when all currently queued commands have finished.
    /// </summary>
    void Wait(WaitHandle waitHandle);
}