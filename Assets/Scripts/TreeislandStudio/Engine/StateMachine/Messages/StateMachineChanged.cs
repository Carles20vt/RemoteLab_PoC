using TreeislandStudio.Engine.Event.Event;
using UnityEngine;

namespace TreeislandStudio.Engine.StateMachine.Messages
{
    /// <inheritdoc />
    /// <summary>
    /// Sent when the StateMachine changes the state.
    /// </summary>
    public class StateMachineChanged : EventMessage
    {
        #region Public properties

        /// <summary>
        /// Sender Object
        /// </summary>
        public readonly Transform Sender;
        
        /// <summary>
        /// Position.
        /// </summary>
        public readonly State NewState;

        #endregion
        
        #region Life cycle

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="newState"></param>
        /// <param name="senderId"></param>
        public StateMachineChanged(Transform sender, State newState)
        {
            Sender = sender;
            NewState = newState;
        }
        
        #endregion
    }
}