using Photon.Pun;
using RemoteLab.Characters;
using RemoteLab.Machinery.Centrifuge.Lid.Messages;
using RemoteLab.Machinery.Centrifuge.States;
using TreeislandStudio.Engine;
using TreeislandStudio.Engine.Environment;
using TreeislandStudio.Engine.Event;
using TreeislandStudio.Engine.StateMachine.Messages;
using UnityEngine;
using Zenject;

namespace RemoteLab.Machinery.Centrifuge.Lid
{
    [RequireComponent(typeof(HingeJoint))]
    public class CentrifugeLid : TreeislandBehaviour
    {
        #region Public Properties

        [SerializeField] [Range(0f, 50f)] private float angleEpsilon = 1f;
        [SerializeField] private XRGrabNetworkInteractable lidInteractable;
        
        #endregion
        
        #region Private Properties
        
        private Transform centrifugeParentTransform;

        private new HingeJoint hingeJoint;

        private bool lastLidOpenStatus;
        
        private Quaternion targetRot = Quaternion.identity;

        private Rigidbody myRigidbody;

        private bool wasLidOpen;

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
            hingeJoint = GetComponent<HingeJoint>();
            myRigidbody = GetComponent<Rigidbody>();

            SendLidStatus();
        }

        private void FixedUpdate()
        {
            if (wasLidOpen == IsLidOpened()) return;
            
            OnLidStatusChanged();
            wasLidOpen = !wasLidOpen;
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

            if (typeof(RunningState) == message.NewState.GetType() || typeof(EnteringParametersState) == message.NewState.GetType())
                lidInteractable.enabled = false;
            else if (!lidInteractable.enabled)
                lidInteractable.enabled = true;
        }

        public void OnLidStatusChanged()
        {
            myRigidbody.useGravity = !IsLidOpened();
            SendLidStatus();
        }

        #endregion

        #region Private Methods

        private void SendLidStatus()
        {
            var currentLidStatus = IsLidOpened();

            if (currentLidStatus == lastLidOpenStatus)
            {
                return;
            }

            lastLidOpenStatus = currentLidStatus;

            photonView.RPC(nameof(PublishCentrifugeLidChanged), RpcTarget.All, currentLidStatus);
        }

        [PunRPC]
        private void PublishCentrifugeLidChanged(bool currentLidStatus)
        {
            eventAgent.Publish(new CentrifugeLidChanged(centrifugeParentTransform, currentLidStatus));
        }
        
        private bool IsLidOpened()
        {
            return hingeJoint.angle - angleEpsilon <= hingeJoint.limits.min;
        }

        #endregion
    }
}