using System;
using NUnit.Framework;
using TreeislandStudio.Engine.Event.Event;

namespace TreeislandStudio.Engine.Event.Editor.Test {
    public class SubscribersListTest {
        #region Public methods
        
        /// <summary>
        /// We can add subscribers to subscriber lists
        /// </summary>
        [Test]
        public void WeCanAddASubscriberToASubscriberList()
        {
            var subscribersList = new SubscribersList();
            
            Action<EventMessage> subscriber = (message) => { };
            
            Assert.AreEqual(0, subscribersList.Count());
            
            subscribersList.Add(subscriber);
            Assert.AreEqual(1, subscribersList.Count());
        }
        
        /// <summary>
        /// We can get a subscribers list subscribers
        /// </summary>
        [Test]
        public void WeCanGetASubscriberListSubscribers()
        {
            var subscribersList = new SubscribersList();
            
            Action<EventMessage> subscriber = (message) => { };
            
            subscribersList.Add(subscriber);
            
            Assert.AreEqual(subscriber, subscribersList.Subscribers[0]);
        }

        /// <summary>
        /// We can invoke all subscribers
        /// </summary>
        [Test]
        public void WeCanInvokeAllSubscribers()
        {
            var subscribersList = new SubscribersList();
            var invokedA = false;
            var invokedB = false;
            
            Action<EventMessage> subscriberA = (message) => { invokedA = true; };
            Action<EventMessage> subscriberB = (message) => { invokedB = true; };
            
            subscribersList.Add(subscriberA);
            subscribersList.Add(subscriberB);
            subscribersList.Invoke(new EventMessage());
            
            Assert.IsTrue(invokedA);
            Assert.IsTrue(invokedB);
        }
        
        /// <summary>
        /// We can remove a given subscriber
        /// </summary>
        [Test]
        public void WeCanRemoveASubscriber()
        {
            var subscribersList = new SubscribersList();
            var invokedA = false;
            var invokedB = false;
            
            Action<EventMessage> subscriberA = (message) => { invokedA = true; };
            Action<EventMessage> subscriberB = (message) => { invokedB = true; };
            
            subscribersList.Add(subscriberA);
            subscribersList.Add(subscriberB);
            subscribersList.Remove(subscriberB);
            subscribersList.Invoke(new EventMessage());
            
            Assert.IsTrue(invokedA);
            Assert.IsFalse(invokedB);
        }
        
        /// <summary>
        /// We can check if a subscriber is on a list
        /// </summary>
        [Test]
        public void WeCheckIfIsInList()
        {
            var subscribersList = new SubscribersList();
            
            Action<EventMessage> subscriberA = (message) => { };
            Action<EventMessage> subscriberB = (message) => { };
            
            subscribersList.Add(subscriberA);
            subscribersList.Add(subscriberB);
            
            Assert.IsTrue(subscribersList.Has(subscriberA));
            Assert.IsTrue(subscribersList.Has(subscriberB));

            subscribersList.Remove(subscriberB);
            
            Assert.IsTrue(subscribersList.Has(subscriberA));
            Assert.IsFalse(subscribersList.Has(subscriberB));
        }
        
        /// <summary>
        /// We can count how many subscribers a list have
        /// </summary>
        [Test]
        public void WeCanCountLists()
        {
            var subscribersList = new SubscribersList();
            
            Action<EventMessage> subscriberA = (message) => { };
            Action<EventMessage> subscriberB = (message) => { };
            
            subscribersList.Add(subscriberA);
            Assert.AreEqual(1, subscribersList.Count());
            subscribersList.Add(subscriberB);
            Assert.AreEqual(2, subscribersList.Count());
            subscribersList.Remove(subscriberB);
            Assert.AreEqual(1, subscribersList.Count());
        }
        
        /// <summary>
        /// We can empty a subscriber list
        /// </summary>
        [Test]
        public void WeCanEmptyAList()
        {
            var subscribersList = new SubscribersList();
            
            Action<EventMessage> subscriberA = (message) => { };

            Assert.AreEqual(0, subscribersList.Count());
            subscribersList.Add(subscriberA);
            Assert.AreEqual(1, subscribersList.Count());
            subscribersList.Empty();
            Assert.AreEqual(0, subscribersList.Count());
        }
        
        #endregion
    }
}