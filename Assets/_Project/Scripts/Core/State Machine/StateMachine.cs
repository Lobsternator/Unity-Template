using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Core
{
    public interface IStateMachine { }

    public abstract class StateMachine<TStateMachine> : MonoBehaviour, IStateMachine where TStateMachine : MonoBehaviour, IStateMachine
    {
        protected State<TStateMachine> _state;

        public event Action<State<TStateMachine>> BeforeStateDisabled;
        public event Action<State<TStateMachine>> BeforeStateEnabled;
        public event Action<State<TStateMachine>> StateDisabled;
        public event Action<State<TStateMachine>> StateEnabled;

        protected virtual IEnumerator EnableState(State<TStateMachine> state)
        {
            BeforeStateEnabled?.Invoke(state);
            yield return state.OnEnable();
            StateEnabled?.Invoke(state);
        }
        protected virtual IEnumerator DisableState(State<TStateMachine> state)
        {
            BeforeStateDisabled?.Invoke(state);
            yield return state.OnDisable();
            StateDisabled?.Invoke(state);
        }
        protected virtual void UpdateState(State<TStateMachine> state)
        {
            state.OnUpdate();
        }
        protected virtual void LateUpdateState(State<TStateMachine> state)
        {
            state.OnLateUpdate();
        }
        protected virtual void FixedUpdateState(State<TStateMachine> state)
        {
            state.OnFixedUpdate();
        }

        public State<TStateMachine> GetState()
        {
            return _state;
        }
        public TState GetState<TState>() where TState : State<TStateMachine>
        {
            return _state as TState;
        }

        public virtual bool SetState<TState>(TState state) where TState : State<TStateMachine>
        {
            if (_state == state)
                return false;

            if (_state is object)
                StartCoroutine(DisableState(_state));

            _state = state;

            if (_state is object)
                StartCoroutine(EnableState(_state));

            return true;
        }

        public bool HasState(Type stateType)
        {
            return _state.GetType() == stateType;
        }
        public bool HasState<TState>() where TState : State<TStateMachine>
        {
            return _state is TState;
        }

        public bool StateTransition(int input)
        {
            if (_state is null)
                return false;

            State<TStateMachine> newState = _state.GetTransition(input);
            if (newState is null)
                return false;

            return SetState(newState);
        }

        protected virtual void Update()
        {
            if (_state is object)
                UpdateState(_state);
        }
        protected virtual void LateUpdate()
        {
            if (_state is object)
                LateUpdateState(_state);
        }
        protected virtual void FixedUpdate()
        {
            if (_state is object)
                FixedUpdateState(_state);
        }
    }

    public interface IStateManager<TStateMachine> where TStateMachine : MonoBehaviour, IStateMachine
    {
        public TStateMachine StateMachine { get; }
        public Dictionary<Type, State<TStateMachine>> States { get; }

        public TState GetState<TState>() where TState : State<TStateMachine>;
        public bool SetState<TState>() where TState : State<TStateMachine>;
    }
}
