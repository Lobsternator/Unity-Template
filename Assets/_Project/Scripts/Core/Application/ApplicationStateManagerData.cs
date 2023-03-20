using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Template.Core
{
    [CreateAssetMenu(fileName = "ApplicationStateManagerData", menuName = "PersistentRuntimeObjectData/ApplicationStateManager")]
    public class ApplicationStateManagerData : PersistentRuntimeObjectData, IStateContainer<ApplicationStateMachine, ApplicationStateBase>
    {
        private Dictionary<Type, ApplicationStateBase> _states;
        public ReadOnlyDictionary<Type, ApplicationStateBase> States
        {
            get => new ReadOnlyDictionary<Type, ApplicationStateBase>(_states);
        }

        [field: SerializeField] public ApplicationStateEntry ApplicationStateTest { get; set; } = new ApplicationStateEntry();
        [field: SerializeField] public ApplicationStateQuit ApplicationStateQuit { get; set; }  = new ApplicationStateQuit();

        public ApplicationStateManagerData()
        {
            _states = new Dictionary<Type, ApplicationStateBase>(2)
            {
                { ApplicationStateTest.GetType(), ApplicationStateTest },
                { ApplicationStateQuit.GetType(), ApplicationStateQuit }
            };
        }
    }
}
