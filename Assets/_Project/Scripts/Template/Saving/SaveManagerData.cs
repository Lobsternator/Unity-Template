using System.IO;
using System.Text;
using Template.Core;
using UnityEngine;

namespace Template.Saving
{
    /// <summary>
    /// <see cref="PersistentRuntimeObjectData{TSingleton}"/> for <see cref="SaveManager"/>.
    /// </summary>
    [CreateAssetMenu(fileName = "SaveManagerData", menuName = "Singleton/PersistentRuntimeObjectData/SaveManager")]
    public class SaveManagerData : PersistentRuntimeObjectData<SaveManagerData>
    {
        [field: SerializeField] public string SavePath { get; private set; }

        public string FullSaveDirectoryPath => Path.GetDirectoryName(Path.Combine(Application.persistentDataPath, SavePath));

        private StringBuilder _saveFileBuilder = new StringBuilder();

        public string GetFullSaveFilePath(int saveSlot)
        {
            string saveFileName = Path.GetFileNameWithoutExtension(SavePath);
            string saveFileExt  = Path.GetExtension(SavePath);

            _saveFileBuilder.Clear()
                .Append(saveFileName)
                .Append(saveSlot)
                .Append(saveFileExt);

            return Path.Combine(FullSaveDirectoryPath, _saveFileBuilder.ToString());
        }
    }
}
