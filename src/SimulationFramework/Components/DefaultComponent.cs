using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Components;

internal class DefaultComponentAttribute<T> : Attribute where T : ISimulationComponent
{
}