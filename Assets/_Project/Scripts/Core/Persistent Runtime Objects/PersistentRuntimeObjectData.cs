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
        public bool startEnabled = true;

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

                Debug.LogError($"Found multiple PersistentRuntimeObjectData assets of same type \'{typeName}\'!{assetsString}");
            }
            else if (foundAssets.Length == 1 && name != typeName)
            {
                string assetPath    = AssetDatabase.GUIDToAssetPath(foundAssets[0]);
                string renameResult = AssetDatabase.RenameAsset(assetPath, typeName);
                name                = typeName;

                if (renameResult.Length != 0)
                    Debug.LogError(renameResult);
            }
        }
#endif
    }
}
