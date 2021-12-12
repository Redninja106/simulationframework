using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Extensions;

public interface IExtensionProvider<T>
{
    T CreateService(ISimulationEnvironment environment);
}
