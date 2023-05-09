using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Template.Core;

namespace Template.Audio
{
    [CreateAssetMenu(fileName = "AudioManagerData", menuName = "Singleton/PersistentRuntimeObjectData/AudioManager")]
    public class AudioManagerData : PersistentRuntimeObjectData<AudioManagerData>
    {
        public StudioBankLoader bankLoaderPrefab;
        public AudioObjectAttacher audioObjectPrefab;
        public int maxNumAudioObjects;
    }
}
