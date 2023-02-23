using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Audio
{
    [Serializable]
    public class AudioEventSettings
    {
        public float volume;
        public float pitch;

        public List<AudioParameter> parameters;
    }
}
