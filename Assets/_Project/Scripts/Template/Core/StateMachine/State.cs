using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Template.Core
{
    /// <summary>
    /// Covering type for <see cref="State{TStateMachine, TBaseState}"/>.
    /// </summary>
    public interface IState
    {
        public bool Enabled { get; }

        public bool RemoveTransition(int input);

        public IEnumerator OnEnable();
        public IEnumerator OnDisable();
    }
    /// <summary>
    /// Covering type for <see cref="State{TStateMachine, TBaseState}"/>.
    /// </summary>
    /// <typeparam name="TStateMachine">The state machine this state belongs to.</typeparam>
    /// <typeparam name="TBaseState">Should be the inheriting class.</typeparam>
    public interface IState<TStateMachine, TBaseState> : IState where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, TBaseState> where TBaseState : State<TStateMachine, TBaseState>
    {
        public TStateMachine StateMachine { get; set; }

        // NOTE: Has to be called manually!
        public void Initialize(TStateMachine stateMachine);

        public bool AddTransition(int input, TBaseState output);
        public bool GetTransition(int input, out TBaseState state);
        public bool SetTransition(int input, TBaseState output);
    }
    /// <summary>
    /// Covering type for <see cref="State{TStateMachine, TBaseState}"/>.
    /// </summary>
    /// <typeparam name="TStateMachine">The state machine this state belongs to.</typeparam>
    public interface IState<TStateMachine> : IState<TStateMachine, State<TStateMachine>> where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, State<TStateMachine>> { }

    /// <summary>
    /// Base state for any state belonging to a <see cref="StateMachine{TStateMachine, TBaseState}"/>.
    /// </summary>
    /// <typeparam name="TStateMachine">The state machine this state belongs to.</typeparam>
    /// <typeparam name="TBaseState">Should be the inheriting class.</typeparam>
    [Serializable]
    public abstract class State<TStateMachine, TBaseState> : IState<TStateMachine, TBaseState> where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, TBaseState> where TBaseState : State<TStateMachine, TBaseState>
    {
        public bool Enabled => ReferenceEquals(StateMachine?.GetState(), this);
        public TStateMachine StateMachine { get; set; }

        protected Dictionary<int, TBaseState> _transitions = new Dictionary<int, TBaseState>();

        public virtual void Initialize(TStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        public bool AddTransition(int input, TBaseState output)
        {
            return _transitions.TryAdd(input, output);
        }
        public bool GetTransition(int input, out TBaseState state)
        {
            return _transitions.TryGetValue(input, out state);
        }
        public bool SetTransition(int input, TBaseState output)
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
    /// Base state for any state belonging to a <see cref="StateMachine{TStateMachine, TBaseState}"/>.
    /// </summary>
    /// <typeparam name="TStateMachine">The state machine this state belongs to.</typeparam>
    [Serializable]
    public abstract class State<TStateMachine> : State<TStateMachine, State<TStateMachine>>, IState<TStateMachine> where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, State<TStateMachine>> { }

    /// <summary>
    /// Covering type for <see cref="IState{TStateMachine, TBaseState}"/>.
    /// </summary>
    public interface IStateContainer { }
    /// <summary>
    /// If a class inherits from this interface and the inheriting class has the <see cref="GenerateStateContainerAttribute"/>, the class will be populated with serialized fields for all states belonging to <typeparamref name="TStateMachine"/>.
    /// </summary>
    /// <typeparam name="TStateMachine">The target state machine to generate the fields for.</typeparam>
    public interface IStateContainer<TStateMachine, TBaseState> : IStateContainer where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, TBaseState> where TBaseState : State<TStateMachine, TBaseState>
    {
        public ReadOnlyDictionary<Type, TBaseState> States { get; }
    }
    /// <summary>
    /// If a class inherits from this interface and the inheriting class has the <see cref="GenerateStateContainerAttribute"/>, the class will be populated with serialized fields for all states belonging to <typeparamref name="TStateMachine"/>.
    /// </summary>
    /// <typeparam name="TStateMachine">The target state machine to generate the fields for.</typeparam>
    public interface IStateContainer<TStateMachine> : IStateContainer<TStateMachine, State<TStateMachine>> where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, State<TStateMachine>> { }

    /// <summary>
    /// If a class inherits from <see cref="IStateContainer{TStateMachine, TBaseState}"/> and the inheriting class has this attribute, the class will be populated with serialized fields for all states belonging to the TStateMachine defined by the <see cref="IStateContainer{TStateMachine, TBaseState}"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class GenerateStateContainerAttribute : Attribute
    {

    }
}
