using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Template.Core
{
    [CreateAssetMenu(fileName = "ApplicationStatesData", menuName = "PersistentRuntimeObjectData/ApplicationStates")]
    public class ApplicationStateManagerData : PersistentRuntimeObjectData, IStateContainer<ApplicationStateMachine>
    {
        [field: SerializeField] public ApplicationStateTest ApplicationStateTest { get; set; } = new ApplicationStateTest();

        private Dictionary<Type, State<ApplicationStateMachine>> _states;

        public ApplicationStateManagerData()
        {
            _states = new Dictionary<Type, State<ApplicationStateMachine>>()
            {
                { ApplicationStateTest.GetType(), ApplicationStateTest }
            };
        }

        public ReadOnlyDictionary<Type, State<ApplicationStateMachine>> GetStates()
        {
            return new ReadOnlyDictionary<Type, State<ApplicationStateMachine>>(_states);
        }
    }
}
