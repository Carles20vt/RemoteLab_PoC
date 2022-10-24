using TMPro;
using TreeislandStudio.Engine.Character;
using TreeislandStudio.Engine.Environment;
using TreeislandStudio.Engine.Event;
using TreeislandStudio.Engine.StateMachine.Messages;
using UnityEngine;
using Zenject;

namespace TreeislandStudio.Engine.StateMachine.Debug
{
    public class ShowStateMachineStatus : TreeislandBehaviour
    {

        #region Private Properties

        private TMP_Text _tmpText;
        
        private Transform _parentTransform;

        #endregion
        
        #region Dependencies

        /// <summary>
        /// EventAgent
        /// </summary>
        private EventAgent _eventAgent;

        /// <summary>
        /// Dependency injection
        /// </summary>
        /// <param name="environmentSetUp"></param>
        [Inject]
        public void Initialize(IEnvironmentSetUp environmentSetUp)
        {
            _eventAgent = new EventAgent(environmentSetUp.EventBroker);
        }

        #endregion
        
        #region Unity Events

        private void Awake()
        {
            _tmpText = GetComponent<TMP_Text>();
            _parentTransform = transform.parent;
            
            _eventAgent.Subscribe<StateMachineChanged>(OnStateMachineChanged);
        }

        private void OnStateMachineChanged(StateMachineChanged message)
        {
            if (!ReferenceEquals(_parentTransform, message.Sender))
            {
                return;
            }

            _tmpText.text = message.NewState.StateName;
        }

        #endregion
    }
}