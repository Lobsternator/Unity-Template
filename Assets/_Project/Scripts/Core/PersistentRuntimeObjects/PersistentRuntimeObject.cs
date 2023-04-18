using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Core
{
    public interface IPersistentRuntimeObject { }
    public interface IPersistentRuntimeObject<TData> : IPersistentRuntimeObject where TData : PersistentRuntimeObjectData<TData>
    {
        public TData PersistentData { get; }
    }

    public abstract class PersistentRuntimeObject<TObject> : MonoBehaviour, IPersistentRuntimeObject where TObject : MonoBehaviour, IPersistentRuntimeObject
    {
        protected static void CreateObjectInstance(string objectName)
        {
            DontDestroyOnLoad(new GameObject(objectName, typeof(TObject)));
        }

        protected virtual void OnDestroy()
        {
            if (ApplicationStateManager.IsApplicationQuitting)
                return;

            Debug.LogWarning("Persistent Runtime Object was destroyed, this is not allowed! \nPersistent runtime objects are expected to be alive during the entire lifetime of the application!");
        }
    }
    public abstract class PersistentRuntimeObject<TObject, TData> : PersistentRuntimeObject<TObject>, IPersistentRuntimeObject<TData> where TObject : MonoBehaviour, IPersistentRuntimeObject<TData> where TData : PersistentRuntimeObjectData<TData>
    {
        public TData PersistentData { get; private set; }

        protected virtual void Awake()
        {
            PersistentData = AssetUtility.GetSingletonAsset<TData>();

            enabled = PersistentData.InitSettings.StartEnabled;
            gameObject.SetActive(PersistentData.InitSettings.StartActive);
            gameObject.isStatic = PersistentData.InitSettings.StartStatic;
        }
    }

    public abstract class PersistentRuntimeSingleton<TSingleton> : SingletonBehaviour<TSingleton>, IPersistentRuntimeObject where TSingleton : MonoBehaviour, ISingleton, IPersistentRuntimeObject
    {
        protected static void CreateObjectInstance(string objectName)
        {
            DontDestroyOnLoad(new GameObject(objectName, typeof(TSingleton)));
        }

        protected virtual void OnDestroy()
        {
            if (ApplicationStateManager.IsApplicationQuitting)
                return;

            Debug.LogWarning("Persistent Runtime Object was destroyed, this is not allowed! \nPersistent runtime objects are expected to be alive during the entire lifetime of the application!");
        }
    }
    public abstract class PersistentRuntimeSingleton<TSingleton, TData> : PersistentRuntimeSingleton<TSingleton>, IPersistentRuntimeObject<TData> where TSingleton : MonoBehaviour, ISingleton, IPersistentRuntimeObject<TData> where TData : PersistentRuntimeObjectData<TData>
    {
        public TData PersistentData { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            if (IsDuplicate)
                return;

            PersistentData = AssetUtility.GetSingletonAsset<TData>();

            enabled = PersistentData.InitSettings.StartEnabled;
            gameObject.SetActive(PersistentData.InitSettings.StartActive);
            gameObject.isStatic = PersistentData.InitSettings.StartStatic;
        }
    }
}
