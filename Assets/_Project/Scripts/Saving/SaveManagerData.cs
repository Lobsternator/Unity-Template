using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Template.Core;
using System.Text;

namespace Template.Saving
{
    [CreateAssetMenu(fileName = "SaveManagerData", menuName = "PersistentRuntimeObjectData/SaveManager")]
    public class SaveManagerData : PersistentRuntimeObjectData
    {
        public string savePath;

        public string FullSaveDirectoryPath => Path.GetDirectoryName(Path.Combine(Application.persistentDataPath, savePath));

        private StringBuilder _saveFileBuilder = new StringBuilder();

        public string GetFullSaveFilePath(int saveSlot)
        {
            string saveFileName = Path.GetFileNameWithoutExtension(savePath);
            string saveFileExt  = Path.GetExtension(savePath);

            _saveFileBuilder.Clear()
                .Append(saveFileName)
                .Append(saveSlot)
                .Append(saveFileExt);

            return Path.Combine(FullSaveDirectoryPath, _saveFileBuilder.ToString());
        }
    }
}
