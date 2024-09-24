using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Template.Core
{
    /// <summary>
    /// Covering type for <see cref="State{TStateMachine, TState}"/>.
    /// </summary>
    public interface IState
    {
        public bool Enabled { get; }

        public bool RemoveTransition(int input);

        public IEnumerator OnEnable();
        public IEnumerator OnDisable();
    }
    /// <summary>
    /// Covering type for <see cref="State{TStateMachine, TState}"/>.
    /// </summary>
    /// <typeparam name="TStateMachine">The state machine this state belongs to.</typeparam>
    /// <typeparam name="TState">Should be the inheriting class.</typeparam>
    public interface IState<TStateMachine, TState> : IState where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, TState> where TState : State<TStateMachine, TState>
    {
        public TStateMachine StateMachine { get; set; }

        // NOTE: Has to be called manually!
        public void Initialize(TStateMachine stateMachine);

        public bool AddTransition(int input, TState output);
        public bool GetTransition(int input, out TState state);
        public bool SetTransition(int input, TState output);
    }
    /// <summary>
    /// Covering type for <see cref="State{TStateMachine, TState}"/>.
    /// </summary>
    /// <typeparam name="TStateMachine">The state machine this state belongs to.</typeparam>
    public interface IState<TStateMachine> : IState<TStateMachine, State<TStateMachine>> where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, State<TStateMachine>> { }

    /// <summary>
    /// Base state for any state belonging to a <see cref="StateMachine{TStateMachine, TState}"/>.
    /// </summary>
    /// <typeparam name="TStateMachine">The state machine this state belongs to.</typeparam>
    /// <typeparam name="TState">Should be the inheriting class.</typeparam>
    [Serializable]
    public abstract class State<TStateMachine, TState> : IState<TStateMachine, TState> where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, TState> where TState : State<TStateMachine, TState>
    {
        public bool Enabled { get; set; }
        public TStateMachine StateMachine { get; set; }

        protected Dictionary<int, TState> _transitions = new Dictionary<int, TState>();

        public virtual void Initialize(TStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        public bool AddTransition(int input, TState output)
        {
            return _transitions.TryAdd(input, output);
        }
        public bool GetTransition(int input, out TState state)
        {
            return _transitions.TryGetValue(input, out state);
        }
        public bool SetTransition(int input, TState output)
        {
            if (_transitions.ContainsKey(input))
                _transitions[input] = output;
            else
                return false;
            
            return true;
        }
        public bool RemoveTransition(int input)
        {
            return _transitions.Remove(input);
        }

        public virtual IEnumerator OnEnable()
        {
            yield break;
        }
        public virtual IEnumerator OnDisable()
        {
            yield break;
        }
    }
    /// <summary>
    /// Base state for any state belonging to a <see cref="StateMachine{TStateMachine, TState}"/>.
    /// </summary>
    /// <typeparam name="TStateMachine">The state machine this state belongs to.</typeparam>
    [Serializable]
    public abstract class State<TStateMachine> : State<TStateMachine, State<TStateMachine>>, IState<TStateMachine> where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, State<TStateMachine>> { }

    /// <summary>
    /// Covering type for <see cref="IState{TStateMachine, TState}"/>.
    /// </summary>
    public interface IStateContainer { }
    /// <summary>
    /// If a class inherits from this interface and the inheriting class has the <see cref="GenerateStateContainerAttribute"/>, the class will be populated with serialized fields for all states belonging to <typeparamref name="TStateMachine"/>.
    /// </summary>
    /// <typeparam name="TStateMachine">The target state machine to generate the fields for.</typeparam>
    public interface IStateContainer<TStateMachine, TState> : IStateContainer where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, TState> where TState : State<TStateMachine, TState>
    {
        public ReadOnlyDictionary<Type, TState> States { get; }
    }
    /// <summary>
    /// If a class inherits from this interface and the inheriting class has the <see cref="GenerateStateContainerAttribute"/>, the class will be populated with serialized fields for all states belonging to <typeparamref name="TStateMachine"/>.
    /// </summary>
    /// <typeparam name="TStateMachine">The target state machine to generate the fields for.</typeparam>
    public interface IStateContainer<TStateMachine> : IStateContainer<TStateMachine, State<TStateMachine>> where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, State<TStateMachine>> { }

    /// <summary>
    /// If a class inherits from <see cref="IStateContainer{TStateMachine, TState}"/> and the inheriting class has this attribute, the class will be populated with serialized fields for all states belonging to the TStateMachine defined by the <see cref="IStateContainer{TStateMachine, TState}"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class GenerateStateContainerAttribute : Attribute
    {

    }
}
