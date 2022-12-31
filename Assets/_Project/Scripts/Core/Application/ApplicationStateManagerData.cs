using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Core
{
    [CreateAssetMenu(fileName = "ApplicationStatesData", menuName = "PersistentRuntimeObjectData/ApplicationStates")]
    public class ApplicationStateManagerData : PersistentRuntimeObjectData, IStateContainer<ApplicationStateMachine>
    {
        [field: SerializeField] public ApplicationStateTest ApplicationStateTest { get; set; } = new ApplicationStateTest();
    }
}
