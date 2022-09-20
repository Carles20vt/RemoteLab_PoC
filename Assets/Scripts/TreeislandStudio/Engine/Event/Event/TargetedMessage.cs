using UnityEngine;

namespace TreeislandStudio.Engine.Event.Event {
    /// <summary>
    /// A message targeted to a given gameobject
    /// </summary>
    public class TargetedMessage : EventMessage {

        /// <summary>
        /// Target game object (intended received of this message)
        /// </summary>
        private readonly GameObject target;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target"></param>
        public TargetedMessage(GameObject target) {
            this.target = target;
        }

        /// <summary>
        /// Returns true if the given gameobject is the intended target
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public bool IsTarget(GameObject gameObject) {
            return gameObject == target;
        }
    }
}