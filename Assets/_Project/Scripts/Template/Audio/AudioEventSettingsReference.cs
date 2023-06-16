using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Template.Core;

namespace Template.Audio
{
    /// <summary>
    /// Stores FMOD event settings.
    /// </summary>
    [Serializable]
    public class AudioEventSettings
    {
        public float volume;
        public float pitch;

        public List<AudioParameter> parameters;
    }

    /// <summary>
    /// <see cref="ScriptableReference{TValue}"/> wrapper around <see cref="AudioEventSettings"/>.
    /// </summary>
    [CreateAssetMenu(fileName = "new AudioEventSettingsReference", menuName = "Audio/EventSettings")]
    public class AudioEventSettingsReference : ScriptableReference<AudioEventSettings> { }
}
