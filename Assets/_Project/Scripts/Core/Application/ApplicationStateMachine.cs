using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Template.Events;

namespace Template.Core
{
    [DisallowMultipleComponent]
    public class ApplicationStateMachine : StateMachine<ApplicationStateMachine>
    {
        protected override IEnumerator EnableState(State<ApplicationStateMachine> state)
        {
            EventManager.Instance.ApplicationEvents.BeforeApplicationStateEnabled?.Invoke(state);
            yield return base.EnableState(state);
            EventManager.Instance.ApplicationEvents.ApplicationStateEnabled?.Invoke(state);
        }
        protected override IEnumerator DisableState(State<ApplicationStateMachine> state)
        {
            EventManager.Instance.ApplicationEvents.BeforeApplicationStateDisabled?.Invoke(state);
            yield return base.DisableState(state);
            EventManager.Instance.ApplicationEvents.ApplicationStateDisabled?.Invoke(state);
        }
    }
}
