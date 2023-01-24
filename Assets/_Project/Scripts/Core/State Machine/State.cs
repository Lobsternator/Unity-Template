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

        // Should be run for every state once (NOTE: Has to be called manually!)
        public IEnumerator Initialize();

        // Should be run for a single state
        public IEnumerator OnEnable();
        public IEnumerator OnDisable();
        public void OnUpdate();
        public void OnLateUpdate();
        public void OnFixedUpdate();
    }
    public interface IState<TStateMachine, TBaseState> : IState where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, TBaseState> where TBaseState : State<TStateMachine, TBaseState>
    {
        public TStateMachine StateMachine { get; set; }

        public bool AddTransition(int input, TBaseState output);
        public TBaseState GetTransition(int input);
        public bool SetTransition(int input, TBaseState output);
    }
    public interface IState<TStateMachine> : IState<TStateMachine, State<TStateMachine>> where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, State<TStateMachine>> { }

    [Serializable]
    public abstract class State<TStateMachine, TBaseState> : IState<TStateMachine, TBaseState> where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, TBaseState> where TBaseState : State<TStateMachine, TBaseState>
    {
        public bool Enabled => StateMachine.GetState() == this;
        public TStateMachine StateMachine { get; set; }

        protected Dictionary<int, TBaseState> _transitions = new Dictionary<int, TBaseState>();

        public bool AddTransition(int input, TBaseState output)
        {
            return _transitions.TryAdd(input, output);
        }
        public TBaseState GetTransition(int input)
        {
            if (_transitions.TryGetValue(input, out var state))
                return state;
            else
                return null;
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

        public virtual IEnumerator Initialize()
        {
            yield break;
        }
        public virtual IEnumerator OnEnable()
        {
            yield break;
        }
        public virtual IEnumerator OnDisable()
        {
            yield break;
        }
        public virtual void OnUpdate()
        {

        }
        public virtual void OnLateUpdate()
        {

        }
        public virtual void OnFixedUpdate()
        {

        }
    }
    [Serializable]
    public abstract class State<TStateMachine> : State<TStateMachine, State<TStateMachine>>, IState<TStateMachine> where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, State<TStateMachine>> { }

    public interface IStateContainer { }
    public interface IStateContainer<TStateMachine, TBaseState> : IStateContainer where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, TBaseState> where TBaseState : State<TStateMachine, TBaseState>
    {
        public ReadOnlyDictionary<Type, TBaseState> GetStates();
    }
    public interface IStateContainer<TStateMachine> : IStateContainer<TStateMachine, State<TStateMachine>> where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, State<TStateMachine>> { }
}
