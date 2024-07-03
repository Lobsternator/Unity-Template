using UnityEngine;

namespace Template.Core
{
    /// <summary>
    /// State machine for handling application-wide state control.
    /// </summary>
    [DisallowMultipleComponent]
    public class ApplicationStateMachine : StateMachine<ApplicationStateMachine, ApplicationState_Base>, IManagedStateMachine<ApplicationStateManager, ApplicationStateMachine, ApplicationState_Base>
    {
        private ApplicationStateManager _stateManager;
        public ApplicationStateManager StateManager
        {
            get
            {
                if (!_stateManager)
                    _stateManager = GetComponent<ApplicationStateManager>();

                return _stateManager;
            }
        }
    }
}
