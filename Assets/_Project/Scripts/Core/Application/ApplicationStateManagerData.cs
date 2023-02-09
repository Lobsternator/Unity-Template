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
        [field: SerializeField] public ApplicationStateEntry ApplicationStateTest { get; set; } = new ApplicationStateEntry();
        [field: SerializeField] public ApplicationStateQuit ApplicationStateQuit { get; set; }  = new ApplicationStateQuit();

        private Dictionary<Type, State<ApplicationStateMachine>> _states;

        private void PopulateStates()
        {
            _states = new Dictionary<Type, State<ApplicationStateMachine>>(2)
            {
                { ApplicationStateTest.GetType(), ApplicationStateTest },
                { ApplicationStateQuit.GetType(), ApplicationStateQuit }
            };
        }

        public ReadOnlyDictionary<Type, State<ApplicationStateMachine>> GetStates()
        {
            return new ReadOnlyDictionary<Type, State<ApplicationStateMachine>>(_states);
        }

        private void Awake()
        {
            PopulateStates();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            PopulateStates();
        }
#endif
    }
}
