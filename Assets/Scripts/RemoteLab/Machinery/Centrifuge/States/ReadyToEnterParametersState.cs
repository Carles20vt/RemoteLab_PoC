using TreeislandStudio.Engine.Character;
using TreeislandStudio.Engine.StateMachine;

namespace RemoteLab.Machinery.Centrifuge.States
{
    public class ReadyToEnterParametersState : State
    {
        private readonly ICentrifuge centrifuge;

        public ReadyToEnterParametersState(IMachinery instrument, StateMachine stateMachine) : base(instrument, stateMachine)
        {
            StateName = "ReadyToEnterParametersState";
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
            if (centrifuge.IsEnteringParameters)
            {
                StateMachine.ChangeState(centrifuge.EnteringParametersState);
                return;
            }
        }
    }
}