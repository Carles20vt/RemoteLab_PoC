using System.Collections;
using Photon.Pun;
using RemoteLab.Machinery.Centrifuge.Screen.Messages;
using TreeislandStudio.Engine;
using TreeislandStudio.Engine.Environment;
using TreeislandStudio.Engine.Event;
using UnityEngine;
using Zenject;

namespace RemoteLab.Machinery.Centrifuge.Screen
{
    [RequireComponent(typeof(AudioSource), typeof(PhotonView))]
    public class CentrifugeRunningTimer : TreeislandBehaviour
    {
        #region Public Constants.Properties

        [SerializeField] private Transform centrifugeTransform;

        [SerializeField] private AudioClip stepVoiceAudioClip;
        [SerializeField] private AudioClip runningNoiseAudioClip;

        [SerializeField] private float delayBetweenAudioSwitching = 0.5f;

        #endregion

        #region Private properties

        private PhotonView photonView;
        private AudioSource audioSource;

        #endregion

        #region Dependencies

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
            photonView = GetComponent<PhotonView>();
            audioSource = GetComponent<AudioSource>();

            ConfigureAudio();
        }

        private void OnEnable()
        {
            StartCoroutine(PlayAudioAndFinishStep());
        }

        /// <summary>
        /// Destroy event
        /// </summary>
        private void OnDestroy()
        {
            eventAgent.Dispose();
        }

        #endregion

        #region Private Methods

        [PunRPC]
        private void PublishCentrifugeRunningStatusChanged(bool centrifugeStatus)
        {
            eventAgent.Publish(new CentrifugeRunningStatusChanged(centrifugeTransform, centrifugeStatus));
        }

        private void ConfigureAudio()
        {
            audioSource.playOnAwake = false;
        }

        private IEnumerator PlayAudioAndFinishStep()
        {
            PlayVoiceClip();

            yield return new WaitForSeconds(
                stepVoiceAudioClip.length +
                delayBetweenAudioSwitching);

            PlayNoiseSound();

            yield return new WaitForSeconds(runningNoiseAudioClip.length);

            photonView.RPC(nameof(PublishCentrifugeRunningStatusChanged), RpcTarget.All, false);
        }

        private void PlayVoiceClip()
        {
            audioSource.clip = stepVoiceAudioClip;
            audioSource.Play();
        }

        private void PlayNoiseSound()
        {
            audioSource.clip = runningNoiseAudioClip;
            audioSource.Play();
        }

        #endregion
    }
}