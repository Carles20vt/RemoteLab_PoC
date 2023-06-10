using TreeislandStudio.Engine.Event;

namespace TreeislandStudio.Engine.Environment
{
    public interface IEnvironmentSetUp
    {
        /// <summary>
        /// Time provider
        /// </summary>
        ITimeProvider TimeProvider { get; }
        
        /// <summary>
        /// Event broker
        /// </summary>
        IEventBroker EventBroker { get; }
        
        /// <summary>
        /// The game configuration
        /// </summary>
        IGameConfiguration GameConfiguration { get; }
    }
}