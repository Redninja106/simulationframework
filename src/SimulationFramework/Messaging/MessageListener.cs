namespace SimulationFramework.Messaging;

/// <summary>
/// Represents a message listener.
/// </summary>
public delegate void MessageListener<in T>(T message) where T : Message;