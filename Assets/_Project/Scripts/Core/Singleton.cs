using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
using Unity.EditorCoroutines.Editor;
#endif
using UnityEngine;

namespace Template.Core
{
    [DisallowMultipleComponent]
    public abstract class SingletonBehaviour<TSingleton> : MonoBehaviour where TSingleton : MonoBehaviour
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

    }

#if UNITY_EDITOR
    public class SingletonAssetValidator : AssetPostprocessor
    {
        private static readonly Type[] _assemblyTypes = Assembly.GetAssembly(typeof(SingletonAssetAttribute)).GetTypes();

        private static void ValidateAssetCount(Type type)
        {
            UnityEngine.Object[] foundAssets = Resources.LoadAll("", type);
            if (foundAssets.Length == 0)
                Debug.LogError($"Could not find singleton asset of type \'{type.Name}\' in resources, a singleton asset must always exist!");

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
                bool isAsset      = type.IsSubclassOf(typeof(ScriptableObject));
                bool hasAttribute = type.GetCustomAttribute(typeof(SingletonAssetAttribute), true) is not null;
                bool isAbstract   = type.IsAbstract;

                if (isAsset && hasAttribute && !isAbstract)
                    ValidateAssetCount(type);
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
            ValidateAllAssetCounts();
        }
    }
#endif
}
