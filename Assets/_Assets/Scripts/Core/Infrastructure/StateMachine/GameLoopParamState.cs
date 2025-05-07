namespace _Assets.Scripts.Core.Infrastructure.StateMachine
{
    public abstract class GameLoopParamState<TParam> : IParamState<TParam>
    {
        protected StateMachine StateMachine;

        public GameLoopParamState(StateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        public abstract void OnEnter(TParam sceneName);

        public abstract void OnExit();
    }
}