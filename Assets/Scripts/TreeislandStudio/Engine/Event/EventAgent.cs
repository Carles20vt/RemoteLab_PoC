using System;
using System.Collections.Generic;
using TreeislandStudio.Engine.Event.Event;

namespace TreeislandStudio.Engine.Event
{

    /// <summary>
    /// Accounts all the subscriptions and un-subscriptions to an event broker so a user has not to do it by itself
    /// </summary>
    public class EventAgent : IDisposable
    {
        #region Private Fields
        
        /// <summary>
        /// Subscriptions
        /// </summary>
        private readonly Dictionary<string, SubscribersList> _subscribers = new Dictionary<string, SubscribersList>();
        
        #endregion
        
        #region Life cycle

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="eventBroker">Event manager</param>
        public EventAgent(IEventBroker eventBroker)
        {
            Broker = eventBroker;
        }

        /// <summary>
        /// Event broker
        /// </summary>
        public IEventBroker Broker { get; }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            UnsubscribeAll();
        }

        #endregion
        
        #region Public Methods

        /// <summary>
        /// Subscribe a subscriber, avoids to subscribe a subscriber twice
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="subscriber"></param>
        public EventAgent Subscribe<TMessage>(Action<TMessage> subscriber) where TMessage : EventMessage
        {
            if (Broker.IsSubscribed(subscriber)) {
                
                return this;
            }

            Broker.Subscribe(subscriber);
            GetSubscribers<TMessage>().Add(subscriber);

            return this;
        }

        /// <summary>
        /// Subscribe a subscriber, avoids to subscribe a subscriber twice
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="tag"></param>
        /// <param name="subscriber"></param>
        public EventAgent Subscribe<TMessage>(string tag, Action<TMessage> subscriber) where TMessage : EventMessage
        {
            if (Broker.IsSubscribed(tag, subscriber))
            {
                return this;
            }

            Broker.Subscribe(tag, subscriber);
            GetSubscribers<TMessage>(tag).Add(subscriber);

            return this;
        }

        /// <summary>
        /// Publish a message using the eventManager
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="message"></param>
        public EventAgent Publish<TMessage>(TMessage message) where TMessage : EventMessage
        {
            Broker.Publish(message);

            return this;
        }

        /// <summary>
        /// Unsubscribe a subscriber
        /// </summary>
        /// <typeparam name="TMessage">Message type</typeparam>
        /// <param name="subscriber">Subscriber</param>
        public EventAgent Unsubscribe<TMessage>(Action<TMessage> subscriber) where TMessage : EventMessage
        {
            Broker.UnSubscribe(subscriber);
            GetSubscribers<TMessage>().Remove(subscriber);

            return this;
        }

        /// <summary>
        /// Unsubscribe a subscriber
        /// </summary>
        /// <typeparam name="TMessage">Message type</typeparam>
        /// <param name="tag"></param>
        /// <param name="subscriber">Subscriber</param>
        public EventAgent Unsubscribe<TMessage>(string tag, Action<TMessage> subscriber) where TMessage : EventMessage
        {
            Broker.UnSubscribe(tag, subscriber);
            GetSubscribers<TMessage>(tag).Remove(subscriber);

            return this;
        }

        /// <summary>
        /// Unsubscribe all subscribers
        /// </summary>
        public EventAgent UnsubscribeAll()
        {
            foreach (var subscribersList in _subscribers)
            {
                foreach (var subscriber in subscribersList.Value.Subscribers)
                {
                    Broker.UnsubscribeByMessageTypeName(subscribersList.Key, subscriber);
                }
            }

            return this;
        }

        #endregion
        
        #region Non public methods

        /// <summary>
        /// Get the events list for the given message type
        /// </summary>
        /// <typeparam name="TMessage">Message type</typeparam>
        /// <returns></returns>
        private SubscribersList GetSubscribers<TMessage>() where TMessage : EventMessage
        {
            SubscribersList matchingSubscribers;
            var subscriberKind = typeof(TMessage).ToString();

            if (_subscribers.TryGetValue(subscriberKind, out matchingSubscribers))
            {
                return matchingSubscribers;
            }
            
            matchingSubscribers = new SubscribersList();
            _subscribers.Add(subscriberKind, matchingSubscribers);

            return matchingSubscribers;
        }
        
        /// <summary>
        /// Get the events list for the given message type and tag
        /// </summary>
        /// <typeparam name="TMessage">Message type</typeparam>
        /// <returns></returns>
        private SubscribersList GetSubscribers<TMessage>(string tag) where TMessage : EventMessage
        {
            SubscribersList matchingSubscribers;
            var subscriberKind = EventBroker.getCompositeMessageType(typeof(TMessage).ToString(), tag);

            if (_subscribers.TryGetValue(subscriberKind, out matchingSubscribers))
            {
                return matchingSubscribers;
            }
            
            matchingSubscribers = new SubscribersList();
            _subscribers.Add(subscriberKind, matchingSubscribers);

            return matchingSubscribers;
        }

        #endregion
    }
}
