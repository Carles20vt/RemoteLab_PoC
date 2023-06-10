using TreeislandStudio.Engine;
using TreeislandStudio.Engine.Environment;
using TreeislandStudio.Engine.Event;
using TreeislandStudio.Engine.Log;
using TreeislandStudio.Engine.Math;
using UnityEngine;
using Zenject;

namespace RemoteLab
{
    public class DefaultProjectInstaller : MonoInstaller<DefaultProjectInstaller>
    {
        #region Public properties
        
        [SerializeField]
        private bool enableMultiPlayer;
        
        #endregion

        #region Private properties
        
        /// <summary>
        /// The project level event broker, it survives between scenes, typically would be added to the container
        /// be the <c>ProjectInstaller</c>
        /// </summary>
        private IEventBroker eventBroker;
        
        #endregion
        
        /// <summary>
        /// Install (binds) services to the container (DIC)
        /// </summary>
        public override void InstallBindings()
        {
            //base.InstallBindings();
            
            BindKernelServices();
            BindInputControllers();
        }
        
        /// <summary>
        /// Bind infrastructure services
        /// </summary>
        private void BindKernelServices()
        {
            var globalTimeProvider = (ITimeProvider) new TimeProvider(() => Time.time);
            var gameConfiguration = (IGameConfiguration) new GameConfiguration(enableMultiPlayer);
            
            Container
                .BindInstance(globalTimeProvider)
                .AsCached();
            
            // Enemies time provider
            var enemyTimeProvider = (ITimeProvider) new TimeProvider(() => globalTimeProvider.Time);
            Container
                .BindInstance(enemyTimeProvider)
                .WithId(ServiceIds.EnemyTimeProvider)
                .AsCached();
            
            // Logger
            var customLogger = (ICustomLogger) new UnityConsoleLogger(LogLevel.Debug);
            Container
                .BindInstance(customLogger)
                .AsSingle();

            // Random generator
            Container
                .BindInstance((IRandomNumberProvider) new RandomNumberProvider())
                .AsSingle();
            
            // Event Broker
            eventBroker = (IEventBroker) new EventBroker(customLogger);
            Container
                .BindInstance(eventBroker)
                .AsSingle()
                .NonLazy();
            
            // Environment global
            Container
                .BindInstance((IEnvironmentSetUp) new EnvironmentSetUp(
                    globalTimeProvider, 
                    eventBroker,
                    gameConfiguration))
                .AsCached();
            
            // Environment for enemies
            Container
                .BindInstance((IEnvironmentSetUp) new EnvironmentSetUp(
                    enemyTimeProvider, 
                    eventBroker,
                    gameConfiguration))
                .WithId(ServiceIds.EnemyEnvironment)
                .AsCached();
            /*
            // Player Character
            Container
                .BindInstance(playerCharacter)
                .AsSingle();
            */
        }

        private void BindInputControllers()
        {
            /*
            Container
                .BindInstance(playerInputController)
                //.To<PlayerInputController>()
                //.WhenInjectedInto<PlayerController>()
                .AsSingle()
                .NonLazy();
            
            Container
                .BindInstance(enemyInputController)
                //.To<EnemyInputController>()
                //.WhenInjectedInto<EnemyController>()
                .AsSingle()
                .NonLazy();
            */
        }
    }
}