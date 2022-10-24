using TreeislandStudio.Engine.Character;
using TreeislandStudio.Engine.StateMachine;

namespace RemoteLab.Machinery.Centrifuge.States
{
    public class RunningState : State
    {
        private readonly ICentrifuge centrifuge;
        
        public RunningState(IMachinery instrument, StateMachine stateMachine) : base(instrument, stateMachine)
        {
            StateName = "RunningState";
            centrifuge = (ICentrifuge) instrument;
        }
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (centrifuge.IsCentrifugationFinished)
            {
                StateMachine.ChangeState(centrifuge.RemoveSamplesState);
            }
        }
    }
}