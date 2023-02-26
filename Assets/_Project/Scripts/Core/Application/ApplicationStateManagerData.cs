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
        [field: SerializeField] public ApplicationStateEntry ApplicationStateTest { get; set; } = new ApplicationStateEntry();
        [field: SerializeField] public ApplicationStateQuit ApplicationStateQuit { get; set; }  = new ApplicationStateQuit();

        private Dictionary<Type, ApplicationStateBase> _states;

        public ApplicationStateManagerData()
        {
            _states = new Dictionary<Type, ApplicationStateBase>(2)
            {
                { ApplicationStateTest.GetType(), ApplicationStateTest },
                { ApplicationStateQuit.GetType(), ApplicationStateQuit }
            };
        }

        public ReadOnlyDictionary<Type, ApplicationStateBase> GetStates()
        {
            return new ReadOnlyDictionary<Type, ApplicationStateBase>(_states);
        }
    }
}
