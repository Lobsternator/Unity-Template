using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Core
{
    [DisallowMultipleComponent]
    public class ApplicationStateMachine : ManagedStateMachine<ApplicationStateManager, ApplicationStateMachine>
    {
        private void Awake()
        {
            StateManager = GetComponent<ApplicationStateManager>();
        }
    }
}
