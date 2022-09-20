using UnityEngine;

namespace TreeislandStudio.Engine.Log {
    /// <summary>
    /// Uses unity console as logging facility
    /// </summary>
    internal class UnityConsoleLogger : ICustomLogger {

        #region Public fields

        /// <summary>
        /// Any message below this level is skipped
        /// </summary>
        public readonly LogLevel LOGLevel;

        #endregion

        #region Life cycle

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logLevel">Initial log level</param>
        public UnityConsoleLogger(LogLevel logLevel) {
            this.LOGLevel = logLevel;
        }

        #endregion

        #region Public methods

        public void Alert(string message, GameObject gameObject = null) {
            Log(LogLevel.Alert, message);
        }

        public void Critical(string message, GameObject gameObject = null) {
            Log(LogLevel.Critical, message);
        }

        public void Debug(string message, GameObject gameObject = null) {
            Log(LogLevel.Debug, message);
        }

        public void Error(string message, GameObject gameObject = null) {
            Log(LogLevel.Error, message);
        }

        public void Info(string message, GameObject gameObject = null) {
            Log(LogLevel.Info, message);
        }

        public void Notice(string message, GameObject gameObject = null) {
            Log(LogLevel.Notice, message);
        }

        public void Warning(string message, GameObject gameObject = null) {
            Log(LogLevel.Warning, message);
        }

        /// <summary>
        /// Logs a message to the console
        /// </summary>
        /// <param name="level">Level of the message</param>
        /// <param name="message">Message</param>
        /// <param name="gameObject">Optional gameobject</param>
        private void Log(LogLevel level, string message, GameObject gameObject = null) {
            if (level < LOGLevel) {
                return;
            }

            if (level > LogLevel.Warning) {
                UnityEngine.Debug.LogError(message, gameObject);

                return;
            }
            if (level > LogLevel.Notice) {
                UnityEngine.Debug.LogWarning(message, gameObject);

                return;
            }

            UnityEngine.Debug.Log(message, gameObject);
        }

        #endregion

    }
}