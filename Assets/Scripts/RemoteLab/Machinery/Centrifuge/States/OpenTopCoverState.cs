using TreeislandStudio.Engine.Character;
using TreeislandStudio.Engine.StateMachine;

namespace RemoteLab.Machinery.Centrifuge.States
{
    public class OpenTopCoverState : State
    {
        private readonly ICentrifuge centrifuge;
        
        public OpenTopCoverState(IMachinery instrument, StateMachine stateMachine) : base(instrument, stateMachine)
        {
            StateName = "OpenTopCoverState";
            centrifuge = (ICentrifuge) instrument;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!centrifuge.IsLidOpened && centrifuge.IsSampleInside)
            {
                StateMachine.ChangeState(centrifuge.EnterParametersState);
                return;
            }

            if (centrifuge.IsLidOpened || centrifuge.IsSampleInside)
            {
                return;
            }
            
            StateMachine.ChangeState(centrifuge.IdleState);
        }
    }
}