using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

internal class DefaultComponentAttribute<T> : Attribute where T : ISimulationComponent
{
}