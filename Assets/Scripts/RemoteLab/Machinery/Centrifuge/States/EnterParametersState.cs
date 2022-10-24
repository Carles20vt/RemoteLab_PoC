using TreeislandStudio.Engine.Character;
using TreeislandStudio.Engine.StateMachine;

namespace RemoteLab.Machinery.Centrifuge.States
{
    public class EnterParametersState : State
    {
        private readonly ICentrifuge centrifuge;
        
        public EnterParametersState(IMachinery instrument, StateMachine stateMachine) : base(instrument, stateMachine)
        {
            StateName = "EnterParametersState";
            centrifuge = (ICentrifuge) instrument;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (centrifuge.IsParametersEntered)
            {
                StateMachine.ChangeState(centrifuge.RunningState);
            }
            
            if (!centrifuge.IsParametersEntered && centrifuge.IsLidOpened)
            {
                StateMachine.ChangeState(centrifuge.OpenTopCoverState);
            }
        }
    }
}