namespace TreeislandStudio.Engine.Environment
{
    /// <summary>
    /// Time provider, provides with an scalable time relative to it's creation
    /// </summary>
    public interface ITimeProvider {
        
        /// <summary>
        /// Timescale
        /// </summary>
        float TimeScale { get; set; }

        /// <summary>
        /// Current time
        /// </summary>
        float Time { get; set; }
    }
}