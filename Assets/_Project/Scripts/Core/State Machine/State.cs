using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Serializable = System.SerializableAttribute;

namespace Template.Core
{
    public interface IState
    {
        // Run for every state (NOTE: Has to be called manually!)
        public IEnumerator Initialize();

        // Run for a single state
        public IEnumerator OnEnable();
        public IEnumerator OnDisable();
        public void OnUpdate();
        public void OnLateUpdate();
        public void OnFixedUpdate();
    }

    [Serializable]
    public abstract class State<TStateMachine, TBaseState> : IState where TStateMachine : MonoBehaviour, IStateMachine where TBaseState : State<TStateMachine, TBaseState>
    {
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
    public abstract class State<TStateMachine> : State<TStateMachine, State<TStateMachine>> where TStateMachine : MonoBehaviour, IStateMachine { }

    public interface IStateContainer<TStateMachine, TBaseState> where TStateMachine : MonoBehaviour, IStateMachine where TBaseState : State<TStateMachine, TBaseState> { }
    public interface IStateContainer<TStateMachine> : IStateContainer<TStateMachine, State<TStateMachine>> where TStateMachine : MonoBehaviour, IStateMachine { }
}
