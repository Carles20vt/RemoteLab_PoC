using TreeislandStudio.Engine.Event.Event;
using UnityEngine;

namespace RemoteLab.Machinery.Centrifuge.Parameters.Messages
{
    /// <inheritdoc />
    /// <summary>
    /// Sent when the Centrifuge Parameters changes the state.
    /// </summary>
    public class CentrifugeParametersChanged : EventMessage
    {
        #region Public properties

        /// <summary>
        /// Sender Object
        /// </summary>
        public readonly Transform Sender;
        
        /// <summary>
        /// Is entering parameters
        /// </summary>
        public readonly bool IsEnteringParameters;

        #endregion

        #region Life cycle

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="IsEnteringParameters"></param>
        public CentrifugeParametersChanged(Transform sender, bool isEnteringParameters)
        {
            Sender = sender;
            IsEnteringParameters = isEnteringParameters;
        }
        
        #endregion
    }
}