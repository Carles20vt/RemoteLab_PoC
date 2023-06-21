using System.Collections;
using RemoteLab.Machinery.Centrifuge.Messages;
using RemoteLab.Machinery.Centrifuge.States;
using TreeislandStudio.Engine;
using TreeislandStudio.Engine.Environment;
using TreeislandStudio.Engine.Event;
using TreeislandStudio.Engine.StateMachine;
using TreeislandStudio.Engine.StateMachine.Messages;
using UnityEngine;
using Zenject;

namespace RemoteLab.Machinery.Centrifuge.Screen
{
    [RequireComponent(typeof(AudioSource))]
    public class CentrifugeScreenChooser : TreeislandBehaviour
    {
        #region Public Properties

        [SerializeField] private GameObject openTopCoverScreenGameObject;
        [SerializeField] private GameObject grabSamplesScreenGameObject;
        [SerializeField] private GameObject enterParametersScreenGameObject;
        [SerializeField] private GameObject runningStateScreenGameObject;
        [SerializeField] private GameObject removeSamplesScreenGameObject;
        
        [SerializeField] private Centrifuge centrifuge;

        [SerializeField] private AudioClip finishedActionAudioClip;
        [SerializeField] private AudioClip attentionAudioClip;

        [SerializeField] private float delayBetweenScreenChanges = 1.5f;

        #endregion

        #region Private Properties

        private GameObject currentScreenGameObject;
        private AudioSource audioSource;
        private bool isStarted;

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
            centrifuge = centrifuge != null ? centrifuge : GetComponentInParent<Centrifuge>();
            audioSource = GetComponent<AudioSource>();

            SubscribeToEvents();
        }

        private void Start()
        {
            ConfigureAudio();
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
            eventAgent.Subscribe<CentrifugeStarted>(OnCentrifugeStarted);
        }
        
        private void OnStateMachineChanged(StateMachineChanged message)
        {
            if (!ReferenceEquals(centrifuge.gameObject.transform, message.Sender))
            {
                return;
            }
            
            ShowScreen(message.NewState);
        }
        
        private void OnCentrifugeStarted(CentrifugeStarted message)
        {
            if (!ReferenceEquals(centrifuge.gameObject.transform, message.Sender))
            {
                return;
            }

            isStarted = message.IsStarted;
        }

        #endregion

        #region Private Methods

        private void ConfigureAudio()
        {
            audioSource.playOnAwake = false;
        }

        private void ShowScreen(State newState)
        {
            HideScreen(currentScreenGameObject);

            if (!isStarted)
            {
                return;
            }
            
            currentScreenGameObject = DetermineCurrentScreen(newState);
            StartCoroutine(ShowScreenAfterFinishedActionAudioClipFinish(currentScreenGameObject));
        }
        
        private static void HideScreen(GameObject screenToHide)
        {
            if (screenToHide == null)
            {
                return;
            }
            
            screenToHide.SetActive(false);
        }
        
        private GameObject DetermineCurrentScreen(State newState)
        {
            if (typeof(IdleState) == newState.GetType()) return openTopCoverScreenGameObject;
            if (typeof(OpenTopCoverState) == newState.GetType()) return grabSamplesScreenGameObject;
            if (typeof(ReadyToEnterParametersState) == newState.GetType()) return enterParametersScreenGameObject;
            if (typeof(RunningState) == newState.GetType()) return runningStateScreenGameObject;
            return typeof(RemoveSamplesState) == newState.GetType() ? removeSamplesScreenGameObject : null;
        }

        private IEnumerator ShowScreenAfterFinishedActionAudioClipFinish(GameObject screenToShow)
        {
            PlayFinishedAction();

            yield return new WaitForSeconds(
                finishedActionAudioClip.length +
                delayBetweenScreenChanges);

            if (screenToShow == null)
            {
                yield break;
            }

            PlayAttention();
            yield return new WaitForSeconds(attentionAudioClip.length);

            screenToShow.SetActive(true);
        }

        private void PlayFinishedAction()
        {
            audioSource.clip = finishedActionAudioClip;
            audioSource.Play();
        }
        private void PlayAttention()
        {
            audioSource.clip = attentionAudioClip;
            audioSource.Play();
        }

        #endregion
    }
}