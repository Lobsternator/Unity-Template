using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Template.Core;

namespace Template.Saving
{
    [CreateAssetMenu(fileName = "SaveManagerData", menuName = "PersistentRuntimeObjectData/SaveManager")]
    public class SaveManagerData : PersistentRuntimeObjectData
    {
        public string savePath;

        public string FullSaveDirectoryPath => Path.GetDirectoryName(Path.Combine(Application.persistentDataPath, savePath));

        public string GetFullSaveFilePath(int saveSlot)
        {
            string saveFileName     = Path.GetFileNameWithoutExtension(savePath);
            string saveFileExt      = Path.GetExtension(savePath);
            string fullSaveFileName = $"{saveFileName}{saveSlot}{saveFileExt}";

            return Path.Combine(FullSaveDirectoryPath, fullSaveFileName);
        }
    }
}
