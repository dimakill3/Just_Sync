namespace _Assets.Scripts.Core.Infrastructure.StateMachine
{
    public interface IState : IExitableState
    {
        public void OnEnter();
    }
    
    public interface IExitableState
    {
        public void OnExit();
    }
    
    public interface IParamState<TParam> : IExitableState
    {
        public void OnEnter(TParam sceneName);
    }
}