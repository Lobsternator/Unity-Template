using Serializable = System.SerializableAttribute;

namespace Template.Core
{
    /// <summary>
    /// Base state belonging to <see cref="ApplicationStateMachine"/>. All <see cref="ApplicationStateMachine"/> states inherit from this state.
    /// </summary>
    [Serializable]
    public abstract class ApplicationState_Base : State<ApplicationStateMachine, ApplicationState_Base>
    {

    }
}
