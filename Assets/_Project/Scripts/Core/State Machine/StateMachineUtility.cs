using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Type = System.Type;

namespace Template.Core
{
    public static class StateMachineUtility
    {
        public static Dictionary<Type, State<TStateMachine>> GetStates<TStateMachine>(object stateContainer) where TStateMachine : MonoBehaviour, IStateMachine
        {
            Dictionary<Type, State<TStateMachine>> states = new Dictionary<Type, State<TStateMachine>>();

            foreach (PropertyInfo property in stateContainer.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (property.PropertyType.IsSubclassOf(typeof(State<TStateMachine>)) && !states.ContainsKey(property.PropertyType))
                    states.Add(property.PropertyType, (State<TStateMachine>)property.GetValue(stateContainer));
            }

            foreach (FieldInfo field in stateContainer.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                if (field.FieldType.IsSubclassOf(typeof(State<TStateMachine>)) && !states.ContainsKey(field.FieldType))
                    states.Add(field.FieldType, (State<TStateMachine>)field.GetValue(stateContainer));
            }

            return states;
        }
        public static void InitializeStates<TStateMachine>(IEnumerable<State<TStateMachine>> states, TStateMachine stateMachine) where TStateMachine : MonoBehaviour, IStateMachine
        {
            foreach (State<TStateMachine> state in states)
                state.StateMachine = stateMachine;
        }
    }
}
