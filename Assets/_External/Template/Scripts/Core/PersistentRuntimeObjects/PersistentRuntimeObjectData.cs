using System;
using UnityEngine;

namespace Template.Core
{
    /// <summary>
    /// Shared data between all <see cref="IPersistentRuntimeObject{TData}"/>s.
    /// </summary>
    [Serializable]
    public class PersistentRuntimeObjectSettings
    {
        [SerializeField]
        private bool _enabled = true;
        public bool Enabled => _enabled;

        [SerializeField]
        private bool _active = true;
        public bool Active => _active;

        [SerializeField]
        private bool _static = false;
        public bool Static => _static;

        [SerializeField]
        private bool _shouldOverrideDefaultName = false;
        public bool ShouldOverrideDefaultName => _shouldOverrideDefaultName;

        [SerializeField]
        private string _nameOverride = "";
        public string NameOverride => _nameOverride;
    }

    /// <summary>
    /// Used by <see cref="IPersistentRuntimeObject{TData}"/> when wanting to change PersistentRuntimeObject data prior to starting the application.
    /// </summary>
    /// <typeparam name="TSingleton"></typeparam>
    public abstract class PersistentRuntimeObjectData<TSingleton> : SingletonAsset<TSingleton> where TSingleton : PersistentRuntimeObjectData<TSingleton>
    {
        [field: SerializeField]
        public PersistentRuntimeObjectSettings Settings { get; private set; }
    }
}
