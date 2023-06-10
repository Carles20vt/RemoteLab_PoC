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

        /// <summary>
        /// The game configuration
        /// </summary>
        public IGameConfiguration GameConfiguration { get; }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="timeProvider"></param>
        /// <param name="eventBroker"></param>
        /// <param name="gameConfiguration"></param>
        public EnvironmentSetUp(
            ITimeProvider timeProvider,
            IEventBroker eventBroker,
            IGameConfiguration gameConfiguration)
        {
            TimeProvider = timeProvider;
            EventBroker  = eventBroker;
            GameConfiguration = gameConfiguration;
        }
    }
}