using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Serializable = System.SerializableAttribute;

namespace Template.Core
{
    public interface IState
    {
        public IEnumerator OnEnable();
        public IEnumerator OnDisable();
        public void OnUpdate();
        public void OnLateUpdate();
        public void OnFixedUpdate();
    }

    [Serializable]
    public abstract class State<TStateMachine> : IState where TStateMachine : MonoBehaviour, IStateMachine
    {
        public TStateMachine StateMachine { get; set; }

        protected Dictionary<int, State<TStateMachine>> _transitions = new Dictionary<int, State<TStateMachine>>();

        public bool AddTransition(int input, State<TStateMachine> output)
        {
            return _transitions.TryAdd(input, output);
        }
        public bool RemoveTransition(int input)
        {
            return _transitions.Remove(input);
        }

        public State<TStateMachine> GetTransition(int input)
        {
            if (_transitions.TryGetValue(input, out var state))
                return state;
            else
                return null;
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
}
