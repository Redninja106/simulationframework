using SimulationFramework.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

public interface IApplication
{
    /// <summary>
    /// Gets the application's dispatcher
    /// </summary>
    /// <returns></returns>
    MessageDispatcher GetDispatcher();
    T? GetComponent<T>();
    void AddComponent<T>(T? component);
}
