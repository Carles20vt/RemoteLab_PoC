namespace TreeislandStudio.Engine.Environment
{
    public class GameConfiguration : IGameConfiguration
    {
        public GameConfiguration(bool isPunEnabled)
        {
            IsMultiPlayerEnabled = isPunEnabled;
        }

        public bool IsMultiPlayerEnabled { get; }
    }
}