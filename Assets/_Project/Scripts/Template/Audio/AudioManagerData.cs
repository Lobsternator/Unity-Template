using FMODUnity;
using Template.Core;
using UnityEngine;

namespace Template.Audio
{
    /// <summary>
    /// <see cref="PersistentRuntimeObjectData{TSingleton}"/> for <see cref="AudioManager"/>.
    /// </summary>
    [CreateAssetMenu(fileName = "AudioManagerData", menuName = "Singleton/PersistentRuntimeObjectData/AudioManager")]
    public class AudioManagerData : PersistentRuntimeObjectData<AudioManagerData>
    {
        public StudioBankLoader bankLoaderPrefab;
        public AudioObjectAttacher audioObjectPrefab;
        public int maxNumAudioObjects;
    }
}
