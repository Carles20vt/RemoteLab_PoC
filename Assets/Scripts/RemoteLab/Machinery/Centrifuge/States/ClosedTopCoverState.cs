using TreeislandStudio.Engine.Character;
using TreeislandStudio.Engine.StateMachine;

namespace RemoteLab.Machinery.Centrifuge.States
{
    public class ClosedTopCoverState : State
    {
        private readonly ICentrifuge centrifuge;

        public ClosedTopCoverState(IMachinery instrument, StateMachine stateMachine) : base(instrument, stateMachine)
        {
            StateName = "ClosedTopCoverState";
            centrifuge = (ICentrifuge) instrument;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (centrifuge.IsLidOpened)
            {
                StateMachine.ChangeState(centrifuge.OpenTopCoverState);
                return;
            }
            if (centrifuge.IsParametersEntered)
            {
                StateMachine.ChangeState(centrifuge.RunningState);
            }
            if (centrifuge.IsEnteringParameters)
            {
                StateMachine.ChangeState(centrifuge.EnterParametersState);
            }

        }
    }
}