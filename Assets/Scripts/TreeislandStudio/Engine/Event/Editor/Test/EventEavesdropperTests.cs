using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using TreeislandStudio.Engine.Event.Event;

namespace TreeislandStudio.Engine.Event.Editor.Test
{
    /// <summary>
    /// EventEavesdropper class tests
    /// </summary>
    public class EventEavesdropperTests {

        /// <inheritdoc />
        /// <summary>
        /// Test class TypeOne
        /// </summary>
        private class TypeOne : EventMessage { };

        /// <inheritdoc />
        /// <summary>
        /// Test class TypeTwo
        /// </summary>
        private class TypeTwo : EventMessage { };

        /// <summary>
        /// A subscriber is added to the subscriber's list
        /// </summary>
        [Test]
        public void TestSubscribe() {
            var eventBroker = new EventEavesdropper(new EventBroker(), new EventBroker());
            Action<TypeOne> listener = message => {};
            eventBroker.Subscribe(listener);
            Assert.IsTrue(eventBroker.IsSubscribed(listener));
        }
        
        /// <summary>
        /// A tagged subscriber is added to the subscriber's list
        /// </summary>
        [Test]
        public void TestTaggedSubscribe() {
            var eventBroker = new EventEavesdropper(new EventBroker(), new EventBroker());
            Action<TypeOne> listener = message => {};
            eventBroker.Subscribe("tag", listener);
            Assert.IsTrue(eventBroker.IsSubscribed("tag", listener));
        }

        /// <summary>
        /// A subscriber is not added to the subscriber's list
        /// </summary>
        [Test]
        public void TestNotSubscribe() {
            var eventBroker = new EventEavesdropper(new EventBroker(), new EventBroker());
            Action<TypeOne> listener = message => { };
            Assert.False(eventBroker.IsSubscribed(listener));
        }
        
        /// <summary>
        /// A tagged subscriber is not added to the subscriber's list
        /// </summary>
        [Test]
        public void TestNotTaggedSubscribe() {
            var eventBroker = new EventEavesdropper(new EventBroker(), new EventBroker());
            Action<TypeOne> listener = message => { };
            Assert.False(eventBroker.IsSubscribed("tag", listener));
        }

        /// <summary>
        /// A subscriber is added to the its subscriber's list
        /// </summary>
        [Test]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public void TestSubscribeWithType() {
            var eventBroker = new EventEavesdropper(new EventBroker(), new EventBroker());
            Action<TypeOne> listener = message => { };
            eventBroker.Subscribe(listener);
            Assert.AreEqual(1, eventBroker.Count<TypeOne>());
            Assert.AreEqual(0, eventBroker.Count<TypeTwo>());
        }
        
        /// <summary>
        /// A tagged subscriber is not added to the subscriber's list
        /// </summary>
        [Test]
        public void TestTaggedNotSubscribe() {
            var eventBroker = new EventEavesdropper(new EventBroker(), new EventBroker());
            Action<TypeOne> listener = message => { };
            Assert.False(eventBroker.IsSubscribed("tag", listener));
        }

        /// <summary>
        /// A published message is received
        /// </summary>
        [Test]
        public void TestPublish() {
            var eventBroker = new EventEavesdropper(new EventBroker(), new EventBroker());
            var received = false;
            Action<TypeOne> listener = message => { received = true; };
            eventBroker.Subscribe(listener);
            eventBroker.Publish(new TypeOne());
            Assert.AreEqual(1, eventBroker.Count<TypeOne>());
            Assert.AreEqual(true, received);
        }
        
        /// <summary>
        /// A tagged published message is received
        /// </summary>
        [Test]
        public void TestTaggedPublish() {
            var eventBroker = new EventEavesdropper(new EventBroker(), new EventBroker());
            var received = false;
            Action<TypeOne> listener = message => { received = true; };
            eventBroker.Subscribe("tag", listener);
            eventBroker.Publish("tag", new TypeOne());
            Assert.AreEqual(0, eventBroker.Count<TypeOne>());
            Assert.AreEqual(1, eventBroker.Count<TypeOne>("tag"));
            Assert.AreEqual(true, received);
        }

        /// <summary>
        /// A tagged published message is received when subscribing to multiple tags
        /// </summary>
        [Test]
        public void TestMultipleTaggedPublish() {
            var eventBroker = new EventEavesdropper(new EventBroker(), new EventBroker());
            var received = false;
            Action<TypeOne> listener = message => { received = true; };
            eventBroker.Subscribe(new List<string> { "tag", "other tag" }, listener);
            eventBroker.Publish("tag", new TypeOne());
            Assert.AreEqual(0, eventBroker.Count<TypeOne>());
            Assert.AreEqual(1, eventBroker.Count<TypeOne>("tag"));
            Assert.AreEqual(true, received);
        }
        
        /// <summary>
        /// A published message is received just by its subscribers
        /// </summary>
        [Test]
        public void TestPublishCheckNotCrossesTypes() {
            var eventBroker = new EventEavesdropper(new EventBroker(), new EventBroker());
            var receivedOne = false;
            var receivedTwo = false;

            Action<TypeOne> listenerOne = message => { receivedOne = true; };
            Action<TypeTwo> listenerTwo = message => { receivedTwo = true; };

            eventBroker.Subscribe(listenerOne);
            eventBroker.Subscribe(listenerTwo);
            eventBroker.Publish(new TypeOne());
            Assert.AreEqual(true, receivedOne);
            Assert.AreEqual(false, receivedTwo);

            receivedOne = false;
            receivedTwo = false;

            eventBroker.Publish(new TypeTwo());
            Assert.AreEqual(false, receivedOne);
            Assert.AreEqual(true, receivedTwo);
        }
        
        /// <summary>
        /// A published message is received just by its subscribers
        /// </summary>
        [Test]
        public void TestPublishCheckNotCrossesTypesOrTags() {
            var eventBroker   = new EventEavesdropper(new EventBroker(), new EventBroker());
            var receivedOne   = false;
            var receivedTwo   = false;
            var receivedThree = false;

            Action<TypeOne> listenerOne   = message => { receivedOne = true; };
            Action<TypeTwo> listenerTwo   = message => { receivedTwo = true; };
            Action<TypeOne> listenerThree = message => { receivedThree = true; };

            eventBroker.Subscribe(listenerOne);
            eventBroker.Subscribe(listenerTwo);
            eventBroker.Subscribe("tag", listenerThree);
            eventBroker.Publish(new TypeOne());
            Assert.AreEqual(true, receivedOne);
            Assert.AreEqual(false, receivedTwo);
            Assert.AreEqual(false, receivedThree);

            receivedOne   = false;
            receivedTwo   = false;
            receivedThree = false;

            eventBroker.Publish(new TypeTwo());
            Assert.AreEqual(false, receivedOne);
            Assert.AreEqual(true, receivedTwo);
            Assert.AreEqual(false, receivedThree);
            
            receivedOne   = false;
            receivedTwo   = false;
            receivedThree = false;

            eventBroker.Publish("tag", new TypeTwo());
            Assert.AreEqual(false, receivedOne);
            Assert.AreEqual(false, receivedTwo);
            Assert.AreEqual(false, receivedThree);
        }
        
        /// <summary>
        /// A tagged published message is received just by its subscribers
        /// </summary>
        [Test]
        public void TestTaggedPublishCheckNotCrossesTypes() {
            var eventBroker   = new EventEavesdropper(new EventBroker(), new EventBroker());
            var receivedOne   = false;
            var receivedTwo   = false;
            var receivedThree = false;

            Action<TypeOne> listenerOne   = message => { receivedOne = true; };
            Action<TypeTwo> listenerTwo   = message => { receivedTwo = true; };
            Action<TypeOne> listenerThree = message => { receivedThree = true; };

            eventBroker.Subscribe("tag", listenerOne);
            eventBroker.Subscribe("tag", listenerTwo);
            eventBroker.Subscribe(listenerThree);
            eventBroker.Publish("tag", new TypeOne());
            Assert.AreEqual(true, receivedOne);
            Assert.AreEqual(false, receivedTwo);
            Assert.AreEqual(false, receivedThree);

            receivedOne   = false;
            receivedTwo   = false;
            receivedThree = false;

            eventBroker.Publish("tag", new TypeTwo());
            Assert.AreEqual(false, receivedOne);
            Assert.AreEqual(true, receivedTwo);
            Assert.AreEqual(false, receivedThree);
        }

        /// <summary>
        /// A published message is received just by its subscribers
        /// </summary>
        [Test]
        public void TestCount() {
            var eventBroker = new EventEavesdropper(new EventBroker(), new EventBroker());

            Action<TypeOne> listenerOne = message => { };
            Action<TypeOne> listenerTwo = message => { };

            eventBroker.Subscribe(listenerOne);
            eventBroker.Subscribe(listenerTwo);
            Assert.AreEqual(2, eventBroker.Count<TypeOne>());
        }

        /// <summary>
        /// A published message is received just by its subscribers
        /// </summary>
        [Test]
        public void TestCountJustExpectedMessages() {
            var eventBroker = new EventEavesdropper(new EventBroker(), new EventBroker());

            Action<TypeOne> listenerOne = message => { };
            Action<TypeTwo> listenerTwo = message => { };

            eventBroker.Subscribe(listenerOne);
            eventBroker.Subscribe(listenerTwo);
            Assert.AreEqual(1, eventBroker.Count<TypeOne>());
        }

        /// <summary>
        /// When a listener is unsubscribed, it is no longer subscribed (duh!)
        /// </summary>
        [Test]
        public void TestRemoveSubscriber() {
            var eventBroker = new EventEavesdropper(new EventBroker(), new EventBroker());
            Action<TypeOne> listenerOne = message => {};

            eventBroker.Subscribe(listenerOne);
            Assert.AreEqual(true, eventBroker.IsSubscribed(listenerOne));

            eventBroker.UnSubscribe(listenerOne);
            Assert.AreEqual(false, eventBroker.IsSubscribed(listenerOne));
        }
        
        /// <summary>
        /// When a listener is unsubscribed from multiple tags , it is no longer subscribed (duh!)
        /// </summary>
        [Test]
        public void TestRemoveMultipleTaggedSubscriber() {
            var eventBroker = new EventEavesdropper(new EventBroker(), new EventBroker());
            Action<TypeOne> listenerOne = message => {};

            eventBroker.Subscribe("tag", listenerOne);
            Assert.AreEqual(true, eventBroker.IsSubscribed("tag", listenerOne));

            eventBroker.UnSubscribe(new List<string> { "tag", "other tag" }, listenerOne);
            Assert.AreEqual(false, eventBroker.IsSubscribed("tag", listenerOne));
        }
        
        /// <summary>
        /// When a listener is unsubscribed from multiple tags, it is no longer subscribed to multiple(duh!)
        /// </summary>
        [Test]
        public void TestRemoveMultipleTaggedMultipleSubscriber() {
            var eventBroker = new EventEavesdropper(new EventBroker(), new EventBroker());
            Action<TypeOne> listenerOne = message => {};

            eventBroker.Subscribe(new List<string> { "tag", "other tag" }, listenerOne);
            Assert.AreEqual(true, eventBroker.IsSubscribed("tag", listenerOne));
            Assert.AreEqual(true, eventBroker.IsSubscribed("other tag", listenerOne));

            eventBroker.UnSubscribe(new List<string> { "tag", "other tag" }, listenerOne);
            Assert.AreEqual(false, eventBroker.IsSubscribed("tag", listenerOne));
            Assert.AreEqual(false, eventBroker.IsSubscribed("other tag", listenerOne));
        }
        
        /// <summary>
        /// When a listener is unsubscribed, it is no longer subscribed (duh!)
        /// </summary>
        [Test]
        public void TestRemoveTaggedSubscriber() {
            var eventBroker = new EventEavesdropper(new EventBroker(), new EventBroker());
            Action<TypeOne> listenerOne = message => {};

            eventBroker.Subscribe("tag", listenerOne);
            Assert.AreEqual(true, eventBroker.IsSubscribed("tag", listenerOne));

            eventBroker.UnSubscribe("tag", listenerOne);
            Assert.AreEqual(false, eventBroker.IsSubscribed("tag", listenerOne));
        }

        /// <summary>
        /// When a listener is unsubscribed using its message type name, it is no longer subscribed (well duh)
        /// </summary>
        [Test]
        public void TestRemoveSubscriberByTypeName() {
            var eventBroker = new EventEavesdropper(new EventBroker(), new EventBroker());
            Action<TypeOne> listenerOne = message => { };

            eventBroker.Subscribe(listenerOne);
            Assert.AreEqual(true, eventBroker.IsSubscribed(listenerOne));
#pragma warning disable 618
            eventBroker.UnsubscribeByMessageTypeName(typeof(TypeOne).ToString(), listenerOne);
#pragma warning restore 618
            Assert.AreEqual(false, eventBroker.IsSubscribed(listenerOne));
        }
        
        /// <summary>
        /// When a tagged listener is unsubscribed using its message type name, it is no longer subscribed (well duh)
        /// </summary>
        [Test]
        public void TestRemoveTaggedSubscriberByTypeName() {
            var eventBroker = new EventEavesdropper(new EventBroker(), new EventBroker());
            Action<TypeOne> listenerOne = message => { };

            eventBroker.Subscribe("tag", listenerOne);
            Assert.AreEqual(true, eventBroker.IsSubscribed("tag", listenerOne));
#pragma warning disable 618
            eventBroker.UnsubscribeByMessageTypeName(EventBroker.getCompositeMessageType(typeof(TypeOne).ToString(),"tag"), listenerOne);
#pragma warning restore 618
            Assert.AreEqual(false, eventBroker.IsSubscribed("tag", listenerOne));
        }
        
        /// <summary>
        /// When a message is sent to the "legit" event broker the eavesdropping one also receives it 
        /// </summary>
        [Test]
        public void TestBothBrokersGetsACopyOfMessages() {
            var legitEventBroker = new EventBroker();
            var eavesdropperEventBroker = new EventBroker();
            
            var eventBroker = new EventEavesdropper(legitEventBroker, eavesdropperEventBroker);

            var receivedOnLegit = false;
            var receivedOnEavesdropper = false;

            Action<TypeOne> legitListener = message => { receivedOnLegit = true; };
            Action<TypeOne> eavesdroppingListener = message => { receivedOnEavesdropper = true; };
            legitEventBroker.Subscribe(legitListener);
            eavesdropperEventBroker.Subscribe(eavesdroppingListener);
            eventBroker.Publish(new TypeOne());
            Assert.AreEqual(1, eventBroker.Count<TypeOne>());
            Assert.AreEqual(true, receivedOnLegit);
            Assert.AreEqual(true, receivedOnEavesdropper);
        }
        
        /// <summary>
        /// When a tagged message is sent to the "legit" event broker the eavesdropping one also receives it 
        /// </summary>
        [Test]
        public void TestBothBrokersGetsACopyOfTaggedMessages() {
            var legitEventBroker = new EventBroker();
            var eavesdropperEventBroker = new EventBroker();
            
            var eventBroker = new EventEavesdropper(legitEventBroker, eavesdropperEventBroker);

            var receivedOnLegit = false;
            var receivedOnEavesdropper = false;

            Action<TypeOne> legitListener = message => { receivedOnLegit = true; };
            Action<TypeOne> eavesdroppingListener = message => { receivedOnEavesdropper = true; };
            legitEventBroker.Subscribe("tag", legitListener);
            eavesdropperEventBroker.Subscribe("tag", eavesdroppingListener);
            eventBroker.Publish("tag", new TypeOne());
            Assert.AreEqual(1, eventBroker.Count<TypeOne>("tag"));
            Assert.AreEqual(true, receivedOnLegit);
            Assert.AreEqual(true, receivedOnEavesdropper);
        }
        
        /// <summary>
        /// Whe a message is sent to an eavesdropping event broker, the message is not relayed to the legit one
        /// </summary>
        [Test]
        public void TestPublishingOnSecondaryDoesNotPropagatesToPrimary() {
            var legitEventBroker = new EventBroker();
            var eavesdroppingEventBroker = new EventBroker();
            var eventBroker = new EventEavesdropper(legitEventBroker, eavesdroppingEventBroker);

            var receivedOnLegit = false;
            var receivedOnEavesdropper = false;

            Action<TypeOne> listenerPrimary = message => { receivedOnLegit = true; };
            Action<TypeOne> listenerSecondary = message => { receivedOnEavesdropper = true; };
            legitEventBroker.Subscribe(listenerPrimary);
            eavesdroppingEventBroker.Subscribe(listenerSecondary);
            eavesdroppingEventBroker.Publish(new TypeOne());
            Assert.AreEqual(1, eventBroker.Count<TypeOne>());
            Assert.AreEqual(false, receivedOnLegit);
            Assert.AreEqual(true, receivedOnEavesdropper);
        }
        
        /// <summary>
        /// When a tagged message is sent to an eavesdropping event broker, the message is not relayed to the legit one
        /// </summary>
        [Test]
        public void TestTaggedPublishingOnSecondaryDoesNotPropagatesToPrimary() {
            var legitEventBroker = new EventBroker();
            var eavesdroppingEventBroker = new EventBroker();
            var eventBroker = new EventEavesdropper(legitEventBroker, eavesdroppingEventBroker);

            var receivedOnLegit = false;
            var receivedOnEavesdropper = false;

            Action<TypeOne> listenerPrimary = message => { receivedOnLegit = true; };
            Action<TypeOne> listenerSecondary = message => { receivedOnEavesdropper = true; };
            legitEventBroker.Subscribe("tag", listenerPrimary);
            eavesdroppingEventBroker.Subscribe("tag", listenerSecondary);
            eavesdroppingEventBroker.Publish("tag", new TypeOne());
            Assert.AreEqual(1, eventBroker.Count<TypeOne>("tag"));
            Assert.AreEqual(false, receivedOnLegit);
            Assert.AreEqual(true, receivedOnEavesdropper);
        }
        
        /// <summary>
        /// An eavesdropping event broker with no name, it's name is an empty string
        /// </summary>
        [Test]
        public void AnEavesdroppingEventBrokerWithNoNameIsEmptyString()
        {
            var legitEventBroker = new EventBroker();
            var eavesdroppingEventBroker = new EventBroker();
            var eventBroker = new EventEavesdropper(legitEventBroker, eavesdroppingEventBroker);
            
            Assert.AreEqual("", eventBroker.Name);
        }

        /// <summary>
        /// An eavesdropping event broker can have a name
        /// </summary>
        [Test]
        public void AnEavesdroppingEventBrokerCanHaveAName()
        {
            var legitEventBroker = new EventBroker();
            var eavesdroppingEventBroker = new EventBroker();
            var eventBroker = new EventEavesdropper("Name", legitEventBroker, eavesdroppingEventBroker);
            
            Assert.AreEqual("Name", eventBroker.Name);
        }
        
        /// <summary>
        /// An eavesdropping event broker can be disposed, no event should be unsubscribed from legit event broker
        /// </summary>
        [Test]
        public void AnEavesdroppingEventBrokerCanBeDisposed()
        {
            var legitEventBroker = new EventBroker();
            var eavesdroppingEventBroker = new EventBroker();
            var eventBroker = new EventEavesdropper("Name", legitEventBroker, eavesdroppingEventBroker);
            
            Action<TypeOne> listener = message => { };
            
            eventBroker.Subscribe(listener);
            
            Assert.AreEqual(1, legitEventBroker.Count());
            eventBroker.Dispose();
            Assert.AreEqual(1, legitEventBroker.Count());
        }
        
        /// <summary>
        /// An eavesdropping event broker can relay an event to another event broker
        /// </summary>
        [Test]
        public void AnEavesdroppingEventBrokerCanRelayAnEvent()
        {
            var legitEventBroker = new EventBroker();
            var eavesdroppingEventBroker = new EventBroker();
            var eventBroker = new EventEavesdropper("Name", legitEventBroker, eavesdroppingEventBroker);
            var receiverEventBroker = new EventBroker();

            var received = false;
            
            eventBroker.RelayTo(receiverEventBroker);
            
            Action<TypeOne> listener = message => { received = true; };
            
            receiverEventBroker.Subscribe(listener);
            
            eventBroker.Publish(new TypeOne());
            Assert.IsTrue(received);
        }
        
        /// <summary>
        /// An eavesdropping event broker can relay a tagged event to another event broker
        /// </summary>
        [Test]
        public void AnEavesdroppingEventBrokerCanRelayATaggedEvent()
        {
            var legitEventBroker = new EventBroker();
            var eavesdroppingEventBroker = new EventBroker();
            var eventBroker = new EventEavesdropper("Name", legitEventBroker, eavesdroppingEventBroker);
            var receiverEventBroker = new EventBroker();

            var received = false;
            
            eventBroker.RelayTo(receiverEventBroker);
            
            Action<TypeOne> listener = message => { received = true; };
            
            receiverEventBroker.Subscribe("tag", listener);
            
            eventBroker.Publish("tag", new TypeOne());
            Assert.IsTrue(received);
        }
        
        /// <summary>
        /// An eavesdropping event broker can stop relaying relay an event to another event broker
        /// </summary>
        [Test]
        public void AnEavesdroppingEventBrokerCanStopRelaying()
        {
            var legitEventBroker = new EventBroker();
            var eavesdroppingEventBroker = new EventBroker();
            var eventBroker = new EventEavesdropper("Name", legitEventBroker, eavesdroppingEventBroker);
            var receiverEventBroker = new EventBroker();

            var received = false;
            
            eventBroker.RelayTo(receiverEventBroker);
            eventBroker.DoNotRelayTo(receiverEventBroker);
            
            Action<TypeOne> listener = message => { received = true; };
            
            receiverEventBroker.Subscribe(listener);
            
            eventBroker.Publish(new TypeOne());
            Assert.IsFalse(received);
        }
        
        /// <summary>
        /// An eavesdropping event broker can stop relaying relay a tagged event to another event broker
        /// </summary>
        [Test]
        public void AnEavesdroppingEventBrokerCanStopRelayingTaggedOnes()
        {
            var legitEventBroker = new EventBroker();
            var eavesdroppingEventBroker = new EventBroker();
            var eventBroker = new EventEavesdropper("Name", legitEventBroker, eavesdroppingEventBroker);
            var receiverEventBroker = new EventBroker();

            var received = false;
            
            eventBroker.RelayTo(receiverEventBroker);
            eventBroker.DoNotRelayTo<TypeOne>("tag", receiverEventBroker);
            
            Action<TypeOne> listener = message => { received = true; };
            
            receiverEventBroker.Subscribe("tag", listener);
            
            eventBroker.Publish("tag", new TypeOne());
            Assert.IsFalse(received);
        }
        
        /// <summary>
        /// An eavesdropping event broker can stop relaying a specific event type relay an event to another event broker
        /// </summary>
        [Test]
        // ReSharper disable once InconsistentNaming
        public void AnEavesdroppingEventBrokerCanStopRelayingByType()
        {
            var legitEventBroker = new EventBroker();
            var eavesdroppingEventBroker = new EventBroker();
            var eventBroker = new EventEavesdropper("Name", legitEventBroker, eavesdroppingEventBroker);
            var receiverEventBroker = new EventBroker();

            var receivedOne = false;
            var receivedTwo = false;
            
            eventBroker.RelayTo(receiverEventBroker);
            eventBroker.DoNotRelayTo<TypeTwo>(receiverEventBroker);
            
            Action<TypeOne> listenerOne = message => { receivedOne = true; };
            Action<TypeTwo> listenerTwo = message => { receivedTwo = true; };
            
            receiverEventBroker.Subscribe(listenerOne);
            receiverEventBroker.Subscribe(listenerTwo);
            
            eventBroker.Publish(new TypeOne());
            Assert.IsTrue(receivedOne);
            Assert.IsFalse(receivedTwo);
        }
        
        /// <summary>
        /// An eavesdropping event broker can stop relaying a specific tagged event type relay an event to another event broker
        /// </summary>
        [Test]
        // ReSharper disable once InconsistentNaming
        public void AnEavesdroppingEventBrokerCanStopRelayingByTaggedType()
        {
            var legitEventBroker = new EventBroker();
            var eavesdroppingEventBroker = new EventBroker();
            var eventBroker = new EventEavesdropper("Name", legitEventBroker, eavesdroppingEventBroker);
            var receiverEventBroker = new EventBroker();

            var receivedOne   = false;
            var receivedTwo   = false;
            var receivedThree = false;
            
            eventBroker.RelayTo(receiverEventBroker);
            eventBroker.DoNotRelayTo<TypeTwo>(receiverEventBroker);
            
            Action<TypeOne> listenerOne   = message => { receivedOne  = true; };
            Action<TypeTwo> listenerTwo   = message => { receivedTwo  = true; };
            Action<TypeTwo> listenerThree = message => { receivedThree = true; };
            
            receiverEventBroker.Subscribe(listenerOne);
            receiverEventBroker.Subscribe(listenerTwo);
            receiverEventBroker.Subscribe("tag", listenerThree);
            
            eventBroker.Publish(new TypeOne());
            Assert.IsTrue(receivedOne);
            Assert.IsFalse(receivedTwo);
            Assert.IsFalse(receivedThree);
            
            receivedOne   = false;
            receivedTwo   = false;
            receivedThree = false;

            receiverEventBroker.Dispose();
            
            receiverEventBroker.Subscribe(listenerOne);
            receiverEventBroker.Subscribe(listenerTwo);
            receiverEventBroker.Subscribe("tag", listenerThree);
            
            eventBroker.Publish("tag", new TypeOne());
            Assert.IsFalse(receivedOne);
            Assert.IsFalse(receivedTwo);
            Assert.IsFalse(receivedThree);
        }
    }
}
