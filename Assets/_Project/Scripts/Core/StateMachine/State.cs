using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Template.Core
{
    public interface IState
    {
        public bool Enabled { get; }

        public bool RemoveTransition(int input);

        public IEnumerator OnEnable();
        public IEnumerator OnDisable();
    }
    public interface IState<TStateMachine, TBaseState> : IState where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, TBaseState> where TBaseState : State<TStateMachine, TBaseState>
    {
        public TStateMachine StateMachine { get; set; }

        // NOTE: Has to be called manually!
        public void Initialize(TStateMachine stateMachine);

        public bool AddTransition(int input, TBaseState output);
        public bool GetTransition(int input, out TBaseState state);
        public bool SetTransition(int input, TBaseState output);
    }
    public interface IState<TStateMachine> : IState<TStateMachine, State<TStateMachine>> where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, State<TStateMachine>> { }

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
    [Serializable]
    public abstract class State<TStateMachine> : State<TStateMachine, State<TStateMachine>>, IState<TStateMachine> where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, State<TStateMachine>> { }

    public interface IStateContainer { }
    public interface IStateContainer<TStateMachine, TBaseState> : IStateContainer where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, TBaseState> where TBaseState : State<TStateMachine, TBaseState>
    {
        public ReadOnlyDictionary<Type, TBaseState> States { get; }
    }
    public interface IStateContainer<TStateMachine> : IStateContainer<TStateMachine, State<TStateMachine>> where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, State<TStateMachine>> { }
}
