using TreeislandStudio.Engine.Event;

namespace TreeislandStudio.Engine.Environment {
    public class EnvironmentSetUp : IEnvironmentSetUp
    {
        
        #region Public properties
        
        /// <inheritdoc />
        /// <summary>
        /// Time provider 
        /// </summary>
        public ITimeProvider TimeProvider { get; }

        /// <inheritdoc />
        /// <summary>
        /// EventBroker
        /// </summary>
        public IEventBroker EventBroker { get; }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="timeProvider"></param>
        /// <param name="eventBroker"></param>
        public EnvironmentSetUp(ITimeProvider timeProvider, IEventBroker eventBroker)
        {
            TimeProvider = timeProvider;
            EventBroker  = eventBroker;
        }
    }
}