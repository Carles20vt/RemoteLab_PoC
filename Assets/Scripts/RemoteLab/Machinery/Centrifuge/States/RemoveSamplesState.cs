using TreeislandStudio.Engine.Character;
using TreeislandStudio.Engine.StateMachine;

namespace RemoteLab.Machinery.Centrifuge.States
{
    public class RemoveSamplesState : State
    {
        private readonly ICentrifuge centrifuge;
        
        public RemoveSamplesState(IMachinery instrument, StateMachine stateMachine) : base(instrument, stateMachine)
        {
            StateName = "RemoveSamplesState";
            centrifuge = (ICentrifuge) instrument;
        }
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (centrifuge.IsCentrifugationFinished 
                && !centrifuge.IsSampleInside)
            {
                StateMachine.ChangeState(centrifuge.OpenTopCoverState);
            }
        }
    }
}