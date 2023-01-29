using RemoteLab.Machinery.Centrifuge.States;
using TreeislandStudio.Engine.Character;

namespace RemoteLab.Machinery.Centrifuge
{
    public interface ICentrifuge : IMachinery
    {
        bool IsLidOpened { get; }

        bool IsSampleInside { get; }
        
        bool IsEnteringParameters { get; }
        
        IdleState IdleState { get; }
        
        bool IsParametersEntered { get; }
        
        bool IsCentrifugationFinished { get; }
        
        
        ClosedTopCoverState ClosedTopCoverState { get; }
        EnterParametersState EnterParametersState { get; }
        OpenTopCoverState OpenTopCoverState { get; }
        RemoveSamplesState RemoveSamplesState { get; }
        RunningState RunningState { get; }
    }
}