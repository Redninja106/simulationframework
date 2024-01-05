using System.Diagnostics;
using SimulationFramework.Components;

namespace SimulationFramework.Messaging;

/// <summary>
/// Dispatches application messages.
/// </summary>
public sealed class MessageDispatcher
{
    private readonly List<IMessageListener> events = new();
    private readonly Queue<Message> messageQueue = new();

    /// <summary>
    /// Dispatches all queued messages. This method is called by the application at the beginning and end of each frame.
    /// </summary>
    public void Flush()
    {
        // process entire queue
        while (messageQueue.TryDequeue(out Message? nextMessage))
        {
            // skip null entries
            if (nextMessage is null)
                continue;

            // dispatch message
            DispatchCore(nextMessage);
        }
    }

    /// <summary>
    /// Adds an event to the event queue. The event is dispatched upon the next call to <see cref="Flush"/> (usually done by the application).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="message"></param>
    public void QueueDispatch<T>(T message) where T : Message
    {
        ArgumentNullException.ThrowIfNull(message);

        messageQueue.Enqueue(message);
    }

    /// <summary>
    /// Immediately dispatches a message, raising notifications and calling listeners.
    /// </summary>
    /// <typeparam name="T">The type of message to dispatch.</typeparam>
    /// <param name="message">The message data.</param>
    public void ImmediateDispatch<T>(T message) where T : Message
    {
        ArgumentNullException.ThrowIfNull(message);
        
        // call non-generic version
        DispatchCore(message);
    }
    
    private void DispatchCore(Message message)
    {
        // mark message with timestamp if provider is available
        if (Application.GetComponentOrDefault<ITimeProvider>() is not null)
        {
            message.DispatchTime = Time.TotalTime;
        }

        // get all listeners listening for message of T
        var messageType = message.GetType();
        var toDispatch = events.Where(e => e.IsListeningFor(messageType));

        // dispatch event to listeners (ToArray() in case of subscription modification during events)
        foreach (var e in toDispatch.ToArray())
        {
            e.Dispatch(message);
        }
    }

    /// <summary>
    /// Subscribes a delegate to listen for messages of a specific type.
    /// </summary>
    /// <typeparam name="T">The type of message to listen for.</typeparam>
    /// <param name="listener">The listener delegate.</param>
    public void Subscribe<T>(MessageListener<T> listener) where T : Message
    {
        ArgumentNullException.ThrowIfNull(listener);

        // TODO BUG:
        // In cases where a listener is subscribed to a message type when it is already subscribed to
        // that message's parent type, that listener would be invoked twice upon dispatch of subclass type.

        // find event if we already have one
        Event<T>? ev = GetOrCreateEvent<T>();

        // subscribe the listener to the event
        ev.AddListener(listener);
    }

    /// <summary>
    /// Unsubscibes a delegate from listening for messages.
    /// </summary>
    /// <typeparam name="T">The type of messages the delegate was listening for.</typeparam>
    /// <param name="listener">The listener to unsubscribe.</param>
    public void Unsubscribe<T>(MessageListener<T> listener) where T : Message
    {
        ArgumentNullException.ThrowIfNull(listener);
        
        // find event to remove
        Event<T>? ev = GetOrCreateEvent<T>();

        // if we've got one remove it
        if (ev is not null)
        {
            ev.RemoveListener(listener);
        }
    }

    /// <summary>
    /// Invokes the provided delegate once before the next dispatch of the specified message type.
    /// </summary>
    /// <typeparam name="T">The type of message to be notified of.</typeparam>
    /// <param name="listener">The notification delegate.</param>
    public void NotifyBefore<T>(MessageListener<T> listener) where T : Message
    {
        ArgumentNullException.ThrowIfNull(listener);

        // forward notification request to event type
        Event<T> ev = GetOrCreateEvent<T>();
        ev.NotifyBefore(listener);
    }

    /// <summary>
    /// Invokes the provided delegate once after the next dispatch of the specified message type.
    /// </summary>
    /// <typeparam name="T">The type of message to be notified of.</typeparam>
    /// <param name="listener">The notification delegate.</param>
    public void NotifyAfter<T>(MessageListener<T> listener) where T : Message
    {
        ArgumentNullException.ThrowIfNull(listener);

        // forward notification request to event type
        Event<T> ev = GetOrCreateEvent<T>();
        ev.NotifyAfter(listener);
    }

    // gets this dispatchers Event<T> or null
    private Event<T> GetOrCreateEvent<T>() where T : Message
    {
        // find event if we already have one
        Event<T>? ev = events.OfType<Event<T>>().SingleOrDefault();

        // if we don't, create one
        if (ev is null)
        {
            ev = new Event<T>();
            events.Add(ev);
        }

        return ev;
    }

    // exposes non generic methods of Event<T> such that they can be accessed from a collection of varying generic args
    private interface IMessageListener
    {
        // does the listener want to be notified for mesasges of this type?
        public bool IsListeningFor(Type type);

        // dispatches a message
        void Dispatch(Message message);
    }

    // a group of listeners for a specific message type.
    private class Event<T> : IMessageListener where T : Message
    {
        // one-time listeners for before dispatch
        private readonly Queue<MessageListener<T>> beforeNotifications = new();

        // one-time listeners for after dispatch
        private readonly Queue<MessageListener<T>> afterNotifications = new();

        // EventListener list, should always be kept sorted by priority
        private readonly List<MessageListener<T>> eventListeners = new();

        // does the listener want to be notified for mesasges of this type?
        public bool IsListeningFor(Type type)
        {
            return type == typeof(T) || type.IsSubclassOf(typeof(T));
        }

        // adds an action to the before dispatch notification queue
        public void NotifyBefore(MessageListener<T> action)
        {
            AddNotify(action, beforeNotifications);
        }

        // adds an action to the after dispatch notification queue
        public void NotifyAfter(MessageListener<T> action)
        {
            AddNotify(action, afterNotifications);
        }

        // adds an action to a notification queue, checking for duplicates
        private static void AddNotify(MessageListener<T> action, Queue<MessageListener<T>> queue)
        {
            if (queue.Any(n => n == action))
            {
                throw Exceptions.DuplicateMessageListener();
            }

            queue.Enqueue(action);
        }

        // adds a listener to this event with the provided priority
        public void AddListener(MessageListener<T> action)
        {
            if (eventListeners.Any(listener => listener == action))
            {
                throw Exceptions.DuplicateMessageListener();
            }

            // add event listener and sort by priority using default comparer
            eventListeners.Add(new(action));
        }

        // removes a listener by its MethodInfo
        public void RemoveListener(MessageListener<T> action)
        {
            // find our target method it in listeners
            var listener = eventListeners.FirstOrDefault(a => a == action);

            // if we have a match, remove it
            if (listener is not null)
            {
                eventListeners.Remove(listener);
            }
        }

        // dispatches the provided message 
        public void Dispatch(Message message)
        {
            // make sure we're listening for this type of message
            Debug.Assert(IsListeningFor(message.GetType()));
            
            T msg = (T)message;

            // notify before-dispatch listeners
            SendNotifications(msg, beforeNotifications);

            // dispatch to each listener (ToArray() in case of subscription modification during events)
            foreach (var listener in eventListeners.ToArray())
            {
                listener(msg);
            }

            // notify after-dispatch listeners
            SendNotifications(msg, afterNotifications);
        }

        // clears a notification queue
        private void SendNotifications(T message, Queue<MessageListener<T>> queue)
        {
            // while the queue has elements
            while (queue.TryDequeue(out MessageListener<T>? notification))
            {
                // skip null values
                if (notification is null)
                    continue;

                // dispatch notification
                notification(message);
            }
        }
    }
}