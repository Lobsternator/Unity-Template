using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Core
{
    [DisallowMultipleComponent]
    public class ApplicationStateMachine : ManagedStateMachine<ApplicationStateManager, ApplicationStateMachine>
    {
        private ApplicationStateManager _stateManager;
        public override ApplicationStateManager StateManager
        {
            get
            {
                if (!_stateManager)
                    _stateManager = GetComponent<ApplicationStateManager>();

                return _stateManager;
            }
            protected set => _stateManager = value;
        }
    }
}
