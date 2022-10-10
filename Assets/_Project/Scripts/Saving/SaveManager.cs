using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Template.Core;
using Template.Saving.Serialization;
using System.Text.RegularExpressions;

namespace Template.Saving
{
    [PersistentRuntimeObject(RuntimeInitializeLoadType.BeforeSceneLoad, -700)]
    public class SaveManager : PersistentRuntimeSingleton<SaveManager, SaveManagerData>
    {
        private string _fullSaveDirectoryPath;

        public static string GetSaveFileRegexPattern()
        {
            SaveManagerData persistentData = PersistentRuntimeObjectUtility.GetPersistentData<SaveManagerData>();
            string saveFileName            = Path.GetFileNameWithoutExtension(persistentData.savePath);
            string saveFileExt             = Path.GetExtension(persistentData.savePath);
            string period                  = "";

            if (saveFileExt.StartsWith('.'))
            {
                saveFileExt = saveFileExt.Substring(1);
                period      = @"\.";
            }

            return @$"{saveFileName}[0-9]*{period}{saveFileExt}";
        }
        public static string GetSaveFileRegexPattern(int saveSlot)
        {
            SaveManagerData persistentData = PersistentRuntimeObjectUtility.GetPersistentData<SaveManagerData>();
            string saveFileName            = Path.GetFileNameWithoutExtension(persistentData.savePath);
            string saveFileExt             = Path.GetExtension(persistentData.savePath);
            string period                  = "";

            if (saveFileExt.StartsWith('.'))
            {
                saveFileExt = saveFileExt.Substring(1);
                period      = @"\.";
            }

            return @$"{saveFileName}{saveSlot}{period}{saveFileExt}";
        }

        private bool WriteSaveDataToFile(Dictionary<DataKey, SerializableObjectDataContainer> saveData, int saveSlot)
        {
            if (!Directory.Exists(_fullSaveDirectoryPath))
                Directory.CreateDirectory(_fullSaveDirectoryPath);

            string fullSaveFilePath = PersistentData.GetFullSaveFilePath(saveSlot);

            using (FileStream fs = new FileStream(fullSaveFilePath, FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                try
                {
                    formatter.Serialize(fs, saveData);
                }
                catch (SerializationException e)
                {
                    Debug.LogError($"WARNING: Unable to write save data! {e.Message}");
                    return false;
                }
            }

            return true;
        }

        private bool ReadSaveDataFromFile(int saveSlot, out Dictionary<DataKey, SerializableObjectDataContainer> saveData)
        {
            string fullSaveFilePath = PersistentData.GetFullSaveFilePath(saveSlot);

            if (!File.Exists(fullSaveFilePath))
            {
                Debug.LogWarning($"WARNING: Unable to find save file at \'{fullSaveFilePath}\'!");
                saveData = null;
                return false;
            }

            using (FileStream fs = new FileStream(fullSaveFilePath, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                try
                {
                    saveData = formatter.Deserialize(fs) as Dictionary<DataKey, SerializableObjectDataContainer>;
                }
                catch (SerializationException e)
                {
                    Debug.LogError($"WARNING: Unable to read save data! {e.Message}");
                    saveData = null;
                    return false;
                }
            }

            return true;
        }

        public bool SaveToSlot(int saveSlot)
        {
            var finalSaveData = new Dictionary<DataKey, SerializableObjectDataContainer>();

            ISavableObject[] savableObjects = ObjectUtility.FindObjectsWithInterface<ISavableObject>(true);
            foreach (ISavableObject savableObject in savableObjects)
                finalSaveData.Add(savableObject.DataKey, savableObject.GetSaveData());

            return WriteSaveDataToFile(finalSaveData, saveSlot);
        }
        public bool LoadFromSlot(int saveSlot)
        {
            if (ReadSaveDataFromFile(saveSlot, out var finalSaveData))
            {
                ISavableObject[] savableObjects = ObjectUtility.FindObjectsWithInterface<ISavableObject>(true);
                foreach (ISavableObject savableObject in savableObjects)
                    if (finalSaveData.TryGetValue(savableObject.DataKey, out var saveData))
                        savableObject.LoadSaveData(saveData);

                return true;
            }

            return false;
        }

        public bool HasSaveSlot(int saveSlot)
        {
            string pattern     = GetSaveFileRegexPattern(saveSlot);
            Regex regex        = new Regex(pattern, RegexOptions.IgnoreCase);

            string[] files     = Directory.GetFiles(_fullSaveDirectoryPath);
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);

                if (regex.IsMatch(fileName))
                    return true;
            }

            return false;
        }

        public bool ClearSaveSlot(int saveSlot)
        {
            string pattern      = GetSaveFileRegexPattern(saveSlot);
            Regex regex         = new Regex(pattern, RegexOptions.IgnoreCase);

            string[] files      = Directory.GetFiles(_fullSaveDirectoryPath);
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);

                if (regex.IsMatch(fileName))
                {
                    File.Delete(file);
                    return true;
                }
            }

            return false;
        }

        public void ClearAllSaveSlots()
        {
            string pattern      = GetSaveFileRegexPattern();
            Regex regex         = new Regex(pattern, RegexOptions.IgnoreCase);

            string[] files      = Directory.GetFiles(_fullSaveDirectoryPath);
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);

                if (regex.IsMatch(fileName))
                    File.Delete(file);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            if (IsDuplicate)
                return;

            _fullSaveDirectoryPath = PersistentData.FullSaveDirectoryPath;
        }
    }
}
