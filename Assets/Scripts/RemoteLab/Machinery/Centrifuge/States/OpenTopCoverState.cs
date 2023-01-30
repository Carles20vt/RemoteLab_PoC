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

            if (!centrifuge.IsLidOpened)
            {
                if (centrifuge.IsSampleInside)
                    StateMachine.ChangeState(centrifuge.ClosedTopCoverState);
                else
                    StateMachine.ChangeState(centrifuge.IdleState);
            }
        }
    }
}