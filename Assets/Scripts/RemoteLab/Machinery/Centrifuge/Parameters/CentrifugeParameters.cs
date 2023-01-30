using Photon.Pun;
using RemoteLab.Machinery.Centrifuge.Parameters.Messages;
using RemoteLab.Machinery.Centrifuge.States;
using TreeislandStudio.Engine;
using TreeislandStudio.Engine.Environment;
using TreeislandStudio.Engine.Event;
using TreeislandStudio.Engine.StateMachine.Messages;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Zenject;

namespace RemoteLab.Machinery.Centrifuge.Parameters
{
    public class CentrifugeParameters : TreeislandBehaviour
    {
        #region Public Properties

        [SerializeField] private GameObject parametersUI;
        [SerializeField] private XRSimpleInteractable parametersInteractable;
        [SerializeField] private MeshRenderer parametersMesh;

        #endregion
        
        #region Private Properties
        
        private Transform centrifugeParentTransform;
        
        private PhotonView photonView;

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

        private void Start()
        {
            centrifugeParentTransform = GetComponentInParent<Centrifuge>().transform;
            photonView = GetComponent<PhotonView>() ?? GetComponentInParent<PhotonView>();
            
            parametersUI.SetActive(false);
            
            EnableParameters(false);
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

            EnableParameters(typeof(ClosedTopCoverState) == message.NewState.GetType());
        }
        
        #endregion

        #region Public Methods

        public void OnParametersButtonPressed(bool parametersOpen)
        {
            photonView.RPC("PublishCentrifugeParametersChanged", RpcTarget.All, parametersOpen);
        }

        #endregion
        
        #region Private Methods
        
        [PunRPC]
        private void PublishCentrifugeParametersChanged(bool parametersOpen)
        {
            eventAgent.Publish(new CentrifugeParametersChanged(centrifugeParentTransform, parametersOpen));
        }

        private void EnableParameters(bool enableParam)
        {
            parametersInteractable.enabled = enableParam;
            parametersMesh.enabled = enableParam;
        }
        #endregion
    }
}