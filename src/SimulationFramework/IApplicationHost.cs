using SimulationFramework.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;
public interface IApplicationHost
{
    void Start(Application application);

    IApplicationComponent CreateComponent<T>();
}