using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using Template.Core;
using Template.Saving.Serialization;

namespace Template.Saving
{
    /// <summary>
    /// Singleton that manages saving/loading game state to/from disk.
    /// <b>Automatically created at the start of the program.</b>
    /// </summary>
    [PersistentRuntimeObject(RuntimeInitializeLoadType.BeforeSceneLoad, -700)]
    public class SaveManager : PersistentRuntimeSingleton<SaveManager, SaveManagerData>
    {
        public static CollectorEvent<SaveData> Saving;
        public static Action<ReadOnlyDictionary<DataKey, SerializableObjectDataContainer>> Loading;

        private static StringBuilder _regexPatternBuilder = new StringBuilder();

        private static string GetSaveFileRegexPattern_Internal(string saveSlotPattern)
        {
            string savePath     = SaveManagerData.Instance.SavePath;
            string saveFileName = Path.GetFileNameWithoutExtension(savePath);
            string saveFileExt  = Path.GetExtension(savePath);
            string period       = "";

            if (saveFileExt.StartsWith('.'))
            {
                saveFileExt = saveFileExt.Substring(1);
                period      = @"\.";
            }

            _regexPatternBuilder.Clear()
                .Append(saveFileName)
                .Append(saveSlotPattern)
                .Append(period)
                .Append(saveFileExt);

            return _regexPatternBuilder.ToString();
        }

        public static string GetSaveFileRegexPattern()
        {
            return GetSaveFileRegexPattern_Internal("[0-9]*");
        }
        public static string GetSaveFileRegexPattern(int saveSlot)
        {
            return GetSaveFileRegexPattern_Internal(saveSlot.ToString());
        }

        private static bool WriteSaveDataToFile(Dictionary<DataKey, SerializableObjectDataContainer> saveData, int saveSlot)
        {
            SaveManagerData data         = SaveManagerData.Instance;
            string fullSaveDirectoryPath = data.FullSaveDirectoryPath;

            if (!Directory.Exists(fullSaveDirectoryPath))
                Directory.CreateDirectory(fullSaveDirectoryPath);

            string fullSaveFilePath = data.GetFullSaveFilePath(saveSlot);

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
        private static bool ReadSaveDataFromFile(int saveSlot, out Dictionary<DataKey, SerializableObjectDataContainer> saveData)
        {
            string fullSaveFilePath = SaveManagerData.Instance.GetFullSaveFilePath(saveSlot);

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

        public static bool SaveToSlot(int saveSlot)
        {
#if UNITY_EDITOR
            if (!Instance)
            {
                Debug.LogWarning($"{nameof(SaveManager)} instance needs to exist in order to save!");
                return false;
            }
#endif

            var finalSaveData    = new Dictionary<DataKey, SerializableObjectDataContainer>();
            SaveData[] saveDatas = Saving?.Invoke();
            if (saveDatas is null)
                return false;

            foreach(SaveData saveData in saveDatas)
            {
                if (!finalSaveData.TryAdd(saveData.Owner.DataKey, saveData.Data))
                {
                    UnityEngine.Object ownerAsObj = saveData.Owner as UnityEngine.Object;
                    string ownerName              = ownerAsObj ? ownerAsObj.name : saveData.Owner.ToString();

                    Debug.LogError($"Data key was not valid for object \'{ownerName}\'!", ownerAsObj);
                }
            }

            return WriteSaveDataToFile(finalSaveData, saveSlot);
        }
        public static bool LoadFromSlot(int saveSlot)
        {
#if UNITY_EDITOR
            if (!Instance)
            {
                Debug.LogWarning($"{nameof(SaveManager)} instance needs to exist in order to load!");
                return false;
            }
#endif

            if (!ReadSaveDataFromFile(saveSlot, out var finalSaveData))
                return false;

            Loading?.Invoke(new ReadOnlyDictionary<DataKey, SerializableObjectDataContainer>(finalSaveData));

            return true;
        }

        public static bool HasSaveSlot(int saveSlot)
        {
            string pattern = GetSaveFileRegexPattern(saveSlot);
            Regex regex    = new Regex(pattern, RegexOptions.IgnoreCase);

            string[] files = Directory.GetFiles(SaveManagerData.Instance.FullSaveDirectoryPath);
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);

                if (regex.IsMatch(fileName))
                    return true;
            }

            return false;
        }

        public static bool ClearSaveSlot(int saveSlot)
        {
            string pattern = GetSaveFileRegexPattern(saveSlot);
            Regex regex    = new Regex(pattern, RegexOptions.IgnoreCase);

            string[] files = Directory.GetFiles(SaveManagerData.Instance.FullSaveDirectoryPath);
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
        public static void ClearAllSaveSlots()
        {
            string pattern = GetSaveFileRegexPattern();
            Regex regex    = new Regex(pattern, RegexOptions.IgnoreCase);

            string[] files = Directory.GetFiles(SaveManagerData.Instance.FullSaveDirectoryPath);
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

            SerializationUtility.CompileAndCacheKnownCastDelegates();
        }

#if UNITY_EDITOR
        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();

            Saving  = null;
            Loading = null;
        }
#endif
    }
}
