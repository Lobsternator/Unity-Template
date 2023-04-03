using UnityEngine;

namespace Template.Core
{
    [GenerateStateContainer]
    [CreateAssetMenu(fileName = "ApplicationStateManagerData", menuName = "PersistentRuntimeObjectData/ApplicationStateManager")]
    public partial class ApplicationStateManagerData : PersistentRuntimeObjectData, IStateContainer<ApplicationStateMachine, ApplicationStateBase>
    {

    }
}
