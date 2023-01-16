using TreeislandStudio.Engine.Event.Event;
using UnityEngine;

namespace RemoteLab.Machinery.Centrifuge.Screen.Messages
{
    /// <inheritdoc />
    /// <summary>
    /// Sent when the Centrifuge Rotor changes the state.
    /// </summary>
    public class CentrifugeRunningStatusChanged : EventMessage
    {
        #region Public properties

        /// <summary>
        /// Sender Object
        /// </summary>
        public readonly Transform Sender;
        
        /// <summary>
        /// Is the machine running
        /// </summary>
        public readonly bool IsRunning;

        #endregion
        
        #region Life cycle

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="isRunning"></param>
        public CentrifugeRunningStatusChanged(Transform sender, bool isRunning)
        {
            Sender = sender;
            IsRunning = isRunning;
        }
        
        #endregion
    }
}