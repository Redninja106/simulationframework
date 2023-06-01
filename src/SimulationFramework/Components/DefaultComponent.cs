using System;

namespace SimulationFramework.Components;

internal class DefaultComponentAttribute<T> : Attribute where T : ISimulationComponent
{
}