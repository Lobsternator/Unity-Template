using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Template.Core;

namespace Template.Scenes
{
    [CreateAssetMenu(fileName = "SceneManagerData", menuName = "PersistentRuntimeObjectData/SceneManager")]
    public class SceneManagerData : PersistentRuntimeObjectData
    {
        public string pathToSceneFolder;
    }
}
