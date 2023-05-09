using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Core
{
    [Serializable]
    public class PersistentRuntimeObjectInitSettings
    {
        [field: SerializeField] public bool StartEnabled { get; private set; } = true;
        [field: SerializeField] public bool StartActive  { get; private set; } = true;
        [field: SerializeField] public bool StartStatic  { get; private set; } = false;
    }
}
