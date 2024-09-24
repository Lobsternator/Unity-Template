using System;
using System.Collections;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Template.Core
{
    /// <summary>
    /// Covering type for <see cref="StateMachine{TStateMachine, TState}"/>.
    /// </summary>
    public interface IStateMachine
    {
        public bool HasState(Type stateType);
        public bool StateTransition(int input);
    }
    /// <summary>
    /// Covering type for <see cref="StateMachine{TStateMachine, TState}"/>.
    /// </summary>
    /// <typeparam name="TStateMachine">Should be the inheriting class.</typeparam>
    public interface IStateMachine<TStateMachine, TState> : IStateMachine where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, TState> where TState : State<TStateMachine, TState>
    {
        public TState GetState();
        public TStateGet GetState<TStateGet>() where TStateGet : TState;
        public bool SetState<TStateSet>(TStateSet state) where TStateSet : TState;
        public bool HasState<TStateHas>() where TStateHas : TState;
    }
    /// <summary>
    /// Covering type for <see cref="StateMachine{TStateMachine, TState}"/>.
    /// </summary>
    /// <typeparam name="TStateMachine">Should be the inheriting class.</typeparam>
    public interface IStateMachine<TStateMachine> : IStateMachine<TStateMachine, State<TStateMachine>> where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, State<TStateMachine>> { }

    /// <summary>
    /// Base class for any finite state machine.
    /// </summary>
    /// <typeparam name="TStateMachine">Should be the inheriting class.</typeparam>
    public abstract class StateMachine<TStateMachine, TState> : MonoBehaviour, IStateMachine<TStateMachine, TState> where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, TState> where TState : State<TStateMachine, TState>
    {
        public event Action<TState> StateChanged;

        protected TState _state;
        protected Coroutine _setStateRoutine;

        protected virtual IEnumerator EnableState(TState state)
        {
            yield return state.OnEnable();
            state.Enabled = true;
        }
        protected virtual IEnumerator DisableState(TState state)
        {
            state.Enabled = false;
            yield return state.OnDisable();
        }

        public TState GetState()
        {
            return _state;
        }
        public TStateGet GetState<TStateGet>() where TStateGet : TState
        {
            return _state as TStateGet;
        }

        public bool CanChangeToState<TStateChange>(TStateChange state) where TStateChange : TState
        {
            return _setStateRoutine is null && _state != state;
        }

        protected virtual IEnumerator SetState_Routine<TStateSet>(TStateSet state) where TStateSet : TState
        {
            if (_state is not null)
                yield return DisableState(_state);

            _state = state;

            if (_state is not null)
                yield return EnableState(_state);

            _setStateRoutine = null;
            StateChanged?.Invoke(_state);
        }

        public virtual bool SetState<TStateSet>(TStateSet state) where TStateSet : TState
        {
            if (!CanChangeToState(state))
                return false;

            _setStateRoutine = StartCoroutine(SetState_Routine(state));
            return true;
        }

        public bool HasState(Type stateType)
        {
            return _state.GetType() == stateType;
        }
        public bool HasState<TStateHas>() where TStateHas : TState
        {
            return _state is TStateHas;
        }

        protected bool CanPerformTransition(int input, out TState state)
        {
            if (_state is not null && _state.GetTransition(input, out state) && CanChangeToState(state))
                return true;

            state = null;
            return false;
        }

        public bool CanPerformTransition(int input)
        {
            return CanPerformTransition(input, out _);
        }

        public virtual bool StateTransition(int input)
        {
            if (!CanPerformTransition(input, out var newState))
                return false;

            SetState(newState);
            return true;
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
    public interface IManagedStateMachine<TStateManager, TStateMachine, TState> : IStateMachine<TStateMachine, TState>, IManagedStateMachine where TStateManager : IStateManager<TStateMachine, TState> where TStateMachine : MonoBehaviour, IManagedStateMachine<TStateManager, TStateMachine, TState> where TState : State<TStateMachine, TState>
    {
        public TStateManager StateManager { get; }
    }
    /// <summary>
    /// Interface for any state machine which has an accompanying <see cref="IStateManager"/>.
    /// </summary>
    /// <typeparam name="TStateMachine">Should be the inheriting class.</typeparam>
    public interface IManagedStateMachine<TStateManager, TStateMachine> : IManagedStateMachine<TStateManager, TStateMachine, State<TStateMachine>>, IStateMachine<TStateMachine> where TStateManager : IStateManager<TStateMachine, State<TStateMachine>> where TStateMachine : MonoBehaviour, IManagedStateMachine<TStateManager, TStateMachine, State<TStateMachine>> { }

    /// <summary>
    /// Covering type for <see cref="IStateManager{TStateMachine, TState}"/>.
    /// </summary>
    public interface IStateManager { }
    /// <summary>
    /// For objects that manage and hold states related to a particular <see cref="StateMachine{TStateMachine, TState}"/>.
    /// </summary>
    /// <typeparam name="TStateMachine">The state machine which is being managed.</typeparam>
    public interface IStateManager<TStateMachine, TState> : IStateManager, IStateContainer<TStateMachine, TState> where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, TState> where TState : State<TStateMachine, TState>
    {
        public TStateMachine StateMachine { get; set; }

        public void Initialize(TStateMachine stateMachine);

        public TStateGet GetState<TStateGet>() where TStateGet : TState;
        public bool SetState<TStateSet>() where TStateSet : TState;
    }
    /// <summary>
    /// For objects that manage and hold states related to a particular <see cref="StateMachine{TStateMachine, TState}"/>.
    /// </summary>
    /// <typeparam name="TStateMachine">The state machine which is being managed.</typeparam>
    public interface IStateManager<TStateMachine> : IStateManager<TStateMachine, State<TStateMachine>> where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, State<TStateMachine>> { }

    /// <summary>
    /// For objects that manage and hold states related to a particular <see cref="StateMachine{TStateMachine, TState}"/>.
    /// </summary>
    /// <typeparam name="TStateMachine">The state machine which is being managed.</typeparam>
    public abstract class StateManager<TStateMachine, TState> : MonoBehaviour, IStateManager<TStateMachine, TState> where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, TState> where TState : State<TStateMachine, TState>
    {
        public abstract TStateMachine StateMachine { get; set; }
        public abstract ReadOnlyDictionary<Type, TState> States { get; }

        public virtual void Initialize(TStateMachine stateMachine)
        {
            StateMachine = stateMachine;
            foreach (var state in States.Values)
                state.Initialize(stateMachine);
        }

        public virtual TStateGet GetState<TStateGet>() where TStateGet : TState
        {
            if (States.TryGetValue(typeof(TStateGet), out var state))
                return (TStateGet)state;

            return null;
        }

        public virtual bool SetState<TStateSet>() where TStateSet : TState
        {
            if (States.TryGetValue(typeof(TStateSet), out var state))
                return StateMachine.SetState(state);

            return false;
        }
    }
    /// <summary>
    /// For objects that manage and hold states related to a particular <see cref="StateMachine{TStateMachine, TState}"/>.
    /// </summary>
    /// <typeparam name="TStateMachine">The state machine which is being managed.</typeparam>
    public abstract class StateManager<TStateMachine> : StateManager<TStateMachine, State<TStateMachine>>, IStateManager<TStateMachine> where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, State<TStateMachine>> { }
}
