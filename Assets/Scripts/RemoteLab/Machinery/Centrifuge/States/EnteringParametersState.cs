using TreeislandStudio.Engine.Character;
using TreeislandStudio.Engine.StateMachine;

namespace RemoteLab.Machinery.Centrifuge.States
{
    public class EnteringParametersState : State
    {
        private readonly ICentrifuge centrifuge;
        
        public EnteringParametersState(IMachinery instrument, StateMachine stateMachine) : base(instrument, stateMachine)
        {
            StateName = "EnteringParametersState";
            centrifuge = (ICentrifuge) instrument;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (centrifuge.IsEnteringParameters)
                return;

            if (centrifuge.IsParametersEntered)
            {
                StateMachine.ChangeState(centrifuge.RunningState);
                return;
            }
            
            StateMachine.ChangeState(centrifuge.ReadyToEnterParametersState);
        }
    }
}