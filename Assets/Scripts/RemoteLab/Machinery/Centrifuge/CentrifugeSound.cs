using RemoteLab.Machinery.Centrifuge.States;
using System;
using System.Collections;
using System.Collections.Generic;
using TreeislandStudio.Engine.Environment;
using TreeislandStudio.Engine.Event;
using TreeislandStudio.Engine.StateMachine.Messages;
using UnityEngine;
using Zenject;

namespace RemoteLab.Machinery.Centrifuge
{
    public class CentrifugeSound : MonoBehaviour
    {
        #region Private Properties

        [SerializeField] private AudioSource sound;
        private Transform centrifugeParentTransform;

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
            centrifugeParentTransform =  GetComponentInParent<Centrifuge>().transform;

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
            if (!ReferenceEquals(centrifugeParentTransform, message.Sender))
                return;

            if (typeof(RunningState) == message.NewState.GetType())
                sound.Play();
            else if (sound.isPlaying)
                sound.Stop();

        }
        #endregion
    }
}