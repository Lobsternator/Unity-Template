using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Core
{
    [CreateAssetMenu(fileName = "PersistentPathData", menuName = "Singleton/PersistentPathData")]
    public class PersistentPathData : SingletonAsset<PersistentPathData>
    {
        [field: SerializeField] public string ResourceFolderPath { get; private set; }
        [field: SerializeField] public string SceneFolderPath { get; private set; }
    }
}
