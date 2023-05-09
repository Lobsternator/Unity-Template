using UnityEngine;

namespace Template.Core
{
    [GenerateStateContainer]
    [CreateAssetMenu(fileName = "ApplicationStateManagerData", menuName = "Singleton/PersistentRuntimeObjectData/ApplicationStateManager")]
    public partial class ApplicationStateManagerData : PersistentRuntimeObjectData<ApplicationStateManagerData>, IStateContainer<ApplicationStateMachine, ApplicationStateBase>
    {

    }
}
