using TreeislandStudio.Engine.Character;

namespace TreeislandStudio.Engine.StateMachine
{
    public abstract class State
    {
        public string StateName = string.Empty;
        
        #region Private Properties
        
        private readonly IMachinery machinery;
        protected readonly StateMachine StateMachine;
        
        #endregion

        protected State(IMachinery machinery, StateMachine stateMachine)
        {
            this.machinery = machinery; 
            this.StateMachine = stateMachine;
        }

        public virtual void Enter() {}

        public virtual void HandleInput() {}

        public virtual void LogicUpdate() {}

        public virtual void PhysicsUpdate() {}

        public virtual void Exit() {}
    }
}
