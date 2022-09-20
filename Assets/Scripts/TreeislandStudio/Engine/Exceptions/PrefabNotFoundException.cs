using System;
using System.Runtime.Serialization;

namespace TreeislandStudio.Engine.Exceptions {
    /// <summary>
    /// The requested prefab cannot be found
    /// </summary>
    [Serializable]
    internal class PrefabNotFoundException : Exception {
        /// <summary>
        /// Constructor
        /// </summary>
        public PrefabNotFoundException()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        public PrefabNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public PrefabNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected PrefabNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}