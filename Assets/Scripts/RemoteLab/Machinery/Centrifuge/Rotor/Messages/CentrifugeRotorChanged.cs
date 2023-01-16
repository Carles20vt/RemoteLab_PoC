using TreeislandStudio.Engine.Event.Event;
using UnityEngine;

namespace RemoteLab.Machinery.Centrifuge.Rotor.Messages
{
    /// <inheritdoc />
    /// <summary>
    /// Sent when the Centrifuge Rotor changes the state.
    /// </summary>
    public class CentrifugeRotorChanged : EventMessage
    {
        #region Public properties

        /// <summary>
        /// Sender Object
        /// </summary>
        public readonly Transform Sender;
        
        /// <summary>
        /// Is Rotor with Vials inside
        /// </summary>
        public readonly bool IsRotorWithVialsInside;

        #endregion
        
        #region Life cycle

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="isRotorWithVialsInside"></param>
        public CentrifugeRotorChanged(Transform sender, bool isRotorWithVialsInside)
        {
            Sender = sender;
            IsRotorWithVialsInside = isRotorWithVialsInside;
        }
        
        #endregion
    }
}