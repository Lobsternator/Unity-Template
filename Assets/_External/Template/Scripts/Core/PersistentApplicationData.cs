using UnityEngine;

namespace Template.Core
{
    /// <summary>
    /// Stores various data which is intended to be persistent (constant) throughout the runtime of the application.
    /// </summary>
    [CreateAssetMenu(fileName = "PersistentApplicationData", menuName = "Singleton/PersistentApplicationData")]
    public class PersistentApplicationData : SingletonAsset<PersistentApplicationData>
    {
        [field: SerializeField]
        public string DefaultResourceFolderPath { get; private set; }

        [field: SerializeField]
        public string DefaultSceneFolderPath { get; private set; }
    }
}
