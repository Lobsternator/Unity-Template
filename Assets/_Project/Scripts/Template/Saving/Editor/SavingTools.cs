#if UNITY_EDITOR
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Template.Core;

namespace Template.Saving
{
    public class SavingTools : MonoBehaviour
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
