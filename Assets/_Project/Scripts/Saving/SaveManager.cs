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
    [PersistentRuntimeObject(RuntimeInitializeLoadType.BeforeSceneLoad, -700)]
    public class SaveManager : PersistentRuntimeSingleton<SaveManager, SaveManagerData>
    {
        public static CollectorEvent<SaveData> Saving;
        public static Action<ReadOnlyDictionary<DataKey, SerializableObjectDataContainer>> Loading;

        private static StringBuilder _regexPatternBuilder = new StringBuilder();
        private string _fullSaveDirectoryPath;

        public static string GetSaveFileRegexPattern()
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
                .Append("[0-9]*")
                .Append(period)
                .Append(saveFileExt);

            return _regexPatternBuilder.ToString();
        }
        public static string GetSaveFileRegexPattern(int saveSlot)
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
                .Append(saveSlot)
                .Append(period)
                .Append(saveFileExt);

            return _regexPatternBuilder.ToString();
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
        public bool LoadFromSlot(int saveSlot)
        {
            if (ReadSaveDataFromFile(saveSlot, out var finalSaveData))
            {
                Loading?.Invoke(new ReadOnlyDictionary<DataKey, SerializableObjectDataContainer>(finalSaveData));

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

            SerializationUtility.CompileAndCacheKnownCastDelegates();
        }

        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();

            Saving  = null;
            Loading = null;
        }
    }
}
