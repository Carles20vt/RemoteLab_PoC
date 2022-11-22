using System.Collections.Generic;
using RemoteLab.Machinery.Centrifuge.Lid;
using RemoteLab.Machinery.Centrifuge.Lid.Messages;
using RemoteLab.Machinery.Centrifuge.Rotor.Messages;
using RemoteLab.Supplies;
using TreeislandStudio.Engine;
using TreeislandStudio.Engine.Environment;
using TreeislandStudio.Engine.Event;
using UnityEngine;
using Zenject;

namespace RemoteLab.Machinery.Centrifuge.Rotor
{
    [RequireComponent(typeof(MeshRenderer))]
    public class CentrifugeRotor : TreeislandBehaviour
    {
        #region Public Properties

        [SerializeField] private GameObject lidGameObject;

        #endregion
        
        #region Private Properties
        
        private Transform centrifugeParentTransform;

        private MeshRenderer meshRenderer;

        private List<Vial> vials;

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
            vials = new List<Vial>();
            
            centrifugeParentTransform = GetComponentInParent<Centrifuge>().transform;
            meshRenderer = GetComponent<MeshRenderer>();
            lidGameObject ??= transform.parent.GetComponentInChildren<CentrifugeLid>()?.gameObject;
            
            SetRotorColor(false);
        }

        /// <summary>
        /// Destroy event
        /// </summary>
        private void OnDestroy()
        {
            eventAgent.Dispose();
        }

        private void OnCollisionEnter(Collision collision)
        {
            var vial = collision.gameObject.GetComponent<Vial>();

            if (vial == null)
            {
                return;
            }
            
            vials.Add(vial);
            CheckRotorCompartment();
        }
        
        private void OnCollisionExit(Collision collision)
        {
            var vial = collision.gameObject.GetComponent<Vial>();

            if (vial == null)
            {
                return;
            }
            
            vials.Remove(vial);
            CheckRotorCompartment();
        }

        #endregion
        
        #region Events

        private void SubscribeToEvents()
        {
            eventAgent.Subscribe<CentrifugeLidChanged>(OnCentrifugeLidChanged);
        }
        
        private void OnCentrifugeLidChanged(CentrifugeLidChanged message)
        {
            if (!ReferenceEquals(centrifugeParentTransform, message.Sender))
            {
                return;
            }

            SetRotorColor(message.IsLidOpen);
        }
        
        #endregion

        #region Private Methods

        private void SetRotorColor(bool lidOpen)
        {
            if (meshRenderer == null)
            {
                return;
            }
            
            if (lidOpen)
            {
                meshRenderer.material.color = Color.green;
                return;
            }
            
            meshRenderer.material.color = Color.red;
        }

        private void CheckRotorCompartment()
        {
            eventAgent.Publish(new CentrifugeRotorChanged(centrifugeParentTransform, vials.Count > 0));
        }

        #endregion
    }
}