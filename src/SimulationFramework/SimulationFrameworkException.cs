using System;

namespace SimulationFramework;

internal class SimulationFrameworkException : Exception
{
    public SimulationFrameworkException() : base()
    {
    }

    public SimulationFrameworkException(string? message) : base(message)
    {
    }

    public SimulationFrameworkException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}