using System;
using System.Runtime.Serialization;

namespace TreeislandStudio.Engine.Exceptions {
    /// <summary>
    /// A requested gameobject was not found
    /// </summary>
    [Serializable]
    internal class GameObjectNotFoundException : Exception {
        /// <summary>
        /// Constructor
        /// </summary>
        public GameObjectNotFoundException()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        public GameObjectNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public GameObjectNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected GameObjectNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}