using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using Unity.EditorCoroutines.Editor;
#endif
using UnityEngine;

namespace Template.Core
{
    public interface ISingleton { }

    [DisallowMultipleComponent]
    public abstract class SingletonBehaviour<TSingleton> : MonoBehaviour, ISingleton where TSingleton : MonoBehaviour, ISingleton
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

    [SingletonAsset]
    public abstract class SingletonAsset<TSingleton> : ScriptableObject, ISingleton where TSingleton : ScriptableObject, ISingleton
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
    public class SingletonAssetValidator : AssetPostprocessor
    {
        private static readonly Type[]   _assemblyTypes               = Assembly.GetAssembly(typeof(SingletonAssetAttribute)).GetTypes();
        private static SortedSet<string> _latestAutoCreatedAssetPaths = new SortedSet<string>();

        private static void ValidateAssetCount(Type type, SingletonAssetAttribute attribute)
        {
            UnityEngine.Object[] foundAssets = Resources.LoadAll("", type);
            if (foundAssets.Length == 0)
            {
                if (attribute.AutoCreate)
                {
                    string assetPath            = Path.Combine(PersistentPathData.Instance.ResourceFolderPath, type.Name).Replace('\\', '/') + ".asset";
                    assetPath                   = AssetDatabase.GenerateUniqueAssetPath(assetPath);
                    _latestAutoCreatedAssetPaths.Add(assetPath);

                    AssetDatabase.CreateAsset(ScriptableObject.CreateInstance(type), assetPath);
                    Debug.LogWarning($"Automatically created missing singleton asset of type \'{type.Name}\' at {assetPath}!");
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
            foreach (Type type in _assemblyTypes)
            {
                var  attribute    = type.GetCustomAttribute<SingletonAssetAttribute>(true);
                bool isAsset      = type.IsSubclassOf(typeof(ScriptableObject));
                bool hasAttribute = attribute is not null;
                bool isAbstract   = type.IsAbstract;

                if (isAsset && hasAttribute && !isAbstract)
                    ValidateAssetCount(type, attribute);
            }
        }

        [InitializeOnLoadMethod]
        private static void ValidateAllAssetCounts_OnLoad()
        {
            EditorCoroutineUtility.StartCoroutineOwnerless(ValidateAllAssetCounts_Delayed());
        }

        private static IEnumerator ValidateAllAssetCounts_Delayed()
        {
            yield return null;
            ValidateAllAssetCounts();
        }

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            SortedSet<string> importedAssetsSet = new SortedSet<string>(importedAssets);
            if (!importedAssetsSet.Overlaps(_latestAutoCreatedAssetPaths))
                ValidateAllAssetCounts();
            else
                _latestAutoCreatedAssetPaths.ExceptWith(importedAssets);
        }
    }
#endif
}
