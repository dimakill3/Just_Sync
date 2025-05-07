namespace _Assets.Scripts.Core.Infrastructure.StateMachine
{
    public abstract class GameLoopState : IState
    {
        protected StateMachine StateMachine;

        public GameLoopState(StateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        public virtual void OnExit()
        {
        }

        public virtual void OnEnter()
        {
        }
    }
}