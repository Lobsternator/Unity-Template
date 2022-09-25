using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Core
{
    public interface IPersistentRuntimeObject { }
    public interface IPersistentRuntimeObject<TData> where TData : PersistentRuntimeObjectData
    {
        public TData PersistentData { get; }
    }

    public abstract class PersistentRuntimeObject<TObject> : MonoBehaviour, IPersistentRuntimeObject where TObject : MonoBehaviour
    {
        protected static void CreateObjectInstance(string objectName)
        {
            DontDestroyOnLoad(new GameObject(objectName, typeof(TObject)));
        }
    }
    public abstract class PersistentRuntimeObject<TObject, TData> : MonoBehaviour, IPersistentRuntimeObject, IPersistentRuntimeObject<TData> where TObject : MonoBehaviour where TData : PersistentRuntimeObjectData
    {
        public TData PersistentData { get; private set; }

        protected static void CreateObjectInstance(string objectName)
        {
            DontDestroyOnLoad(new GameObject(objectName, typeof(TObject)));
        }

        protected virtual void Awake()
        {
            PersistentData = PersistentRuntimeObjectUtility.GetPersistentData<TData>();
            if (!PersistentData)
            {
                Debug.LogError($"Could not find PersistentRuntimeObjectData \'{typeof(TData).Name}\'!", this);
                return;
            }

            enabled = PersistentData.startEnabled;
        }
    }

    public abstract class PersistentRuntimeSingleton<TSingleton> : Singleton<TSingleton>, IPersistentRuntimeObject where TSingleton : MonoBehaviour
    {
        protected static void CreateObjectInstance(string objectName)
        {
            DontDestroyOnLoad(new GameObject(objectName, typeof(TSingleton)));
        }
    }
    public abstract class PersistentRuntimeSingleton<TSingleton, TData> : Singleton<TSingleton>, IPersistentRuntimeObject, IPersistentRuntimeObject<TData> where TSingleton : MonoBehaviour where TData : PersistentRuntimeObjectData
    {
        public TData PersistentData { get; private set; }

        protected static void CreateObjectInstance(string objectName)
        {
            DontDestroyOnLoad(new GameObject(objectName, typeof(TSingleton)));
        }

        protected override void Awake()
        {
            base.Awake();
            if (IsDuplicate)
                return;

            PersistentData = PersistentRuntimeObjectUtility.GetPersistentData<TData>();
            if (!PersistentData)
            {
                Debug.LogError($"Could not find PersistentRuntimeObjectData \'{typeof(TData).Name}\'!", this);
                return;
            }

            enabled = PersistentData.startEnabled;
        }
    }
}
