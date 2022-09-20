using UnityEngine;

namespace TreeislandStudio.Engine.Log {

    /// <summary>
    /// Standard log levels, the order is important as can determine if a message is logged
    /// </summary>
    public enum LogLevel {
        Debug,
        Info,
        Notice,
        Warning,
        Error,
        Critical,
        Alert
    }

    /// <summary>
    /// Logger interface
    /// </summary>
    public interface ICustomLogger {
        /// <summary>
        /// Detailed debug information
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="gameObject">Optional game object</param>
        void Debug(string message, GameObject gameObject = null);

        /// <summary>
        /// Interesting events
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="gameObject"></param>
        void Info(string message, GameObject gameObject = null);

        /// <summary>
        /// Normal but significant events
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="gameObject"></param>
        void Notice(string message, GameObject gameObject = null);

        ///  <summary>
        ///  Exceptional occurrences that are not errors
        /// 
        ///  e.g. Poor use of an API, use of deprecated APIs, undesired things that are not an error
        ///  </summary>
        ///  <param name="message">Message</param>
        /// <param name="gameObject"></param>
        void Warning(string message, GameObject gameObject = null);

        /// <summary>
        /// Runtime errors that do not require immediate action but should typically be logged and monitored.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="gameObject"></param>
        void Error(string message, GameObject gameObject = null);

        ///  <summary>
        ///  Critical conditions.
        /// 
        ///  Parts of the code is  unable to run, application component unavailable, the application is unable to work properly
        ///  </summary>
        ///  <param name="message"></param>
        /// <param name="gameObject"></param>
        void Critical(string message, GameObject gameObject = null);

        /// <summary>
        /// The application is not working at all, immediate action should be taken
        /// </summary>
        /// <param name="message"></param>
        /// <param name="gameObject"></param>
        void Alert(string message, GameObject gameObject = null);
    }
}
