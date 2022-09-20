using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using TreeislandStudio.Engine.Event.Event;

namespace TreeislandStudio.Engine.Event.Editor.Test
{
    /// <summary>
    /// Tests EventBroker class
    /// </summary>
    public class EventBrokerTest {

        /// <inheritdoc />
        /// <summary>
        /// Test class TypeOne
        /// </summary>
        private class TestMessageTypeOne : EventMessage { };

        /// <inheritdoc />
        /// <summary>
        /// Test class TypeTwo
        /// </summary>
        private class TestMessageTypeTwo : EventMessage { };

        /// <summary>
        /// When a subscriber is added to the subscriber's list, it is subscribed (duh!)
        /// </summary>
        [Test]
        public void Subscribe() {
            var eventBroker = new EventBroker();
            Action<TestMessageTypeOne> listener = message => {};
            eventBroker.Subscribe(listener);
            Assert.IsTrue(eventBroker.IsSubscribed(listener));
        }
        
        /// <summary>
        /// When a subscriber is added to the subscriber's list for a tagged message, it is subscribed (duh!)
        /// </summary>
        [Test]
        public void SubscribeSingleTagged() {
            var eventBroker = new EventBroker();
            Action<TestMessageTypeOne> listener = message => {};
            eventBroker.Subscribe("tag", listener);
            Assert.IsTrue(eventBroker.IsSubscribed("tag", listener));
        }
        
        /// <summary>
        /// When a subscriber is added to the subscriber's list for a tagged message, it is not subscribed also on general channel
        /// </summary>
        [Test]
        public void SubscribeSingleTaggedNotInGeneral() {
            var eventBroker = new EventBroker();
            Action<TestMessageTypeOne> listener = message => {};
            eventBroker.Subscribe("tag", listener);
            Assert.IsFalse(eventBroker.IsSubscribed(listener));
        }
        
        /// <summary>
        /// When a subscriber is added to the subscriber's list for tagged messages, it is subscribed (duh!)
        /// </summary>
        [Test]
        public void SubscribeTagged() {
            var eventBroker = new EventBroker();
            Action<TestMessageTypeOne> listener = message => {};
            var tags = new List<string>() { "tag" };
            eventBroker.Subscribe(tags, listener);
            Assert.IsTrue(eventBroker.IsSubscribed("tag", listener));
        }
        
        /// <summary>
        /// When a subscriber is added to the subscriber's list for multiple tagged messages, it is subscribed (duh!)
        /// </summary>
        [Test]
        public void SubscribeTaggedMultiple() {
            var eventBroker = new EventBroker();
            Action<TestMessageTypeOne> listener = message => {};
            var tags = new List<string>() { "tagA", "tagB" };
            eventBroker.Subscribe(tags, listener);
            Assert.IsTrue(eventBroker.IsSubscribed("tagA", listener));
            Assert.IsTrue(eventBroker.IsSubscribed("tagB", listener));
        }
        
        /// <summary>
        /// When a subscriber is added to the subscriber's list for multiple tagged messages, it is subscribed but not for others
        /// </summary>
        [Test]
        public void SubscribeTaggedMultipleNotOthers() {
            var eventBroker = new EventBroker();
            Action<TestMessageTypeOne> listenerA = message => {};
            Action<TestMessageTypeOne> listenerB = message => {};
            var tags = new List<string>() { "not this one" };
            eventBroker.Subscribe(tags, listenerA);
            eventBroker.Subscribe("just this", listenerB);
            Assert.IsTrue(eventBroker.IsSubscribed("just this", listenerB));
            Assert.IsFalse(eventBroker.IsSubscribed("not this one", listenerB));
        }

        /// <summary>
        /// A subscriber that is not added to the subscriber's list, is not subscribed
        /// </summary>
        [Test]
        public void NotSubscribe() {
            var eventBroker = new EventBroker();
            Action<TestMessageTypeOne> listener = message => { };
            Assert.False(eventBroker.IsSubscribed(listener));
        }
        
        /// <summary>
        /// A subscriber that is not added to the subscriber's list, is not subscribed, even with tags
        /// </summary>
        [Test]
        public void NotSubscribeTest() {
            var eventBroker = new EventBroker();
            Action<TestMessageTypeOne> listener = message => { };
            Assert.False(eventBroker.IsSubscribed(listener));
        }

        /// <summary>
        /// When a subscriber is added to the its subscriber's list it is counted whe asking for the subscribers count
        /// </summary>
        [Test]
        public void SubscribeWithType() {
            var eventBroker = new EventBroker();
            Action<TestMessageTypeOne> listener = message => { };
            eventBroker.Subscribe(listener);
            Assert.AreEqual(1, eventBroker.Count<TestMessageTypeOne>());
            Assert.AreEqual(0, eventBroker.Count<TestMessageTypeTwo>());
        }
        
        /// <summary>
        /// When a subscriber is added to the its subscriber's list with tag it is counted whe asking for the subscribers count (not as untagged)
        /// </summary>
        [Test]
        public void SubscribeWithTypeAndTag() {
            var eventBroker = new EventBroker();
            Action<TestMessageTypeOne> listener = message => { };
            eventBroker.Subscribe("tag", listener);
            Assert.AreEqual(1, eventBroker.Count<TestMessageTypeOne>("tag"));
            Assert.AreEqual(0, eventBroker.Count<TestMessageTypeOne>());
            Assert.AreEqual(0, eventBroker.Count<TestMessageTypeTwo>("tag"));
        }

        /// <summary>
        /// When a message is published and has a subscribed, it receives the message
        /// </summary>
        [Test]
        public void Publish() {
            var eventBroker = new EventBroker();
            var received = false;
            Action<TestMessageTypeOne> listener = message => { received = true; };
            eventBroker.Subscribe(listener);
            eventBroker.Publish(new TestMessageTypeOne());
            Assert.AreEqual(1, eventBroker.Count<TestMessageTypeOne>());
            Assert.IsTrue(received);
        }
        
        /// <summary>
        /// When a tagged message is published and has a subscribed, it receives the message
        /// </summary>
        [Test]
        public void PublishTagged() {
            var eventBroker = new EventBroker();
            var received = false;
            Action<TestMessageTypeOne> listener = message => { received = true; };
            eventBroker.Subscribe("tag", listener);
            eventBroker.Publish("tag", new TestMessageTypeOne());
            Assert.AreEqual(1, eventBroker.Count<TestMessageTypeOne>("tag"));
            Assert.IsTrue(received);
        }

        /// <summary>
        /// When a tagged message is published and has subscribers, only them receives the message
        /// </summary>
        [Test]
        public void PublishTaggedCheckNotCrossesTypes() {
            var eventBroker = new EventBroker();
            var receivedOne = false;
            var receivedTwo = false;

            Action<TestMessageTypeOne> listenerOne = message => { receivedOne = true; };
            Action<TestMessageTypeTwo> listenerTwo = message => { receivedTwo = true; };

            eventBroker.Subscribe("one", listenerOne);
            eventBroker.Subscribe("two", listenerTwo);
            eventBroker.Publish("one", new TestMessageTypeOne());
            Assert.IsTrue(receivedOne);
            Assert.IsFalse(receivedTwo);

            receivedOne = false;
            receivedTwo = false;

            eventBroker.Publish("two", new TestMessageTypeTwo());
            Assert.IsFalse(receivedOne);
            Assert.IsTrue(receivedTwo);
        }
        
        /// <summary>
        /// When a tagged message is published and has subscribers, only them receives the message (not between channels)
        /// </summary>
        [Test]
        public void PublishTaggedCheckNotCrossesTags() {
            var eventBroker = new EventBroker();
            var receivedOne = false;
            var receivedTwo = false;

            Action<TestMessageTypeOne> listenerOne = message => { receivedOne = true; };
            Action<TestMessageTypeOne> listenerTwo = message => { receivedTwo = true; };

            eventBroker.Subscribe("one", listenerOne);
            eventBroker.Subscribe("two", listenerTwo);
            eventBroker.Publish("one", new TestMessageTypeOne());
            Assert.IsTrue(receivedOne);
            Assert.IsFalse(receivedTwo);

            receivedOne = false;
            receivedTwo = false;

            eventBroker.Publish("two", new TestMessageTypeOne());
            Assert.IsFalse(receivedOne);
            Assert.IsTrue(receivedTwo);
        }
        
        /// <summary>
        /// When a tagged message is published and has subscribers, only them receives the message (not between tagged & untagged ones)
        /// </summary>
        [Test]
        public void PublishTaggedCheckNotCrossesTagsUntagged() {
            var eventBroker = new EventBroker();
            var receivedOne = false;
            var receivedTwo = false;

            Action<TestMessageTypeOne> listenerOne = message => { receivedOne = true; };
            Action<TestMessageTypeOne> listenerTwo = message => { receivedTwo = true; };

            eventBroker.Subscribe("one", listenerOne);
            eventBroker.Subscribe(listenerTwo);
            eventBroker.Publish("one", new TestMessageTypeOne());
            Assert.IsTrue(receivedOne);
            Assert.IsFalse(receivedTwo);

            receivedOne = false;
            receivedTwo = false;

            eventBroker.Publish(new TestMessageTypeOne());
            Assert.IsFalse(receivedOne);
            Assert.IsTrue(receivedTwo);
        }

        /// <summary>
        /// When we add two subscribers to a message, its subscribers count is two
        /// </summary>
        [Test]
        public void Count() {
            var eventBroker = new EventBroker();

            Action<TestMessageTypeOne> listenerOne = message => { };
            Action<TestMessageTypeOne> listenerTwo = message => { };

            eventBroker.Subscribe(listenerOne);
            eventBroker.Subscribe(listenerTwo);
            Assert.AreEqual(2, eventBroker.Count<TestMessageTypeOne>());
        }
        
        /// <summary>
        /// When we add two subscribers to a tagged message, its subscribers count is two
        /// </summary>
        [Test]
        public void CountTagged() {
            var eventBroker = new EventBroker();

            Action<TestMessageTypeOne> listenerOne = message => { };
            Action<TestMessageTypeOne> listenerTwo = message => { };

            eventBroker.Subscribe("tag", listenerOne);
            eventBroker.Subscribe("tag", listenerTwo);
            Assert.AreEqual(2, eventBroker.Count<TestMessageTypeOne>("tag"));
        }

        /// <summary>
        /// When we unsubscribe a listener, its no longer subscribed
        /// </summary>
        [Test]
        public void UnsubscribeSubscriber() {
            var eventBroker = new EventBroker();
            Action<TestMessageTypeOne> listenerOne = message => {};

            eventBroker.Subscribe(listenerOne);
            Assert.IsTrue(eventBroker.IsSubscribed(listenerOne));

            eventBroker.UnSubscribe(listenerOne);
            Assert.IsFalse(eventBroker.IsSubscribed(listenerOne));
        }
        
        /// <summary>
        /// When we unsubscribe a listener by type name, its no longer subscribed
        /// </summary>
        [Test]
        public void UnsubscribeSubscriberByTypeName() {
            var eventBroker = new EventBroker();
            Action<TestMessageTypeOne> listenerOne = message => {};

            object untypedListener = listenerOne;

            eventBroker.Subscribe(listenerOne);
            Assert.IsTrue(eventBroker.IsSubscribed(listenerOne));
            Assert.AreEqual(1, eventBroker.Count());
            
            eventBroker.UnsubscribeByMessageTypeName(typeof(TestMessageTypeOne).ToString(), untypedListener);
            Assert.IsFalse(eventBroker.IsSubscribed(listenerOne));
            Assert.AreEqual(0, eventBroker.Count());
        }
        
        /// <summary>
        /// When we unsubscribe a tagged listener, it's no longer subscribed
        /// </summary>
        [Test]
        public void UnsubscribeTaggedSubscriber() {
            var eventBroker = new EventBroker();
            Action<TestMessageTypeOne> listenerOne = message => {};

            eventBroker.Subscribe("tag", listenerOne);
            Assert.IsTrue(eventBroker.IsSubscribed("tag", listenerOne));

            eventBroker.UnSubscribe("tag", listenerOne);
            Assert.IsFalse(eventBroker.IsSubscribed("tag", listenerOne));
        }
        
        /// <summary>
        /// When we unsubscribe a tagged listener to multiple tags, it's no longer subscribed
        /// </summary>
        [Test]
        public void UnsubscribeMultipleTaggedSubscriber() {
            var eventBroker = new EventBroker();
            Action<TestMessageTypeOne> listenerOne = message => {};

            eventBroker.Subscribe("tag", listenerOne);
            eventBroker.Subscribe("other tag", listenerOne);
            Assert.IsTrue(eventBroker.IsSubscribed("tag", listenerOne));
            Assert.IsTrue(eventBroker.IsSubscribed("other tag", listenerOne));

            eventBroker.UnSubscribe("tag", listenerOne);
            Assert.IsFalse(eventBroker.IsSubscribed("tag", listenerOne));
            Assert.IsTrue(eventBroker.IsSubscribed("other tag", listenerOne));

            eventBroker.UnSubscribe("other tag", listenerOne);
            Assert.IsFalse(eventBroker.IsSubscribed("tag", listenerOne));
            Assert.IsFalse(eventBroker.IsSubscribed("other tag", listenerOne));
        }
        
        /// <summary>
        /// When we unsubscribe a tagged listener to multiple tags at once, it's no longer subscribed
        /// </summary>
        [Test]
        public void UnsubscribeMultipleTaggedSubscriberAtOnce() {
            var eventBroker = new EventBroker();
            Action<TestMessageTypeOne> listenerOne = message => {};

            eventBroker.Subscribe("tag", listenerOne);
            eventBroker.Subscribe("other tag", listenerOne);
            Assert.IsTrue(eventBroker.IsSubscribed("tag", listenerOne));
            Assert.IsTrue(eventBroker.IsSubscribed("other tag", listenerOne));

            eventBroker.UnSubscribe(new List<string>() {"tag" , "other tag"}, listenerOne);
            Assert.IsFalse(eventBroker.IsSubscribed("tag", listenerOne));
            Assert.IsFalse(eventBroker.IsSubscribed("other tag", listenerOne));
        }
        
        /// <summary>
        /// When we unsubscribe a tagged listener using a different tag, it continues to be subscribed
        /// </summary>
        [Test]
        public void UnsubscribeWrongTaggedSubscriber() {
            var eventBroker = new EventBroker();
            Action<TestMessageTypeOne> listenerOne = message => {};

            eventBroker.Subscribe("tag", listenerOne);
            Assert.IsTrue(eventBroker.IsSubscribed("tag", listenerOne));

            eventBroker.UnSubscribe("tag wrong", listenerOne);
            Assert.IsTrue(eventBroker.IsSubscribed("tag", listenerOne));
        }

        /// <summary>
        /// An event broker has a name that can be set at construction time
        /// </summary>
        [Test]
        public void HaveAName()
        {
            var eventBroker = new EventBroker("name");
            Assert.AreEqual("name", eventBroker.Name);
        }

        /// <summary>
        /// When an event broker all subscribers are freed
        /// </summary>
        [Test]
        public void WhenDisposedTheSubscribersAreFreed()
        {
            var eventBroker = new EventBroker();
            Action<TestMessageTypeOne> listenerOne = message => {};

            eventBroker.Subscribe(listenerOne);
            
            Assert.AreEqual(1, eventBroker.Count());
            
            eventBroker.Dispose();
            Assert.AreEqual(0, eventBroker.Count());
        }
        
        /// <summary>
        /// When we unsubscribe a listener by tag, it is subscribed to untagged messages
        /// </summary>
        [Test]
        public void UnsubscribeSubscriberUntagged() {
            var eventBroker = new EventBroker();
            Action<TestMessageTypeOne> listenerOne = message => {};

            eventBroker.Subscribe(listenerOne);
            Assert.IsTrue(eventBroker.IsSubscribed(listenerOne));

            eventBroker.UnSubscribe("tag", listenerOne);
            Assert.IsTrue(eventBroker.IsSubscribed(listenerOne));
        }
        
        /// <summary>
        /// When we unsubscribe an untagged listener, it is subscribed to tagged messages
        /// </summary>
        [Test]
        public void UnsubscribeSubscriberTagged() {
            var eventBroker = new EventBroker();
            Action<TestMessageTypeOne> listenerOne = message => {};

            eventBroker.Subscribe("tag", listenerOne);
            Assert.IsTrue(eventBroker.IsSubscribed("tag", listenerOne));

            eventBroker.UnSubscribe(listenerOne);
            Assert.IsTrue(eventBroker.IsSubscribed("tag", listenerOne));
        }

        /// <summary>
        /// We can remove a listener from the event broker ty its message type name, instead of using the type itself
        /// </summary>
        [Test]
        public void RemoveSubscriberByTypeName() {
            var eventBroker = new EventBroker();
            Action<TestMessageTypeOne> listenerOne = message => { };

            eventBroker.Subscribe(listenerOne);
            Assert.IsTrue(eventBroker.IsSubscribed(listenerOne));
#pragma warning disable 618
            eventBroker.UnsubscribeByMessageTypeName(typeof(TestMessageTypeOne).ToString(), listenerOne);
#pragma warning restore 618
            Assert.IsFalse(eventBroker.IsSubscribed(listenerOne));
        }

        /// <summary>
        /// When disposing an event broker, the subscriber list is emptied
        /// </summary>
        [Test]
        public void WhenDisposingTheEventBrokerSubscribersListIsEmptied()
        {
            var eventBroker = new EventBroker();
            
            eventBroker.Subscribe<TestMessageTypeOne>((message) => { });
            eventBroker.Subscribe<TestMessageTypeTwo>((message) => { });
            
            Assert.AreEqual(2, eventBroker.Count());
            eventBroker.Dispose();
            Assert.AreEqual(0, eventBroker.Count());
        }

        /// <summary>
        /// When an event broker publishes a message, it got sent also to all its peers
        /// </summary>
        [Test]
        public void WhenEventBrokerPublishAMessageItDoesItToItsPeers()
        {
            var eventBroker = new EventBroker();
            var peerEventBroker = new EventBroker();
            
            eventBroker.RelayTo(peerEventBroker);

            var receivedByMain = false;
            var receivedByPeer = false;
            
            eventBroker.Subscribe<TestMessageTypeOne>((message) => { receivedByMain = true; });
            peerEventBroker.Subscribe<TestMessageTypeOne>((message) => { receivedByPeer = true; });
            
            eventBroker.Publish(new TestMessageTypeOne());
            
            Assert.IsTrue(receivedByMain);
            Assert.IsTrue(receivedByPeer);
        }
        
        /// <summary>
        /// When an event broker publishes a tagged message, it got sent also to all its peers
        /// </summary>
        [Test]
        public void WhenEventBrokerPublishATaggedMessageItDoesItToItsPeers()
        {
            var eventBroker = new EventBroker();
            var peerEventBroker = new EventBroker();
            
            eventBroker.RelayTo(peerEventBroker);

            var receivedByMain = false;
            var receivedByPeer = false;
            
            eventBroker.Subscribe<TestMessageTypeOne>("tag", (message) => { receivedByMain = true; });
            peerEventBroker.Subscribe<TestMessageTypeOne>("tag", (message) => { receivedByPeer = true; });
            
            eventBroker.Publish("tag", new TestMessageTypeOne());
            
            Assert.IsTrue(receivedByMain);
            Assert.IsTrue(receivedByPeer);
        }
        
        /// <summary>
        /// When an event broker publishes a tagged message, it got sent also to all its peers but not received by untagged listeners
        /// </summary>
        [Test]
        public void WhenEventBrokerPublishATaggedMessageItDoesItToItsPeersTagSurvives()
        {
            var eventBroker = new EventBroker();
            var peerEventBroker = new EventBroker();
            
            eventBroker.RelayTo(peerEventBroker);

            var receivedByMain = false;
            var receivedByPeer = false;
            var receivedByUntaggedPeer = false;
            
            eventBroker.Subscribe<TestMessageTypeOne>("tag", (message) => { receivedByMain = true; });
            peerEventBroker.Subscribe<TestMessageTypeOne>("tag", (message) => { receivedByPeer = true; });
            peerEventBroker.Subscribe<TestMessageTypeOne>((message) => { receivedByUntaggedPeer = true; });
            
            eventBroker.Publish("tag", new TestMessageTypeOne());
            
            Assert.IsTrue(receivedByMain);
            Assert.IsTrue(receivedByPeer);
            Assert.IsFalse(receivedByUntaggedPeer);
        }
        
        /// <summary>
        /// When an event broker publishes a message, it got sent also to all its peers but is not received back (dead lock)
        /// </summary>
        [Test]
        public void WhenEventBrokerPublishAMessageToItsPeersItIsNotGetBack()
        {
            var eventBroker = new EventBroker();
            var peerEventBroker = new EventBroker();
            
            eventBroker.RelayTo(peerEventBroker);
            peerEventBroker.RelayTo(eventBroker);

            var receivedByMain = false;
            var receivedByPeer = false;
            
            eventBroker.Subscribe<TestMessageTypeOne>((message) => { receivedByMain = true; });
            peerEventBroker.Subscribe<TestMessageTypeOne>((message) => { receivedByPeer = true; });
            
            eventBroker.Publish(new TestMessageTypeOne());
            
            Assert.IsTrue(receivedByMain);
            Assert.IsTrue(receivedByPeer);
        }
        
        /// <summary>
        /// When an event broker is added as peer twice, the second one is ignored
        /// </summary>
        [Test]
        public void WhenEventBrokerPeerTwiceSecondOneIgnored()
        {
            var eventBroker = new EventBroker();
            var peerEventBroker = new EventBroker();
            
            eventBroker.RelayTo(peerEventBroker);
            eventBroker.RelayTo(peerEventBroker);

            var receivedByPeer = 0;
            
            peerEventBroker.Subscribe<TestMessageTypeOne>((message) => { receivedByPeer++; });
            
            eventBroker.Publish(new TestMessageTypeOne());
            
            Assert.AreEqual(1, receivedByPeer);
        }
        
        /// <summary>
        /// When an event broker is removed as peer, it does not received the messages any longer
        /// </summary>
        [Test]
        public void WhenEventBrokerRemovesPeerDoesNotReceiveMessages()
        {
            var eventBroker = new EventBroker();
            var peerEventBroker = new EventBroker();
            
            eventBroker.RelayTo(peerEventBroker);

            var receivedByPeer = false;
            
            peerEventBroker.Subscribe<TestMessageTypeOne>((message) => { receivedByPeer = true; });
            
            eventBroker.Publish(new TestMessageTypeOne());
            
            Assert.IsTrue(receivedByPeer);

            receivedByPeer = false;
            
            eventBroker.DoNotRelayTo(peerEventBroker);
            eventBroker.Publish(new TestMessageTypeOne());
            
            Assert.IsFalse(receivedByPeer);
        }
        
        /// <summary>
        /// When an event broker is removed as peer, it does not received the messages any longer even if it is tagged
        /// </summary>
        [Test]
        public void WhenEventBrokerRemovesPeerDoesNotReceiveMessagesEvenTagged()
        {
            var eventBroker = new EventBroker();
            var peerEventBroker = new EventBroker();
            
            eventBroker.RelayTo(peerEventBroker);

            var receivedByPeer = false;
            
            peerEventBroker.Subscribe<TestMessageTypeOne>("tag", (message) => { receivedByPeer = true; });
            
            eventBroker.Publish("tag", new TestMessageTypeOne());
            
            Assert.IsTrue(receivedByPeer);

            receivedByPeer = false;
            
            eventBroker.DoNotRelayTo(peerEventBroker);
            eventBroker.Publish("tag", new TestMessageTypeOne());
            
            Assert.IsFalse(receivedByPeer);
        }
        
        /// <summary>
        /// When an event broker is removed as peer, it does not received the messages any longer. If removed
        /// twice, the second is ignored (has no effect)
        /// </summary>
        [Test]
        public void WhenEventBrokerRemovesPeerTwiceHasNoEffect()
        {
            var eventBroker = new EventBroker();
            var peerEventBroker = new EventBroker();
            
            eventBroker.RelayTo(peerEventBroker);

            var receivedByPeer = false;
            
            peerEventBroker.Subscribe<TestMessageTypeOne>((message) => { receivedByPeer = true; });
            
            eventBroker.Publish(new TestMessageTypeOne());
            
            Assert.IsTrue(receivedByPeer);

            receivedByPeer = false;
            
            eventBroker.DoNotRelayTo(peerEventBroker);
            eventBroker.DoNotRelayTo(peerEventBroker);
            
            eventBroker.Publish(new TestMessageTypeOne());
            
            Assert.IsFalse(receivedByPeer);
        }
              
        /// <summary>
        /// When an event broker does not want a particular message type, it does not received that messages any longer
        /// but all the others are received
        /// </summary>
        [Test]
        public void WhenEventBrokerRemovesPeerForATypeDoesNotReceiveMessages()
        {
            var eventBroker = new EventBroker();
            var peerEventBroker = new EventBroker();
            
            eventBroker.RelayTo(peerEventBroker);

            var receivedOne = false;
            var receivedTwo = false;
            
            peerEventBroker.Subscribe<TestMessageTypeOne>((message) => { receivedOne = true; });
            peerEventBroker.Subscribe<TestMessageTypeTwo>((message) => { receivedTwo = true; });
            
            eventBroker.Publish(new TestMessageTypeOne());
            eventBroker.Publish(new TestMessageTypeTwo());
            
            Assert.IsTrue(receivedOne);
            Assert.IsTrue(receivedTwo);

            receivedOne = false;
            receivedTwo = false;
            
            eventBroker.DoNotRelayTo<TestMessageTypeOne>(peerEventBroker);
            
            eventBroker.Publish(new TestMessageTypeOne());
            eventBroker.Publish(new TestMessageTypeTwo());
            
            Assert.IsFalse(receivedOne);
            Assert.IsTrue(receivedTwo);
        }
        
        /// <summary>
        /// When an event broker does not want a particular message type, it does not received that messages any longer even when tagged
        /// but all the others are received
        /// </summary>
        [Test]
        public void WhenEventBrokerRemovesPeerForATypeDoesNotReceiveMessagesTagged()
        {
            var eventBroker = new EventBroker();
            var peerEventBroker = new EventBroker();
            
            eventBroker.RelayTo(peerEventBroker);

            var receivedOne = false;
            var receivedTwo = false;
            
            peerEventBroker.Subscribe<TestMessageTypeOne>("tag", (message) => { receivedOne = true; });
            peerEventBroker.Subscribe<TestMessageTypeTwo>("tag", (message) => { receivedTwo = true; });
            
            eventBroker.Publish("tag", new TestMessageTypeOne());
            eventBroker.Publish("tag", new TestMessageTypeTwo());
            
            Assert.IsTrue(receivedOne);
            Assert.IsTrue(receivedTwo);

            receivedOne = false;
            receivedTwo = false;
            
            eventBroker.DoNotRelayTo<TestMessageTypeOne>("tag", peerEventBroker);
            
            eventBroker.Publish("tag", new TestMessageTypeOne());
            eventBroker.Publish("tag", new TestMessageTypeTwo());
            
            Assert.IsFalse(receivedOne);
            Assert.IsTrue(receivedTwo);
        }
        
        /// <summary>
        /// When an event broker is instructed to not relays a tagged message type twice, the second one is ignored and no error is issued and works
        /// just fine
        /// </summary>
        [Test]
        public void WhenEventBrokerDoNotRelaysAMessageTwiceItIsIgnored()
        {
            var eventBroker = new EventBroker();
            var peerEventBroker = new EventBroker();
            
            eventBroker.RelayTo(peerEventBroker);

            var receivedOne = false;
            var receivedTwo = false;
            
            peerEventBroker.Subscribe<TestMessageTypeOne>("tag", (message) => { receivedOne = true; });
            peerEventBroker.Subscribe<TestMessageTypeTwo>("tag", (message) => { receivedTwo = true; });
            
            eventBroker.Publish("tag", new TestMessageTypeOne());
            eventBroker.Publish("tag", new TestMessageTypeTwo());
            
            Assert.IsTrue(receivedOne);
            Assert.IsTrue(receivedTwo);

            receivedOne = false;
            receivedTwo = false;
            
            eventBroker.DoNotRelayTo<TestMessageTypeOne>("tag", peerEventBroker);
            eventBroker.DoNotRelayTo<TestMessageTypeOne>("tag", peerEventBroker);
            
            eventBroker.Publish("tag", new TestMessageTypeOne());
            eventBroker.Publish("tag", new TestMessageTypeTwo());
            
            Assert.IsFalse(receivedOne);
            Assert.IsTrue(receivedTwo);
        }
        
        /// <summary>
        /// When an event broker does not want a particular message type, it does not received that messages any longer
        /// but all the others are received. When removing it twice, has no effect.
        /// </summary>
        [Test]
        public void WhenEventBrokerRemovesPeerForATypeTwiceDoesNothing()
        {
            var eventBroker = new EventBroker();
            var peerEventBroker = new EventBroker();
            
            eventBroker.RelayTo(peerEventBroker);

            var receivedOne = false;
            var receivedTwo = false;
            
            peerEventBroker.Subscribe<TestMessageTypeOne>((message) => { receivedOne = true; });
            peerEventBroker.Subscribe<TestMessageTypeTwo>((message) => { receivedTwo = true; });
            
            eventBroker.Publish(new TestMessageTypeOne());
            eventBroker.Publish(new TestMessageTypeTwo());
            
            Assert.IsTrue(receivedOne);
            Assert.IsTrue(receivedTwo);

            receivedOne = false;
            receivedTwo = false;
            
            eventBroker.DoNotRelayTo<TestMessageTypeOne>(peerEventBroker);
            eventBroker.DoNotRelayTo<TestMessageTypeOne>(peerEventBroker);
            
            eventBroker.Publish(new TestMessageTypeOne());
            eventBroker.Publish(new TestMessageTypeTwo());
            
            Assert.IsFalse(receivedOne);
            Assert.IsTrue(receivedTwo);
        }
        
        /// <summary>
        /// When an event broker does not want a particular message type, it does not received that messages any longer
        /// but all the others are received. When removing it twice, has no effect even when tagged.
        /// </summary>
        [Test]
        public void WhenEventBrokerRemovesPeerForATaggedTypeTwiceDoesNothing()
        {
            var eventBroker = new EventBroker();
            var peerEventBroker = new EventBroker();
            
            eventBroker.RelayTo(peerEventBroker);

            var receivedOne = false;
            var receivedTwo = false;
            
            peerEventBroker.Subscribe<TestMessageTypeOne>("tag", (message) => { receivedOne = true; });
            peerEventBroker.Subscribe<TestMessageTypeTwo>("tag", (message) => { receivedTwo = true; });
            
            eventBroker.Publish("tag", new TestMessageTypeOne());
            eventBroker.Publish("tag", new TestMessageTypeTwo());
            
            Assert.IsTrue(receivedOne);
            Assert.IsTrue(receivedTwo);

            receivedOne = false;
            receivedTwo = false;
            
            eventBroker.DoNotRelayTo<TestMessageTypeOne>("tag", peerEventBroker);
            eventBroker.DoNotRelayTo<TestMessageTypeOne>("tag", peerEventBroker);
            
            eventBroker.Publish("tag", new TestMessageTypeOne());
            eventBroker.Publish("tag", new TestMessageTypeTwo());
            
            Assert.IsFalse(receivedOne);
            Assert.IsTrue(receivedTwo);
        }

        /// <summary>
        /// When an event broker has no name, it equals to empty string
        /// </summary>
        [Test]
        public void WhenEventBrokerHasNoNameEqualsToEmptyString()
        {
            var eventBroker = new EventBroker();
            Assert.AreEqual("", eventBroker.Name);
        }
        
        /// <summary>
        /// An event broker can have a name and we can recall it
        /// </summary>
        [Test]
        public void EventBrokerCanHaveAName()
        {
            var eventBroker = new EventBroker("Name");
            Assert.AreEqual("Name", eventBroker.Name);
        }
        
    }
}
