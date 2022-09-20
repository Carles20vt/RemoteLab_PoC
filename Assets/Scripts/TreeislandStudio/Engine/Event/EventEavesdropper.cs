using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TreeislandStudio.Engine.Event.Event;

namespace TreeislandStudio.Engine.Event {
    /// <inheritdoc />
    /// <summary>
    /// This class acts as an event broker but additionally sends a copy to a second one
    /// </summary>
    public class EventEavesdropper: IEventBroker {

        #region Public fields
        
        /// <summary>
        /// Event broker name
        /// </summary>
        public string Name { [UsedImplicitly] get; }

        #endregion
        
        /// <summary>
        /// The legit or primary event broker
        /// </summary>
        private readonly IEventBroker eventBroker;
        
        /// <summary>
        /// The event broker that gets a copy of published the messages
        /// </summary>
        private readonly IEventBroker eavesdroppingEventBroker;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="eventBroker">The legit or primary event broker</param>
        /// <param name="eavesdroppingEventBroker">The event broker that gets a copy of published the messages</param>
        public EventEavesdropper(IEventBroker eventBroker, IEventBroker eavesdroppingEventBroker)
        {
            Name = "";
            this.eventBroker   = eventBroker;
            this.eavesdroppingEventBroker = eavesdroppingEventBroker;
        }

        /// <inheritdoc />
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="eventBroker">The legit or primary event broker</param>
        /// <param name="eavesdroppingEventBroker">The event broker that gets a copy of published the messages</param>
        public EventEavesdropper(string name, IEventBroker eventBroker, IEventBroker eavesdroppingEventBroker) :
            this (eventBroker, eavesdroppingEventBroker)
        {
            Name = name;
        }
        
        /// <summary>
        /// Does not need to dispose of anything as does not allocate anything
        /// </summary>
        public void Dispose() {
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <typeparam name="MessageType"></typeparam>
        /// <returns></returns>
        public int Count<MessageType>() where MessageType : EventMessage {
            return eventBroker.Count<MessageType>();
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="tag"></param>
        /// <typeparam name="MessageType"></typeparam>
        /// <returns></returns>
        public int Count<MessageType>(string tag) where MessageType : EventMessage
        {
            return eventBroker.Count<MessageType>(tag);
        }

        /// <inheritdoc />
        /// <summary>
        /// Relay messages to another even broker
        /// </summary>
        /// <param name="peerEventBroker"></param>
        public void RelayTo(IEventBroker peerEventBroker)
        {
            eventBroker.RelayTo(peerEventBroker);
        }

        /// <inheritdoc />
        /// <summary>
        /// Stops relaying messages to another even broker
        /// </summary>
        /// <param name="peerEventBroker"></param>
        public void DoNotRelayTo(IEventBroker peerEventBroker)
        {
            eventBroker.DoNotRelayTo(peerEventBroker);
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="subscriber"></param>
        /// <typeparam name="MessageType"></typeparam>
        /// <returns></returns>
        public bool IsSubscribed<MessageType>(Action<MessageType> subscriber) where MessageType : EventMessage {
            return eventBroker.IsSubscribed(subscriber);
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="subscriber"></param>
        /// <typeparam name="MessageType"></typeparam>
        /// <returns></returns>
        public bool IsSubscribed<MessageType>(string tag, Action<MessageType> subscriber) where MessageType : EventMessage
        {
            return eventBroker.IsSubscribed(tag, subscriber);
        }

        /// <inheritdoc />
        /// <summary>
        /// Publish a message (by MessageType)
        /// </summary>
        /// <typeparam name="MessageType">Message type</typeparam>
        /// <param name="message">Message</param>
        /// <param name="firstEventBroker"></param>
        public void Publish<MessageType>(MessageType message, IEventBroker firstEventBroker = null) where MessageType : EventMessage {
            if (firstEventBroker == null) {
                firstEventBroker = this;
            }
            eventBroker.Publish(message, firstEventBroker);
            eavesdroppingEventBroker.Publish(message, firstEventBroker);
        }

        /// <inheritdoc />
        /// <summary>
        /// Publish a message (by MessageType)
        /// </summary>
        /// <typeparam name="MessageType">Message type</typeparam>
        /// <param name="tag"></param>
        /// <param name="message">Message</param>
        /// <param name="firstEventBroker"></param>
        public void Publish<MessageType>(string tag, MessageType message, IEventBroker firstEventBroker = null) where MessageType : EventMessage
        {
            if (firstEventBroker == null) {
                firstEventBroker = this;
            }
            eventBroker.Publish(tag, message, firstEventBroker);
            eavesdroppingEventBroker.Publish(tag, message, firstEventBroker);
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="messageTypeName"></param>
        /// <param name="subscriber"></param>
        public void UnsubscribeByMessageTypeName(string messageTypeName, object subscriber) {
            eventBroker.UnsubscribeByMessageTypeName(messageTypeName, subscriber);
        }

        /// <inheritdoc />
        /// <summary>
        /// Do not relay some message type to another event broker
        /// </summary>
        /// <param name="peerEventBroker"></param>
        /// <typeparam name="MessageType"></typeparam>
        public void DoNotRelayTo<MessageType>(IEventBroker peerEventBroker)
        {
            eventBroker.DoNotRelayTo<MessageType>(peerEventBroker);
        }

        public void DoNotRelayTo<MessageType>(string tag, IEventBroker peerEventBroker)
        {
            eventBroker.DoNotRelayTo<MessageType>(tag, peerEventBroker);
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="subscriber"></param>
        /// <typeparam name="MessageType"></typeparam>
        public void Subscribe<MessageType>(Action<MessageType> subscriber) where MessageType : EventMessage {
            eventBroker.Subscribe(subscriber);
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="subscriber"></param>
        /// <typeparam name="MessageType"></typeparam>
        public void Subscribe<MessageType>(IEnumerable<string> tags, Action<MessageType> subscriber) where MessageType : EventMessage
        {
            eventBroker.Subscribe(tags, subscriber);
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="subscriber"></param>
        /// <typeparam name="MessageType"></typeparam>
        public void Subscribe<MessageType>(string tag, Action<MessageType> subscriber) where MessageType : EventMessage
        {
            eventBroker.Subscribe(tag, subscriber);
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="subscriber"></param>
        /// <typeparam name="MessageType"></typeparam>
        /// <returns></returns>
        public bool UnSubscribe<MessageType>(Action<MessageType> subscriber) where MessageType : EventMessage {
            return eventBroker.UnSubscribe(subscriber);
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="subscriber"></param>
        /// <typeparam name="MessageType"></typeparam>
        /// <returns></returns>
        public bool UnSubscribe<MessageType>(IEnumerable<string> tags, Action<MessageType> subscriber) where MessageType : EventMessage
        {
            return eventBroker.UnSubscribe(tags, subscriber);
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="subscriber"></param>
        /// <typeparam name="MessageType"></typeparam>
        /// <returns></returns>
        public bool UnSubscribe<MessageType>(string tag, Action<MessageType> subscriber) where MessageType : EventMessage
        {
            return eventBroker.UnSubscribe(tag, subscriber);
        }
    }
}