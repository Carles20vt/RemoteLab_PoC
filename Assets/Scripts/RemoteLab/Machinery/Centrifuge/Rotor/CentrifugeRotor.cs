using RemoteLab.Machinery.Centrifuge.Lid;
using RemoteLab.Machinery.Centrifuge.Lid.Messages;
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

        private MeshRenderer meshRenderer;

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

        #endregion
        
        #region Events

        private void SubscribeToEvents()
        {
            eventAgent.Subscribe<CentrifugeLidChanged>(OnCentrifugeLidChanged);
        }
        
        private void OnCentrifugeLidChanged(CentrifugeLidChanged message)
        {
            if (!ReferenceEquals(lidGameObject.transform, message.Sender))
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

        #endregion
    }
}