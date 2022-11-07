using TreeislandStudio.Engine.Event.Event;
using UnityEngine;

namespace RemoteLab.Machinery.Centrifuge.Lid.Messages
{
    /// <inheritdoc />
    /// <summary>
    /// Sent when the Centrifuge Lid changes the state.
    /// </summary>
    public class CentrifugeLidChanged : EventMessage
    {
        #region Public properties

        /// <summary>
        /// Sender Object
        /// </summary>
        public readonly Transform Sender;
        
        /// <summary>
        /// Position.
        /// </summary>
        public readonly bool IsLidOpen;

        #endregion
        
        #region Life cycle

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="islIdOpen"></param>
        public CentrifugeLidChanged(Transform sender, bool islIdOpen)
        {
            Sender = sender;
            IsLidOpen = islIdOpen;
        }
        
        #endregion
    }
}