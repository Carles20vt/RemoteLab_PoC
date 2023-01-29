﻿using System.Collections;
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
        #region Public Properties

        [SerializeField] [Range(0f, 10f)] private float runningTime = 3f;

        #endregion
        
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
        
        #region Events

        public void OnStartButtonPressed()
        {
            //eventAgent.Publish(new CentrifugeRunningStatusChanged(centrifugeParentTransform, true));
            photonView.RPC("PublishCentrifugeRunningStatusChanged", RpcTarget.All, true);

            StartCoroutine(WaitForCentrifugationProcess());
        }
        
        #endregion

        #region Private Methods

        private IEnumerator WaitForCentrifugationProcess()
        {
            yield return new WaitForSeconds(runningTime);
            
            //eventAgent.Publish(new CentrifugeRunningStatusChanged(centrifugeParentTransform, false));
            photonView.RPC("PublishCentrifugeRunningStatusChanged", RpcTarget.All, false);
        }
        
        [PunRPC]
        private void PublishCentrifugeRunningStatusChanged(bool centrifugeStatus)
        {
            eventAgent.Publish(new CentrifugeRunningStatusChanged(centrifugeParentTransform, centrifugeStatus));
        }
        
        #endregion
    }
}