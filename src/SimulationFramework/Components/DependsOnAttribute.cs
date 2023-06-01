using System;

namespace SimulationFramework.Components;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class DependsOnAttribute<T> : Attribute where T : class, ISimulationComponent
{
}
