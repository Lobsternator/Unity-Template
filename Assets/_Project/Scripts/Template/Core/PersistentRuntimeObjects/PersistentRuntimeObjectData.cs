using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Core
{
    /// <summary>
    /// Shared data between all <see cref="IPersistentRuntimeObject"/>s.
    /// </summary>
    [Serializable]
    public class PersistentRuntimeObjectInitSettings
    {
        [field: SerializeField] public bool StartEnabled { get; private set; } = true;
        [field: SerializeField] public bool StartActive { get; private set; } = true;
        [field: SerializeField] public bool StartStatic { get; private set; } = false;
    }

    /// <summary>
    /// Used by <see cref="IPersistentRuntimeObject{TData}"/> when wanting to change PersistentRuntimeObject data prior to starting the application.
    /// </summary>
    /// <typeparam name="TSingleton"></typeparam>
    public abstract class PersistentRuntimeObjectData<TSingleton> : SingletonAsset<TSingleton> where TSingleton : PersistentRuntimeObjectData<TSingleton>
    {
        [field: SerializeField] public PersistentRuntimeObjectInitSettings InitSettings { get; private set; }
    }
}
