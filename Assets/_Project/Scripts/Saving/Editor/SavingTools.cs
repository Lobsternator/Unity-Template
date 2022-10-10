#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using Template.Core;
using System.Text.RegularExpressions;

namespace Template.Saving
{
    public class SavingTools : MonoBehaviour
    {
        [MenuItem("Tools/Saving/Regenerate data keys")]
        public static void RegenerateDataKeys()
        {
            ISavableObject[] savableObjects = ObjectUtility.FindObjectsWithInterface<ISavableObject>(true);

            foreach (ISavableObject savableObject in savableObjects)
            {
                savableObject.DataKey = DataKey.CreateNew();
                EditorUtility.SetDirty(savableObject as MonoBehaviour);
            }
        }

        [MenuItem("Tools/Saving/Save to first slot")]
        public static void SaveToFirstSlot()
        {
            if (!Application.isPlaying)
            {
                Debug.LogWarning("The application needs to be running in order to save!");
                return;
            }

            SaveManager.Instance.SaveToSlot(0);
        }

        [MenuItem("Tools/Saving/Load from first slot")]
        public static void LoadFromFirstSlot()
        {
            if (!Application.isPlaying)
            {
                Debug.LogWarning("The application needs to be running in order to load!");
                return;
            }

            SaveManager.Instance.LoadFromSlot(0);
        }

        [MenuItem("Tools/Saving/Clear save data")]
        public static void ClearSaveData()
        {
            SaveManagerData saveManagerData = PersistentRuntimeObjectUtility.GetPersistentData<SaveManagerData>();
            if (!saveManagerData)
            {
                Debug.LogError($"Could not find PersistentRuntimeObjectData \'{typeof(SaveManagerData).Name}\'!");
                return;
            }

            string pattern = SaveManager.GetSaveFileRegexPattern();
            Regex regex    = new Regex(pattern, RegexOptions.IgnoreCase);

            string[] files = Directory.GetFiles(saveManagerData.FullSaveDirectoryPath);
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);

                if (regex.IsMatch(fileName))
                    File.Delete(file);
            }
        }
    }
}
#endif
