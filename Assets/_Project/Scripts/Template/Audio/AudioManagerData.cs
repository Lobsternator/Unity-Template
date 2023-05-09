using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Template.Core;

namespace Template.Audio
{
    [CreateAssetMenu(fileName = "AudioManagerData", menuName = "Singleton/PersistentRuntimeObjectData/AudioManager")]
    public class AudioManagerData : PersistentRuntimeObjectData<AudioManagerData>
    {
        public GameObject bankLoaderPrefab;
        public GameObject audioObjectPrefab;
        public int maxNumAudioObjects;
    }
}
