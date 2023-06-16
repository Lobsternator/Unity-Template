using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Serializable = System.SerializableAttribute;

namespace Template.Core
{
    /// <summary>
    /// Base state belonging to <see cref="ApplicationStateMachine"/>. All ApplicationStateMachine states inherit from this state.
    /// </summary>
    [Serializable]
    public abstract class ApplicationStateBase : State<ApplicationStateMachine, ApplicationStateBase>
    {

    }
}
