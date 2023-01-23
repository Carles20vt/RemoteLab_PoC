using System.Collections;
using RemoteLab.Machinery.Centrifuge.Lid.Messages;
using TreeislandStudio.Engine;
using TreeislandStudio.Engine.Environment;
using TreeislandStudio.Engine.Event;
using UnityEngine;
using Zenject;

namespace RemoteLab.Machinery.Centrifuge.Lid
{
    [RequireComponent(typeof(HingeJoint))]
    public class CentrifugeLid : TreeislandBehaviour
    {
        #region Public Properties

        [SerializeField] [Range(0f, 50f)] private float angleEpsilon = 1f;
        
        #endregion
        
        #region Private Properties
        
        private Transform centrifugeParentTransform;

        private new HingeJoint hingeJoint;

        private bool lastLidOpenStatus;
        
        private Quaternion targetRot = Quaternion.identity;

        private Rigidbody myRigidbody;

        private bool wasLidOpen;

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
        
        private void Start()
        {
            centrifugeParentTransform = GetComponentInParent<Centrifuge>().transform;
            hingeJoint = GetComponent<HingeJoint>();
            myRigidbody = GetComponent<Rigidbody>();

            SendLidStatus();
        }

        private void FixedUpdate()
        {
            if(wasLidOpen != IsLidOpened())
            {
                OnLidStatusChanged();
                wasLidOpen = !wasLidOpen;
            }
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

        public void OnLidStatusChanged(string msg = "")
        {
            myRigidbody.useGravity = !IsLidOpened();
            SendLidStatus();
            Debug.Log("ONSTATUS " + msg);
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

            eventAgent.Publish(new CentrifugeLidChanged(centrifugeParentTransform, currentLidStatus));
        }
        
        private bool IsLidOpened()
        {
            return hingeJoint.angle - angleEpsilon <= hingeJoint.limits.min;
        }

        #endregion
    }
}