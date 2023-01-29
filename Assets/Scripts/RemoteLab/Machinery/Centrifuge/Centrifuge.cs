using System;
using RemoteLab.Machinery.Centrifuge.Lid.Messages;
using RemoteLab.Machinery.Centrifuge.Rotor.Messages;
using RemoteLab.Machinery.Centrifuge.Screen.Messages;
using RemoteLab.Machinery.Centrifuge.States;
using TreeislandStudio.Engine;
using TreeislandStudio.Engine.Environment;
using TreeislandStudio.Engine.Event;
using Zenject;

namespace RemoteLab.Machinery.Centrifuge
{
    public class Centrifuge : TreeislandBehaviour, ICentrifuge, IDisposable
    {
        #region Public Properties
        public bool IsLidOpened { get; private set; }
        public bool IsSampleInside { get; private set; }
        public bool IsParametersEntered { get; private set; }
        public bool IsCentrifugationFinished { get; private set; }

        public IdleState IdleState { get; private set; }
        public ClosedTopCoverState ClosedTopCoverState { get; private set; }
        public EnterParametersState EnterParametersState { get; private set; }
        public OpenTopCoverState OpenTopCoverState { get; private set; }
        public RemoveSamplesState RemoveSamplesState { get; private set; }
        public RunningState RunningState { get; private set; }

        #endregion
        
        #region Private Properties
        
        private bool disposed;
        
        private TreeislandStudio.Engine.StateMachine.StateMachine instrumentStateMachine;
        
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
        public void Initialize(IEnvironmentSetUp environmentSetUp)
        {
            eventAgent = new EventAgent(environmentSetUp.EventBroker);
        }

        #endregion
        
        #region MonoBehaviour Callbacks

        public void Awake()
        {
            SubscribeToEvents();
        }

        public void Start()
        {
            SetupInstrumentStateMachine();
        }

        /// <summary>
        /// Destroy event
        /// </summary>
        private void OnDestroy()
        {
            eventAgent.Dispose();
        }

        /// <summary>
        /// Standard Unity function called once every frame after update.
        /// </summary>
        public void LateUpdate()
        {
            instrumentStateMachine.CurrentState?.HandleInput();
            instrumentStateMachine.CurrentState?.LogicUpdate();
        }

        public void FixedUpdate()
        {
            instrumentStateMachine.CurrentState?.PhysicsUpdate();
        }

        #endregion
        
        #region Events

        private void SubscribeToEvents()
        {
            eventAgent.Subscribe<CentrifugeLidChanged>(OnCentrifugeLidChanged);
            eventAgent.Subscribe<CentrifugeRotorChanged>(OnCentrifugeRotorChanged);
            eventAgent.Subscribe<CentrifugeRunningStatusChanged>(OnCentrifugeRunningStatusChanged);
        }
        
        private void OnCentrifugeLidChanged(CentrifugeLidChanged message)
        {
            if (!ReferenceEquals(transform, message.Sender))
            {
                return;
            }

            IsLidOpened = message.IsLidOpen;
        }
        
        private void OnCentrifugeRotorChanged(CentrifugeRotorChanged message)
        {
            if (!ReferenceEquals(transform, message.Sender))
            {
                return;
            }

            IsSampleInside = message.IsRotorWithVialsInside;
        }
        
        private void OnCentrifugeRunningStatusChanged(CentrifugeRunningStatusChanged message)
        {
            if (!ReferenceEquals(transform, message.Sender))
            {
                return;
            }

            IsParametersEntered = message.IsRunning;
            IsCentrifugationFinished = !message.IsRunning;
        }
        
        #endregion
        
        #region Public Methods
        
        public void Dispose()
        {
            if (disposed)
            {
                return;
            }
            
            disposed = true;
        }
        
        #endregion

        #region Private Methods

        private void SetupInstrumentStateMachine()
        {
            if (instrumentStateMachine != null)
            {
                return;
            }
            
            instrumentStateMachine = new TreeislandStudio.Engine.StateMachine.StateMachine(eventAgent, transform);
            
            IdleState = new IdleState(this, instrumentStateMachine);
            ClosedTopCoverState = new ClosedTopCoverState(this, instrumentStateMachine);
            EnterParametersState = new EnterParametersState(this, instrumentStateMachine);
            OpenTopCoverState = new OpenTopCoverState(this, instrumentStateMachine);
            RemoveSamplesState = new RemoveSamplesState(this, instrumentStateMachine);
            RunningState = new RunningState(this, instrumentStateMachine);
            
            instrumentStateMachine.Initialize(IdleState);
        }

        #endregion
    }
}