using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Template.Core;

namespace Template.Scenes
{
    [SingletonAsset]
    [CreateAssetMenu(fileName = "ExtendedSceneManagerData", menuName = "Singleton/Scene/ExtendedSceneManagerData")]
    public class ExtendedSceneManagerData : ScriptableObject
    {
        public string pathToSceneFolder;
    }
}
