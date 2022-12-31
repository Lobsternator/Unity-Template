using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Type = System.Type;

namespace Template.Core
{
    public static class StateMachineUtility
    {
        public static Dictionary<Type, TBaseState> GetStates<TStateMachine, TBaseState>(IStateContainer<TStateMachine, TBaseState> stateContainer) where TStateMachine : MonoBehaviour, IStateMachine where TBaseState : State<TStateMachine, TBaseState>
        {
            Dictionary<Type, TBaseState> states = new Dictionary<Type, TBaseState>();

            foreach (PropertyInfo property in stateContainer.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (property.PropertyType.IsSubclassOf(typeof(State<TStateMachine>)) && !states.ContainsKey(property.PropertyType))
                    states.Add(property.PropertyType, (TBaseState)property.GetValue(stateContainer));
            }

            foreach (FieldInfo field in stateContainer.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                if (field.FieldType.IsSubclassOf(typeof(State<TStateMachine>)) && !states.ContainsKey(field.FieldType))
                    states.Add(field.FieldType, (TBaseState)field.GetValue(stateContainer));
            }

            return states;
        }
        public static void InitializeStates<TStateMachine, TBaseState>(IEnumerable<TBaseState> states, TStateMachine stateMachine) where TStateMachine : MonoBehaviour, IStateMachine where TBaseState : State<TStateMachine, TBaseState>
        {
            foreach (TBaseState state in states)
                state.StateMachine = stateMachine;
        }
    }
}
