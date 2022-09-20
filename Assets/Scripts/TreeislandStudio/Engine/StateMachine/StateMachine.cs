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
        //private readonly EventAgent _eventAgent;

        public StateMachine(/*EventAgent eventAgent,*/ Transform senderTransform)
        {
            //_eventAgent = eventAgent;
            this.senderTransform = senderTransform;
        }

        #endregion

        public void Initialize(State startingState)
        {
            CurrentState = startingState;
            
            startingState.Enter();
            
            //_eventAgent.Publish(new StateMachineChanged(_senderTransform, startingState));
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

            //_eventAgent.Publish(new StateMachineChanged(_senderTransform, newState));
        }
    }
}
