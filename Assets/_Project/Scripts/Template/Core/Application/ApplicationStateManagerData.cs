using UnityEngine;

namespace Template.Core
{
    /// <summary>
    /// <see cref="PersistentRuntimeObjectData{TSingleton}"/> for <see cref="ApplicationStateMachine"/>.
    /// </summary>
    [GenerateStateContainer]
    [CreateAssetMenu(fileName = "ApplicationStateManagerData", menuName = "Singleton/PersistentRuntimeObjectData/ApplicationStateManager")]
    public partial class ApplicationStateManagerData : PersistentRuntimeObjectData<ApplicationStateManagerData>, IStateContainer<ApplicationStateMachine, ApplicationStateBase>
    {

    }
}
