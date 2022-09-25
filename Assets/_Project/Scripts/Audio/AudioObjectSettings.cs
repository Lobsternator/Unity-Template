using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Serializable = System.SerializableAttribute;

namespace Template.Audio
{
    [Serializable]
    [CreateAssetMenu(fileName = "new AudioObjectSettings", menuName = "Audio/Settings")]
    public class AudioObjectSettings : ScriptableObject
    {
        public float volume;
    }
}
