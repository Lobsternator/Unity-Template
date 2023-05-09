using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Core
{
    public abstract class PersistentRuntimeObjectData<TSingleton> : SingletonAsset<TSingleton> where TSingleton : PersistentRuntimeObjectData<TSingleton>
    {
        [field: SerializeField] public PersistentRuntimeObjectInitSettings InitSettings { get; private set; }
    }
}
