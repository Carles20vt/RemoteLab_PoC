namespace TreeislandStudio.Engine {
    
    #region Public types
    /// <summary>
    /// Service ids (usually services are inferred by interface, but when we have two or more of the same interface,
    /// distinguish them by this service identifier (or name), is used as is, no need to convert to string 
    /// </summary>
    public enum ServiceIds
    {
        #region Pools
        
        /// <summary>
        /// Pool where sound clips should be created
        /// </summary>
        EnemiesPool,
        
        #endregion
        
        /// <summary>
        /// Time provider for enemies
        /// </summary>
        EnemyTimeProvider,

        /// <summary>
        /// Environment for enemies
        /// </summary>
        EnemyEnvironment
    }
    #endregion
}