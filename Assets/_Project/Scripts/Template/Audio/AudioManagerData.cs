using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Template.Core;

namespace Template.Audio
{
    /// <summary>
    /// PersistentRuntimeObjectData for <see cref="AudioManager"/>.
    /// </summary>
    [CreateAssetMenu(fileName = "AudioManagerData", menuName = "Singleton/PersistentRuntimeObjectData/AudioManager")]
    public class AudioManagerData : PersistentRuntimeObjectData<AudioManagerData>
    {
        public StudioBankLoader bankLoaderPrefab;
        public AudioObjectAttacher audioObjectPrefab;
        public int maxNumAudioObjects;
    }
}
