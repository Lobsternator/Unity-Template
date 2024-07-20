using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
#if UNITY_EDITOR
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.Callbacks;
#endif
using UnityEngine;

namespace Template.Core
{
    /// <summary>
    /// Covering type for <see cref="SingletonBehaviour{TSingleton}"/> and <see cref="SingletonAsset{TSingleton}"/>.
    /// </summary>
    public interface ISingleton<TSingleton>
    {
        public static TSingleton Instance { get; }
    }

    /// <summary>
    /// Exposes a public <see cref="Instance"/> property for getting the first avaiable instance of this singleton. Prevents multiple singletons from existing at once.
    /// </summary>
    /// <typeparam name="TSingleton">Should be the inheriting class.</typeparam>
    [DisallowMultipleComponent]
    public abstract class SingletonBehaviour<TSingleton> : MonoBehaviour, ISingleton<TSingleton> where TSingleton : MonoBehaviour, ISingleton<TSingleton>
    {
        private static TSingleton _instance;
        public static TSingleton Instance
        {
            get
            {
                if (!_instance)
                    _instance = FindObjectOfType<TSingleton>(true);

                return _instance;
            }
        }

        public bool IsDuplicate { get; private set; }

        protected virtual void Awake()
        {
            if (_instance)
            {
                IsDuplicate = true;
                gameObject.SetActive(false);

                Debug.LogError($"Deactivated duplicate singleton \'{GetType()}\'!", gameObject);
                return;
            }

            _instance = this as TSingleton;
        }

        protected virtual void OnApplicationQuit()
        {
            _instance = null;
        }
    }

    /// <summary>
    /// Any <see cref="ScriptableObject"/> which has this attribute will be added to a list of asset types which should have their asset counts validated while in the editor. A warning will be raised when no, or multiple assets of the class type exist. There is optional functionality to automatically create the asset if it doesn't exist.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class SingletonAssetAttribute : Attribute
    {
        private readonly bool autoCreate;

        public SingletonAssetAttribute()
        {
            autoCreate = false;
        }
        public SingletonAssetAttribute(bool autoCreate)
        {
            this.autoCreate = autoCreate;
        }

        public bool AutoCreate => autoCreate;
    }

    /// <summary>
    /// Any class which inherits this will be added to a list of asset types which should have their asset counts validated while in the editor. A warning will be raised when no, or multiple assets of type <typeparamref name="TSingleton"/> exist.
    /// Exposes a <see cref="Instance"/> property to get the first available asset of this singleton.
    /// </summary>
    /// <typeparam name="TSingleton">Should be the inheriting class.</typeparam>
    [SingletonAsset]
    public abstract class SingletonAsset<TSingleton> : ScriptableObject, ISingleton<TSingleton> where TSingleton : ScriptableObject, ISingleton<TSingleton>
    {
        private static TSingleton _instance;
        public static TSingleton Instance
        {
            get
            {
                if (!_instance)
                    _instance = AssetUtility.GetSingletonAsset<TSingleton>();

                return _instance;
            }
        }

        private void Awake()
        {
            _instance = AssetUtility.GetSingletonAsset<TSingleton>();
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// Static class for validating asset counts for <see cref="ScriptableObject"/>s that have the <see cref="SingletonAssetAttribute"/>.
    /// </summary>
    public class SingletonAssetValidator : AssetPostprocessor
    {
        private static readonly IList<Type> _scriptableObjectTypes  = TypeCache.GetTypesDerivedFrom<ScriptableObject>();
        private static HashSet<string> _latestAutoCreatedAssetPaths = new HashSet<string>();

        private static void ValidateAssetCount(Type type, bool autoCreate)
        {
            UnityEngine.Object[] foundAssets = Resources.LoadAll("", type);
            if (foundAssets.Length == 0)
            {
                if (autoCreate)
                {
                    string defaultAssetPath = Path.Combine(PersistentApplicationData.Instance.DefaultResourceFolderPath, type.Name).Replace('\\', '/') + ".asset";
                    defaultAssetPath        = AssetDatabase.GenerateUniqueAssetPath(defaultAssetPath);
                    _latestAutoCreatedAssetPaths.Add(defaultAssetPath);

                    AssetDatabase.CreateAsset(ScriptableObject.CreateInstance(type), defaultAssetPath);
                    Debug.LogWarning($"Automatically created missing singleton asset of type \'{type.Name}\' at {defaultAssetPath}!");
                }
                else
                    Debug.LogError($"Could not find singleton asset of type \'{type.Name}\' in resources, a singleton asset must always exist!");
            }

            else if (foundAssets.Length > 1)
            {
                string assetsString = "";
                for (int i = 0; i < foundAssets.Length; i++)
                    assetsString += $"\n{AssetDatabase.GetAssetPath(foundAssets[i])}";

                Debug.LogError($"Found multiple singleton assets of type \'{type.Name}\' in resources, only one singleton asset can exist at once!{assetsString}");
            }
        }
        private static void ValidateAllAssetCounts()
        {
            foreach (Type type in _scriptableObjectTypes)
            {
                var  attribute    = type.GetCustomAttribute<SingletonAssetAttribute>(true);
                bool isAsset      = type.IsSubclassOf(typeof(ScriptableObject));
                bool hasAttribute = attribute is not null;
                bool isAbstract   = type.IsAbstract;

                if (isAsset && hasAttribute && !isAbstract)
                    ValidateAssetCount(type, attribute.AutoCreate);
            }
        }

        private static IEnumerator ValidateAllAssetCounts_AfterUpdating()
        {
            yield return new WaitWhile(() => EditorApplication.isUpdating);
            ValidateAllAssetCounts();
        }

        [DidReloadScripts]
        private static void ValidateAllAssetCounts_OnReloadScripts()
        {
            EditorCoroutineUtility.StartCoroutineOwnerless(ValidateAllAssetCounts_AfterUpdating());
        }

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            HashSet<string> importedAssetsSet = new HashSet<string>(importedAssets);
            if (!importedAssetsSet.Overlaps(_latestAutoCreatedAssetPaths))
                EditorCoroutineUtility.StartCoroutineOwnerless(ValidateAllAssetCounts_AfterUpdating());
            else
                _latestAutoCreatedAssetPaths.ExceptWith(importedAssets);
        }
    }
#endif
}
