using System;
using NUnit.Framework;
using TreeislandStudio.Engine.Event.Event;

namespace TreeislandStudio.Engine.Event.Editor.Test
{
    public class EventAgentTest
    {

        /// <summary>
        /// Test class TypeOne
        /// </summary>
        class TypeOne : EventMessage { };

        /// <summary>
        /// Tests subscription
        /// </summary>
        [Test] public void TestSubscription() 
        {
            var eventBroker = new EventBroker();
            var agent = new EventAgent(eventBroker);
            Action<TypeOne> listener = message => { };
            agent.Subscribe(listener);
            Assert.IsTrue(eventBroker.IsSubscribed(listener));
        }
        
        /// <summary>
        /// Tests tagged subscription
        /// </summary>
        [Test] public void TestTaggedSubscription() 
        {
            var eventBroker = new EventBroker();
            var agent = new EventAgent(eventBroker);
            Action<TypeOne> listener = message => { };
            agent.Subscribe("tag", listener);
            Assert.IsTrue(eventBroker.IsSubscribed("tag", listener));
        }
        
        /// <summary>
        /// We can subscribe twice a given subscriber, that should have no effect
        /// </summary>
        [Test] public void TestSubscriptionTwice() 
        {
            var eventBroker = new EventBroker();
            var agent = new EventAgent(eventBroker);
            Action<TypeOne> listener = message => { };
            agent.Subscribe(listener);
            Assert.IsTrue(eventBroker.IsSubscribed(listener));
            Assert.AreEqual(1, eventBroker.Count());
            agent.Subscribe(listener);
            Assert.IsTrue(eventBroker.IsSubscribed(listener));
            Assert.AreEqual(1, eventBroker.Count());
        }
        
        /// <summary>
        /// We can subscribe twice a given tagged subscriber, that should have no effect
        /// </summary>
        [Test] public void TestTaggedSubscriptionTwice() 
        {
            var eventBroker = new EventBroker();
            var agent = new EventAgent(eventBroker);
            Action<TypeOne> listener = message => { };
            agent.Subscribe("tag", listener);
            Assert.IsTrue(eventBroker.IsSubscribed("tag", listener));
            Assert.AreEqual(1, eventBroker.Count());
            agent.Subscribe("tag", listener);
            Assert.IsTrue(eventBroker.IsSubscribed("tag", listener));
            Assert.AreEqual(1, eventBroker.Count());
        }

        /// <summary>
        /// Tests unsubscription
        /// </summary>
        [Test]
        public void TestUnSubscription() 
        {
            var eventBroker = new EventBroker();
            var agent = new EventAgent(eventBroker);
            Action<TypeOne> listener = message => { };
            agent.Subscribe(listener);
            Assert.IsTrue(eventBroker.IsSubscribed(listener));
            agent.Unsubscribe(listener);
            Assert.IsFalse(eventBroker.IsSubscribed(listener));
        }
        
        /// <summary>
        /// Tests tagged unsubscription
        /// </summary>
        [Test]
        public void TestTaggedUnSubscription() 
        {
            var eventBroker = new EventBroker();
            var agent = new EventAgent(eventBroker);
            Action<TypeOne> listener = message => { };
            agent.Subscribe("tag", listener);
            Assert.IsTrue(eventBroker.IsSubscribed("tag", listener));
            agent.Unsubscribe("tag", listener);
            Assert.IsFalse(eventBroker.IsSubscribed("tag", listener));
        }

        /// <summary>
        /// Tests auto unsubscription
        /// </summary>
        [Test]
        public void TestUnsubscribeAll() 
        {
            var eventBroker = new EventBroker();
            Action<TypeOne> listener = message => { };
            var agent = new EventAgent(eventBroker);
            agent.Subscribe(listener);
            Assert.IsTrue(eventBroker.IsSubscribed(listener));
            agent.UnsubscribeAll();
            Assert.IsFalse(eventBroker.IsSubscribed(listener));
        }
        
        /// <summary>
        /// When unsubscribing all events from an agent with no subscriptions, nothing happens (no errors)
        /// </summary>
        [Test]
        public void TestUnsubscribeAllEmpty() 
        {
            var eventBroker = new EventBroker();
            Action<TypeOne> listener = message => { };
            var agent = new EventAgent(eventBroker);
            agent.UnsubscribeAll();
            Assert.IsFalse(eventBroker.IsSubscribed(listener));
        }
        
        /// <summary>
        /// Tests auto tagged unsubscription
        /// </summary>
        [Test]
        public void TestTaggedUnsubscribeAll() 
        {
            var eventBroker = new EventBroker();
            Action<TypeOne> listener = message => { };
            var agent = new EventAgent(eventBroker);
            agent.Subscribe("tag", listener);
            Assert.IsTrue(eventBroker.IsSubscribed("tag", listener));
            agent.UnsubscribeAll();
            Assert.IsFalse(eventBroker.IsSubscribed("tag", listener));
        }

        /// <summary>
        /// Test a published message is received
        /// </summary>
        [Test]
        public void TestPublish() 
        {
            EventBroker eventBroker = new EventBroker();
            bool received = false;
            Action<TypeOne> listener = message => { received = true; };
            var agent = new EventAgent(eventBroker);

            agent.Subscribe(listener);
            agent.Publish(new TypeOne());
            Assert.AreEqual(1, eventBroker.Count<TypeOne>());
            Assert.AreEqual(true, received);
        }

        /// <summary>
        /// An event broker is disposable and should unsubscribe all events when disposed
        /// </summary>
        [Test]
        public void AnEventAgentIsDisposable()
        {
            EventBroker eventBroker = new EventBroker();
            Action<TypeOne> listener = message => { };
            var agent = new EventAgent(eventBroker);

            agent.Subscribe(listener);
            Assert.AreEqual(1, eventBroker.Count<TypeOne>());
            agent.Dispose();
            Assert.AreEqual(0, eventBroker.Count<TypeOne>());
        }
    }
}