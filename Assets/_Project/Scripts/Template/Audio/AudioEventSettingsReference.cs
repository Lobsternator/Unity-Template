using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Template.Core;

namespace Template.Audio
{
    [Serializable]
    public class AudioEventSettings
    {
        public float volume;
        public float pitch;

        public List<AudioParameter> parameters;
    }

    [CreateAssetMenu(fileName = "new AudioEventSettingsReference", menuName = "Audio/EventSettings")]
    public class AudioEventSettingsReference : ScriptableReference<AudioEventSettings> { }
}
