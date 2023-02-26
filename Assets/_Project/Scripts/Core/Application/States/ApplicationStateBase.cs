using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Serializable = System.SerializableAttribute;

namespace Template.Core
{
    [Serializable]
    public class ApplicationStateBase : State<ApplicationStateMachine, ApplicationStateBase>
    {

    }
}
