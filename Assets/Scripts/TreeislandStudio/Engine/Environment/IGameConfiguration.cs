namespace TreeislandStudio.Engine.Environment
{
    /// <summary>
    /// Stores the game configuration options.
    /// </summary>
    public interface IGameConfiguration
    {
        /// <summary>
        /// Multiplayer is enabled or not.
        /// </summary>
        bool IsMultiPlayerEnabled { get; }
    }
}