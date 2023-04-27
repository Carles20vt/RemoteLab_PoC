using RemoteLab.Machinery.Centrifuge.States;
using TreeislandStudio.Engine.Environment;
using TreeislandStudio.Engine.Event;
using TreeislandStudio.Engine.StateMachine.Messages;
using UnityEngine;
using Zenject;

namespace RemoteLab.Characters
{
    public class XROriginHandsSwitcher : MonoBehaviour
    {
        #region Public Properties

        [SerializeField] private GameObject[] controllersWithRay;
        [SerializeField] private GameObject[] controllersWithoutRay;

        #endregion
        
        #region Dependencies
        
        /// <summary>
        /// EventAgent
        /// </summary>
        private EventAgent eventAgent;

        /// <summary>
        /// Dependency injection
        /// </summary>
        /// <param name="environmentSetUp"></param>
        [Inject]
        private void Initialize(IEnvironmentSetUp environmentSetUp)
        {
            eventAgent = new EventAgent(environmentSetUp.EventBroker);
        }

        #endregion
        
        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            SubscribeToEvents();
        }

        /// <summary>
        /// Destroy event
        /// </summary>
        private void OnDestroy()
        {
            eventAgent.Dispose();
        }

        #endregion
        
        #region Events

        private void SubscribeToEvents()
        {
            eventAgent.Subscribe<StateMachineChanged>(OnStateMachineChanged);
        }
        
        private void OnStateMachineChanged(StateMachineChanged message)
        {
            ChangeRayHandsVisibility(typeof(EnteringParametersState) == message.NewState.GetType());
        }

        #endregion
        
        #region Private Methods

        private void ChangeRayHandsVisibility(bool isVisible)
        {
            foreach (var controller in controllersWithRay)
            {
                controller.SetActive(isVisible);
            }
            
            foreach (var controller in controllersWithoutRay)
            {
                controller.SetActive(!isVisible);
            }
        }
        
        #endregion
    }
}
