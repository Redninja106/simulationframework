using System;

namespace SimulationFramework.Components;

/// <summary>
/// Specifies that a component requires another component to be present.
/// </summary>
/// <typeparam name="T">The type of component.</typeparam>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class DependsOnAttribute<T> : Attribute where T : class, ISimulationComponent
{
}
