using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Template.Core
{
    public abstract class PersistentRuntimeObjectData : ScriptableObject
    {
        [field: SerializeField] public PersistentRuntimeObjectInitSettings InitSettings { get; private set; }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            string typeName = GetType().Name;

            string[] foundAssets = AssetDatabase.FindAssets($"t: {typeName}");
            if (foundAssets.Length > 1)
            {
                string assetsString = "";
                for (int i = 0; i < foundAssets.Length; i++)
                    assetsString += $"\n{AssetDatabase.GUIDToAssetPath(foundAssets[i])}";

                Debug.LogError($"Found multiple PersistentRuntimeObjectData assets of type \'{typeName}\', this is not allowed!{assetsString}");
            }
        }
#endif
    }
}
