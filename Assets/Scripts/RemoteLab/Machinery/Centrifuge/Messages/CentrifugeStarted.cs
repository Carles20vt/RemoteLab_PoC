using TreeislandStudio.Engine.Event.Event;
using UnityEngine;

namespace RemoteLab.Machinery.Centrifuge.Messages
{
    /// <inheritdoc />
    /// <summary>
    /// Sent when the Centrifuge Starts running.
    /// </summary>
    public class CentrifugeStarted : EventMessage
    {
        #region Public properties

        /// <summary>
        /// Sender Object
        /// </summary>
        public readonly Transform Sender;
        
        /// <summary>
        /// Is started?.
        /// </summary>
        public readonly bool IsStarted;

        #endregion
        
        #region Life cycle

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="isStarted"></param>
        public CentrifugeStarted(Transform sender, bool isStarted)
        {
            Sender = sender;
            IsStarted = isStarted;
        }
        
        #endregion
    }
}