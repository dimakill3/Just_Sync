using System;
using System.Collections.Generic;

namespace _Assets.Scripts.Core.Infrastructure.StateMachine
{
    public class StateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states = new();
        private IExitableState _currentState;

        public void AddState<TState>(TState state) where TState : IExitableState => 
            _states.Add(typeof(TState), state);
        
        public void Enter<TState>() where TState : class, IState
        {
            var state = ChangeState<TState>();
            state.OnEnter();
        }

        public void Enter<TState, TParam>(TParam param) where TState : class, IParamState<TParam>
        {
            var state = ChangeState<TState>();
            state.OnEnter(param);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _currentState?.OnExit();
            
            var state = GetState<TState>();
            _currentState = state;
            
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState =>
            _states[typeof(TState)] as TState;
    }
}