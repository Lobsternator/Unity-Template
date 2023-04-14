using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Template.Core;

namespace Template.Scenes
{
    [SingletonAsset]
    [CreateAssetMenu(fileName = "SceneManagerData", menuName = "Singleton/Scene/SceneManagerData")]
    public class ExtendedSceneManagerData : ScriptableObject
    {
        public string pathToSceneFolder;
    }
}
