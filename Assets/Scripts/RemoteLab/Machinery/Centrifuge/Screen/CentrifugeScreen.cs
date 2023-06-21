using Photon.Pun;
using RemoteLab.Machinery.Centrifuge.Screen.Messages;
using TreeislandStudio.Engine;
using TreeislandStudio.Engine.Environment;
using TreeislandStudio.Engine.Event;
using UnityEngine;
using Zenject;

namespace RemoteLab.Machinery.Centrifuge.Screen
{
    public class CentrifugeScreen : TreeislandBehaviour
    {
        #region Private properties

        private Transform centrifugeParentTransform;
        
        private PhotonView photonView;

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

        private void Start()
        {
            centrifugeParentTransform = GetComponentInParent<Centrifuge>().transform;
            photonView = GetComponent<PhotonView>();
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

        public void OnStartButtonPressed()
        {
            photonView.RPC(nameof(PublishCentrifugeRunningStatusChanged), RpcTarget.All, true);
        }
        
        #endregion

        #region Private Methods
        
        [PunRPC]
        private void PublishCentrifugeRunningStatusChanged(bool centrifugeStatus)
        {
            eventAgent.Publish(new CentrifugeRunningStatusChanged(centrifugeParentTransform, centrifugeStatus));
        }
        
        #endregion
    }
}
