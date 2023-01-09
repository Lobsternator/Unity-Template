using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Audio
{
    [CreateAssetMenu(fileName = "new AudioEventSettings", menuName = "Audio/EventSettings")]
    public class AudioEventSettings : ScriptableObject
    {
        public float volume;
        public float pitch;
    }
}
