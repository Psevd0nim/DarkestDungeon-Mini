using System;
using System.Collections.Generic;

namespace MyProject
{
    public class GameStateMachine
    {
        public AppServices AppServices { get; private set; }

        private Dictionary<Type, IState> _states;
        private IState _activeState;

        public GameStateMachine()
        {
            _states = new Dictionary<Type, IState>
            {
                [typeof(BootstrapState)] = new BootstrapState(this),
                [typeof(LoadLevelState)] = new LoadLevelState(this),
                [typeof(StartGameState)] = new StartGameState(this)
            };
        }

        public void SetAppServices(AppServices appServices)
        {
            AppServices = appServices;
        }

        public void Enter<TState>() where TState : class, IDefaultState
        {
            ChangeState<TState>().Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            ChangeState<TState>().Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IState
        {
            TState nextState = (TState)_states[typeof(TState)];
            _activeState?.Exit();
            _activeState = nextState;
            return nextState;
        }
    }
}
