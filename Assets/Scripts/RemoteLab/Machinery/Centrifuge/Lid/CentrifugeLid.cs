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
        #region Private Properties

        private new HingeJoint hingeJoint;

        private bool lastLidOpenStatus;

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
            hingeJoint = GetComponent<HingeJoint>();

            SendLidStatus();
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

        private void OnCollisionStay(Collision other)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                return;
            }

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

            eventAgent.Publish(new CentrifugeLidChanged(transform, currentLidStatus));
        }
        
        private bool IsLidOpened()
        {
            return hingeJoint.angle > hingeJoint.limits.max - 0.01f;
        }

        #endregion
    }
}