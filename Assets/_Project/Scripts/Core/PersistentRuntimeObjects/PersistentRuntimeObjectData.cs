using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Core
{
    [SingletonAsset]
    public abstract class PersistentRuntimeObjectData : ScriptableObject
    {
        [field: SerializeField] public PersistentRuntimeObjectInitSettings InitSettings { get; private set; }
    }
}
