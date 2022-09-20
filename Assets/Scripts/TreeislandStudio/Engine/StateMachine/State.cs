using TreeislandStudio.Engine.Character;

namespace TreeislandStudio.Engine.StateMachine
{
    public abstract class State
    {
        public string AnimatorStateName = string.Empty;
        protected readonly ICharacter Character;
        protected readonly StateMachine StateMachine;

        protected State(ICharacter character, StateMachine stateMachine)
        {
            Character = character; 
            StateMachine = stateMachine;
        }

        public virtual void Enter() { }

        public virtual void HandleInput() {}

        public virtual void LogicUpdate() {}

        public virtual void PhysicsUpdate() {}

        public virtual void Exit() {}
    }
}
