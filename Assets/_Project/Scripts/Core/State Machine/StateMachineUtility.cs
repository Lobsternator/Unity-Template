using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Type = System.Type;

namespace Template.Core
{
    public static class StateMachineUtility
    {
        public static Dictionary<Type, TBaseState> GetStates<TStateMachine, TBaseState>(IStateContainer<TStateMachine, TBaseState> stateContainer, BindingFlags bindingFlags) where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, TBaseState> where TBaseState : State<TStateMachine, TBaseState>
        {
            Dictionary<Type, TBaseState> states = new Dictionary<Type, TBaseState>();

            foreach (PropertyInfo property in stateContainer.GetType().GetProperties(bindingFlags))
            {
                if (property.PropertyType.IsSubclassOf(typeof(TBaseState)) && !states.ContainsKey(property.PropertyType))
                    states.Add(property.PropertyType, (TBaseState)property.GetValue(stateContainer));
            }

            foreach (FieldInfo field in stateContainer.GetType().GetFields(bindingFlags))
            {
                if (field.FieldType.IsSubclassOf(typeof(TBaseState)) && !states.ContainsKey(field.FieldType))
                    states.Add(field.FieldType, (TBaseState)field.GetValue(stateContainer));
            }

            return states;
        }
        public static void InitializeStates<TStateMachine, TBaseState>(IEnumerable<TBaseState> states, TStateMachine stateMachine) where TStateMachine : MonoBehaviour, IStateMachine<TStateMachine, TBaseState> where TBaseState : State<TStateMachine, TBaseState>
        {
            foreach (TBaseState state in states)
            {
                state.StateMachine = stateMachine;
                stateMachine.StartCoroutine(state.Initialize());
            }
        }
    }
}
