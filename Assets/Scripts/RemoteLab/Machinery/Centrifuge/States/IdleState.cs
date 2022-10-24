using TreeislandStudio.Engine.Character;
using TreeislandStudio.Engine.StateMachine;

namespace RemoteLab.Machinery.Centrifuge.States
{
    public class IdleState : State
    {
        private readonly ICentrifuge centrifuge;
        
        public IdleState(IMachinery instrument, StateMachine stateMachine) : base(instrument, stateMachine)
        {
            StateName = "Idle";
            centrifuge = (ICentrifuge) instrument;
        }
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!centrifuge.IsLidOpened)
            {
                return;
            }
            
            StateMachine.ChangeState(centrifuge.OpenTopCoverState);
        }
    }
}