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
        /*
        public PlayerInputController playerInputController;

        public EnemyInputController enemyInputController;

        public PlayerCharacter playerCharacter;
        */
        #endregion

        #region Private properties
        
        /// <summary>
        /// The project level event broker, it survives between scenes, typically would be added to the container
        /// be the <c>ProjectInstaller</c>
        /// </summary>
        private IEventBroker _eventBroker;
        
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
            // Time provider
            var globalTimeProvider = (ITimeProvider) new TimeProvider(() => Time.time);
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
            _eventBroker = (IEventBroker) new EventBroker(customLogger);
            Container
                .BindInstance(_eventBroker)
                .AsSingle()
                .NonLazy();
            
            // Environment global
            Container
                .BindInstance((IEnvironmentSetUp) new EnvironmentSetUp(globalTimeProvider, _eventBroker))
                .AsCached();
            
            // Environment for enemies
            Container
                .BindInstance((IEnvironmentSetUp) new EnvironmentSetUp(enemyTimeProvider, _eventBroker))
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