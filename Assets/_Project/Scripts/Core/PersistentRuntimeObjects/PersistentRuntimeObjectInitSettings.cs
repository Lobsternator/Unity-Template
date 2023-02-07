using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Core
{
    [Serializable]
    public class PersistentRuntimeObjectInitSettings
    {
        [field: SerializeField] public bool IsEnabled { get; private set; } = true;
        [field: SerializeField] public bool IsActive  { get; private set; } = true;
        [field: SerializeField] public bool IsStatic  { get; private set; } = false;
    }
}
