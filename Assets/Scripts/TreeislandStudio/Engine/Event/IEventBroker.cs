using System;
using System.Collections.Generic;
using TreeislandStudio.Engine.Event.Event;

namespace TreeislandStudio.Engine.Event
{
    /// <summary>
    /// Sends events, implements the pub/sub pattern
    /// </summary>
    public interface IEventBroker : IDisposable
    {
        /// <summary>
        /// Event broker name
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// Returns the number of subscribers (by MessageType)
        /// </summary>
        /// <typeparam name="MessageType">Message type</typeparam>
        /// <returns>Number of subscribers</returns>
        int Count<MessageType>() where MessageType : EventMessage;

        /// <summary>
        /// Returns the number of subscribers (by MessageType)
        /// </summary>
        /// <typeparam name="MessageType">Message type</typeparam>
        /// <returns>Number of subscribers</returns>
        int Count<MessageType>(string tag) where MessageType : EventMessage;
        
        /// <summary>
        /// Relay messages to another even broker
        /// </summary>
        /// <param name="peerEventBroker"></param>
        void RelayTo(IEventBroker peerEventBroker);

        /// <summary>
        /// Stops relaying messages to another even broker
        /// </summary>
        /// <param name="peerEventBroker"></param>
        void DoNotRelayTo(IEventBroker peerEventBroker);
        
        /// <summary>
        /// Checks if a given subscriber is already subscribed
        /// </summary>
        /// <typeparam name="MessageType">Event type</typeparam>
        /// <param name="subscriber">Subscriber</param>
        /// <returns></returns>
        bool IsSubscribed<MessageType>(Action<MessageType> subscriber) where MessageType : EventMessage;

        /// <summary>
        /// Checks if a given subscriber is already subscribed
        /// </summary>
        /// <typeparam name="MessageType">Event type</typeparam>
        /// <param name="tag"></param>
        /// <param name="subscriber">Subscriber</param>
        /// <returns></returns>
        bool IsSubscribed<MessageType>(string tag, Action<MessageType> subscriber) where MessageType : EventMessage;
        
        /// <summary>
        /// Publish a message (by MessageType)
        /// </summary>
        /// <typeparam name="MessageType">Message type</typeparam>
        /// <param name="message">Message</param>
        /// <param name="firstEventBroker"></param>
        void Publish<MessageType>(MessageType message, IEventBroker firstEventBroker = null)
            where MessageType : EventMessage;

        /// <summary>
        /// Publish a message (by MessageType)
        /// </summary>
        /// <typeparam name="MessageType">Message type</typeparam>
        /// <param name="tag"></param>
        /// <param name="message">Message</param>
        /// <param name="firstEventBroker"></param>
        void Publish<MessageType>(string tag, MessageType message, IEventBroker firstEventBroker = null)
            where MessageType : EventMessage;
            
        /// <summary>
        /// Removes a subscriber
        /// </summary>
        /// <param name="messageTypeName">Message type</param>
        /// <param name="subscriber">subscriber</param>
        void UnsubscribeByMessageTypeName(string messageTypeName, object subscriber);

        /// <summary>
        /// Do not relay some message type to another event broker
        /// </summary>
        /// <param name="peerEventBroker"></param>
        /// <typeparam name="MessageType"></typeparam>
        void DoNotRelayTo<MessageType>(IEventBroker peerEventBroker);

        /// <summary>
        /// Do not relay some message type to another event broker
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="peerEventBroker"></param>
        /// <typeparam name="MessageType"></typeparam>
        void DoNotRelayTo<MessageType>(string tag, IEventBroker peerEventBroker);
        
        /// <summary>
        /// Subscribe a subscriber (by MessageType)
        /// </summary>
        /// <typeparam name="MessageType">Message type the subscriber is subscriber to</typeparam>
        /// <param name="subscriber">Subscriber</param>
        void Subscribe<MessageType>(Action<MessageType> subscriber) where MessageType : EventMessage;

        /// <summary>
        /// Subscribe a subscriber (by MessageType)
        /// </summary>
        /// <typeparam name="MessageType">Message type the subscriber is subscribing to</typeparam>
        /// <param name="tags"></param>
        /// <param name="subscriber">Subscriber</param>
        void Subscribe<MessageType>(IEnumerable<string> tags, Action<MessageType> subscriber) where MessageType : EventMessage;

        /// <summary>
        /// Subscribe a subscriber (by MessageType)
        /// </summary>
        /// <typeparam name="MessageType">Message type the subscriber is subscribing to</typeparam>
        /// <param name="tag"></param>
        /// <param name="subscriber">Subscriber</param>
        void Subscribe<MessageType>(string tag, Action<MessageType> subscriber) where MessageType : EventMessage;
        
        /// <summary>
        /// Subscribe a subscriber (by MessageType)
        /// </summary>
        /// <typeparam name="MessageType">Message type the subscriber is subscriber to</typeparam>
        /// <param name="subscriber">Subscriber</param>
        bool UnSubscribe<MessageType>(Action<MessageType> subscriber) where MessageType : EventMessage;

        /// <summary>
        /// Unsubscribe a subscriber (by MessageType)
        /// </summary>
        /// <typeparam name="MessageType">Message type the subscriber is subscribing to</typeparam>
        /// <param name="tags"></param>
        /// <param name="subscriber">Subscriber</param>
        bool UnSubscribe<MessageType>(IEnumerable<string> tags, Action<MessageType> subscriber) where MessageType : EventMessage;

        /// <summary>
        /// Unsubscribe a subscriber (by MessageType)
        /// </summary>
        /// <typeparam name="MessageType">Message type the subscriber is subscribing to</typeparam>
        /// <param name="tag"></param>
        /// <param name="subscriber">Subscriber</param>
        bool UnSubscribe<MessageType>(string tag, Action<MessageType> subscriber) where MessageType : EventMessage;
    }
}
