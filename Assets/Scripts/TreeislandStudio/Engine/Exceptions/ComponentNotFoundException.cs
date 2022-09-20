using System;
using System.Runtime.Serialization;

namespace TreeislandStudio.Engine.Exceptions {
    /// <summary>
    /// The component has not been found
    /// </summary>
    [Serializable]
    internal class ComponentNotFoundException : Exception {
        /// <summary>
        /// Constructor
        /// </summary>
        public ComponentNotFoundException() {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ComponentNotFoundException(string message) : base(message) {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ComponentNotFoundException(string message, Exception innerException) : base(message, innerException) {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected ComponentNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}