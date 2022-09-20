/*
using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using TreeislandStudio.Engine.Event;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Zenject;
*/

namespace TreeislandStudio.Engine.Environment
{
    /*
     
    /// <inheritdoc />
    /// <summary>
    /// Send tick events on fixed update
    /// This scripts should be scheduled (from project properties) for early execution
    /// </summary>
    public class Ticker : TreeislandBehaviour {
        
        #region Public types
        
        /// <summary>
        /// Event broker is able to use
        /// </summary>
        public enum Tickers {
            Global,
            Enemies
        }
        #endregion

        #region Public properties

        /// <summary>
        /// Ticker to use
        /// </summary>
        [ExposeProperty, UsedImplicitly]
        public Tickers EventBroker {
            get => selectedEventBroker;
            set => SetTicker(value);
        }
        
        #endregion region
        
        #region Private properties

        /// <summary>
        /// Selector for event broker
        /// </summary>
        [SerializeField, HideInInspector]
        private Tickers selectedEventBroker = Tickers.Global;

        /// <summary>
        /// Selected time provider
        /// </summary>
        private ITimeProvider timeProvider;
        
        /// <summary>
        /// Selected event broker
        /// </summary>
        private EventBrokerInterface eventBroker;


        /// <summary>
        /// Last time on fixed update (used to calculate delta time)
        /// </summary>
        private float lastTime;
        
        #endregion
        
        #region Dependencies

        /// <summary>
        /// Global environment setup
        /// </summary>
        private IEnvironmentSetUp globalEnvironmentSetup;
        
        /// <summary>
        /// Environment setup for enemies
        /// </summary>
        private IEnvironmentSetUp enemyEnvironmentSetup;

        /// <summary>
        /// Initialize dependencies
        /// </summary>
        [Inject, UsedImplicitly]
        [SuppressMessage("ReSharper", "ParameterHidesMember")]
        private void Initialize(
            IEnvironmentSetUp globalEnvironmentSetUp,
            [Inject(Id = ServiceIds.EnemyEnvironment)]IEnvironmentSetUp enemyEnvironment
        ) {      
            globalEnvironmentSetup =  globalEnvironmentSetUp;
            enemyEnvironmentSetup  =  enemyEnvironment;
        }

        #endregion

        #region Unity events

        /// <summary>
        /// Unity event, on start
        /// </summary>
        private void Start()
        {
            SetTicker(selectedEventBroker);
        }

        /// <summary>
        /// Unity event, on every frame
        /// </summary>
        private void FixedUpdate()
        {
            var time = timeProvider.Time;
            var deltaTime = time - lastTime;
            lastTime = time;
            switch (EventBroker) {
                case Tickers.Global:
                    globalEnvironmentSetup.EventBroker.Publish(new EarlyFixedUpdate(time, deltaTime));
                    globalEnvironmentSetup.EventBroker.Publish(new FixedUpdate(time, deltaTime));
                    break;
                case Tickers.Enemies:
                    enemyEnvironmentSetup.EventBroker.Publish(TagAccess.Enemy, new EarlyFixedUpdate(time, deltaTime));
                    enemyEnvironmentSetup.EventBroker.Publish(TagAccess.Enemy, new FixedUpdate(time, deltaTime));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            eventBroker.Publish(new EarlyFixedUpdate(time, deltaTime));
            eventBroker.Publish(new FixedUpdate(time, deltaTime));
        }

        #endregion
        
        #region Private methods

        /// <summary>
        /// Set ticker and selects time provider and lastTime
        /// </summary>
        /// <param name="value"></param>
        private void SetTicker(Tickers value)
        {
            selectedEventBroker = value;

            if (enemyEnvironmentSetup == null
                || globalEnvironmentSetup == null
            ) {
                return;
            }

            IEnvironmentSetUp setup;
            
            switch (value) {
                case Tickers.Enemies:
                    setup = enemyEnvironmentSetup;
                break;
                // ReSharper disable once RedundantCaseLabel
                case Tickers.Global:
                default:
                    setup = globalEnvironmentSetup;
                break;
            }

            timeProvider = setup.TimeProvider;
            eventBroker  = setup.EventBroker;
            
            lastTime = timeProvider.Time;
        }

        #endregion
    }
    */
}