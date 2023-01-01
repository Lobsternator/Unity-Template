using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Core
{
    public interface IStateMachine
    {
        public bool HasState(Type stateType);
        public bool StateTransition(int input);
    }
    public interface IStateMachine<TStateMachine, TBaseState> : IStateMachine where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, TBaseState> where TBaseState : State<TStateMachine, TBaseState>
    {
        public TBaseState GetState();
        public TState GetState<TState>() where TState : TBaseState;
        public bool SetState<TState>(TState state) where TState : TBaseState;
        public bool HasState<TState>() where TState : TBaseState;
    }
    public interface IStateMachine<TStateMachine> : IStateMachine<TStateMachine, State<TStateMachine>> where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, State<TStateMachine>> { }

    public abstract class StateMachine<TStateMachine, TBaseState> : MonoBehaviour, IStateMachine<TStateMachine, TBaseState> where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, TBaseState> where TBaseState : State<TStateMachine, TBaseState>
    {
        protected TBaseState _state;

        protected virtual IEnumerator EnableState(TBaseState state)
        {
            yield return state.OnEnable();
        }
        protected virtual IEnumerator DisableState(TBaseState state)
        {
            yield return state.OnDisable();
        }
        protected virtual void UpdateState(TBaseState state)
        {
            state.OnUpdate();
        }
        protected virtual void LateUpdateState(TBaseState state)
        {
            state.OnLateUpdate();
        }
        protected virtual void FixedUpdateState(TBaseState state)
        {
            state.OnFixedUpdate();
        }

        public TBaseState GetState()
        {
            return _state;
        }
        public TState GetState<TState>() where TState : TBaseState
        {
            return _state as TState;
        }

        public virtual bool SetState<TState>(TState state) where TState : TBaseState
        {
            if (_state == state)
                return false;

            if (_state is not null)
                StartCoroutine(DisableState(_state));

            _state = state;

            if (_state is not null)
                StartCoroutine(EnableState(_state));

            return true;
        }

        public bool HasState(Type stateType)
        {
            return _state.GetType() == stateType;
        }
        public bool HasState<TState>() where TState : TBaseState
        {
            return _state is TState;
        }

        public bool StateTransition(int input)
        {
            if (_state is null)
                return false;

            TBaseState newState = _state.GetTransition(input);
            if (newState is null)
                return false;

            return SetState(newState);
        }

        protected virtual void Update()
        {
            if (_state is not null)
                UpdateState(_state);
        }
        protected virtual void LateUpdate()
        {
            if (_state is not null)
                LateUpdateState(_state);
        }
        protected virtual void FixedUpdate()
        {
            if (_state is not null)
                FixedUpdateState(_state);
        }
    }
    public abstract class StateMachine<TStateMachine> : StateMachine<TStateMachine, State<TStateMachine>>, IStateMachine<TStateMachine> where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, State<TStateMachine>> { }

    public interface IStateManager { }
    public interface IStateManager<TStateMachine, TBaseState> : IStateManager where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, TBaseState> where TBaseState : State<TStateMachine, TBaseState>
    {
        public TStateMachine StateMachine { get; }
        public Dictionary<Type, TBaseState> States { get; }

        public TState GetState<TState>() where TState : TBaseState;
        public bool SetState<TState>() where TState : TBaseState;
    }
    public interface IStateManager<TStateMachine> : IStateManager<TStateMachine, State<TStateMachine>> where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, State<TStateMachine>> { }
}
