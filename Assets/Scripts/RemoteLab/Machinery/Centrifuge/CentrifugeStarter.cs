using Photon.Pun;
using RemoteLab.Machinery.Centrifuge.Messages;
using TreeislandStudio.Engine;
using TreeislandStudio.Engine.Environment;
using TreeislandStudio.Engine.Event;
using UnityEngine;
using Zenject;

namespace RemoteLab.Machinery.Centrifuge
{
    public class CentrifugeStarter : TreeislandBehaviour
    {
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

        private void Start()
        {
            centrifugeParentTransform = GetComponentInParent<Centrifuge>().transform;
            photonView = GetComponent<PhotonView>() ?? GetComponentInParent<PhotonView>();
        }

        /// <summary>
        /// Destroy event
        /// </summary>
        private void OnDestroy()
        {
            eventAgent.Dispose();
        }

        #endregion

        #region PublicMethods
        
        public void OnPlayerStartedMachine()
        {
            photonView.RPC(nameof(PublishCentrifugeParametersChanged), RpcTarget.All, true);
        }


        #endregion
        
        #region Private Methods
        
        [PunRPC]
        private void PublishCentrifugeParametersChanged(bool isStarted)
        {
            eventAgent.Publish(new CentrifugeStarted(centrifugeParentTransform, isStarted));
        }
        
        #endregion
    }
}