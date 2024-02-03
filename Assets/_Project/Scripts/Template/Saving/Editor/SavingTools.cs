#if UNITY_EDITOR
using System.Collections.Generic;
using Template.Core;
using UnityEditor;
using UnityEngine;

namespace Template.Saving
{
    /// <summary>
    /// Various editor tools relating to the save system.
    /// </summary>
    public static class SavingTools
    {
        [MenuItem("Tools/Saving/Regenerate data keys")]
        public static void RegenerateDataKeys()
        {
            List<ISavableObject> savableObjects = ObjectUtility.FindObjectsWithInterface<ISavableObject>(true);

            foreach (ISavableObject savableObject in savableObjects)
            {
                savableObject.DataKey = new DataKey();
                EditorUtility.SetDirty(savableObject as MonoBehaviour);
            }
        }

        [MenuItem("Tools/Saving/Save to first slot")]
        public static void SaveToFirstSlot()
        {
            SaveManager.SaveToSlot(0);
        }

        [MenuItem("Tools/Saving/Load from first slot")]
        public static void LoadFromFirstSlot()
        {
            SaveManager.LoadFromSlot(0);
        }

        [MenuItem("Tools/Saving/Clear save data")]
        public static void ClearSaveData()
        {
            SaveManager.ClearAllSaveSlots();
        }
    }
}
#endif
