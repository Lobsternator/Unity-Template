using Serializable = System.SerializableAttribute;

namespace Template.Core
{
    /// <summary>
    /// State belonging to <see cref="ApplicationStateMachine"/>.
    /// Is entered at the start of the application.
    /// </summary>
    [Serializable]
    public class ApplicationStateEntry : ApplicationStateBase
    {

    }
}
