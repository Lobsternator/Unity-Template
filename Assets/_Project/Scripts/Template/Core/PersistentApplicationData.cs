using UnityEngine;

namespace Template.Core
{
    /// <summary>
    /// Stores various data which is intended to be persistent (constant) throughout the runtime of the application.
    /// </summary>
    [CreateAssetMenu(fileName = "PersistentPathData", menuName = "Singleton/PersistentPathData")]
    public class PersistentApplicationData : SingletonAsset<PersistentApplicationData>
    {
        [field: SerializeField]
        public string ResourceFolderPath { get; private set; }

        [field: SerializeField]
        public string SceneFolderPath { get; private set; }
    }
}
