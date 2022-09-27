using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Messaging;

/// <summary>
/// Dispatches application messages.
/// </summary>
public sealed class MessageDispatcher
{
    private readonly List<IMessageListener> events = new();

    public void Dispatch<T>(T message) where T : Message
    {
        if (Application.Current.GetComponent<ITimeProvider>() is not null)
        {
            message.DispatchTime = Time.TotalTime;
        }

        var toDispatch = events.Where(e => e.IsListeningFor(typeof(T)));

        foreach (var e in events.ToArray())
        {
            e.Dispatch(message);
        }
    }

    public void Subscribe<T>(Action<T> listener, MessagePriority priority = MessagePriority.Normal) where T : Message
    {
        if (events.SingleOrDefault(e => e is Event<T>) is not Event<T> ev)
        {
            ev = new Event<T>();
            events.Add(ev);
        }

        ev.AddListener(listener, priority);
    }

    public void Unsubscribe<T>(Action<T> listener) where T : Message
    {
        if (events.SingleOrDefault(e => e is Event<T>) is Event<T> ev)
        {
            ev.RemoveListener(listener);
        }
    }

    private interface IMessageListener
    {
        public bool IsListeningFor(Type type);
        void Dispatch(Message message);
    }

    private class Event<T> : IMessageListener where T : Message
    {
        private readonly List<EventListener> eventListeners = new();

        public bool IsListeningFor(Type type)
        {
            return type == typeof(T) || type.IsSubclassOf(typeof(T));
        }

        public void AddListener(Action<T> action, MessagePriority priority)
        {
            if (eventListeners.Contains(new(action, priority)))
            {
                throw new Exception();
            }

            eventListeners.Add(new(action, priority));
            eventListeners.Sort((a, b) => Comparer<MessagePriority>.Default.Compare(a.Priority, b.Priority));
        }

        public void RemoveListener(Action<T> action)
        {
            var listener = eventListeners.FirstOrDefault(a => a.Action == action);

            if (listener is not null)
            {
                eventListeners.Remove(listener);
            }
        }

        public void Dispatch(Message message)
        {
            if (!IsListeningFor(message.GetType()))
                return;

            foreach (var listener in eventListeners)
            {
                listener.Action((T)message);
            }
        }

        private record EventListener(Action<T> Action, MessagePriority Priority);
    }
}