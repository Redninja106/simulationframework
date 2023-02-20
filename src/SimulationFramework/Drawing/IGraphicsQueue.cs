using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;

public interface IGraphicsQueue : IDisposable
{
    // event Action Completed;
    // void WaitForCompleted(IGraphicsQueue other);
    void Flush();
}