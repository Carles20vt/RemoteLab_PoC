using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using TreeislandStudio.Engine.Event.Event;
using TreeislandStudio.Engine.Log;

namespace TreeislandStudio.Engine.Event
{
    /// <inheritdoc />
    /// <summary>
    /// Class to send events, implements the pub/sub pattern
    /// </summary>
    public class EventBroker : IEventBroker
    {
        #region Public fields

        /// <inheritdoc />
        /// <summary>
        /// Event broker name
        /// </summary>
        public string Name { [UsedImplicitly] get; } = "";

        #endregion
        
        #region Private fields

        /// <summary>
        /// List of subscriber lists, per event type
        /// </summary>
        private readonly Dictionary<string, SubscribersList> _subscribers = new Dictionary<string, SubscribersList>();
        
        /// <summary>
        /// List of peer event brokers to hom all the messages will be relayed
        /// </summary>
        private readonly List<IEventBroker> _relayTo = new List<IEventBroker>();
        
        /// <summary>
        /// Dictionary of messages types that peers are not interested in
        /// </summary>
        private readonly Dictionary<IEventBroker, List<string>> _doNotRelay = 
            new Dictionary<IEventBroker, List<string>>();

        #endregion

        #region Dependencies

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ICustomLogger _logger;

        #endregion
        
        #region Life cycle
        
        /// <summary>
        /// Constructor
        /// </summary>
        public EventBroker() : this (null) { }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">Logger to use</param>
        public EventBroker(ICustomLogger logger = null)
        {
            _logger = logger;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="logger">Logger to use</param>
        public EventBroker(string name, ICustomLogger logger = null) : this(logger)
        {
            Name = name;
        }

        /// <summary>
        /// Frees resources by removing all subscribers from the list
        /// </summary>
        public void Dispose()
        {
            _subscribers.Clear();
        }
        #endregion
        
        #region Public methods

        /// <inheritdoc />
        /// <summary>
        /// Subscribe a subscriber (by MessageType)
        /// </summary>
        /// <typeparam name="TMessage">Message type the subscriber is subscribing to</typeparam>
        /// <param name="subscriber">Subscriber</param>
        public void Subscribe<TMessage>(Action<TMessage> subscriber) where TMessage : EventMessage
        {
            _logger?.Debug("A new listener has been subscribed to " + typeof(TMessage));
            GetSubscribers<TMessage>().Add(subscriber);
        }

        /// <inheritdoc />
        /// <summary>
        /// Subscribe a subscriber (by MessageType)
        /// </summary>
        /// <typeparam name="TMessage">Message type the subscriber is subscribing to</typeparam>
        /// <param name="tags"></param>
        /// <param name="subscriber">Subscriber</param>
        public void Subscribe<TMessage>(IEnumerable<string> tags, Action<TMessage> subscriber) where TMessage : EventMessage
        {
            foreach (var subscribersByTag in GetSubscribers<TMessage>(tags)) {
                _logger?.Debug("A new listener has been subscribed to " + typeof(TMessage));
                subscribersByTag.Add(subscriber);
            }
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Subscribe a subscriber (by MessageType)
        /// </summary>
        /// <typeparam name="TMessage">Message type the subscriber is subscribing to</typeparam>
        /// <param name="tag"></param>
        /// <param name="subscriber">Subscriber</param>
        public void Subscribe<TMessage>(string tag, Action<TMessage> subscriber) where TMessage : EventMessage
        {
            Subscribe(new List<string>() { tag }, subscriber);
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Unsubscribe a subscriber (by MessageType)
        /// </summary>
        /// <typeparam name="TMessage">Message type the subscriber is subscribing to</typeparam>
        /// <param name="subscriber">Subscriber</param>
        public bool UnSubscribe<TMessage>(Action<TMessage> subscriber) where TMessage : EventMessage
        {
            _logger?.Debug("A listener is being unsubscribed from " + typeof(TMessage));
            return GetSubscribers<TMessage>().Remove(subscriber);
        }

        /// <inheritdoc />
        /// <summary>
        /// Unsubscribe a subscriber (by MessageType)
        /// </summary>
        /// <typeparam name="TMessage">Message type the subscriber is subscribing to</typeparam>
        /// <param name="tags"></param>
        /// <param name="subscriber">Subscriber</param>
        public bool UnSubscribe<TMessage>(IEnumerable<string> tags, Action<TMessage> subscriber) where TMessage : EventMessage
        {
            var iEnumerable = tags as string[] ?? tags.ToArray();
            
            _logger?.Debug(
                "A listener is being unsubscribed from " + typeof(TMessage) + " (" + string.Join(", ", iEnumerable.ToArray()) + ")"
            );

            return GetSubscribers<TMessage>(iEnumerable)
                .Aggregate(false, (current, subscribersByTag) => current | subscribersByTag.Remove(subscriber))
            ;
        }

        /// <inheritdoc />
        /// <summary>
        /// Unsubscribe a subscriber (by MessageType)
        /// </summary>
        /// <typeparam name="TMessage">Message type the subscriber is subscribing to</typeparam>
        /// <param name="tag"></param>
        /// <param name="subscriber">Subscriber</param>
        public bool UnSubscribe<TMessage>(string tag, Action<TMessage> subscriber) where TMessage : EventMessage
        {
            return UnSubscribe(new List<string>() {tag}, subscriber);
        }

        /// <inheritdoc />
        /// <summary>
        /// Publish a message (by MessageType)
        /// </summary>
        /// <typeparam name="TMessage">Message type</typeparam>
        /// <param name="message">Message</param>
        /// <param name="firstEventBroker"></param>
        public void Publish<TMessage>(TMessage message, IEventBroker firstEventBroker = null) where TMessage : EventMessage
        {
            if (firstEventBroker == null)
            {
                firstEventBroker = this;
            }
            
            _logger?.Debug("A " + typeof(TMessage) + " message has been sent: > " + message + " < ");
            GetSubscribers<TMessage>().Invoke(message);

            foreach (var eventBroker in _relayTo
                .Where(eventBroker => !ReferenceEquals(
                    eventBroker, firstEventBroker) && (!_doNotRelay.ContainsKey(eventBroker) || !_doNotRelay[eventBroker]
                    .Contains(typeof(TMessage).ToString()))))
            {
                _logger?.Debug("  Relaying message");
                eventBroker.Publish(message, firstEventBroker);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Publish a message (by MessageType)
        /// </summary>
        /// <typeparam name="TMessage">Message type</typeparam>
        /// <param name="tag"></param>
        /// <param name="message">Message</param>
        /// <param name="firstEventBroker"></param>
        public void Publish<TMessage>(
            string tag,
            TMessage message,
            IEventBroker firstEventBroker = null) where TMessage : EventMessage
        {
            
            if (firstEventBroker == null)
            {
                firstEventBroker = this;
            }
            
            _logger?.Debug("A " + typeof(TMessage) + " message has been sent: > " + message + " (" + tag + ") < ");
            GetSubscribers<TMessage>(tag).Invoke(message);

            foreach (var eventBroker in _relayTo
                .Where(eventBroker => !ReferenceEquals(eventBroker, firstEventBroker) && (!_doNotRelay.ContainsKey(eventBroker) || !_doNotRelay[eventBroker]
                    .Contains(getCompositeMessageType(typeof(TMessage).ToString(), tag)))))
            {
                _logger?.Debug("  Relaying message");
                eventBroker.Publish(tag, message, firstEventBroker);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Checks if a given subscriber is already subscribed
        /// </summary>
        /// <typeparam name="TMessage">Event type</typeparam>
        /// <param name="subscriber">Subscriber</param>
        /// <returns></returns>
        public bool IsSubscribed<TMessage>(Action<TMessage> subscriber) where TMessage : EventMessage
        {
            return GetSubscribers<TMessage>().Has(subscriber);
        }

        /// <inheritdoc />
        /// <summary>
        /// Checks if a given subscriber is already subscribed
        /// </summary>
        /// <typeparam name="TMessage">Event type</typeparam>
        /// <param name="tag"></param>
        /// <param name="subscriber">Subscriber</param>
        /// <returns></returns>
        public bool IsSubscribed<TMessage>(string tag, Action<TMessage> subscriber) where TMessage : EventMessage
        {
            return GetSubscribers<TMessage>(tag).Has(subscriber);
        }

        /// <inheritdoc />
        /// <summary>
        /// Remove the given subscriber by messageType.
        /// ALWAYS use Unsubscribe if MessageType is available, this method exists to avoid using reflexion (you know,
        /// for kids!) when the message type is unknown at compile time, the use cases for this method are very scarce,
        /// if you think you need this method, probably you probably are wrong and probably you are using it wrong. 
        /// </summary>
        /// <param name="messageTypeName"></param>
        /// <param name="subscriber">Subscriber to remove</param>
        public void UnsubscribeByMessageTypeName(string messageTypeName, object subscriber)
        {
            _logger?.Debug("A listener has been removed from " + subscriber.GetType());
            GetSubscribers (messageTypeName).Remove(subscriber);
        }

        /// <inheritdoc />
        /// <summary>
        /// Returns the number of subscribers (by MessageType)
        /// </summary>
        /// <typeparam name="TMessage">Message type</typeparam>
        /// <returns>Number of subscribers</returns>
        public int Count<TMessage>() where TMessage : EventMessage
        {
            return GetSubscribers<TMessage>().Count();
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Returns the number of subscribers (by MessageType)
        /// </summary>
        /// <typeparam name="TMessage">Message type</typeparam>
        /// <returns>Number of subscribers</returns>
        public int Count<TMessage>(string tag) where TMessage : EventMessage
        {
            return GetSubscribers<TMessage>(tag).Count();
        }
        
        /// <summary>
        /// Returns the number of subscribers (of any type)
        /// </summary>
        /// <returns>Number of subscribers</returns>
        public int Count()
        {
            return _subscribers.Sum(entry => entry.Value.Count());
        }

        /// <inheritdoc />
        /// <summary>
        /// Relay messages to another even broker
        /// </summary>
        /// <param name="peerEventBroker"></param>
        public void RelayTo(IEventBroker peerEventBroker)
        {
            if (_relayTo.Contains(peerEventBroker)) {
                return;
            }
            
            _relayTo.Add(peerEventBroker);
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Stops relaying messages to another even broker
        /// </summary>
        /// <param name="peerEventBroker"></param>
        public void DoNotRelayTo(IEventBroker peerEventBroker)
        {
            if (! _relayTo.Contains(peerEventBroker)) {
                return;
            }
            
            _relayTo.Remove(peerEventBroker);
        }

        /// <inheritdoc />
        /// <summary>
        /// Do not relay some message type to another event broker
        /// </summary>
        /// <param name="peerEventBroker"></param>
        /// <typeparam name="TMessage"></typeparam>
        public void DoNotRelayTo<TMessage>(IEventBroker peerEventBroker)
        {
            if (! _doNotRelay.ContainsKey(peerEventBroker))
            {
                _doNotRelay[peerEventBroker] = new List<string>();
            }

            // ReSharper disable once InconsistentNaming
            var messageType = typeof(TMessage).ToString();
            
            if (_doNotRelay[peerEventBroker].Contains(messageType)) {
                return;
            }
            
            _doNotRelay[peerEventBroker].Add(messageType);
        }

        /// <inheritdoc />
        /// <summary>
        /// Do not relay some message type to another event broker
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="peerEventBroker"></param>
        /// <typeparam name="TMessage"></typeparam>
        public void DoNotRelayTo<TMessage>(string tag, IEventBroker peerEventBroker)
        {
            if (! _doNotRelay.ContainsKey(peerEventBroker))
            {
                _doNotRelay[peerEventBroker] = new List<string>();
            }

            // ReSharper disable once InconsistentNaming
            var messageType = getCompositeMessageType(typeof(TMessage).ToString(), tag);
            
            if (_doNotRelay[peerEventBroker].Contains(messageType)) {
                return;
            }
            
            _doNotRelay[peerEventBroker].Add(messageType);
        }
        
        #endregion
        
        #region private Methods

        /// <summary>
        /// Get the events list for the given message type
        /// </summary>
        /// <typeparam name="TMessage">Message type</typeparam>
        /// <returns></returns>
        private SubscribersList GetSubscribers<TMessage>() where TMessage : EventMessage
        {
            return GetSubscribers(typeof(TMessage).ToString());
        }
        
        /// <summary>
        /// Get the events list for the given message type
        /// </summary>
        /// <typeparam name="TMessage">Message type</typeparam>
        /// <returns></returns>
        private IEnumerable<SubscribersList> GetSubscribers<TMessage>(IEnumerable<string> tags) where TMessage : EventMessage
        {
            var subscribersByTag = new List<SubscribersList>();
            
            subscribersByTag.AddRange(
                tags.Select(
                    tag => GetSubscribers(typeof(TMessage).ToString(), tag)
                )
            );

            return subscribersByTag;
        }
        
        /// <summary>
        /// Get the events list for the given message type
        /// </summary>
        /// <typeparam name="TMessage">Message type</typeparam>
        /// <returns></returns>
        private SubscribersList GetSubscribers<TMessage>(string tag) where TMessage : EventMessage
        {
            return GetSubscribers(getCompositeMessageType(typeof(TMessage).ToString(), tag));
        }
        
        /// <summary>
        /// Get the events list for the given message type
        /// </summary>
        /// <param name="messageType"></param>
        /// <returns></returns>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private SubscribersList GetSubscribers(string messageType) {
            SubscribersList foundSubscribers;

            if (_subscribers.TryGetValue(messageType, out foundSubscribers))
            {
                return foundSubscribers;
            }
            
            foundSubscribers = new SubscribersList();
            _subscribers.Add(messageType, foundSubscribers);

            return foundSubscribers;
        }

        /// <summary>
        /// Get the events list for the given message type
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private SubscribersList GetSubscribers(string messageType, string tag) {
            SubscribersList foundSubscribers;

            if (_subscribers.TryGetValue(getCompositeMessageType(messageType, tag), out foundSubscribers)) {
                return foundSubscribers;
            }
            
            foundSubscribers = new SubscribersList();
            _subscribers.Add(getCompositeMessageType(messageType, tag), foundSubscribers);

            return foundSubscribers;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public static string getCompositeMessageType(string messageType, string tag)
        {
            return messageType + ":" + tag;
        }

        #endregion
    }
}
