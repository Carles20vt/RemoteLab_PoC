using System;
using System.Collections.Generic;
using TreeislandStudio.Engine.Event.Event;

namespace TreeislandStudio.Engine.Event {
    /// <summary>
    /// Manages lists of subscribers
    /// </summary>
    internal class SubscribersList {

        #region public properties

        internal List<object> Subscribers { get; } = new List<object>();

        #endregion

        #region Private fields

        #endregion

        #region Public methods

        /// <summary>
        /// Sends a message to all the subscribers
        /// </summary>
        /// <typeparam name="MessageType">Type of the message</typeparam>
        /// <param name="message">Message</param>
        public void Invoke<MessageType>(MessageType message) where MessageType : EventMessage {
            // To array is used here to force a copy of the list, so modifications on the list itself does not break the foreach
            foreach (Action<MessageType> action in Subscribers.ToArray()) {
                action(message);
            }
        }

        /// <summary>
        /// Empties the subscribers list
        /// </summary>
        public void Empty() {
            Subscribers.Clear();
        }

        /// <summary>
        /// Adds a subscriber
        /// </summary>
        /// <param name="subscriber">Subscriber</param>
        public void Add(object subscriber) {
            Subscribers.Add(subscriber);
        }

        /// <summary>
        /// Remove a subscriber
        /// </summary>
        /// <param name="subscriber">Subscriber</param>
        public bool Remove(object subscriber) {
            return Subscribers.Remove(subscriber);
        }

        /// <summary>
        /// Check if the given subscriber is already on the list
        /// </summary>
        /// <param name="subscriber"></param>
        /// <returns></returns>
        public bool Has(object subscriber) {
            return Subscribers.Contains(subscriber);
        }

        /// <summary>
        /// Returns the number of subscribers
        /// </summary>
        /// <returns>Number of subscribers</returns>
        public int Count() {
            return Subscribers.Count;
        }

        #endregion
    }
}
