using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Core
{
    [DisallowMultipleComponent]
    public class ApplicationStateMachine : StateMachine<ApplicationStateMachine, ApplicationStateBase>, IManagedStateMachine<ApplicationStateManager, ApplicationStateMachine, ApplicationStateBase>
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
