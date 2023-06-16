using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Template.Core
{
    /// <summary>
    /// Covering type for <see cref="StateMachine{TStateMachine, TBaseState}"/>.
    /// </summary>
    public interface IStateMachine
    {
        public bool HasState(Type stateType);
        public bool StateTransition(int input);
    }
    /// <summary>
    /// Covering type for <see cref="StateMachine{TStateMachine, TBaseState}"/>.
    /// </summary>
    /// <typeparam name="TStateMachine">Should be the inheriting class.</typeparam>
    public interface IStateMachine<TStateMachine, TBaseState> : IStateMachine where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, TBaseState> where TBaseState : State<TStateMachine, TBaseState>
    {
        public TBaseState GetState();
        public TState GetState<TState>() where TState : TBaseState;
        public bool SetState<TState>(TState state) where TState : TBaseState;
        public bool HasState<TState>() where TState : TBaseState;
    }
    /// <summary>
    /// Covering type for <see cref="StateMachine{TStateMachine, TBaseState}"/>.
    /// </summary>
    /// <typeparam name="TStateMachine">Should be the inheriting class.</typeparam>
    public interface IStateMachine<TStateMachine> : IStateMachine<TStateMachine, State<TStateMachine>> where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, State<TStateMachine>> { }

    /// <summary>
    /// Base class for any finite state machine.
    /// </summary>
    /// <typeparam name="TStateMachine">Should be the inheriting class.</typeparam>
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

        public virtual bool StateTransition(int input)
        {
            if (_state is null)
                return false;

            if (!_state.GetTransition(input, out var newState))
                return false;

            return SetState(newState);
        }
    }
    /// <summary>
    /// Base class for any finite state machine.
    /// </summary>
    /// <typeparam name="TStateMachine">Should be the inheriting class.</typeparam>
    public abstract class StateMachine<TStateMachine> : StateMachine<TStateMachine, State<TStateMachine>>, IStateMachine<TStateMachine> where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, State<TStateMachine>> { }

    /// <summary>
    /// Interface for any state machine which has an accompanying <see cref="IStateManager"/>.
    /// </summary>
    public interface IManagedStateMachine : IStateMachine { }
    /// <summary>
    /// Interface for any state machine which has an accompanying <see cref="IStateManager"/>.
    /// </summary>
    /// <typeparam name="TStateMachine">Should be the inheriting class.</typeparam>
    public interface IManagedStateMachine<TStateManager, TStateMachine, TBaseState> : IStateMachine<TStateMachine, TBaseState>, IManagedStateMachine where TStateManager : IStateManager<TStateMachine, TBaseState> where TStateMachine : MonoBehaviour, IManagedStateMachine<TStateManager, TStateMachine, TBaseState> where TBaseState : State<TStateMachine, TBaseState>
    {
        public TStateManager StateManager { get; }
    }
    /// <summary>
    /// Interface for any state machine which has an accompanying <see cref="IStateManager"/>.
    /// </summary>
    /// <typeparam name="TStateMachine">Should be the inheriting class.</typeparam>
    public interface IManagedStateMachine<TStateManager, TStateMachine> : IManagedStateMachine<TStateManager, TStateMachine, State<TStateMachine>>, IStateMachine<TStateMachine> where TStateManager : IStateManager<TStateMachine, State<TStateMachine>> where TStateMachine : MonoBehaviour, IManagedStateMachine<TStateManager, TStateMachine, State<TStateMachine>> { }

    /// <summary>
    /// Covering type for <see cref="IStateManager{TStateMachine, TBaseState}"/>.
    /// </summary>
    public interface IStateManager { }
    /// <summary>
    /// For objects that manage and hold states related to a particular <see cref="StateMachine{TStateMachine, TBaseState}"/>.
    /// </summary>
    /// <typeparam name="TStateMachine">The state machine which is being managed.</typeparam>
    public interface IStateManager<TStateMachine, TBaseState> : IStateManager where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, TBaseState> where TBaseState : State<TStateMachine, TBaseState>
    {
        public TStateMachine StateMachine { get; set; }
        public ReadOnlyDictionary<Type, TBaseState> States { get; }

        public void Initialize(TStateMachine stateMachine);

        public TState GetState<TState>() where TState : TBaseState;
        public bool SetState<TState>() where TState : TBaseState;
    }
    /// <summary>
    /// For objects that manage and hold states related to a particular <see cref="StateMachine{TStateMachine, TBaseState}"/>.
    /// </summary>
    /// <typeparam name="TStateMachine">The state machine which is being managed.</typeparam>
    public interface IStateManager<TStateMachine> : IStateManager<TStateMachine, State<TStateMachine>> where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, State<TStateMachine>> { }

    /// <summary>
    /// For objects that manage and hold states related to a particular <see cref="StateMachine{TStateMachine, TBaseState}"/>.
    /// </summary>
    /// <typeparam name="TStateMachine">The state machine which is being managed.</typeparam>
    public abstract class StateManager<TStateMachine, TBaseState> : MonoBehaviour, IStateManager<TStateMachine, TBaseState> where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, TBaseState> where TBaseState : State<TStateMachine, TBaseState>
    {
        public abstract TStateMachine StateMachine { get; set; }
        public abstract ReadOnlyDictionary<Type, TBaseState> States { get; }

        public virtual void Initialize(TStateMachine stateMachine)
        {
            StateMachine = stateMachine;
            foreach (var state in States.Values)
                state.Initialize(stateMachine);
        }

        public virtual TState GetState<TState>() where TState : TBaseState
        {
            if (States.TryGetValue(typeof(TState), out var state))
                return (TState)state;

            return null;
        }

        public virtual bool SetState<TState>() where TState : TBaseState
        {
            if (States.TryGetValue(typeof(TState), out var state))
                return StateMachine.SetState(state);

            return false;
        }
    }
    /// <summary>
    /// For objects that manage and hold states related to a particular <see cref="StateMachine{TStateMachine, TBaseState}"/>.
    /// </summary>
    /// <typeparam name="TStateMachine">The state machine which is being managed.</typeparam>
    public abstract class StateManager<TStateMachine> : StateManager<TStateMachine, State<TStateMachine>>, IStateManager<TStateMachine> where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, State<TStateMachine>> { }
}
