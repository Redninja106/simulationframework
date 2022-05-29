using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Messaging;

public sealed class MessageDispatcher
{
    List<(Type Type, List<(object action, MessagePriority priority)> Listeners)> events;

    public MessageDispatcher()
    {
        events = new();
    }

    public void Dispatch<T>(T message) where T : Message
    {
        //message.DispatchTime = Time.TotalTime;

        var e = events.First(e=>e.Type == typeof(T));
        foreach (var listener in e.Listeners)
        {
            ((Action<T>)listener.action)(message);
        }    
    }
    
    public void Subscribe<T>(Action<T> listener, MessagePriority priority = MessagePriority.Normal) where T : Message
    {
        var listenerGroup = events.FirstOrDefault(e => e.Type == typeof(T));

        if (listenerGroup.Listeners is null)
        {
            listenerGroup = (typeof(T), new List<(object action, MessagePriority priority)>());
            events.Add(listenerGroup);
        }
        
        listenerGroup.Listeners.Add((listener, priority));
        listenerGroup.Listeners.Sort((a, b) => -a.priority.CompareTo(b.priority));
    }
}