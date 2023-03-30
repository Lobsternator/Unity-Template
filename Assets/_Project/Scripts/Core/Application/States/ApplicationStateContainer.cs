using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Template.Core
{
    [GenerateStateContainer]
    public partial class ApplicationStateContainer : IStateContainer<ApplicationStateMachine, ApplicationStateBase>
    {
        public ReadOnlyDictionary<Type, ApplicationStateBase> States { get; }
    }
}
