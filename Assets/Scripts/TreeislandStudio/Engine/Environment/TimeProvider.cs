using System;
using TreeislandStudio.Engine.Log;

namespace TreeislandStudio.Engine.Environment
{
    public class TimeProvider : ITimeProvider
    {
        public TimeProvider(Func<float> func)
        {
            
        }

        public float TimeScale { get; set; }
        public float Time { get; set; }
    }
    
    /*
    /// <inheritdoc />
    /// <summary>
    /// Time provider, provides with an scalable time relative to it's creation
    /// </summary>
    public class TimeProvider : ITimeProvider
    {
        
        #region Public properties
        
        /// <inheritdoc />
        public float TimeScale {
            get => timeScale;
            set {
                SetTime(GetTime());
                timeScale = value;
                logger?.Debug("Timescale changed to " + value);
            }
        }
        
        /// <inheritdoc />
        public float Time {
            get => GetTime();
            set => SetTime(value);
        }
        
        #endregion
        
        #region Private properties

        /// <summary>
        /// Time scale
        /// </summary>
        private float timeScale;

        /// <summary>
        /// Last change of alphaTime in realtime
        /// </summary>
        private float lastChangeInRealTime;
        
        /// <summary>
        /// Last change of alphaTime in own time
        /// </summary>
        private float lastChangeInOwnTime;

        /// <summary>
        /// Real time provider
        /// </summary>
        private readonly Func<float> realTimeProvider;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ICustomLogger;
        
        #endregion
        
        #region Life cycle

        /// <summary>
        /// Constructor
        /// </summary>
        public TimeProvider(Func<float> realTime, ICustomLogger logger = null) {
            realTimeProvider = realTime;
            this.logger = logger;

            SetTime(realTimeProvider());
            timeScale = 1f;
        }      
        
        #endregion
        
        #region private Methods

        /// <summary>
        /// Sets the current time
        /// </summary>
        /// <param name="time"></param>
        private void SetTime(float time)
        {
            logger?.Debug("Time set to " + time);
            lastChangeInRealTime = realTimeProvider();
            lastChangeInOwnTime  = time;
        }
        
        /// <summary>
        /// Return the current time
        /// </summary>
        /// <returns></returns>
        private float GetTime()
        {
            return TimeScale * (realTimeProvider() - lastChangeInRealTime) + lastChangeInOwnTime;
        }
        
        #endregion
    }
    
    */
}