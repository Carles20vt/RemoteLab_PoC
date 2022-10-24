using System;
using TreeislandStudio.Engine.Event;
using TreeislandStudio.Engine.StateMachine.Messages;
using UnityEngine;

namespace TreeislandStudio.Engine.StateMachine
{
    public class StateMachine
    {
        #region Public Properties
        
        public State CurrentState { get; private set; }
        
        #endregion

        #region Private Properties

        private readonly Transform senderTransform;

        #endregion
        
        #region Dependencies

        /// <summary>
        /// EventAgent
        /// </summary>
        private readonly EventAgent eventAgent;

        public StateMachine(EventAgent eventAgent, Transform senderTransform)
        {
            this.eventAgent = eventAgent;
            this.senderTransform = senderTransform;
        }

        #endregion

        public void Initialize(State startingState)
        {
            CurrentState = startingState;
            
            startingState.Enter();
            
            eventAgent.Publish(new StateMachineChanged(senderTransform, startingState));
        }

        public void ChangeState(State newState)
        {
            if (CurrentState.Equals(newState))
            {
                return;
            }
            
            CurrentState.Exit();
            CurrentState = newState;
            newState.Enter();

            eventAgent.Publish(new StateMachineChanged(senderTransform, newState));
        }
    }
}
