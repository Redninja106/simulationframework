using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Components;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class DependsOnAttribute<T> : Attribute where T : class, ISimulationComponent
{
}
